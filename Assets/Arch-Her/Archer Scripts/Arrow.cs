using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public float damage;
    int counter = 0;
    public ArcherDetail archer;
    public Shoot shoot;
    public Explosion explosion;
    public poisonExplosion poisonExplosion;

    bool lockA = true;
    bool hitLock = false;
    bool poisonLock = false;


	// Use this for initialization
	void Start () {
        archer = GameObject.FindGameObjectWithTag("Player").GetComponent<ArcherDetail>();
        shoot = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
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
                        damage = (shoot.arrowSpeed*.75f ) + 20;
                        break;
                    case ArcherDetail.arrowType.poison:
                        poisonLock = true;
                        transform.tag = "PoisonArrow";
                        damage = (shoot.arrowSpeed )+80;
                        break;
                    case ArcherDetail.arrowType.explosive:
                        hitLock = true;
                        transform.tag = "ExplosiveArrow";
                        damage = (shoot.arrowSpeed) +500;
                        break;

                }
            }
        }
        if (counter > 60)
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
            Destroy(this.gameObject);
        }
        else if(poisonLock == true)
        {
            Instantiate(poisonExplosion, transform.position, Quaternion.identity);
            poisonLock = false;
            Destroy(this.gameObject);
        }
    }
}
