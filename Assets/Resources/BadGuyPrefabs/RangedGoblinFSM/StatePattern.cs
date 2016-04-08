using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatePattern : MonoBehaviour {

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


	public GameObject throwingAxe;
	public GameObject spear;
	public GameObject throwPosition;


	private GameObject controller;
	private GameObject player;
	private bool gameStart = false;

	// Use this for initialization
	void Start () {
		Random.seed = System.DateTime.Now.Millisecond;
		attackState = new AttackState (this);
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



	IEnumerator GetTreePositions(){
		yield return new WaitForSeconds (4.0f);
		treeController = controller.GetComponent<GameController>();
		listOfTreeNodes = treeController.GetTreeNodes ();

		pathFindState = new PathFindState (this);
		currentState = pathFindState;
		gameStart = true;
	}

	public void ThrowAxe(){
		StartCoroutine (ThrowDelayAxe ());
	}

	public void ThrowSpear(){
		StartCoroutine (ThrowDelaySpear ());
	}

	public void startWait(){
		StartCoroutine(WaitToMove());
	}

	public IEnumerator WaitToMove(){
		yield return new WaitForSeconds (2.0f);
		canChangeState = true;
	}

	public IEnumerator ThrowDelaySpear(){
		yield return new WaitForSeconds (.2f);
		Instantiate (spear, throwPosition.transform.position, Quaternion.identity);
	}

	public IEnumerator ThrowDelayAxe(){
		yield return new WaitForSeconds (.2f);
		Instantiate (throwingAxe, throwPosition.transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
	
		playerTarget = player.transform.position;

		if (gameStart) {
			currentState.UpdateState ();
		}
	}
}
