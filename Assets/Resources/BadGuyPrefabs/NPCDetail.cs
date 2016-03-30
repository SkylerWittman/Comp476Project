using UnityEngine;
using System.Collections;

public class NPCDetail : MonoBehaviour {
    bool LockA = false;
    Animation anim;
    public AnimationClip die;
    public float health = 100.0f;
    public float damage = 20.0f;
    int counter = 0;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
        anim[die.name].layer = 2;
        anim[die.name].speed = .7f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (LockA == true)
        {
            if (counter > 5)
            {
                health -= 10f;
                counter = 0;
            }
            counter++; 
        }
        Vector3 pos = transform.position;
        if(health < 0.0f)
        {
           
			anim.CrossFade(die.name, 0.5f);
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); ;
            transform.position = pos;
            //Freeze all movement/rotations when NPC has been slain
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
           

			Destroy(this.gameObject, anim[die.name].length+.5f);
            
        }

    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "RegularArrow":
                health -= other.GetComponent<Arrow>().damage;
                Destroy(other.gameObject);
                break;
            case "PoisionArrow":
                health -= other.GetComponent<Arrow>().damage;
                LockA = true;
                Destroy(other.gameObject);
                break;
            case "ExplosiveArrow":
                Destroy(this.gameObject, anim[die.name].length + .5f);
                Destroy(other.gameObject);
                break;
            case "Gas":
                LockA = true;
                break;
        }


        
    }

}
