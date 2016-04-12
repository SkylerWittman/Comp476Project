using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArcherDetail : MonoBehaviour {
    public enum arrowType { regular, poison, explosive}
    public arrowType arrow_type;
    public int regularArrowStock = 999;
    public int poisonArrowStock;
    public int poisonPowerupAddedArrowStock;
    public int explosiveArrowStock;
    public int explosivePowerupAddedArrowStock;
    public int ArrowState = 0;

	public float health;
    public float maxHealth;
    public float playerHealthCritical;
    public float healthPowerupAdded;
    public CameraShake cameraShakeObject;
    public Text arrowStock;
    HUD hudScript;
    public bool isDead;
    public bool deathAnimationPlayed;
    Animation anim;
    public AnimationClip deathClip;
    public AnimationClip lyingDeadClip;


    GameObject arrowDisplay;
	// Use this for initialization
	void Start () {
        hudScript = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();
        arrow_type = arrowType.regular;
        arrowDisplay = GameObject.FindGameObjectWithTag("ArrowDisplayParticle");
        deathAnimationPlayed = false;
        isDead = false;
        anim = GetComponent<Animation>();
        anim[deathClip.name].wrapMode = WrapMode.Once;
        cameraShakeObject = GameObject.FindGameObjectWithTag("HUD").GetComponent<CameraShake>();
        arrowStock = GameObject.FindGameObjectWithTag("ArrowTag").GetComponent<Text>();
        arrowStock.text = "X" + regularArrowStock;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.O))
        {
            if(ArrowState == 0)
            {
                if(poisonArrowStock > 0)
                {
                    ArrowState = 1;
                    
                }
                else if(explosiveArrowStock > 0){
                    ArrowState = 2;

                }
                else
                {
                    ArrowState = 0;
                }
            }
            else if (ArrowState == 1)
            {
                if(explosiveArrowStock > 0)
                {
                    ArrowState = 2;
                }
                else
                {
                    ArrowState = 0;
                }
            }
            else if(ArrowState == 2)
            {
                ArrowState = 0;
            }
        }
        setArrow();

        if (health <= 0.0f)
        {
            isDead = true;
            if (!deathAnimationPlayed)
            {
                //Debug.Log("got here");
                
                anim.Play(deathClip.name, PlayMode.StopAll);
                StartCoroutine(waitToDisableAnim());
                //anim.Play(lyingDeadClip.name);
            }
            //Freeze all movement/rotations when NPC has been slain
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //lock position of camera maybe?
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform.rotation;
        }
    }

    void setArrow()
    {
        switch (ArrowState)
        {
            case 0:
                arrow_type = arrowType.regular;
                arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.white;
                arrowStock.text = "X" + regularArrowStock;
                arrowStock.color = new Color(1.0f, 0.976f, 0.611f);
                ;
                break;
            case 1:
                if (poisonArrowStock > 0)
                {
                    arrow_type = arrowType.poison;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.green;
                    arrowStock.text = "X" + poisonArrowStock;
                    arrowStock.color = Color.green;
                }
                else if (explosiveArrowStock > 0)
                {
                    ArrowState = 2;
                    arrow_type = arrowType.explosive;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.red;
                    arrowStock.text = "X" + explosiveArrowStock;
                }
                else {
                    arrow_type = arrowType.regular;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.white;
                    arrowStock.text = "X" + regularArrowStock;
                    arrowStock.color = new Color(1.0f, 0.976f, 0.611f);
                    ArrowState = 0;
                }
                break;
            case 2:
                if (explosiveArrowStock > 0)
                {
                    arrow_type = arrowType.explosive;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.red;
                    arrowStock.text = "X" + explosiveArrowStock;
                    arrowStock.color = Color.red;
                }
                else {
                    arrow_type = arrowType.regular;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.white;
                    arrowStock.text = "X" + regularArrowStock;
                    arrowStock.color = new Color(1.0f, 0.976f, 0.611f);
                    ArrowState = 0;
                }
                break;
        }
    }

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("PoisonPowerup"))
		{
            Debug.Log("+ " + poisonPowerupAddedArrowStock + " poison arrows");
            poisonArrowStock += poisonPowerupAddedArrowStock;
            Destroy(col.gameObject);
        }

		if (col.gameObject.CompareTag("ExplosivePowerup"))
		{
            Debug.Log("+ " + explosivePowerupAddedArrowStock + " explosive arrows");
            explosiveArrowStock += explosivePowerupAddedArrowStock;
            Destroy(col.gameObject);
		}

        if (col.gameObject.CompareTag("HealthPowerup"))
        {
            if ((health + healthPowerupAdded) <= maxHealth)
            {
                health += healthPowerupAdded;
            } else if ((health + healthPowerupAdded) >= maxHealth)
            {
                health = maxHealth;
            }

            Debug.Log("+ " + healthPowerupAdded + " health");
            Destroy(col.gameObject);
        }
	}

    public void takeDamage(float amountOfDamage)
    {
        Debug.Log("Health is currently " + health);
        health -= amountOfDamage;
        cameraShakeObject.enabled = true;
        cameraShakeObject.addShakeDuration();
    }

    private IEnumerator waitToDisableAnim()
    {
        yield return new WaitForSeconds(2.5f);
        deathAnimationPlayed = true;
    }

    public bool checkAliveStatus()
    {
        return isDead;
    }
}
