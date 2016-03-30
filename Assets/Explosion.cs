using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position - transform.up * 10.0f, 10.0f, transform.up, 100f);
        if (hit != null)
        {
            Debug.Log(hit.Length);

            for (int i = 0; i < hit.Length; i++)
            {
                switch (hit[i].transform.gameObject.tag)
                {
                    case "BadGuy":
                        hit[i].transform.GetComponent<NPCDetail>().health -= Vector3.Distance(transform.position, hit[i].transform.position) *20f;
                        break;
                    case "SwarmGoblin":
                        hit[i].transform.GetComponent<SwarmGoblinDeath>().health -= Vector3.Distance(transform.position, hit[i].transform.position) * 20f;
                        break;
                }

            }

           Destroy(this.gameObject, 1f);

        }
    }
}
