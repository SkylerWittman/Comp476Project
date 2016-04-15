using UnityEngine;
using System.Collections;

public class ThrowAxe : MonoBehaviour {

	private Rigidbody rb;
	public float spinForce = 100.0f;
	public float impulseForce = 75.0f;
	private GameObject playerTarget;
	private Vector3 directionToThrow;
	private Vector3 throwRotation;
	private AudioSource audio;
	public AudioClip throwSound;
	public AudioClip chopSound;
	public float axeDamage = 4.0f;

	// Use this for initialization
	void Start () {
		Invoke ("EnableCollider", .2f); //collider is enable shortly affter its thrown so it doesnt collide with the goblin throwing it
		rb = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		audio.PlayOneShot (throwSound, 0.4f);
		playerTarget = GameObject.FindGameObjectWithTag ("Player"); //when axe is instaniated it gets the direction to the player and a force is applied in this direction
		// this allows the goblins to be not overly accurate but still somewhat accurate. 
		directionToThrow = (new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y + 4.0f , playerTarget.transform.position.z) - this.transform.position).normalized;

		rb.AddForce (directionToThrow * impulseForce, ForceMode.VelocityChange);
		transform.LookAt (playerTarget.transform); //makes sure oreintation is correct
		transform.Rotate (new Vector3 (0, -90, 0));
		Destroy (this.gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		rb.maxAngularVelocity = 30.0f; //gives the axe a spinning effect
		rb.AddTorque (-transform.forward *spinForce);

	}

	void EnableCollider(){
		GetComponent<BoxCollider> ().enabled = true;
	}

	void OnCollisionEnter(Collision coll){

		if (coll.gameObject.tag == "Player") {
			playerTarget.GetComponent<ArcherDetail>().takeDamage(axeDamage);
			Debug.Log ("Axe throwing goblin did " + axeDamage + " damage");
			audio.PlayOneShot (chopSound, 1.0f);
			Destroy (this.gameObject);
        }
	}
}
