using UnityEngine;
using System.Collections;

public class ArcherDetail : MonoBehaviour {
    public enum arrowType { regular, poison, explosive}
    public arrowType arrow_type;
	// Use this for initialization
	void Start () {
        arrow_type = arrowType.regular;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
