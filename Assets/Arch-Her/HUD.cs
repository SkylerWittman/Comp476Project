using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	//can be gotten from player script health value
    public GameObject playerObject;
    public ArcherDetail archerDetailScript;
	float playerHealth;
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

    //Arrow type
    Canvas arrowDisplay;
    ParticleSystem poisonParticleSystem;
    ParticleSystem explosiveParticleSystem;
    public Transform cameraTransform;
    public Transform HUDarrowTransform;

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

        //arrowDisplay = GameObject.FindGameObjectWithTag("ArrowDisplay").GetComponent<Canvas>();
        //poisonParticleSystem = GameObject.FindGameObjectWithTag("PoisonParticleSystem").GetComponentInChildren<ParticleSystem>();
        //explosiveParticleSystem = GameObject.FindGameObjectWithTag("ExplosiveParticleSystem").GetComponentInChildren<ParticleSystem>();
        //poisonParticleSystem.Stop();
        //explosiveParticleSystem.Stop();
        //Debug.Log(poisonParticleSystem);
        //Debug.Log(explosiveParticleSystem);
        //cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //HUDarrowTransform = GameObject.FindGameObjectWithTag("arrow").transform;
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

        //Debug.Log(HUDarrowTransform);
        //HUDarrowTransform.rotation = cameraTransform.rotation;
        switch (archerDetailScript.arrow_type)
        {

            case ArcherDetail.arrowType.regular:
                //Debug.Log("arrow type is " + archerDetailScript.arrow_type);
                //poisonParticleSystem.Stop();
                //explosiveParticleSystem.Stop();
                break;
            case ArcherDetail.arrowType.poison:
                //Debug.Log("arrow type is " + archerDetailScript.arrow_type);
                //poisonParticleSystem.Play();
                break;
            case ArcherDetail.arrowType.explosive:
                //Debug.Log("arrow type is " + archerDetailScript.arrow_type);
                //poisonParticleSystem.Stop();
                //explosiveParticleSystem.Play();
                break;
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
