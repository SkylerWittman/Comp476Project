using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinoBehavior : MonoBehaviour {

    public float acceleration = 3.0f;
    public float maxSpeed = 5.0f;
    public float maxRunSpeed = 7.5f;

    private Rigidbody rigidbody;
    private Vector3 newDirection;
    private float directionToHeadTo;
    private Transform player;

    private Grid grid;
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

    private float pursueDistance = 100.0f;
    private bool canAttack;
    public float dinoHealth = 600.0f;
    public float dinoDamage = 15.0f;
    public float dinoAttackCooldown = 3.0f;

    //Animation vars
    private Animation anim;
    private AnimationClip walkClip;
    private AnimationClip runClip;
    private AnimationClip attackClip;

    //Audio vars
    private AudioSource audioSource;
    private AudioClip dinoCall;
    private AudioClip dinoBite;
    private AudioClip dinoHit;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        GetComponent<NPCDetail>().health = dinoHealth;
        GetComponent<NPCDetail>().damage = dinoDamage;

        //RNG initialization
        Random.seed = (int)System.DateTime.Now.Ticks;
        
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = transform.position;
        currentState = State.WANDER;
        InvokeRepeating("assessState", 0.0f, 0.1f);

        //Each dino will cast a ray at some random interval between 2 and 3
        //Using a random time value because if all dino were set to the same value, then performance would take a hit every x seconds
        float ranTime = Random.Range(2.0f, 3.0f);
        InvokeRepeating("castSphere", 0.0f, ranTime);

        InvokeRepeating("collisionAvoidance", 0.0f, 0.1f);

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

        grid = GameObject.FindGameObjectWithTag("TerrainEngine").GetComponent<Grid>();
        pathFinder = GameObject.FindGameObjectWithTag("TerrainEngine").GetComponent<AStar>();
        currentPath = new List<Node>();
        currentNode = null;
        goalNode = null;

        dinoCall = Resources.Load("Sounds/Dino/DinoCall") as AudioClip;
        dinoBite = Resources.Load("Sounds/Dino/DinoBite") as AudioClip;
        dinoHit = Resources.Load("Sounds/Dino/DinoHit") as AudioClip;
    }

    private void assessState()
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

    private void wander()
    {
        //If there is no current path, then we will have to find a new path
        if (currentPath.Count == 0)
        {
            //We will loop until we have found a path that contains at least 2 nodes
            int numIterations = 0; //TEMPORARY DEBUGGING
            while (currentPath.Count < 1)
            {
                currentPath = pathFinder.getPath(transform.position, grid.getRandomNodeCloseToPos(transform.position).position);
                
                numIterations++;
                if (numIterations == 20)
                {
                    Debug.Log("I BUGGED OUT LOL. COMMITTING SUICIDE NOW. IGNORE THE ARUMENT OUT OF RANGE EXCEPTION");
                    gameObject.name = "I BUGGED OUT LOL";
                    Destroy(GetComponent<DinoBehavior>());
                    break;
                }
            }
            //We'll assign the currentNode to the first node in the path, and goalNode to the last node in the path
            currentNode = currentPath[0];
            goalNode = currentPath[currentPath.Count - 1];
        }

        Vector3 chaseDirection = currentNode.position - transform.position;
        chaseDirection.y = 0.0f;
        wanderMovement(chaseDirection);
    }

    private void wanderMovement(Vector3 chaseDirection)
    {
        //-chaseDirection because the dino model thinks its forward position is behind it. If not negative, the dino would walk backwards
        Quaternion chaseRotation = Quaternion.LookRotation(-chaseDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, chaseRotation, Time.deltaTime * maxSpeed);

        if (rigidbody.velocity.magnitude < maxSpeed)
        {
            //Same logic applied to chaseRotation
            rigidbody.AddForce(-transform.forward * acceleration, ForceMode.VelocityChange);
        }

        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }

        //If position is less than 5 units away from node, we'll start to look for the next node in the path
        //This will for sure trigger a currentNode change
        if (Vector3.Distance(new Vector3(transform.position.x, 0.0f, transform.position.z), new Vector3(currentNode.position.x, 0.0f, currentNode.position.z)) < 5.0f)
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

    private void pursue()
    {   
        //If player is not null, it means we've been assigned a player to pursue
        if (player != null)
        {   
            Vector3 chaseDirection = player.position - transform.position;
			chaseDirection.y = 0.0f;
            pursueMovement(chaseDirection);
        }
        //Else, we've lost the player and we can transition back to wander state
        else
        {
            currentState = State.WANDER;
            anim.Play(walkClip.name);
        }
    }

    private void pursueMovement(Vector3 chaseDirection)
    {
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
        if (Vector3.Distance(transform.position, player.position) > pursueDistance)
        {
            player = null;
        }
        //If the player is less than 10 units away, we transition to combat state
        else if (Vector3.Distance(transform.position, player.position) < 10.0f)
        {
            currentState = State.COMBAT;
        }
    }

    private void combat()
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
    private void castSphere()
    {
        if (currentState == State.WANDER)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 70.0f);
            foreach (Collider c in colliders)
            {
                if (c.gameObject.tag == "Player")
                {
                    setPursue();
                    //Assign the transform of the player so that way we have a way to track their position
                    player = c.gameObject.transform;
                    break;
                }
            }
        }
    }

    //Pursuing will follow a certain routine
    private void setPursue()
    {
        //We'll clear the path, as we are now pursuing the player. 
        //This will ensure that if we return to wander state, we will search for a new path.
        currentState = State.PURSUE;
        currentNode = null;
        currentPath.Clear();
        anim.Play(runClip.name);
        audioSource.PlayOneShot(dinoCall, 0.1f);
    }

    //The attack routine, which triggers the animation and do damage to the player.
    private IEnumerator attack()
    {
        canAttack = false;
        //only works if there is a single player
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>().takeDamage(dinoDamage);
        Debug.Log("Dino did " + dinoDamage + " damage");
        anim.Play(attackClip.name);
        audioSource.PlayOneShot(dinoBite);
        yield return new WaitForSeconds(dinoAttackCooldown);
        canAttack = true;
    }

    //If the dino has been hit by an arrow, he will find the player and then follow the pursue routine
    public void gotHit()
    {
        if (currentState != State.PURSUE)
        {
            setPursue();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            pursueDistance = float.MaxValue;
        }
    }

    private void collisionAvoidance()
    {
        if (currentState == State.PURSUE)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10.0f);
            foreach (Collider c in colliders)
            {
                if (c.gameObject.tag == "TreeMarker")
                {
                    rigidbody.AddForce(transform.right.normalized * acceleration, ForceMode.VelocityChange);
                }
            }
        }
    }
}
