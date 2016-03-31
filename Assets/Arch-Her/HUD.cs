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
    ParticleSystem poison;
    ParticleSystem explosive;

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

        arrowDisplay = GameObject.FindGameObjectWithTag("ArrowDisplay").GetComponent<Canvas>();
        poison = GameObject.FindGameObjectWithTag("Poison").GetComponent<ParticleSystem>();
        explosive = GameObject.FindGameObjectWithTag("Explosive").GetComponent<ParticleSystem>();
        poison.Stop();
        explosive.Stop();
        Debug.Log(poison);
        Debug.Log(explosive);
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

        switch (archerDetailScript.arrow_type)
        {

            case ArcherDetail.arrowType.regular:
                //Debug.Log("arrow type is " + archerDetailScript.arrow_type);
                //arrowDisplay.GetComponentInChildren<ParticleSystem>().enableEmission = false;
                break;
            case ArcherDetail.arrowType.poison:
                //Debug.Log("arrow type is " + archerDetailScript.arrow_type);
                //arrowDisplay.GetComponentInChildren<ParticleSystem>().enableEmission = true;
                break;
            case ArcherDetail.arrowType.explosive:
                //Debug.Log("arrow type is " + archerDetailScript.arrow_type);
                //arrowDisplay.GetComponentInChildren<ParticleSystem>().enableEmission = true;
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

    public void dodamageHUD(float amountOfDamage)
    {
        display.value -= amountOfDamage;
    }
}
