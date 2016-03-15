using UnityEngine;
using System.Collections;

public class GoblinBehavior : MonoBehaviour {


	public float acceleration = 7;
	public float maxSpeed = 12;
	public int minDistance = 10;
	public int distanceFromTreeToStop = 3;
	public int distanceFromTreeToSlowDown = 8;
	public float timeToChangeBetweenTrees = 10;


	private Vector3 targetTreeDirection;
	private Rigidbody rb;
	private GameObject[] arrayOfTrees;
	private bool canWalk = true;
	public GameController controller;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		StartCoroutine (GetTreePositions ());
		StartCoroutine (FindNewPosition ());
	}

	IEnumerator GetTreePositions(){
		yield return new WaitForSeconds (3);
		arrayOfTrees = controller.GetTreeMarkers ();
		Debug.Log ("THIS IS IT" + arrayOfTrees.Length);
		FindClosetTree ();
	}


	IEnumerator FindNewPosition(){
		while (true) {
			yield return new WaitForSeconds (timeToChangeBetweenTrees);
			FindClosetTree ();
		}
	}

	void FindClosetTree(){ //this method looks through all of the trees on the map and find the closest one to the character and sets that has his target direction
		float closetTree = 1000;

		foreach (GameObject tree in arrayOfTrees) {
			if (Vector3.Distance(transform.position,tree.transform.position) < closetTree) {
				closetTree = Vector3.Distance (transform.position, tree.transform.position);
				targetTreeDirection = tree.transform.position;

				if (Vector3.Distance (transform.position, tree.transform.position) < minDistance) {
					return;
				}
			}
		}
	}
		
	// Update is called once per frame
	void Update () {
	
	}


	void FixedUpdate(){

		if (Vector3.Distance (transform.position, targetTreeDirection) < distanceFromTreeToStop) { //stops character when he is very close to tree
			rb.velocity = Vector3.zero;
			canWalk = false;
		}

		if (Vector3.Distance (transform.position, targetTreeDirection) < distanceFromTreeToSlowDown && canWalk) { //slows down characetr once he is getting close to target tree
			rb.AddForce (-targetTreeDirection * acceleration, ForceMode.VelocityChange);
		}

		if (rb.velocity.magnitude < maxSpeed && canWalk) { 
			rb.AddForce (targetTreeDirection * acceleration, ForceMode.VelocityChange);

		}

		if (rb.velocity.magnitude > maxSpeed && canWalk) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}

			
	}

}
