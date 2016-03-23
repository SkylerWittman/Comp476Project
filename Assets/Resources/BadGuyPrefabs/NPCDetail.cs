using UnityEngine;
using System.Collections;

public class NPCDetail : MonoBehaviour {
    bool LockA = false;
    public float health = 100; 
    int counter = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (LockA == true)
        {
            if (counter < 100)
            {
                health -= 50;
                ++counter;
            }
            else
            {
                LockA = false;
                counter = 0;
            }
        }
        if(health < 0)
        {
            Destroy(this.gameObject);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "RegularArrow":
                health -= other.GetComponent<Arrow>().damage;
                break;
            case "PoisionArrow":
                health -= other.GetComponent<Arrow>().damage;
                LockA = true;
                break;
            case "ExplosiveArrow":
                //No clue yet
                break;
        }



        Destroy(other.gameObject);
    }
}
