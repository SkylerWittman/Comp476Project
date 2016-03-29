using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
    public Arrow arrowScript;
	public int playerHealth;
	public float projectileDamage;

	//tag used to compare collision tag and damage player
	public string collisionTag;

	//UI elements
	//Health
	public Text hpText;
	public Slider display;
	public Image sliderFill;
	
    //Score
	public static int score;
	public Text scoreText;

	// Use this for initialization
	void Start () 
	{
        playerObject = GameObject.FindGameObjectWithTag("Player");

        archerDetailScript = playerObject.GetComponent<ArcherDetail>();
        // ----------BUG---------------
        // After the first shot, the newly instantiated arrow object has a different tag if it's anything but a regular arrow
        // so we have to deal with that
        arrowScript = GameObject.FindGameObjectWithTag ("RegularArrow").GetComponent<Arrow>();
		display = GameObject.FindGameObjectWithTag("Health").GetComponentInChildren<Slider> ();
		sliderFill = GameObject.FindGameObjectWithTag ("HPFill").GetComponent<Image> ();
		hpText = GameObject.FindGameObjectWithTag ("HPText").GetComponent<Text> ();
        scoreText = GameObject.FindGameObjectWithTag ("score").GetComponent<Text>();

        playerHealth = archerDetailScript.health;
        projectileDamage = arrowScript.damage;

        score = 0;
		StartCoroutine(HealthWait());
	}
	
	void Update () 
	{
		//Health
		display.value = playerHealth;
        if (playerHealth <= archerDetailScript.playerHealthCritical) 
		{
			sliderFill.color = Color.red;
			hpText.color = Color.red;

		}
//		if (playerHealth <= 0)
//		{
//			//die? restart screen?
//		}

		//Score
		scoreText.text = "score: " + score;

	}

	IEnumerator HealthWait()
	{
		yield return new WaitForSeconds(3);
        playerHealth = archerDetailScript.playerHealthCritical;
        score += 10;
	}
}
