using UnityEngine;
using System.Collections;

public class ThrowAxe : MonoBehaviour {

	private Rigidbody rb;
	public float spinForce = 100.0f;
	public float impulseForce = 50.0f;
	private GameObject playerTarget;
	private Vector3 directionToThrow;
	private AudioSource audio;
	public AudioClip throwSound;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		audio.PlayOneShot (throwSound);
		playerTarget = GameObject.FindGameObjectWithTag ("Player");
		directionToThrow = (playerTarget.transform.position - this.transform.position).normalized;
		rb.AddForce (directionToThrow * impulseForce, ForceMode.VelocityChange);
		transform.LookAt (playerTarget.transform);
	}
	
	// Update is called once per frame
	void Update () {
		rb.maxAngularVelocity = 25.0f;
		rb.AddTorque (-transform.forward *spinForce);

	}
}
