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
	public AudioClip chopSound;
	public float spearDamage = 4.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		audio.PlayOneShot (throwSound, 0.4f);
		playerTarget = GameObject.FindGameObjectWithTag ("Player"); //when spear is instaniated it gets the direction to the player and a force is applied in this direction
		// this allows the goblins to be not overly accurate but still somewhat accurate. 
		directionToThrow = (new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y + 3.0f , playerTarget.transform.position.z) - this.transform.position).normalized;

		rb.AddForce (directionToThrow * impulseForce, ForceMode.VelocityChange);
		transform.LookAt (playerTarget.transform); //makes sure the rotation and orientation of the spear is correct
		transform.Rotate (new Vector3 (90, 0, 0));
		Destroy (this.gameObject, 3.0f); 
	}

	void OnCollisionEnter(Collision coll){

		if (coll.gameObject.tag == "Player") {
			playerTarget.GetComponent<ArcherDetail>().takeDamage(spearDamage);
			audio.PlayOneShot (chopSound, 1.0f);
			Destroy (this.gameObject);
        }
	}
}
