using UnityEngine;
using System.Collections;

public class TreeGoblinBehavior : MonoBehaviour {

	private GameObject target;
	private Rigidbody rb;
	private Vector3 directionToMove;
	private Vector3 steering;
	private Animation anim;
	private AnimationClip runClip;
	private AnimationClip attackClip;
	private AnimationClip fallClip;
	private AnimationClip getUpClip;
	private bool canAttack = true;
	private bool canChaseTarget = false;

	public float maxSpeed = 6;
	public float acceleration = 3;
	public float attackDistance = 3;
	public float impulse = 50;

    public float treeGoblinHealth = 350.0f;
    public float treeGoblinDamage = 10.0f;

    void Start () {

        GetComponent<NPCDetail>().health = treeGoblinHealth;
        GetComponent<NPCDetail>().damage = treeGoblinDamage;

        target = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animation>();
		StartCoroutine (chaseTarget ());

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


	IEnumerator chaseTarget()
	{
		yield return new WaitForSeconds (3.0f);
		anim.Play(getUpClip.name);
		yield return new WaitForSeconds (1.5f);
		canChaseTarget = true;
	}


	IEnumerator attack()
	{
		canAttack = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>().takeDamage(treeGoblinDamage);
        Debug.Log("Tree goblin did " + treeGoblinDamage + " damage");
        Debug.Log("Strike player with goblin appendage");
		anim.Play(attackClip.name);
		yield return new WaitForSeconds(2.0f);
		canAttack = true;
	}
	
	// Update is called once per frame

	void Update(){
		if (Vector3.Distance (target.transform.position, this.transform.position) < attackDistance) {
			StartCoroutine (attack ());
		}

	}


	void FixedUpdate () {

		if (canChaseTarget) {

			anim.Play(runClip.name);

			//apply gravity to enemies
			rb.AddForce (Vector3.down * rb.mass * 30);

			//find direction towards target and look towards target
			directionToMove = (target.transform.position - this.transform.position).normalized;
			Quaternion newRotation = Quaternion.LookRotation (directionToMove);
			newRotation.x = 0.0f;
			newRotation.z = 0.0f;
			transform.rotation = Quaternion.RotateTowards (transform.rotation, newRotation, 15.0f);


			//addforce in direction of character
			if (rb.velocity.magnitude < maxSpeed) {
				rb.AddForce ((directionToMove) * acceleration, ForceMode.VelocityChange);
			}

			if (rb.velocity.magnitude > maxSpeed) {
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}
		}

	}
}
