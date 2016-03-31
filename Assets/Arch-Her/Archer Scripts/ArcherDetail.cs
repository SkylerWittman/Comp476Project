using UnityEngine;
using System.Collections;

public class ArcherDetail : MonoBehaviour {
    public enum arrowType { regular, poison, explosive}
    public arrowType arrow_type;
    public int regularArrowStock = 99999;
    public int poisonArrowStock = 2;
    public int explosiveArrowStock = 2;
    public int ArrowState = 0;
	public int health;
	public int playerHealthCritical;
	// Use this for initialization
	void Start () {
        arrow_type = arrowType.regular;
	
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


    }

    void setArrow()
    {
        switch (ArrowState)
        {
            case 0:
                arrow_type = arrowType.regular;
                break;
            case 1:
                if (poisonArrowStock > 0)
                    arrow_type = arrowType.poison;
                else if (explosiveArrowStock > 0)
                {
                    ArrowState = 2;
                    arrow_type = arrowType.explosive;
                }
                else {
                    arrow_type = arrowType.regular;
                    ArrowState = 0;
                }
                break;
            case 2:
                if (explosiveArrowStock > 0)
                    arrow_type = arrowType.explosive;
                else {
                    arrow_type = arrowType.regular;
                    ArrowState = 0;
                }
                break;
        }
    }

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("poison"))
		{
			arrow_type = arrowType.poison;
		}

		if (col.gameObject.CompareTag("explosive"))
		{
			arrow_type = arrowType.explosive;
		}
	}

    public void doDamage(int amountOfDamage)
    {
        health -= amountOfDamage;
        GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().dodamageHUD(amountOfDamage);
    }
}
