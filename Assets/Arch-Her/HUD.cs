using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	float playerHealth;
    Animation anim;
    //public GameObject deathScreenPrefab;
    bool deathScreenPlayed;
    public GameObject deathScreenObject;
    public AnimationClip deathScreenAnimation;

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
        //deathScreenObject = GameObject.FindGameObjectWithTag("DeathScreen").GetComponent<GameObject>();
        deathScreenPlayed = false;
        anim = GetComponent<Animation>();
    }

    void Update () 
	{
        display.value = playerHealth;
        playerHealth = archerDetailScript.health;
        //display.value = playerHealth;
        if (playerHealth <= archerDetailScript.playerHealthCritical)
        {
            sliderFill.color = Color.red;
            hpText.color = Color.red;
        }
        
        //if player is dead
        if (archerDetailScript.checkAliveStatus() && !deathScreenPlayed)
        {
            deathScreenPlayed = true;
            deathScreenObject.SetActive(true);
            StartCoroutine(waitToDisableTime());
        }

        //Score
        scoreText.text = "score: " + score;

        if (score >= winScore)
        {
            //Time.timeScale = 0.0f;
            //anim.Play(gameOverClip.name);
        }
    }

    private IEnumerator waitToDisableTime()
    {
        yield return new WaitForSeconds(2.8f);
        Time.timeScale = 0.0f;
    }

    public void addScore(int addedScore)
    {
        score += addedScore;
    }

    public void addHealthHUD(float amountOfHealth)
    {
        display.value += amountOfHealth;
    }
}
