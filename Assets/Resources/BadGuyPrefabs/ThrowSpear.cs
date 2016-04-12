using UnityEngine;
using System.Collections;

public class ThrowSpear : MonoBehaviour {

	private Rigidbody rb;
	public float spinForce = 100.0f;
	public float impulseForce = 75.0f;
	private GameObject playerTarget;
	private Vector3 directionToThrow;
	private Vector3 throwRotation;
	private AudioSource audio;
	public AudioClip throwSound;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		audio.PlayOneShot (throwSound, 0.4f);
		playerTarget = GameObject.FindGameObjectWithTag ("Player");
		directionToThrow = (new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y + 3.0f , playerTarget.transform.position.z) - this.transform.position).normalized;

		rb.AddForce (directionToThrow * impulseForce, ForceMode.VelocityChange);
		transform.LookAt (playerTarget.transform);
		transform.Rotate (new Vector3 (90, 0, 0));
		Destroy (this.gameObject, 3.0f);
	}

	void OnCollisionEnter(Collision coll){

		if (coll.gameObject.tag == "Player") {
			playerTarget.GetComponent<ArcherDetail>().takeDamage(4.0f);
			Destroy (this.gameObject);
        }
	}
}
