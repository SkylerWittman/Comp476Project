using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinoBehavior : MonoBehaviour {

    public float acceleration = 3.0f;
    public float maxSpeed = 5.0f;
    public float maxRunSpeed = 10.0f;
    public float directionChangeTime = 3.0f;
    public float rotateTime = 1.5f;
    public float maxDirectionChange = 15.0f;

    private Rigidbody rigidbody;
    private Vector3 newDirection;
    private float directionToHeadTo;
    private Transform player;

    private AStar pathFinder;
    private List<Node> currentPath;
    private Node currentNode;
    private Node goalNode;

    /*
    FSM:
     * WANDER -> PURSUE : Transition occurs when the castSphere function finds the player within a 70 unit radius of the dino
     * PURSUE -> COMBAT : Transition occurs when the player is less than 5 units from the dino
     * COMBAT -> PURSUE : Transition occurs after an attempt to attack the player
     * PURSUE -> WANDER : Transition occurs when the player is more than 100 units away from the dino
    */

    private enum State { WANDER, PURSUE, COMBAT };
    private State currentState;

    private bool canAttack;
    public float dinoHealth = 600.0f;
    public float dinoDamage = 15.0f;
    public float dinoAttackCooldown = 3.0f;

    private Animation anim;
    private AnimationClip walkClip;
    private AnimationClip runClip;
    private AnimationClip attackClip;

    void Start()
    {
        GetComponent<NPCDetail>().health = dinoHealth;
        GetComponent<NPCDetail>().damage = dinoDamage;

        //RNG initialization
        Random.seed = (int)System.DateTime.Now.Ticks;
        
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = transform.position;
        directionToHeadTo = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, directionToHeadTo, 0);
        currentState = State.WANDER;
        StartCoroutine(FaceNewDirection());
        InvokeRepeating("assessState", 0.0f, 0.1f);

        //Each dino will cast a ray at some random interval between 2 and 3
        //Using a random time value because if all dino were set to the same value, then performance would take a hit every x seconds
        float ranTime = Random.Range(2.0f, 3.0f);
        InvokeRepeating("castSphere", 0.0f, ranTime);

        //Initially do not know the position of player
        player = null;

        canAttack = true;

        anim = GetComponent<Animation>();
        //Changing layers and adding weights allows us to play 2 animations at the same time
        anim["Allosaurus_Attack02"].layer = 1;
        anim["Allosaurus_Attack02"].weight = 0.7f;
        walkClip = anim.GetClip("Allosaurus_Walk");
        runClip = anim.GetClip("Allosaurus_Run");
        attackClip = anim.GetClip("Allosaurus_Attack02");

        anim.Play(walkClip.name);

        pathFinder = GameObject.FindGameObjectWithTag("TerrainEngine").GetComponent<AStar>();
        currentPath = new List<Node>();
        currentNode = null;
        goalNode = null;
    }

    IEnumerator FaceNewDirection()
    {
        while (true)
        {
            GetNewDirection();
            yield return new WaitForSeconds(directionChangeTime);
        }
    }

    void GetNewDirection()
    {
        float floor = Mathf.Clamp(directionToHeadTo - maxDirectionChange, 0, 360);
        float ceiling = Mathf.Clamp(directionToHeadTo + maxDirectionChange, 0, 360);
        directionToHeadTo = Random.Range(floor, ceiling);
        newDirection = new Vector3(0, directionToHeadTo, 0);
    }

    void assessState()
    {
        if (currentState == State.WANDER)
        {
            wander();
        }
        else if (currentState == State.PURSUE)
        {
            pursue();
        }
        else if (currentState == State.COMBAT)
        {
            combat();
        }
    }

    void wander()
    {
        //If there is no current path, then we will have to find a new path
        if (currentPath.Count == 0)
        {
            //We will loop until we have found a path that contains at least 2 nodes
            while (currentPath.Count < 2)
            {
                //We will attempt to wander to some position that is +/- 150 units from the dino's current position. It is clamped to the boundaries of the level
                float wanderX = Random.Range(Mathf.Clamp(transform.position.x - 150.0f, 0.0f, 1000.0f), Mathf.Clamp(transform.position.x + 150.0f, 0.0f, 1000.0f));
                float wanderZ = Random.Range(Mathf.Clamp(transform.position.z - 150.0f, 0.0f, 1000.0f), Mathf.Clamp(transform.position.z + 150.0f, 0.0f, 1000.0f));
                Vector3 targetPos = new Vector3(wanderX, 0.0f, wanderZ);
                currentPath = pathFinder.getPath(transform.position, targetPos);
            }
            //We'll assign the currentNode to the first node in the path, and goalNode to the last node in the path
            currentNode = currentPath[0];
            goalNode = currentPath[currentPath.Count - 1];
        }

        Vector3 chaseDirection = currentNode.position - transform.position;
        chaseDirection.y = 0.0f;
        //-chaseDirection because the dino model thinks its forward position is behind it. If not negative, the dino would walk backwards
        Quaternion chaseRotation = Quaternion.LookRotation(-chaseDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, chaseRotation, Time.deltaTime * maxRunSpeed);

        if (rigidbody.velocity.magnitude < maxSpeed)
        {
            //Same logic applied to chaseRotation
            rigidbody.AddForce(-transform.forward * acceleration, ForceMode.VelocityChange);
        }

        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }

        //If position is less than 10 units away from node, we'll start to look for the next node in the path
        //This will for sure trigger a currentNode change
        if (Vector3.Distance(transform.position, currentNode.position) < 10.0f)
        {  
            //If we've reached the goal, then we'll clear our path so we can get another one
            if (currentNode.Equals(goalNode))
            {
                currentPath.Clear();
            }
            //Else, get the next node in the path
            else
            {
                currentNode = currentPath[currentPath.IndexOf(currentNode) + 1];
            }
        }
    }

    void pursue()
    {
        //If player is not null, it means we've been assigned a player to pursue
        if (player != null)
        {   
            Vector3 chaseDirection = player.position - transform.position;
			chaseDirection.y = 0.0f;
            //Look at the comment in wander if -chaseDirection looks funny
            Quaternion chaseRotation = Quaternion.LookRotation(-chaseDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, chaseRotation, Time.deltaTime * maxRunSpeed);

            if (rigidbody.velocity.magnitude < maxRunSpeed)
            {
                rigidbody.AddForce(chaseDirection.normalized * acceleration, ForceMode.VelocityChange);
            }

            if (rigidbody.velocity.magnitude > maxRunSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxRunSpeed;
            }

            //If the player is more than 100 units away, then we have "lost" the player, and we'll assign it to null so we can transition to wander state
            if (Vector3.Distance(transform.position, player.position) > 100.0f)
            {
                player = null;
            }
            //If the player is less than 10 units away, we transition to combat state
            else if (Vector3.Distance(transform.position, player.position) < 10.0f)
            {
                currentState = State.COMBAT;
            }
        }
        //Else, we've lost the player and we can transition back to wander state
        else
        {
            currentState = State.WANDER;
            anim.Play(walkClip.name);
        }
    }

    void combat()
    {
        //Attacks occur every 2 seconds
        if (canAttack)
        {
            StartCoroutine(attack());
        }
        //If combat is called when the attack is on cooldown, we will continue to pursue the target
        currentState = State.PURSUE;
    }

    //This finds whether the player is in a 70 unit radius of the dino
    void castSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 70.0f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                //We'll clear the path, as we are now pursuing the player. 
                //This will ensure that if we return to wander state, we will search for a new path.
                currentPath.Clear();
                currentState = State.PURSUE;
                anim.Play(runClip.name);
                //Assign the transform of the player so that way we have a way to track their position
                player = c.gameObject.transform;
                break;
            }
        }
    }

    //The attack routine, which triggers the animation and do damage to the player.
    IEnumerator attack()
    {
        canAttack = false;
        //only works if there is a single player
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>().takeDamage(dinoDamage);
        Debug.Log("Dino did " + dinoDamage + " damage");
        anim.Play(attackClip.name);
        yield return new WaitForSeconds(dinoAttackCooldown);
        canAttack = true;
    }

    
}
