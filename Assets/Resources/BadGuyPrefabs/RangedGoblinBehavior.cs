using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedGoblinBehavior : MonoBehaviour {


	public float acceleration = 10;
	public float maxSpeed = 25;
	public int minDistance = 10;
	public int distanceFromTreeToStop = 3;
	public int distanceFromTreeToSlowDown = 8;
	public float timeToChangeBetweenTrees = 10;

	private Vector3 targetTreeDirection;
	private Rigidbody rb;
	private AStarPathFinding pathFinder;
	private List<TreeNode> listOfTreeNodes = new List<TreeNode>();
	private List<TreeNode> myTreeNodes = new List<TreeNode>();
	private List<TreeNode> finalPath = new List<TreeNode>();
	private TreeNode startTree;
	private TreeNode targetTree;
	private TreeNode currentNode;
	private TreeNode targetNode;
	private bool canTraversePath = false;
	private bool pathFound = false;
	private bool startPath = false;
	private GameObject controller;
	private GameController treeController;
	private int pathCount = 0;
	private float minDistanceFromTrees = 500;
    public float rangedGoblinHealth = 300.0f;
    public float rangedGoblinDamage = 6.0f;
	public float distanceToNewTree = 25.0f;
	private Animation anim;
	private AnimationClip runClip;
	private AnimationClip standClip;

    void Start () {
        GetComponent<NPCDetail>().health = rangedGoblinHealth;
        GetComponent<NPCDetail>().damage = rangedGoblinDamage;

		anim = GetComponent<Animation>();
		runClip = anim.GetClip("run");
		standClip = anim.GetClip ("stand");
        rb = GetComponent<Rigidbody> ();
		pathFinder = GetComponent<AStarPathFinding> ();
		controller = GameObject.FindGameObjectWithTag ("controller");
		StartCoroutine (GetTreePositions ());
		int time = Random.Range (5, 8);
		Invoke ("SetCanTraversePath", time);
		InvokeRepeating ("CollisionAvoidance", 8.0f, 2.0f);

	}

	IEnumerator GetTreePositions(){
		yield return new WaitForSeconds (4.0f);
		treeController = controller.GetComponent<GameController>();
		listOfTreeNodes = treeController.GetTreeNodes ();

	}


	private void SetCanTraversePath(){
		canTraversePath = true;
	}

	private void FindATreeTarget(){
		int rand = Random.Range (0, listOfTreeNodes.Count);

		while (Vector3.Distance (transform.position, listOfTreeNodes [rand].getPosition ()) < minDistanceFromTrees) {
			 rand = Random.Range (0, listOfTreeNodes.Count-1);
		}

		targetTree = listOfTreeNodes [rand];
		//Debug.Log (targetTree.getPosition () + " target position");
	
	}

	private void FindStartTree(){
		float closestTree = 100;

		foreach (TreeNode node in listOfTreeNodes) {
			if(Vector3.Distance(transform.position, node.getPosition()) < closestTree){
				closestTree = Vector3.Distance (transform.position, node.getPosition ());
				startTree = node;
				//Debug.Log (startTree.getPosition () + " start position");
			}
		}
	}

	private void FindPath(){
		treeController.ResetGCostOfNeighbours ();
		canTraversePath = false;
		FindATreeTarget ();
		FindStartTree ();
		finalPath = pathFinder.FindPathEuclidean (startTree, targetTree);
		pathFound = true;
		startPath = true;
		//Debug.Log ("Size of final path" + finalPath.Count);

	}


	IEnumerator ScanArea(){
		anim.Play(standClip.name);
		int rand = Random.Range (2, 5);
		yield return new WaitForSeconds (rand);
		canTraversePath = true;


	}

	private void CollisionAvoidance(){
		RaycastHit hit;
		Vector3 avoidanceVector;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 50)) {
			if (hit.collider.tag == "TreeMarker") {
				//Debug.Log ("HIT TREE");
				avoidanceVector = (hit.collider.transform.position - rb.velocity).normalized;
				avoidanceVector *= 30;
				rb.AddForce (avoidanceVector, ForceMode.VelocityChange);
			}
		}
	}
	// Update is called once per frame


	void FixedUpdate(){



		if (canTraversePath) {
			FindPath ();
			//Debug.Log ("FindingPath");
		}

		if (finalPath == null) {
			//Debug.Log ("NULL");
		}

		if (pathFound) {


			//apply gravity to enemies
			rb.AddForce (Vector3.down * rb.mass * 30);


			anim.Play(runClip.name);

			//We'll assign the currentNode to the first node in the path, and goalNode to the last node in the path
			if (startPath) {
				currentNode = finalPath [0];
				targetNode = finalPath [finalPath.Count - 1];
				startPath = false;
			}

			Vector3 moveDirection = (currentNode.getPosition () - transform.position).normalized;
			Vector3 steeringDirection = (moveDirection - rb.velocity).normalized;
			moveDirection.y = 0.0f;
			steeringDirection.y = 0.0f;

			Quaternion newRotation = Quaternion.LookRotation (moveDirection);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, newRotation, 8.0f);


			if (rb.velocity.magnitude > maxSpeed) {
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}

			if (rb.velocity.magnitude < maxSpeed) {
				rb.velocity += (steeringDirection + moveDirection) * acceleration;

			}

			if (Vector3.Distance (this.transform.position, currentNode.getPosition ()) < distanceToNewTree) {

				//If we've reached the goal, then we'll clear our path so we can get another one
				if (currentNode.Equals (targetNode)) {
					rb.velocity = Vector3.zero;
					pathFound = false;
					finalPath.Clear ();
					StartCoroutine (ScanArea ());
				}
				//Else, get the next node in the path
				else {
					currentNode = finalPath [finalPath.IndexOf (currentNode) + 1];
				}
			}

		}
	}




	}

    //IEnumerator attack()
    //{
        //canAttack = false;
        //only works if there is a single player
        //GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>().takeDamage(rangedGoblinDamage);
        //anim.Play(attackClip.name);
        //yield return new WaitForSeconds(dinoAttackCooldown);
        //canAttack = true;
    //}


