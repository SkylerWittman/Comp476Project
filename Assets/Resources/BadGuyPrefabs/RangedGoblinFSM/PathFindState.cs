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
	private Vector3 tempMoveTarget;
	private GameController treeController;
	private AStarPathFinding pathFinder;
	private Rigidbody rb;
	private bool noPath = true;
	private bool pathFound = false;
	private bool startPath = false;
	private bool avoidTree = false;
	private float minDistanceFromTrees = 500;
	public float distanceToNewTree = 15.0f;
	private float distanceToAttack = 75;
	private float slowDownRadius = 30;
	private float rotateSpeed = 8.0f;

	public PathFindState(StatePattern pattern){
		rangeGoblin = pattern;
		listOfTreeNodes = rangeGoblin.listOfTreeNodes;
		treeController = rangeGoblin.treeController;
		pathFinder = rangeGoblin.pathFinder;
		rb = rangeGoblin.rb;
	}

	public void UpdateState (){

		CollisionAvoidance ();
		LookForTarget ();

		if (noPath) {
			noPath = false;
			FindPath ();
		} 

		if (pathFound) {
			TraversePath ();
		}

		if (avoidTree) {
			TraverseAvoidenceRoute ();
		}
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

	private void LookForTarget(){
		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.playerTarget) < distanceToAttack) {
			ToAttackState ();
			rangeGoblin.rb.velocity = Vector3.zero;
		}
	}

	private void FindATreeTarget(){
		int rand = Random.Range (0, listOfTreeNodes.Count);

		while (Vector3.Distance (rangeGoblin.transform.position, listOfTreeNodes [rand].getPosition ()) < minDistanceFromTrees) {
			rand = Random.Range (0, listOfTreeNodes.Count-1);
		}

		targetTree = listOfTreeNodes [rand];


	}

	private void FindStartTree(){
		float closestTree = 200;

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
		if (Physics.Raycast (rangeGoblin.transform.position, rangeGoblin.transform.forward, out hit, 10.0f)) {
			if (hit.collider.tag == "TreeMarker") {
				pathFound = false;
				avoidTree = true;
				tempMoveTarget = rangeGoblin.transform.right;
				tempMoveTarget.x += 15.0f;
			}
		}
	}

	private void FindPath(){
		treeController.ResetGCostOfNeighbours ();
		FindATreeTarget ();
		FindStartTree ();
		finalPath = pathFinder.FindPathEuclidean (startTree, targetTree);
		startPath = true;
		pathFound = true;
	}

	private void TraverseAvoidenceRoute(){
		Vector3 moveDirection = (tempMoveTarget - rangeGoblin.transform.position);
		Vector3 steeringDirection = (moveDirection - rb.velocity).normalized;
		moveDirection.Normalize ();
		moveDirection.y = 0.0f;
		steeringDirection.y = 0.0f;

		//Debug.Log ("Avoiding tree");

		Vector3 newRotation = Vector3.RotateTowards (rangeGoblin.transform.forward, moveDirection, rotateSpeed * Time.deltaTime, 0.0f);
		rangeGoblin.transform.rotation = Quaternion.LookRotation (newRotation);


		moveDirection *= acceleration;
		steeringDirection *= acceleration;

		if (rb.velocity.magnitude < maxSpeed) {
			rb.velocity += (steeringDirection + moveDirection);

		}

		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;

		}



		if (Vector3.Distance (rangeGoblin.transform.position, tempMoveTarget) < 5.0f) {
			pathFound = true;
			avoidTree = false;
		}
	}



	private void TraversePath(){
		//apply gravity to enemies
		rb.AddForce (5.0f * Physics.gravity);

		rangeGoblin.anim.Play(rangeGoblin.runClip.name);

		//We'll assign the currentNode to the first node in the path, and goalNode to the last node in the path
		if (startPath) {
			rangeGoblin.currentNode = finalPath [0];
			rangeGoblin.targetNode = finalPath [finalPath.Count - 1];
			startPath = false;
		}

		Vector3 moveDirection = (rangeGoblin.currentNode.getPosition () - rangeGoblin.transform.position);
		Vector3 steeringDirection = (moveDirection - rb.velocity).normalized;
		moveDirection.Normalize ();
		moveDirection.y = 0.0f;
		steeringDirection.y = 0.0f;


		Vector3 newRotation = Vector3.RotateTowards (rangeGoblin.transform.forward, moveDirection, rotateSpeed * Time.deltaTime, 0.0f);
		rangeGoblin.transform.rotation = Quaternion.LookRotation (newRotation);


		moveDirection *= acceleration;
		steeringDirection *= acceleration;

		if (rb.velocity.magnitude < maxSpeed) {
			rb.velocity += (steeringDirection + moveDirection);

		}

		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;

		}



		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.currentNode.getPosition ()) < distanceToNewTree) {

			//If we've reached the goal, then we'll clear our path so we can get another one
			if (rangeGoblin.currentNode.Equals (rangeGoblin.targetNode)) {
				rangeGoblin.anim.Play(rangeGoblin.standClip.name);
				rb.velocity = Vector3.zero;
				finalPath.Clear ();
				ToWaitState ();
				noPath = true;

			}
			//Else, get the next node in the path
			else {
				rangeGoblin.currentNode = finalPath [finalPath.IndexOf (rangeGoblin.currentNode) + 1];
				rb.velocity = Vector3.zero;
				rangeGoblin.anim.Play(rangeGoblin.standClip.name);
				ToWaitState ();
			}
		}
	}
}
