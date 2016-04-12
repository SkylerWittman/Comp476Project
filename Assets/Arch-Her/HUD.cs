using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	float playerHealth;
    Animation anim;
    Shoot shoot;

    //UI elements
    bool deathScreenPlayed;
    public GameObject deathScreenObject;
    public AnimationClip deathScreenAnimation;
    public GameObject pauseMenuObject;
    bool isGamePaused;

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
    bool winScreenPlayed;
    public GameObject winScreenObject;
    public AnimationClip winScreenAnimation;

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

        score = 0;
        deathScreenPlayed = false;
        winScreenPlayed = false;
        isGamePaused = false;
        anim = GetComponent<Animation>();

        timer = GameObject.FindGameObjectWithTag("WaveCount").GetComponent<Timer>();
        currentTime = timer.getCurrentTime();

        guitarSolo = Resources.Load("Sounds/GeneralSounds/GuitarSolo") as AudioClip;
    }

    void Update () 
	{
        display.value = playerHealth;
        playerHealth = archerDetailScript.health;
        currentTime = timer.getCurrentTime();

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
            doEndGameThings();           
        }

        //Score
        scoreText.text = "score: " + score;
        highScoreText.text = "HighScore: " + highScore;

        //if player has won by getting enough score
        if (score >= winScore && !winScreenPlayed)
        {
            
            winScreenPlayed = true;
            winScreenObject.SetActive(true);
            doEndGameThings();
        }

        if (currentTime <= 0 && !deathScreenPlayed)
        {
            deathScreenPlayed = true;
            deathScreenObject.SetActive(true);
            doEndGameThings(); 
        }

        //if you press escape to pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if the game is not already paused then pause time and allow cursor movement, etc.
            if (!isGamePaused)
            {
                isGamePaused = true;
                Time.timeScale = 0.0f;
                pauseMenuObject.SetActive(true);
                shoot.crosshairLockRestart = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCamera>().enabled = false;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
            }
            else
            //if the game is paused and you want to start again, unpause time, enable crosshair again etc.
            {
                isGamePaused = false;
                Time.timeScale = 1.0f;
                pauseMenuObject.SetActive(false);
                shoot.crosshairLockRestart = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCamera>().enabled = false;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            }
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

    public void doEndGameThings()
    {
        shoot.crosshairLockRestart = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(waitToDisableTime());
        GameObject.FindGameObjectWithTag("controller").GetComponent<AudioSource>().enabled = false;
        audioSource.PlayOneShot(guitarSolo, 0.1f);
    }
}
