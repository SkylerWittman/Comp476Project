using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	float playerHealth;
	float projectileDamage;
    Animation anim;

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
    public int winScore;
    public AnimationClip gameOverClip;

    //Arrow type
    Canvas arrowDisplay;
    ParticleSystem poisonParticleSystem;
    ParticleSystem explosiveParticleSystem;

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
        anim = GetComponent<Animation>();
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

        if (score >= winScore)
        {
            GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<Image>().enabled = true;
            anim.Play(gameOverClip.name);
        }
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

    public void doDamageHUD(float amountOfDamage)
    {
        display.value -= amountOfDamage;
    }
}
