using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//The HUD is meant to contain all visual elements that a player needs to see during a game
//Depending on the element, some things are contained entirely in HUD like the score but other 
//elements are shared between objects
//The HUD also uses a lot of references to variables and objects stored elsewhere like player health, and all the UI elements

//Because of how we implemented the arrow type visual effect, it isn't contained in the HUD at all because 
//it's an element of the character, that was the easiest way for us to go about implementing it


public class HUD : MonoBehaviour {

	//get info from archer ie: health
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	float playerHealth;
    Animation anim;
    Shoot shoot;

    //UI elements to change between screens and bools to check when do change
    bool deathScreenPlayed;
    public GameObject deathScreenObject;
    public AnimationClip deathScreenAnimation;
    public GameObject pauseMenuObject;
    bool isGamePaused;
    bool winScreenPlayed;
    public GameObject winScreenObject;
    public AnimationClip winScreenAnimation;

    //Health
    public Text hpText;
	public Slider display;
	public Image sliderFill;
	
    //Score
	public int score;
    public int highScore;
	public Text scoreText;
    public Text highScoreText;
    public int winScore;

    //timer
    private Timer timer;
    private int currentTime;

    //Guitar Solo
    private AudioSource audioSource;
    private AudioClip guitarSolo;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () 
	{
        //getting references to all of the different UI elements, player elements, scripts etc. so set up the rest of the HUD
        playerObject = GameObject.FindGameObjectWithTag("Player");
        archerDetailScript = playerObject.GetComponent<ArcherDetail>();
		display = GameObject.FindGameObjectWithTag("Health").GetComponentInChildren<Slider> ();
		sliderFill = GameObject.FindGameObjectWithTag ("HPFill").GetComponent<Image> ();
		hpText = GameObject.FindGameObjectWithTag ("HPText").GetComponent<Text> ();
        scoreText = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text>();
        highScoreText = GameObject.FindGameObjectWithTag("Highscore").GetComponent<Text>();
        highScore = GameObject.FindGameObjectWithTag("Player").GetComponent<HighScore>().highScore;
        shoot = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        playerHealth = archerDetailScript.health;

        //initializing beginning values for score and setting screen change bools to false
        score = 0;
        deathScreenPlayed = false;
        winScreenPlayed = false;
        isGamePaused = false;
        anim = GetComponent<Animation>();
        
        //more references to scripts for the timer
        timer = GameObject.FindGameObjectWithTag("WaveCount").GetComponent<Timer>();
        currentTime = timer.getCurrentTime();

        //load guitar solo clip
        guitarSolo = Resources.Load("Sounds/GeneralSounds/GuitarSolo") as AudioClip;
    }

    void Update () 
	{
        //check if player has been damaged or picked up health and change HUD health accordingly
        display.value = playerHealth;
        playerHealth = archerDetailScript.health;
        
        //updating timer
        currentTime = timer.getCurrentTime();

        //if player health gets critical, change HP colour to red
        if (playerHealth <= archerDetailScript.playerHealthCritical)
        {
            sliderFill.color = new Color(1.0f, 0.607f, 0.317f);
            hpText.color = new Color(1.0f, 0.607f, 0.317f);
        }
        //otherwise hp text and health colour is green (non-critical)
        else
        {
            sliderFill.color = Color.green;
            hpText.color = Color.green;
        }
        
        //if player is dead play the death screen
        if (archerDetailScript.checkAliveStatus() && !deathScreenPlayed)
        {
            deathScreenPlayed = true;
            deathScreenObject.SetActive(true);
            doEndGameThings();           
        }

        //update score values in HUD
        scoreText.text = "score: " + score;
        highScore = GameObject.FindGameObjectWithTag("Player").GetComponent<HighScore>().highScore;
        highScoreText.text = "HighScore: " + highScore;

        //if player has won by getting enough score then play win screen (obsolete now)
        if (score >= winScore && !winScreenPlayed)
        {
            
            winScreenPlayed = true;
            winScreenObject.SetActive(true);
            doEndGameThings();
        }

        //if player survives for given amount of time then play win screen
        if (currentTime <= 0 && !winScreenPlayed)
        {
            winScreenPlayed = true;
            winScreenObject.SetActive(true);
            doEndGameThings();
        }

        //if you press escape to pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if the game is not already paused then pause time and allow cursor movement, pause camera movement etc.
            if (!isGamePaused)
            {
                isGamePaused = true;
                Time.timeScale = 0.0f;
                //pause menu screen being activated
                pauseMenuObject.SetActive(true);
                //disabling crosshair
                shoot.crosshairLockRestart = false;
                //showing cursor to be able to move around and click restart if need be
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //disabling camera components so that the background doesn't move around while player moves mouse
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCamera>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>().enabled = false;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
            }
            else
            //if the game is paused and you want to start again, unpause time, enable crosshair again, enable camera movement etc.
            {
                isGamePaused = false;
                Time.timeScale = 1.0f;
                pauseMenuObject.SetActive(false);
                shoot.crosshairLockRestart = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCamera>().enabled = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>().enabled = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            }
        }
    }

    //method to wait before disabling time at the end of the game to allow a seamless transition
    private IEnumerator waitToDisableTime()
    {
        yield return new WaitForSeconds(2.8f);
        Time.timeScale = 0.0f;
    }

    //public method to allow score to be added to the HUD (game score is stored in the HUD object, not the character scripts)
    //this way when the player kills an enemy, each enemy can have their own score and send it to the HUD with a simple call
    //in the respective enemy scripts
    public void addScore(int addedScore)
    {
        score += addedScore;
    }

    //convenient script for doing things at the end of the game
    public void doEndGameThings()
    {
        //lock cursor
        shoot.crosshairLockRestart = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(waitToDisableTime());
        //disabling game sound to allow for guitar solo
        GameObject.FindGameObjectWithTag("controller").GetComponent<AudioSource>().enabled = false;
        audioSource.PlayOneShot(guitarSolo, 0.1f);
    }
}
