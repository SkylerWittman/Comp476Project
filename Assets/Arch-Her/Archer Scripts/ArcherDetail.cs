using UnityEngine;
using System.Collections;

public class ArcherDetail : MonoBehaviour {
    public enum arrowType { regular, poison, explosive}
    public arrowType arrow_type;
    //public int regularArrowStock = 99999;
    public int poisonArrowStock;
    public int poisonPowerupAddedArrowStock;
    public int explosiveArrowStock;
    public int explosivePowerupAddedArrowStock;
    public int ArrowState = 0;

	public float health;
	public float playerHealthCritical;
    public float healthPowerupAdded;
    public bool isDead;
    public bool deathAnimationPlayed;
    Animation anim;
    public AnimationClip deathClip;


    GameObject arrowDisplay;
	// Use this for initialization
	void Start () {
        arrow_type = arrowType.regular;
        arrowDisplay = GameObject.FindGameObjectWithTag("ArrowDisplayParticle");
        isDead = false;
        anim = GetComponent<Animation>();
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
            //if (!deathAnimationPlayed)
            //{
                anim.Play(deathClip.name);
              //  deathAnimationPlayed = true;
            //}
            //Freeze all movement/rotations when NPC has been slain
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void setArrow()
    {
        switch (ArrowState)
        {
            case 0:
                arrow_type = arrowType.regular;
                arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.white;
                break;
            case 1:
                if (poisonArrowStock > 0)
                {
                    arrow_type = arrowType.poison;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.green;
                }
                else if (explosiveArrowStock > 0)
                {
                    ArrowState = 2;
                    arrow_type = arrowType.explosive;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.red;
                }
                else {
                    arrow_type = arrowType.regular;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.white;
                    ArrowState = 0;
                }
                break;
            case 2:
                if (explosiveArrowStock > 0)
                {
                    arrow_type = arrowType.explosive;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.red;
                }
                else {
                    arrow_type = arrowType.regular;
                    arrowDisplay.GetComponent<ParticleSystem>().startColor = Color.white;
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
            Debug.Log("+ " + healthPowerupAdded + " health");
            health += healthPowerupAdded;
            Destroy(col.gameObject);
        }
	}

    public void takeDamage(float amountOfDamage)
    {
        Debug.Log("Health is currently " + health);
        health -= amountOfDamage;
        GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().doDamageHUD(amountOfDamage);
    }
}
