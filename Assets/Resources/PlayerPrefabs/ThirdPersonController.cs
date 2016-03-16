

using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour 

{

	public float rotationSpeed = 10;
	public float walkSpeed = 10;
	public float runSpeed = 15;
	public float gravity = 5;
	public float jumpSpeed = 10;
	public float rotateSpeed = 1;
	public Transform player;


	private float moveSpeed;
	private Animation animator;
	private CharacterController controller;
	private float yRotation;
	private bool isGrounded;
	private bool isRunning;
	private bool isJumping;
	private Vector3 forwardVector;
	private Vector3 upVector = Vector3.up;
	private Vector3 rightVector;
	private Vector3 moveDirection;


	void Start(){
		animator = GetComponent<Animation>();
		controller = GetComponent<CharacterController> ();
		isGrounded = true;
		isRunning = false;
		isJumping = false;
		//Cursor.visible = false;
	}


	void Update(){
		
		forwardVector = player.forward;
		rightVector = player.right;

		if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0) {
			animator.Play("IDDLE");
		}
			


		if (Input.GetKeyDown ("left shift") && !isRunning) {
			isRunning = true;
		} else {
			isRunning = false;
		}
			
		if (!isRunning) {
			moveSpeed = walkSpeed;
			animator.Play ("WALK");

		}

		if (isRunning) {
			moveSpeed = runSpeed;
			animator.Play("RUN");
			Debug.Log ("RUNNING");

		}
				


		if(Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse X") < 0){
			yRotation += rotateSpeed* Input.GetAxis("Mouse X");
		}




		if (Input.GetKeyDown ("space")) {
			isJumping = true;
			moveDirection.y = jumpSpeed;

		}

		if (moveDirection.y < 5) {
			isJumping = false;
		}
				

		if (isJumping) {
			controller.Move (rightVector*(moveSpeed*Input.GetAxis ("Horizontal") * Time.deltaTime));
		}

		if (controller.isGrounded && !isJumping) {
			moveDirection = (forwardVector*(moveSpeed*Input.GetAxis ("Vertical"))) + (rightVector*(moveSpeed*Input.GetAxis ("Horizontal")));
		}

		transform.rotation = Quaternion.Euler(0, yRotation, 0);
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);

	}

}



