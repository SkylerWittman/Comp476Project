using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmSpiderBehavior : MonoBehaviour {

	private Rigidbody rb;
	private GameObject target;
	private GameObject swarmController;
	private SwarmController swarmer;
	private List<GameObject> swarmNeighbors = new List<GameObject>();
	private Vector3 alignment;
	private Vector3 cohesion;
	private Vector3 seperation;
	private Vector3 moveDirection;
	private Vector3 wanderVector;
	private bool canHunt = false;
	private bool canWander = false;
	private bool cantSeePlayer = true;
	private bool canAttack = true;
	private float moveSpeed = 10;
	private float collisionTimer = 120;
	private float searchTimer = 120;
	private float distanceToHunt = 50;
	private float spiderDamage = 2.0f;
	private float attackDistance = 3.0f;
	private Animation anim;
	private AnimationClip runClip;
	private AnimationClip attackClip;

	public float swarmDistance = 100.0f;

    
    public float swarmSpiderHealth = 100.0f;
	public float swarmSpiderDamage = 4.0f;

    void Start () {
		GetComponent<SwarmSpiderDeath>().health = swarmSpiderHealth;
		GetComponent<SwarmSpiderDeath>().damage = swarmSpiderDamage;

        rb = GetComponent<Rigidbody> ();
		swarmController = GameObject.FindGameObjectWithTag("controller");
		swarmer = swarmController.GetComponent<SwarmController> ();

		StartCoroutine (RecalculateSwarm ());
		target = GameObject.FindGameObjectWithTag ("Player");

		anim = GetComponent<Animation>();

		anim ["walkSpider"].speed = 7.0f;
		runClip = anim.GetClip("walkSpider");
		attackClip = anim.GetClip("attackSpider");

		canWander = true;

	}

	private IEnumerator Attack()
	{
		canAttack = false;
		//only works if there is a single player
		GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>().takeDamage(spiderDamage);
		Debug.Log("Spider did " + spiderDamage + " damage");
		anim.Play(attackClip.name);
		yield return new WaitForSeconds(2.0f);
		canAttack = true;
	}

	public void SpiderDeath(){
		canWander = false;
		canHunt = false;
		anim.Stop ();
	}


	//when a spider in the swarm dies he must inform his swarm members so they can recalcualte thier swarm mebers for proper movement
	public void InformSwarmMembersOfDeath(GameObject swarmMember){
		swarmer.UpdateSwarm (); //updates the master swarm array in swarm controller
		swarmNeighbors.Remove(swarmMember); //removes the dead spider who called this method

		for(int i =0; i< swarmNeighbors.Count; i++){
			swarmNeighbors [i].GetComponent<SwarmSpiderBehavior> ().FindMySwarm ();

		}
	}

	//when a spider in the swarm finds the player he notifys his swarm
	public void InformSwarmMembersOfSighting(){
		swarmer.UpdateSwarm (); //updates the master swarm array in swarm controller
		for(int i =0; i< swarmNeighbors.Count; i++){
			swarmNeighbors [i].GetComponent<SwarmSpiderBehavior> ().setCanHunt ();

		}
	}

	//finds swarm spider who are nearby and add them to the list to keep track
	public void FindMySwarm(){
		swarmer.UpdateSwarm (); //updates the master swarm array in swarm controller
		GameObject[] swarm = swarmer.getSwarm (); //gets most recent list of spider on the map
		swarmNeighbors.Clear (); //clears the list
		foreach (GameObject spider in swarm) {
			if (Vector3.Distance (spider.transform.position, this.transform.position) < swarmDistance && (spider != this)) {
				swarmNeighbors.Add (spider);		
			}
		}
	}

	private Vector3 AlignVectorCalculate(){
		Vector3 compuationVector = new Vector3();

		//if you have nobody in the swarm then do nothing
		if (swarmNeighbors.Count == 0) {
			return compuationVector;
		}

		//find velocity of all Spider lins in the swarm
		foreach (GameObject spider in swarmNeighbors) {
			if (spider == null) {
				continue;
			}

			if (spider != this) {
				compuationVector.x += spider.GetComponent<Rigidbody> ().velocity.x;
				compuationVector.z += spider.GetComponent<Rigidbody> ().velocity.z;

			}
		}

		//divide the vector values by the amount of Spider in the swarm
		compuationVector.x /= swarmNeighbors.Count;
		compuationVector.z /= swarmNeighbors.Count;
		compuationVector.Normalize (); 

		return compuationVector;
	}


	//finds the center of mass of the swarm, so the swarm can steer towards the center of the mass
	private Vector3 CenterOfMassOfSwarm(){
		Vector3 compuationVector = new Vector3();
		Vector3 finalVector;

		//if you have nobody in the swarm then do nothing
		if (swarmNeighbors.Count == 0) {
			return compuationVector;
		}

		//find position of all Spider in the swarm
		foreach (GameObject spider in swarmNeighbors) {
			if (spider == null) {
				continue;
			}

			if (spider != this) {
				compuationVector.x += spider.transform.position.x;
				compuationVector.z += spider.transform.position.z;

			}
		}

		//divide the vector values by the amount of spider in the swarm
		compuationVector.x /= swarmNeighbors.Count;
		compuationVector.z /= swarmNeighbors.Count;

		//find the direction to the center of mass relitive to this spider and steer towards it
		finalVector = new Vector3((compuationVector.x - this.transform.position.x), this.transform.position.y, (compuationVector.z - this.transform.position.z));
		finalVector.Normalize (); 

		return finalVector;
	}


	//finds the distance between spider in the swarm, so the spider can stay serperated
	private Vector3 SeperationOfSwarm(){
		Vector3 compuationVector = new Vector3();
		Vector3 finalVector;

		//if you have nobody in the swarm then do nothing
		if (swarmNeighbors.Count == 0) {
			return compuationVector;
		}

		//find distance from spider and all other neighbours in the swarm and add it to the serperation vector
		foreach (GameObject spider in swarmNeighbors) {

			if (spider == null) {
				continue;
			}

			if (spider != this) {
				compuationVector.x += spider.transform.position.x - this.transform.position.x;
				compuationVector.z += spider.transform.position.z - this.transform.position.z;

			}
		}

		//divide the vector values by the amount of spider in the swarm and negated the x and z values so the spider steers away from swarm properly
		compuationVector.x /= swarmNeighbors.Count;
		compuationVector.z /= swarmNeighbors.Count;
		compuationVector.x *= -20;
		compuationVector.z *= -20;
		compuationVector.Normalize ();

		return compuationVector;
	}


	IEnumerator RecalculateSwarm(){
		while (true) {
			FindMySwarm ();
			int rand = Random.Range (15, 20);
			yield return new WaitForSeconds (rand);
		}
	}

	private void CollisionAvoidance(){
		RaycastHit hit;
		Vector3 avoidanceVector;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 10)) {
			if (hit.collider.tag == "TreeMarker") {
				rb.AddForce (transform.right * 30 * Time.deltaTime, ForceMode.VelocityChange);
			}
		}
	}
		

	public void setCanHunt(){
		canHunt = true;
		canWander = false;
	}


	private void PlayerCloseEnoughToHunt(){
		//checks if player is close enough to chase if they are they inform there swarm members
		if (Vector3.Distance (target.transform.position, transform.position) < distanceToHunt) {
			cantSeePlayer = false;
			setCanHunt ();
			InformSwarmMembersOfSighting ();
		} 
	}

	// Update is called once per frame

	void Update(){
		if (Vector3.Distance (target.transform.position, this.transform.position) < attackDistance && canAttack) {
			StartCoroutine (Attack ());
		}
	}

	void FixedUpdate () {
	
		collisionTimer--;
		searchTimer--;

		//if the spider hasnt spotted a player then every 2 seconds he will see if the player is in sight
		if (cantSeePlayer && searchTimer < 0) {
			PlayerCloseEnoughToHunt ();
			searchTimer = 120;

		}
		//every 3seconds the spider raycast ahead to make sure the wont run into a tree
		if (collisionTimer < 0) {
			CollisionAvoidance ();
			collisionTimer = 180;
		}


		if (canWander) {

			//apply gravity to enemies
			rb.AddForce (Vector3.down * rb.mass * 40);


			//recalcualte swarm information to be used in movement
			alignment = AlignVectorCalculate ();
			cohesion = CenterOfMassOfSwarm ();
			seperation = SeperationOfSwarm ();

			//getCurrent Swarms Wander Direction
			wanderVector = (swarmer.GetWanderTarget() - transform.position);
			wanderVector.y = 0.0f;

			Quaternion newRotation = Quaternion.LookRotation(wanderVector);
			newRotation.x = 0.0f;
			newRotation.z = 0.0f;
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 500.0f * Time.deltaTime);

			rb.velocity += ( wanderVector + new Vector3 ((alignment.x + cohesion.x + seperation.x), 0, (alignment.z + cohesion.z + seperation.z)));
			rb.velocity = rb.velocity.normalized * moveSpeed;
		
			anim.Play(runClip.name);
		}


		if (canHunt) {

			//apply gravity to enemies
			rb.AddForce (Vector3.down * rb.mass * 30);

			//recalcualte swarm information to be used in movement
			alignment = AlignVectorCalculate ();
			cohesion = CenterOfMassOfSwarm ();
			seperation = SeperationOfSwarm ();

			//look towards the target
			Vector3 newDirection = (target.transform.position - this.transform.position).normalized;
			Quaternion newRotation = Quaternion.LookRotation(newDirection + new Vector3 ((alignment.x + cohesion.x + seperation.x), 0, (alignment.z + cohesion.z + seperation.z)).normalized);
			newRotation.x = 0.0f;
			newRotation.z = 0.0f;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 500.0f * Time.deltaTime);

			//move towards the target as a swarm
			newDirection.y = 0.0f;
			rb.velocity += (newDirection + new Vector3 ((alignment.x + cohesion.x + seperation.x), 0, (alignment.z + cohesion.z + seperation.z)).normalized);
			rb.velocity = rb.velocity.normalized * moveSpeed;

			anim.Play(runClip.name);
		}
			
	}
}
