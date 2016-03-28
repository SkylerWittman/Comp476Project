using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public float damage;
    int counter = 0;
    public ArcherDetail archer;
    bool lockA = true;

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
    void OnCollisionEnter(Collision Collision)
    {
        Debug.Log("collide");
    }
}
