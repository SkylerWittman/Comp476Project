using UnityEngine;
using System.Collections;

public class RangedGoblinBehavior : MonoBehaviour {


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
	private GameObject controller;

    public float rangedGoblinHealth = 300.0f;
    public float rangedGoblinDamage = 6.0f;

    void Start () {
        GetComponent<NPCDetail>().health = rangedGoblinHealth;
        GetComponent<NPCDetail>().damage = rangedGoblinDamage;
        rb = GetComponent<Rigidbody> ();
		controller = GameObject.FindGameObjectWithTag ("controller");
		StartCoroutine (GetTreePositions ());
		StartCoroutine (FindNewPosition ());
	}

	IEnumerator GetTreePositions(){
		yield return new WaitForSeconds (3);
		arrayOfTrees = controller.GetComponent<GameController> ().GetTreeMarkers ();

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
			rb.velocity = rb.velocity.normalized * 0;
			canWalk = false;
		}

		if (Vector3.Distance (transform.position, targetTreeDirection) < distanceFromTreeToSlowDown && canWalk) { //slows down characetr once he is getting close to target tree
			
		}

		if (rb.velocity.magnitude < maxSpeed && canWalk) { 
			
			rb.velocity += rb.velocity + new Vector3 (targetTreeDirection.x, transform.position.y, targetTreeDirection.z);
		}

		if (rb.velocity.magnitude > maxSpeed && canWalk) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
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

}
