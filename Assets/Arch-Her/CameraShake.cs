using UnityEngine;
using System.Collections;


//This script is a simple attachment to the player to allow camera shake on an event trigger
//in our case the trigger was when being attacked by an enemy, the camera would shake each time the player took damage
public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform if null
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration;
    public float newShakeDuration;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount ;
    public float decreaseFactor;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        //if there is a shake duration then shake camera
        if (shakeDuration > 0)
        {
            //shake camera by shake amount
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            //decrease shake duration each frame
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            //if ther is no shake duration put camera back to normal position
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    //method to add shake duration so that each enemy attack will shake the camera
    public void addShakeDuration()
    {
        shakeDuration = newShakeDuration;
    }
}
