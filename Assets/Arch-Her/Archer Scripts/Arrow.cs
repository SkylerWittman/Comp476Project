using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public float damage;
    int counter = 0;
    public ArcherDetail archer;
    public Explosion explosion;
    bool lockA = true;
    bool hitLock = false;


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
                        damage += 50;
                        break;
                    case ArcherDetail.arrowType.explosive:
                        hitLock = true;
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
            Instantiate(explosion, transform.position, Quaternion.identity);
            hitLock = false;
        }
    }
}
