using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatePattern : MonoBehaviour {

	//these must all be public so each non mono behavior state class has access to the prefab and its mono behavior features
	[HideInInspector] public IGoblinState currentState;
	[HideInInspector] public AttackState attackState;
	[HideInInspector] public WaitState waitState;
	[HideInInspector] public PathFindState pathFindState;
	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public AStarPathFinding pathFinder;
	[HideInInspector] public Vector3 playerTarget;
	[HideInInspector] public GameController treeController;
	[HideInInspector] public Animation anim;
	[HideInInspector] public AnimationClip runClip;
	[HideInInspector] public AnimationClip standClip;
	[HideInInspector] public AnimationClip throwClip;
	[HideInInspector] public bool canChangeState = false;
	[HideInInspector] public List<TreeNode> listOfTreeNodes = new List<TreeNode>();
	[HideInInspector] public TreeNode currentNode;
	[HideInInspector] public TreeNode targetNode;
	[HideInInspector] public bool pathFound;

	public GameObject throwingAxe;
	public GameObject spear;
	public GameObject throwPosition;


	private GameObject controller;
	private GameObject player;
	private bool gameStart = false;

	// Use this for initialization
	void Start () {
		Random.seed = System.DateTime.Now.Millisecond;
		attackState = new AttackState (this); //creates new state objects
		waitState = new WaitState (this);
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animation>();
		anim ["attack01"].speed = 3.0f;
		runClip = anim.GetClip("run");
		standClip = anim.GetClip ("stand");
		throwClip = anim.GetClip ("attack01");
		rb = GetComponent<Rigidbody> ();
		pathFinder = GetComponent<AStarPathFinding> ();
		controller = GameObject.FindGameObjectWithTag ("controller");
		StartCoroutine (GetTreePositions ());
	}

	public IEnumerator ChangePathFound(){
		yield return new WaitForSeconds (2.0f);
		pathFound = true;
	}

	IEnumerator GetTreePositions(){ //finds all the positions of the tree nodes on the map after they have been generated
		yield return new WaitForSeconds (3.0f);
		treeController = controller.GetComponent<GameController>();
		listOfTreeNodes = treeController.GetTreeNodes ();

		pathFindState = new PathFindState (this); //starts the goblin in path finding state
		currentState = pathFindState;
		gameStart = true;
	}

	public void ThrowAxe(){ //methods used to throw weapons
		StartCoroutine (ThrowDelayAxe ());
	}

	public void ThrowSpear(){
		StartCoroutine (ThrowDelaySpear ());
	}

	public void startWait(){
		StartCoroutine(WaitToMove());
	}

	public IEnumerator WaitToMove(){ //allows the goblin to begin pathfinding again after he has waited at the goal tree for 2 seconds
		yield return new WaitForSeconds (2.0f);
		canChangeState = true;
	}

	public IEnumerator ThrowDelaySpear(){
		yield return new WaitForSeconds (.2f); //delays the throw so it lines up with the animation
		Instantiate (spear, throwPosition.transform.position, Quaternion.identity);
	}

	public IEnumerator ThrowDelayAxe(){
		yield return new WaitForSeconds (.2f);
		Instantiate (throwingAxe, throwPosition.transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void FixedUpdate () {
	
		playerTarget = player.transform.position;

		if (gameStart) {
			currentState.UpdateState (); //used to update the state of the goblin
		}
	}
}
