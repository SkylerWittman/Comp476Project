using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position - transform.up * 10.0f, 10.0f, transform.up, 100f);
        if (hit != null)
        {
            Debug.Log(hit.Length);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.gameObject.tag == "Player")
                {
                    Debug.Log("Totally");
                    hit[i].transform.GetComponent<Rigidbody>().AddExplosionForce(50, transform.position, 0, 50);
                }
            }

        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
