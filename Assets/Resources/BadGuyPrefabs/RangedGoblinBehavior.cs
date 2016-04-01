using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedGoblinBehavior : MonoBehaviour {


	public float acceleration = 7;
	public float maxSpeed = 12;
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
	private bool canSearchForNewPath = true;
	private GameObject controller;
	private GameController treeController;
	private int pathCount = 0;
	private float radiusOfTrees = 70;
    public float rangedGoblinHealth = 300.0f;
    public float rangedGoblinDamage = 6.0f;

    void Start () {
        GetComponent<NPCDetail>().health = rangedGoblinHealth;
        GetComponent<NPCDetail>().damage = rangedGoblinDamage;
        rb = GetComponent<Rigidbody> ();
		pathFinder = GetComponent<AStarPathFinding> ();
		controller = GameObject.FindGameObjectWithTag ("controller");
		StartCoroutine (GetTreePositions ());
		int time = Random.Range (4, 8);
		Invoke ("SetCanTraversePath", time);
		InvokeRepeating ("CollisionAvoidance", 8.0f, 2.0f);

	}

	IEnumerator GetTreePositions(){
		yield return new WaitForSeconds (3);
		treeController = controller.GetComponent<GameController>();
		listOfTreeNodes = treeController.GetTreeNodes ();
		FindTreesInMyArea();

	}


	private void FindTreesInMyArea(){ //this method looks through all of the trees on the map and find the closest one to the character and sets that has his target direction
		
		foreach (TreeNode tree in listOfTreeNodes) {
			if (Vector3.Distance(transform.position,tree.getPosition()) < radiusOfTrees) {
				myTreeNodes.Add (tree);
			}
		}

		Debug.Log ("Size of list of trees in my area " + myTreeNodes.Count);
	}
		


	private void SetCanTraversePath(){
		canTraversePath = true;
	}

	private void FindATreeTarget(){
		int rand = Random.Range (0, myTreeNodes.Count);
		targetTree = myTreeNodes [rand];
	
	}

	private void FindStartNode(){
		float closestTree = 200;

		foreach (TreeNode node in myTreeNodes) {
			if(Vector3.Distance(transform.position, node.getPosition()) < closestTree){
				closestTree = Vector3.Distance (transform.position, node.getPosition ());
				startTree = node;

			}
		}
	}

	private void FindPath(){
		
		FindATreeTarget ();
		FindStartNode ();
		finalPath = pathFinder.FindPathEuclidean (startTree, targetTree);


		if (finalPath.Count >= 1) {
			canTraversePath = false;
			Debug.Log ("final paht count is:" + finalPath.Count);
		}

	}


	IEnumerator ScanArea(){
		int rand = Random.Range (2, 5);
		yield return new WaitForSeconds (rand);
		canTraversePath = true;


	}

	private void CollisionAvoidance(){
		RaycastHit hit;
		Vector3 avoidanceVector;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 50)) {
			if (hit.collider.tag == "TreeMarker") {
				Debug.Log ("HIT TREE");
				avoidanceVector = (hit.collider.transform.position - rb.velocity).normalized;
				avoidanceVector *= 30;
				rb.AddForce (avoidanceVector, ForceMode.VelocityChange);
			}
		}
	}
	// Update is called once per frame


	void Update(){

		//apply gravity to enemies
		rb.AddForce (Vector3.down * rb.mass * 30);

		if (canTraversePath) {
			FindPath ();
		}

		if (finalPath.Count > 0 ) {

			//We'll assign the currentNode to the first node in the path, and goalNode to the last node in the path
			currentNode = finalPath [0];
			targetNode = finalPath [finalPath.Count - 1];

			Vector3 moveDirection = (currentNode.getPosition () - transform.position).normalized;
			//Vector3 steeringDirection = (moveDirection - rb.velocity)*acceleration;

			Quaternion newRotation = Quaternion.LookRotation (moveDirection);
			newRotation.x = 0.0f;
			newRotation.z = 0.0f;
			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * 10.0f);

			if (rb.velocity.magnitude < maxSpeed) {
				rb.velocity += (moveDirection) * acceleration;

			}


			if (rb.velocity.magnitude > maxSpeed) {
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}

			if (Vector3.Distance (this.transform.position, currentNode.getPosition ()) < 5.0f) {

				//If we've reached the goal, then we'll clear our path so we can get another one
				if (currentNode.Equals (targetNode)) {
					finalPath.Clear ();
					Debug.Log ("they are equal");
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


