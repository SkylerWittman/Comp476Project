using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public float damage;
    int counter = 0;
    public ArcherDetail archer;
    public Explosion explosion;
    bool lockA = true;
    bool hitLock = true;

	// Use this for initialization
	void Start () {
        archer = GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>();


	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(archer.transform.position, transform.position);
        if (lockA == true)
        {
            if (archer != null)
            {
                lockA = false;
                switch (archer.arrow_type)
                {
                    case ArcherDetail.arrowType.regular:
                        transform.tag = "RegularArrow";
                        damage += 10;
                        break;
                    case ArcherDetail.arrowType.poison:
                        transform.tag = "PoisonArrow";
                        damage += 12;
                        break;
                    case ArcherDetail.arrowType.explosive:
                        transform.tag = "ExplosiveArrow";
                        damage += 15;
                        break;

                }
            }
        }
        if (counter > 45)
        {
            GameObject.Destroy(gameObject, 0);
        }

        counter++;
	}
    void OnTriggerEnter(Collider Collision)
    {

        if (hitLock == true)
        {
            /*
            //RaycastHit[] hit = Physics.SphereCastAll(transform.position - transform.up * 10.0f, 10.0f, transform.up, 100f);
            if (hit != null)
            {
                Debug.Log(hit.Length);
                
                for (int i = 0; i < hit.Length; i++)
                {
                    if(hit[i].transform.gameObject.tag == "Player")
                    {

                        hit[i].transform.GetComponent<Rigidbody>().AddExplosionForce(50, transform.position, 15f, 50);
                    }
                }
                    
            }*/
            Instantiate(explosion, transform.position, Quaternion.identity);
            hitLock = false;
        }
    }
}
