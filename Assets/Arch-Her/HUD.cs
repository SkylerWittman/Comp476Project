using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    //PlayerMovement PlayerMovementScript;
	public int playerHealth;
	public int playerHealthCritical;
	public int projectileDamage;

	//tag used to compare collision tag and damage player
	public string collisionTag;

	//UI elements
	//Health
	public Slider display;
	public Image sliderFill;
	
    //Score
	public static int score;
	public Text scoreText;

	// Use this for initialization
	void Start () 
	{
        //PlayerMovementScript = GetComponentInParent<PlayerMovement>();
        //playerHealth = PlayerMovementScript.
        display = GetComponentInChildren<Slider> ();
		sliderFill = GetComponentInChildren<Image> ();
        
        score = 0;
		//StartCoroutine(HealthWait());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Perhaps to run smoother, best to only check for these things in an external method whenever the player gets hit
		//by an enemy sword or projectile

		//Debug.Log (sliderFill.color.r);
		//Debug.Log (sliderFill.color.g);
		//Debug.Log (sliderFill.color.b);
		//Health
		display.value = playerHealth;
		if (playerHealth <= playerHealthCritical) 
		{
			sliderFill.color = Color.red;
		}
//		if (playerHealth <= 0)
//		{
//			//die? restart screen?
//		}

		//Score
		scoreText.text = "score: " + score;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == collisionTag) 
		{
			playerHealth -= projectileDamage;
		}
	}

	IEnumerator HealthWait()
	{
		yield return new WaitForSeconds(3);
		playerHealth = playerHealthCritical;
	}
}
