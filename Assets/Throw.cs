using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {


	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		rb.AddTorque (-transform.forward * 1000);
	}
}
