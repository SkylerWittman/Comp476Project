using UnityEngine;
using System.Collections;

public class poisonExplosion : MonoBehaviour {
    int gasTime;
	// Use this for initialization
	void Start () {
        gasTime = 350;
	}
	
	// Update is called once per frame
	void Update () {

        if (gasTime < 1)
        {
            Destroy(this.gameObject);
        }
        --gasTime;
    }
}
