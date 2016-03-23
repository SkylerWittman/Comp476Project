using UnityEngine;
using System.Collections;

public class GetHit : MonoBehaviour {

    bool LockA = false;
    int counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(LockA == true)
        {
           if(counter < 100)
            {
                //health - 50 ?
                ++counter;
            }
            else
            {
                LockA = false;
                counter = 0;
            }
        }


	}
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "RegularArrow":
                //health - arrowDamage
                break;
            case "PoisionArrow":
                //health - arrowDamage
                LockA = true;
                break;
            case "ExplosiveArrow":
                //No clue yet
                break;
        }



        Destroy(other.gameObject);
    }
}
