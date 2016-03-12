using UnityEngine;
using System.Collections;



//KNOWN BUGS:
//1. Issue with shooting, if you shoot before the coroutine finishes again things get all fucked up. Shoot only after coroutine is finished executing.
//              a. REMARK: The coroutine constantly runs, multiple times therefore it is changing values, see if you can only run the values once in the coroutine

public class Shoot : MonoBehaviour {
    Animation anim;
    public GameObject arrow;
    public AnimationClip attack;
    public AnimationClip hold;
    public bool isAttack = false;
    bool isShot = false;
    Transform rightHand;
    Transform bow;
    int playOnce = 0;
    int shootOnce = 0;
    int timeHeld = 0;
    float arrowSpeed = 20;
	// Use this for initialization
	void Start () {
        
        anim = GetComponent<Animation>();
        anim[attack.name].speed = 0.8f;
        rightHand = GameObject.FindGameObjectWithTag("righthand").transform;
        bow = GameObject.FindGameObjectWithTag("bow").transform;
	}
	void FixedUpdate()
    {

    }
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            if (playOnce == 0)
            {
                anim.Play(attack.name);
                StartCoroutine(shoot());
                
            }
            Debug.Log("Time Held: " + timeHeld);
            // timeHeld = 0;
            timeHeld++;
            if (timeHeld > 240)
            {
                timeHeld = 240;
            }
        }
        else
        {
            
            
        }
        arrowSpeed = timeHeld;
        if(arrowSpeed < 12)
        {
            arrowSpeed = 12;
        }
        if (isAttack)
        {
            if (!Input.GetMouseButton(0) && (shootOnce == 0)) //&& (shootOnce == 0))
            {


                isAttack = false;
                playOnce = 0;
                shootOnce = 1;
                //shootArrow.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward);
                var rotationVec = bow.rotation.eulerAngles;

                rotationVec.y += 178;
                GameObject shootArrow = Instantiate(arrow, bow.position, Quaternion.Euler(rotationVec)) as GameObject;
                shootArrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(-.2f,0,0);
                Debug.Log("Arrow Speed: " + arrowSpeed);
                shootArrow.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * arrowSpeed);
                

            }
            else if (Input.GetMouseButton(0))
            {
                anim.Play(hold.name);
            }
            else
            {
                isAttack = false;
                playOnce = 0;
            }

        }
        else
        {
            Debug.Log("isAttack: " + isAttack);
            shootOnce = 0;
            timeHeld = 0;
        }



    }


    IEnumerator shoot()
    {
        yield return new WaitForSeconds(anim[attack.name].length);
        Debug.Log("IT GETS HERE");
        playOnce = 1;
        isAttack = true;
        yield break;
    }
}
