using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindState : IGoblinState {


	public float acceleration = 8;
	public float maxSpeed = 15;

	private readonly StatePattern rangeGoblin;
	private List<TreeNode> listOfTreeNodes = new List<TreeNode>();
	private List<TreeNode> finalPath = new List<TreeNode>();
	private TreeNode startTree;
	private TreeNode targetTree;

	private GameController treeController;
	private AStarPathFinding pathFinder;
	private Rigidbody rb;
	private bool noPath = true;
	private bool pathFound = false;
	private bool startPath = false;
	private float minDistanceFromTrees = 500;
	public float distanceToNewTree = 25.0f;

	public PathFindState(StatePattern pattern){
		rangeGoblin = pattern;
		listOfTreeNodes = rangeGoblin.listOfTreeNodes;
		treeController = rangeGoblin.treeController;
		pathFinder = rangeGoblin.pathFinder;
		rb = rangeGoblin.rb;
	}

	public void UpdateState (){
		if (noPath) {
			noPath = false;
			FindPath ();
		} 

		if (pathFound) {
			TraversePath ();
		}

		CollisionAvoidance ();
	}
		

	public void ToAttackState(){
		rangeGoblin.currentState = rangeGoblin.attackState;
	}

	public void ToWaitState(){
		rangeGoblin.currentState = rangeGoblin.waitState;
	}
		
	public void ToPathFindingState(){
		Debug.Log ("current state");
	}


	private void FindATreeTarget(){
		int rand = Random.Range (0, listOfTreeNodes.Count);

		while (Vector3.Distance (rangeGoblin.transform.position, listOfTreeNodes [rand].getPosition ()) < minDistanceFromTrees) {
			rand = Random.Range (0, listOfTreeNodes.Count-1);
		}

		targetTree = listOfTreeNodes [rand];


	}

	private void FindStartTree(){
		float closestTree = 100;

		foreach (TreeNode node in listOfTreeNodes) {
			if(Vector3.Distance(rangeGoblin.transform.position, node.getPosition()) < closestTree){
				closestTree = Vector3.Distance (rangeGoblin.transform.position, node.getPosition ());
				startTree = node;

			}
		}
	}

	private void CollisionAvoidance(){
		RaycastHit hit;
		Vector3 avoidanceVector;
		if (Physics.Raycast (rangeGoblin.transform.position, rangeGoblin.transform.forward, out hit, 10)) {
			if (hit.collider.tag == "TreeMarker") {
				Debug.Log ("HIT TREE");

				rb.AddForce (rangeGoblin.transform.right * 20, ForceMode.VelocityChange);
			}
		}
	}

	private void FindPath(){
		treeController.ResetGCostOfNeighbours ();
		FindATreeTarget ();
		FindStartTree ();
		finalPath = pathFinder.FindPathEuclidean (startTree, targetTree);
		startPath = true;
		Debug.Log ("Size of final path" + finalPath.Count);
		pathFound = true;
	}

	private void TraversePath(){
		//apply gravity to enemies
		rb.AddForce (Vector3.down * rb.mass * 30);
	
		rangeGoblin.anim.Play(rangeGoblin.runClip.name);

		//We'll assign the currentNode to the first node in the path, and goalNode to the last node in the path
		if (startPath) {
			rangeGoblin.currentNode = finalPath [0];
			rangeGoblin.targetNode = finalPath [finalPath.Count - 1];
			startPath = false;
		}

		Vector3 moveDirection = (rangeGoblin.currentNode.getPosition () - rangeGoblin.transform.position).normalized;
		Vector3 steeringDirection = (moveDirection - rb.velocity).normalized;
		moveDirection.y = 0.0f;
		steeringDirection.y = 0.0f;

		Quaternion newRotation = Quaternion.LookRotation (moveDirection);
		newRotation.x = 0.0f;
		newRotation.z = 0.0f;
		rangeGoblin.transform.rotation = Quaternion.RotateTowards (rangeGoblin.transform.rotation, newRotation, 15.0f);

		moveDirection *= acceleration;

		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;

		}

		if (rb.velocity.magnitude < maxSpeed) {
			rb.velocity += (steeringDirection + moveDirection);

		}

		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.currentNode.getPosition ()) < distanceToNewTree) {

			//If we've reached the goal, then we'll clear our path so we can get another one
			if (rangeGoblin.currentNode.Equals (rangeGoblin.targetNode)) {
				Debug.Log ("At end of path, now waiting");
				rangeGoblin.anim.Play(rangeGoblin.standClip.name);
				rb.velocity = Vector3.zero;
				noPath = true;
				finalPath.Clear ();
				ToWaitState ();

			}
			//Else, get the next node in the path
			else {
				rangeGoblin.currentNode = finalPath [finalPath.IndexOf (rangeGoblin.currentNode) + 1];
				Debug.Log ("At next position of path, now waiting");
				rb.velocity = Vector3.zero;
				rangeGoblin.anim.Play(rangeGoblin.standClip.name);
				ToWaitState ();
			}
		}
	}
}
