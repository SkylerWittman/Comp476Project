using UnityEngine;
using System.Collections;

public class DinoBehavior : MonoBehaviour {

	public float acceleration = 4;
	public float maxSpeed = 10;
	public float directionChangeTime = 3;
	public float rotateTime = 1.5f;
	public float maxDirectionChange = 60;

	private Rigidbody rb;
	private Vector3 newDirection;
	private float directionToHeadTo;
	private Transform player;


	void Start () {
		rb = GetComponent<Rigidbody> ();
		directionToHeadTo = Random.Range (0, 360);
		transform.eulerAngles = new Vector3 (0, directionToHeadTo, 0);
		StartCoroutine (FaceNewDirection ());
	}




	IEnumerator FaceNewDirection(){
		while (true) {
			GetNewDirection ();
			yield return new WaitForSeconds (directionChangeTime);
		}

	}

	void GetNewDirection(){
		float floor = Mathf.Clamp (directionToHeadTo - maxDirectionChange, 0, 360);
		float ceiling = Mathf.Clamp (directionToHeadTo + maxDirectionChange, 0, 360);
		directionToHeadTo = Random.Range (floor, ceiling);
		newDirection = new Vector3 (0, directionToHeadTo, 0);
	}

	// Update is called once per frame
	void Update () {
		
		//call method to attack player if player is in sight and closeby
	}

	void FixedUpdate(){
		transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, newDirection, Time.deltaTime * rotateTime);


		if (rb.velocity.magnitude < maxSpeed) {
			rb.AddForce (-transform.forward * acceleration, ForceMode.VelocityChange);
		}

		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}


	}
}
