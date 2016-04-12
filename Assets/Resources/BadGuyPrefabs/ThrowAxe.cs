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

	// Use this for initialization
	void Start () {
		Invoke ("EnableCollider", .2f);
		rb = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		audio.PlayOneShot (throwSound, 0.4f);
		playerTarget = GameObject.FindGameObjectWithTag ("Player");
		directionToThrow = (new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y + 4.0f , playerTarget.transform.position.z) - this.transform.position).normalized;

		rb.AddForce (directionToThrow * impulseForce, ForceMode.VelocityChange);
		transform.LookAt (playerTarget.transform);
		transform.Rotate (new Vector3 (0, -90, 0));
		Destroy (this.gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		rb.maxAngularVelocity = 30.0f;
		rb.AddTorque (-transform.forward *spinForce);

	}

	void EnableCollider(){
		GetComponent<BoxCollider> ().enabled = true;
	}

	void OnCollisionEnter(Collision coll){

		if (coll.gameObject.tag == "Player") {
			playerTarget.GetComponent<ArcherDetail>().takeDamage(4.0f);
			audio.PlayOneShot (chopSound, 0.4f);
			Destroy (this.gameObject);
        }
	}
}
