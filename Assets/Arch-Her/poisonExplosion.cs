using UnityEngine;
using System.Collections;

public class poisonExplosion : MonoBehaviour {
    public int gasTime = 300;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (gasTime < 0)
        {
            Destroy(this.gameObject);
        }
        --gasTime;
    }
}
