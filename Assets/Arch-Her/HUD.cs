using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	int playerHealth;
	float projectileDamage;

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
		display = GameObject.FindGameObjectWithTag("Health").GetComponentInChildren<Slider> ();
		sliderFill = GameObject.FindGameObjectWithTag ("HPFill").GetComponent<Image> ();
		hpText = GameObject.FindGameObjectWithTag ("HPText").GetComponent<Text> ();
        scoreText = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text>();

        playerHealth = archerDetailScript.health;

        score = 0;
		//StartCoroutine(HealthWait());
	}
	
	void Update () 
	{
        //display.value = playerHealth;
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

	//IEnumerator HealthWait()
	//{
	//	yield return new WaitForSeconds(3);
 //       playerHealth = archerDetailScript.playerHealthCritical;
 //       addScore(50);
	//}

    public void addScore(int addedScore)
    {
        score += addedScore;
    }

    public void dodamageHUD(int amountOfDamage)
    {
        display.value -= amountOfDamage;
    }
}
