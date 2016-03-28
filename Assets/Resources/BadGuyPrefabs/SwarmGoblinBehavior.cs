using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmGoblinBehavior : MonoBehaviour {

	private Rigidbody rb;
	private GameObject target;
	//private GameObject[] SwarmGoblins;
	private GameObject swarmController;
	private SwarmController swarmer;
	private List<GameObject> swarmNeighbors = new List<GameObject>();
	private Vector3 alignment;
	private Vector3 cohesion;
	private Vector3 seperation;
	private Vector3 moveDirection;
	private bool canMove = false;
	private float moveSpeed = 10;
	private Animation anim;
	private AnimationClip runClip;
	private AnimationClip attackClip;

	public float swarmDistance = 100.0f;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		//SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
		swarmController = GameObject.FindGameObjectWithTag("controller");
		swarmer = swarmController.GetComponent<SwarmController> ();

		StartCoroutine (RecalculateSwarm ());
		target = GameObject.FindGameObjectWithTag ("Player");

		anim = GetComponent<Animation>();
		//Changing layers and adding weights allows us to play 2 animations at the same time
		anim["attack02"].layer = 1;
		anim["attack02"].weight = 0.7f;
		runClip = anim.GetClip("run");
		attackClip = anim.GetClip("attack02");

	}
	
	//when a goblin in the swarm dies he must inform his swarm members so they can recalcualte thier swarm mebers for proper movement
	public void InformSwarmMembersOfDeath(GameObject swarmMember){
		swarmer.UpdateSwarm (); //updates the master swarm array in swarm controller
		swarmNeighbors.Remove(swarmMember); //removes the dead goblin who alled this method

		for(int i =0; i< swarmNeighbors.Count; i++){
			swarmNeighbors [i].GetComponent<SwarmGoblinBehavior> ().FindMySwarm ();

		}
	}


	//finds swarm goblins who are nearby and add them to the list to keep track
	public void FindMySwarm(){
		GameObject[] swarm = swarmer.getSwarm (); //gets most recent list of goblins on the map
		swarmNeighbors.Clear (); //clears the list
		foreach (GameObject goblin in swarm) {
			if (Vector3.Distance (goblin.transform.position, this.transform.position) < swarmDistance && (goblin != this)) {
					swarmNeighbors.Add (goblin);		
			}
		}
		canMove = true;
	}

	private Vector3 AlignVectorCalculate(){
		Vector3 compuationVector = new Vector3();

		//if you have nobody in the swarm then do nothing
		if (swarmNeighbors.Count == 0) {
			return compuationVector;
		}

		//find velocity of all goblins in the swarm
		foreach (GameObject goblin in swarmNeighbors) {
			if (goblin == null) {
				continue;
			}

			if (goblin != this) {
				compuationVector.x += goblin.GetComponent<Rigidbody> ().velocity.x;
				compuationVector.z += goblin.GetComponent<Rigidbody> ().velocity.z;

			}
		}

		//divide the vector values by the amount of goblins in the swarm
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

		//find position of all goblins in the swarm
		foreach (GameObject goblin in swarmNeighbors) {
			if (goblin == null) {
				continue;
			}

			if (goblin != this) {
				compuationVector.x += goblin.transform.position.x;
				compuationVector.z += goblin.transform.position.z;

			}
		}

		//divide the vector values by the amount of goblins in the swarm
		compuationVector.x /= swarmNeighbors.Count;
		compuationVector.z /= swarmNeighbors.Count;

		//find the direction to the center of mass relitive to this goblin and steer towards it
		finalVector = new Vector3((compuationVector.x - this.transform.position.x), this.transform.position.y, (compuationVector.z - this.transform.position.z));
		finalVector.Normalize (); 

		return finalVector;
	}


	//finds the distance between goblins in the swarm, so the goblins can stay serperated
	private Vector3 SeperationOfSwarm(){
		Vector3 compuationVector = new Vector3();
		Vector3 finalVector;

		//if you have nobody in the swarm then do nothing
		if (swarmNeighbors.Count == 0) {
			return compuationVector;
		}

		//find distance from goblin and all other neighbours in the swarm and add it to the serperation vector
		foreach (GameObject goblin in swarmNeighbors) {

			if (goblin == null) {
				continue;
			}

			if (goblin != this) {
				compuationVector.x += goblin.transform.position.x - this.transform.position.x;
				compuationVector.z += goblin.transform.position.z - this.transform.position.z;

			}
		}

		//divide the vector values by the amount of goblins in the swarm and negated the x and z values so the goblin steers away from swarm properly
		compuationVector.x /= swarmNeighbors.Count;
		compuationVector.z /= swarmNeighbors.Count;
		compuationVector.x *= -8;
		compuationVector.z *= -8;
		compuationVector.Normalize ();

		return compuationVector;
	}


	IEnumerator RecalculateSwarm(){
		while (true) {
			FindMySwarm ();
			yield return new WaitForSeconds (15.0f);
		}
	}



	// Update is called once per frame
	void FixedUpdate () {
	
		if (canMove) {

			rb.AddForce (Vector3.down * rb.mass * 10);
			//recalcualte swarm information to be used in movement
			alignment = AlignVectorCalculate ();
			cohesion = CenterOfMassOfSwarm ();
			seperation = SeperationOfSwarm ();

			//look towards the target
			Vector3 newDirection = (target.transform.position - this.transform.position).normalized;
			Quaternion newRotation = Quaternion.LookRotation(newDirection);
			newRotation.x = 0.0f;
			newRotation.z = 0.0f;
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10.0f);

			//move towards the target as a swarm, zeroing out the y coordinate restricts ability to move upwards when player jumps
			//newDirection.y = 0.0f;
			rb.velocity += (newDirection + new Vector3 ((alignment.x + cohesion.x + seperation.x), 0, (alignment.z + cohesion.z + seperation.z)).normalized);
			rb.velocity = rb.velocity.normalized * moveSpeed;

			anim.Play(runClip.name);
		}



	}
}
