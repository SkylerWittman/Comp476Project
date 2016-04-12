using UnityEngine;
using System.Collections;



//KNOWN BUGS:
//1. Issue with shooting, if you shoot before the coroutine finishes again things get all fucked up. Shoot only after coroutine is finished executing.
//              a. REMARK: The coroutine constantly runs, multiple times therefore it is changing values, see if you can only run the values once in the coroutine

public class Shoot : MonoBehaviour {
    Animation anim;
    public GameObject arrow;
    public GameObject regularArrow;
    public GameObject poisonArrow;
    public GameObject explosiveArrow;

    public AnimationClip attack;
    public AnimationClip hold;
    bool isAttack = false;
    bool isShot = false;
    Transform rightHand;
    Transform bow;
    int playOnce = 0;
    int shootOnce = 0;
    int timeHeld = 0;
    public float arrowSpeed = 75;
    bool lockC = true;
    GameObject c;
    Camera camera;

    public Camera mainCamera;
    Ray ray;


    public Texture2D crosshairTexture;
    Rect crossHairPosition;
    bool OriginalOn = true;
    bool lockCam = true;
    public bool crosshairLockLoad = false;
    public bool crosshairLockRestart = true;
    float mouseCounter;
    public ArcherDetail archer;
    int chCounter = 0;
    // Use this for initialization
    void Start () {
        crosshairLockLoad = false;
        anim = GetComponent<Animation>();
        anim[attack.name].speed = 1.4f;
        anim[hold.name].speed = 1.4f;
        rightHand = GameObject.FindGameObjectWithTag("righthand").transform;
        bow = GameObject.FindGameObjectWithTag("bow").transform;
        archer = GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>();

        crossHairPosition = new Rect( ((Screen.width - crosshairTexture.width)/ 2.05f),
            ((Screen.height - crosshairTexture.height) /1.9f), crosshairTexture.width, crosshairTexture.height);
        mouseCounter = 0.0f;
    }

    void OnGUI()// GUI for the crosshair
    {
        if (crosshairLockLoad == true && crosshairLockRestart == true)
        {
            GUI.DrawTexture(crossHairPosition, crosshairTexture);
        }
    }

    // Update is called once per frame
    void Update () {
        if(crosshairLockLoad == false)
        {
            chCounter++;
            if(chCounter > 40)
            {
                chCounter = 0;
                crosshairLockLoad = true;
            }
        }
        
        if (Input.GetMouseButton(0))//lock for knowing when the fire button has been pressed
        {
            if (playOnce == 0)// lock to play animation only once
            {
                
                anim.Play(attack.name);//plays load and shoot
                StartCoroutine(shoot());
                if(lockCam == true)// cam lock to ensure that it only zooms in once
                {
                    Vector3 moveUp = transform.forward;
                    mainCamera.transform.position += moveUp;
                    lockCam = false;
                }
            }
            // timeHeld = 0;
            timeHeld++;//multiplier for how long it is held
            mouseCounter++;//counts how many frames the mouse has been held
            if (timeHeld > 240)
            {
                timeHeld = 240;//sets the Cap of how long it is held

            }
        }
        else
        {
            
            
        }
        arrowSpeed = timeHeld * 4;
        if(arrowSpeed < 120 )
        {
            arrowSpeed = 120;// the minimum for the arrow speed
        }
        if (isAttack == true)
        {
            
            if (!Input.GetMouseButton(0) && (shootOnce == 0)) //Lock to verify animations
            {
                isAttack = false;
                playOnce = 0;
                if (mouseCounter > 2.0f)
                {

                    mouseCounter = 0.0f;
                    arrow = getArrow();
                    checkArrowStock();
                    Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z); // gets rotation of center of screen
					GameObject shootArrow = Instantiate(arrow, bow.position, mainCamera.transform.rotation) as GameObject;   // instantiates arrow
                    runForce(shootArrow); // applies physics to the arrow
                    mouseCounter = 0.0f; // reset mouse counter
                }
                shootOnce = 1; // Ensuring multiple arrows cant be instantiated 
                if (lockCam == false) // Ensuring the zooom out of the camera
                {
                    Vector3 moveUp = transform.forward;
                    mainCamera.transform.position -= moveUp;
                    lockCam = true;

                }
            }
            else if (Input.GetMouseButton(0))//Playing the "Hold" Animation
            {
                anim.Play(hold.name);
            }
            else // Reseting all the locks 
            {

                isAttack = false;
                playOnce = 0;
                lockC = true;
            }
        }
        else // Reseting more locks
        {
            shootOnce = 0;
            timeHeld = 0;
        }
    }

    GameObject getArrow()
    {
        
        switch (archer.arrow_type)
        {
            case ArcherDetail.arrowType.poison:
                return poisonArrow;
            case ArcherDetail.arrowType.explosive:
                return explosiveArrow;
            default:
                return regularArrow;

        }
        
    }

    void checkArrowStock()
    {
        switch (archer.arrow_type)
        {
            case ArcherDetail.arrowType.regular:
                break;
            case ArcherDetail.arrowType.poison:
                --archer.poisonArrowStock;
                break;
            case ArcherDetail.arrowType.explosive:
                --archer.explosiveArrowStock;
                break;
        }
    }
    void runForce(GameObject arrow)//Method for physics
    {
        arrow.GetComponent<Rigidbody>().AddForce((arrow.transform.forward * 5 * arrowSpeed) , ForceMode.Impulse);
        arrow.GetComponent<Arrow>().damage = arrowSpeed;
    }

    IEnumerator shoot() // CoRoutine to play the animation
    {
        yield return new WaitForSeconds(anim[attack.name].length);
        playOnce = 1;
        isAttack = true;
        yield break;
       
    }


}
