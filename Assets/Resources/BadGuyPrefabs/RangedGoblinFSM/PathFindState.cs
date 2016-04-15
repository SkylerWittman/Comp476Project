using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindState : IGoblinState {


	public float acceleration = 7.0f;
	public float maxSpeed = 18.0f;

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
	private bool startPath = false;
	private bool avoidTree = false;
	private float minDistanceFromTrees = 500.0f;
	public float distanceToNewTree = 15.0f;
	private float distanceToAttack = 75.0f;
	private float rotateSpeed = 2.0f;
	private float timer;

	public PathFindState(StatePattern pattern){
		rangeGoblin = pattern;
		listOfTreeNodes = rangeGoblin.listOfTreeNodes;
		treeController = rangeGoblin.treeController;
		pathFinder = rangeGoblin.pathFinder;
		rb = rangeGoblin.rb;
		rangeGoblin.pathFound = false;
	}

	public void UpdateState (){

		CollisionAvoidance ();
		LookForTarget ();

		if (noPath) {
			noPath = false;
			FindPath ();
		} 

		if (rangeGoblin.pathFound) {
			TraversePath ();
		}
	}

	//methods to change states
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

	private void FindATreeTarget(){ //finds a target tree to pathfind too, keeps looking for a tree target until a tree is found that is at least 500 units away
		int rand = Random.Range (0, listOfTreeNodes.Count);

		while (Vector3.Distance (rangeGoblin.transform.position, listOfTreeNodes [rand].getPosition ()) < minDistanceFromTrees) {
			rand = Random.Range (0, listOfTreeNodes.Count-1);
		}

		targetTree = listOfTreeNodes [rand];


	}

	private void FindStartTree(){ //finds the closest tree to the goblin to be used as the start node
		float closestTree = 200;

		foreach (TreeNode node in listOfTreeNodes) {
			if(Vector3.Distance(rangeGoblin.transform.position, node.getPosition()) < closestTree){
				closestTree = Vector3.Distance (rangeGoblin.transform.position, node.getPosition ());
				startTree = node;

			}
		}
	}


	private void CollisionAvoidance(){ //casts a ray forward looking for collisions with tree, if a collision is found a force is applied in the oppisite direction of the characters transform forward
		//as well as in the direction of the characters transform right, this slows him down and moves him out of the way of the tree
		RaycastHit hit;

		if (Physics.Raycast (rangeGoblin.transform.position + Vector3.up, rangeGoblin.transform.forward, out hit, 40.0f)) {
			if (hit.collider.tag == "TreeMarker") {
				rangeGoblin.pathFound = false; //stops his movement toward the next node in his path so he can avoid the tree properly
				rangeGoblin.StartCoroutine (rangeGoblin.ChangePathFound ()); //allows him to move to his next node after 2 seconds

				//apply gravity to enemies
				rb.AddForce (5.0f * Physics.gravity);

				rangeGoblin.anim.Play(rangeGoblin.runClip.name);
				rb.AddForce (-rangeGoblin.transform.forward * 15.0f);
					
				if (rb.velocity.magnitude <= maxSpeed) {
					rb.AddForce (rangeGoblin.transform.right * acceleration, ForceMode.VelocityChange);
				}

				if (rb.velocity.magnitude > maxSpeed) {
					rb.velocity = rb.velocity.normalized* maxSpeed;
				}

				Quaternion rotation = Quaternion.LookRotation(rangeGoblin.transform.right);
				rangeGoblin.transform.rotation = Quaternion.Slerp(rangeGoblin.transform.rotation, rotation, Time.deltaTime * rotateSpeed);
			}
		}
	}

	private void FindPath(){ //finds start and target node and uses a star to find the best possible route
		treeController.ResetGCostOfNeighbours ();
		FindATreeTarget ();
		FindStartTree ();
		finalPath = pathFinder.FindPathEuclidean (startTree, targetTree);
		startPath = true;
		rangeGoblin.pathFound = true;
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

		Vector3 moveDirection = (rangeGoblin.currentNode.getPosition () - rangeGoblin.transform.position).normalized;
		Vector3 steeringDirection = (moveDirection - rb.velocity).normalized;
		moveDirection.y = 0.0f;
		steeringDirection.y = 0.0f;


		Vector3 newRotation = Vector3.RotateTowards (rangeGoblin.transform.forward, moveDirection, rotateSpeed * Time.deltaTime, 0.0f);
		rangeGoblin.transform.rotation = Quaternion.LookRotation (newRotation);


		if (rb.velocity.magnitude <= maxSpeed) {
			rb.AddForce (moveDirection * acceleration, ForceMode.VelocityChange);
		}

		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized* maxSpeed;
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
