using UnityEngine;
using System.Collections;

public class ArcherDetail : MonoBehaviour {
    public enum arrowType { regular, poison, explosive}
    public arrowType arrow_type;

	public int health;
	public int playerHealthCritical;
	// Use this for initialization
	void Start () {
        arrow_type = arrowType.regular;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("poison"))
		{
			arrow_type = arrowType.poison;
		}

		if (col.gameObject.CompareTag("explosive"))
		{
			arrow_type = arrowType.explosive;
		}
	}
}
