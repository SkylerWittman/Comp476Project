using UnityEngine;
using System.Collections;

public class TreeGoblinBehavior : MonoBehaviour {

	private GameObject target;
	private Rigidbody rb;
	private Rigidbody targetRb;
	private Vector3 directionToMove;
	private Animation anim;
	private AnimationClip runClip;
	private AnimationClip attackClip;
	private AnimationClip fallClip;
	private AnimationClip getUpClip;
	private bool canAttack = true;
	private bool canChaseTarget = false;
	private bool gameStart = false;
	private float rotateSpeed = 2.0f;
	private float maxSpeed = 15.0f;
	private float acceleration = 3.0f;
	private float attackDistance = 4.0f;
	private float impulse = 50;

	float treeGoblinHealth;
	float treeGoblinDamage;

	void Start () {

		treeGoblinHealth = GetComponent<NPCDetail> ().health;
		treeGoblinDamage = GetComponent<NPCDetail> ().damage;


		target = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody> ();
		StartCoroutine (EnableCollider ());
		StartCoroutine (chaseTarget ());
		targetRb = target.GetComponent<Rigidbody> ();
		anim = GetComponent<Animation>();

		//Changing layers and adding weights allows us to play 2 animations at the same time
		anim["attack02"].layer = 1;
		anim["attack02"].weight = 0.7f;
		runClip = anim.GetClip("run");
		attackClip = anim.GetClip("attack02");
		fallClip = anim.GetClip ("drop down");
		getUpClip = anim.GetClip ("sit up");

		rb.AddForce (Vector3.down * impulse, ForceMode.Acceleration);
		anim.Play(fallClip.name);

	}

	IEnumerator EnableCollider()
	{
		yield return new WaitForSeconds (0.5f);
		GetComponent<Collider> ().enabled = true;
	}


	IEnumerator chaseTarget()
	{
		yield return new WaitForSeconds (3.0f);
		anim.Play(getUpClip.name);
		yield return new WaitForSeconds (1.5f);
		canChaseTarget = true;
		gameStart = true;
	}


	IEnumerator attack()
	{
		canAttack = false;
		GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>().takeDamage(treeGoblinDamage);
		Debug.Log("Tree goblin did " + treeGoblinDamage + " damage");
		anim.Play(attackClip.name);
		yield return new WaitForSeconds(2.0f);
		canAttack = true;
	}

	// Update is called once per frame

	void Update(){
		if (Vector3.Distance (target.transform.position, this.transform.position) < attackDistance && canAttack) {
			canChaseTarget = false;
			StartCoroutine (attack ());

		}

		if (Vector3.Distance (target.transform.position, this.transform.position) > attackDistance && gameStart) {
			canChaseTarget = true;
		}
	}


	void FixedUpdate () {

		if (canChaseTarget) {

			anim.Play(runClip.name);

			//apply gravity to enemies
			rb.AddForce (5.0f * Physics.gravity);

			//find direction towards target and look towards target
			directionToMove = (target.transform.position - this.transform.position).normalized;

			Vector3 newRotation = Vector3.RotateTowards (transform.forward, directionToMove, rotateSpeed * Time.deltaTime, 0.0f);
			newRotation.y = 0.0f;
			transform.rotation = Quaternion.LookRotation (newRotation);

			directionToMove.y = 0.0f;

			//addforce in direction of character
			if (rb.velocity.magnitude <= maxSpeed) {
				rb.AddForce ((directionToMove ) * acceleration, ForceMode.VelocityChange);

			}

			if (rb.velocity.magnitude > maxSpeed) {
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}
		}

	}
}
