using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GoblinBehavior : MonoBehaviour {


	public float acceleration = 7;
	public float maxSpeed = 12;
	public int minDistance = 10;
	public int distanceFromTreeToStop = 3;
	public int distanceFromTreeToSlowDown = 8;
	public float timeToChangeBetweenTrees = 10;
	public AStarPathFinding findPath;
	public float maxNodeDistance = 300;

	private Node startNode;
	private Node targetNode;
	private Rigidbody rb;
	private List<Node> nodeList = new List<Node>();
	private List<Node> pathToTraverse = new List<Node>();
	private List<Node> nearList = new List<Node>();
	private bool canWalk = true;
	private GameObject controller;
	private bool finishedPath;
	private int pathCount;
	private Vector3 pathDirection;
	private Vector3 steering;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		controller = GameObject.FindGameObjectWithTag ("controller");
		StartCoroutine (GetNodeList ());

	}

	IEnumerator GetNodeList(){
		yield return new WaitForSeconds (3);
		nodeList = controller.GetComponent<NodeCreation> ().allNodeObjects;
		FindNewPosition ();
	}


	void FindNewPosition(){
			pathCount = 0;
			FindClosestNode ();
			FindTargetNode ();
			pathToTraverse = findPath.FindPathEuclidean (startNode, targetNode);
			finishedPath = false;

	}

	void FindClosestNode(){ //this method looks through all of the trees on the map and find the closest one to the character and sets that has his target direction
		float closetTree = 100;

		foreach (Node n in nodeList) {
			if (Vector3.Distance(transform.position,n.getPosition()) < closetTree) {
				closetTree = Vector3.Distance (transform.position, n.getPosition());
				startNode = n;

			}
		}
		Debug.Log ("closet node is: " + startNode.getPosition ());
	}
		
	void FindTargetNode(){ //this method looks through all of the trees on the map and find the closest one to the character and sets that has his target directio


		foreach (Node n in nodeList) {
			if (Vector3.Distance (transform.position, n.getPosition ()) < maxNodeDistance && Vector3.Distance (transform.position, n.getPosition ()) > 50 ) {
				targetNode = n;
				Debug.Log ("target node is: " + targetNode.getPosition ());
				return;
			}
				
		}
	}



	// Update is called once per frame
	void Update () {

	}


	void FixedUpdate(){

		Debug.Log ("length" + pathToTraverse.Count);

		if (finishedPath == false) {
			pathDirection = (pathToTraverse [pathCount].getPosition () - transform.position).normalized;
			steering = (pathDirection - rb.velocity) * 3;
			rb.velocity += (pathDirection + steering);
			//transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (pathDirection), 5);
		}

		if (Vector3.Distance (transform.position, pathToTraverse [pathCount].getPosition()) < distanceFromTreeToStop) {
			pathCount += 1;
		}

		if (pathCount >= pathToTraverse.Count) {
			finishedPath = true;
			FindNewPosition ();
		}
	}

}
