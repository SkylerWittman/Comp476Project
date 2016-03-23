using UnityEngine;
using System.Collections;

public class ZombieBehavior : MonoBehaviour {

	public float acceleration = 3;
	public float maxSpeed = 5;
	public float directionChangeTime = 3;
	public float rotateTime = 1.5f;
	public float maxDirectionChange = 15;

	private Rigidbody rigidbody;
	private Vector3 newDirection;
	private float directionToHeadTo;
	private Transform player;

    /*
    FSM:
     * WANDER -> PURSUE : Transition occurs when the castSphere function finds the player within a 30 unit radius of the zombie
     * PURSUE -> COMBAT : Transition occurs when the player is less than 5 units from the zombie
     * COMBAT -> PURSUE : Transition occurs after an attempt to attack the player
     * PURSUE -> WANDER : Transition occurs when the player is more than 100 units away from the zombie
    */
    private enum State { WANDER, PURSUE, COMBAT };
    private State currentState;

    private bool canAttack;

    Animator anim;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		directionToHeadTo = Random.Range (0, 360);
		transform.eulerAngles = new Vector3 (0, directionToHeadTo, 0);
        currentState = State.WANDER;
        StartCoroutine (FaceNewDirection ());
        InvokeRepeating("assessState", 0.0f, 0.1f);

        //Each zombie will cast a ray at some random interval between 2 and 3
        //Using a random time value because if all zombies were set to the same value, then performance would take a hit every x seconds
        float ranTime = Random.Range(2.0f, 3.0f);
        InvokeRepeating("castSphere", 0.0f, ranTime);

        //Initially do not know the position of player
        player = null;

        canAttack = true;

        anim = GetComponent<Animator>();
	}

	IEnumerator FaceNewDirection(){
		while (true) {
			GetNewDirection ();
			yield return new WaitForSeconds (directionChangeTime);
		}
	}

	void GetNewDirection(){
		float floor = Mathf.Clamp (directionToHeadTo - maxDirectionChange, 0, 360);
		float ceiling = Mathf.Clamp (directionToHeadTo + maxDirectionChange, 0, 360);
		directionToHeadTo = Random.Range (floor, ceiling);
		newDirection = new Vector3 (0, directionToHeadTo, 0);
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
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, newDirection, Time.deltaTime * rotateTime);

        if (rigidbody.velocity.magnitude < maxSpeed)
        {
            rigidbody.AddForce(transform.forward * acceleration, ForceMode.VelocityChange);
        }

        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
    }

    void pursue()
    {
        if (player != null)
        {
            Vector3 chaseDirection = player.position - transform.position;
            transform.rotation = Quaternion.LookRotation(chaseDirection);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(chaseDirection), Time.deltaTime);
            if (rigidbody.velocity.magnitude < maxSpeed)
            {
                rigidbody.AddForce(chaseDirection.normalized * acceleration, ForceMode.VelocityChange);
            }

            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
            }

            if (Vector3.Distance(transform.position, player.position) > 100.0f)
            {
                player = null;
            }
            else if (Vector3.Distance(transform.position, player.position) < 5.0f)
            {
                currentState = State.COMBAT;
            }
        }
        else
        {
            currentState = State.WANDER;
        }
    }

    void combat()
    {
        if (canAttack)
        {
            StartCoroutine(attack());
        }
        currentState = State.PURSUE;
    }

    void castSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30.0f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                currentState = State.PURSUE;
                player = c.gameObject.transform;
                break;
            }
        }
    }

    IEnumerator attack()
    {
        canAttack = false;
        Debug.Log("Strike player with fleshy appendage");
        anim.Play("attack");
        yield return new WaitForSeconds(2.0f);
        canAttack = true;
    }
}
