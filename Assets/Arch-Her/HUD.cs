using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	float playerHealth;
    Animation anim;

    //UI elements
    bool deathScreenPlayed;
    public GameObject deathScreenObject;
    public AnimationClip deathScreenAnimation;

    //Health
    public Text hpText;
	public Slider display;
	public Image sliderFill;
	
    //Score
	public static int score;
	public Text scoreText;
    public int winScore;
    bool winScreenPlayed;
    public GameObject winScreenObject;
    public AnimationClip winScreenAnimation;

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
        deathScreenPlayed = false;
        winScreenPlayed = false;
        anim = GetComponent<Animation>();
    }

    void Update () 
	{
        display.value = playerHealth;
        playerHealth = archerDetailScript.health;

        //if player health gets critical, change HP colour to red
        if (playerHealth <= archerDetailScript.playerHealthCritical)
        {
            sliderFill.color = new Color(1.0f, 0.607f, 0.317f);
            hpText.color = new Color(1.0f, 0.607f, 0.317f);
        }
        else
        {
            sliderFill.color = Color.green;
            hpText.color = Color.green;
        }
        
        //if player is dead
        if (archerDetailScript.checkAliveStatus() && !deathScreenPlayed)
        {
            deathScreenPlayed = true;
            deathScreenObject.SetActive(true);
            Cursor.visible = enabled;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(waitToDisableTime());
           
        }

        //Score
        scoreText.text = "score: " + score;

        //if player has won by getting enough score
        if (score >= winScore && !winScreenPlayed)
        {
            winScreenPlayed = true;
            winScreenObject.SetActive(true);
            Cursor.visible = enabled;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(waitToDisableTime());
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
