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
    float arrowSpeed = 75;
    bool lockC = true;


    public Camera mainCamera;
    Ray ray;

    int mouseCounter = 0;
	// Use this for initialization
	void Start () {
        
        anim = GetComponent<Animation>();
        anim[attack.name].speed = .7f;
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
            mouseCounter++;
            if (timeHeld > 240)
            {
                timeHeld = 240;
            }
        }
        else
        {
            
            
        }
        arrowSpeed = timeHeld * 4;
        if(arrowSpeed < 120 )
        {
            arrowSpeed = 120;
        }
        if (isAttack == true)
        {
            if (!Input.GetMouseButton(0) && (shootOnce == 0)) //&& (shootOnce == 0))
            {


                isAttack = false;
                playOnce = 0;
                
              
                var rotationVec = bow.rotation.eulerAngles;

                rotationVec.y += 178;
                if (mouseCounter > 13)
                {
                    //Ray ray2 = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 150));
                    GameObject shootArrow = Instantiate(arrow, bow.position, Quaternion.Euler(rotationVec)) as GameObject;

                    //var newDir = Vector3.RotateTowards(shootArrow.transform.position, ray2.direction,0, 0);
                    Vector3 newDir = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
                    Debug.Log("mouse x pos: " + Input.mousePosition.x + " mouse y pos: " + Input.mousePosition.y);
                    Debug.Log("bow x pos: " + bow.transform.position.x + " bow y pos: " + bow.transform.position.y);

                    Vector3 aimDirection = shootArrow.transform.position - transform.forward;
                    shootArrow.transform.rotation = Quaternion.LookRotation(aimDirection);
                    runForce(shootArrow);
                    //shootArrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0, -2f);
                    Debug.Log("Arrow Speed: " + arrowSpeed);
                    mouseCounter = 0;
                }
                shootOnce = 1;
                

            }
            else if (Input.GetMouseButton(0))
            {
                anim.Play(hold.name);
            }
            else
            {
                isAttack = false;
                playOnce = 0;
                lockC = true;
            }

        }
        else
        {
            //Debug.Log("isAttack: " + isAttack);
            shootOnce = 0;
            timeHeld = 0;
        }



    }

    void runForce(GameObject arrow)
    {
        for(int i = 0; i < 55; i++)
        {
            ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            arrow.GetComponent<Rigidbody>().AddForce(( (transform.forward + (ray.direction*2)) *5* arrowSpeed)/i, ForceMode.Acceleration);
        }
    }
    IEnumerator shoot()
    {
        yield return new WaitForSeconds(anim[attack.name].length);
        //Debug.Log("IT GETS HERE");
        playOnce = 1;
        isAttack = true;
        yield break;
       
    }
}
