using UnityEngine;
using System.Collections;

public class SwarmGoblinDeath : MonoBehaviour {

	bool LockA = false;
	private bool isDead = false;
	Animation anim;
	public AnimationClip die;
	public float health = 100.0f;
	public float damage = 20.0f;
	int counter = 0;
	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animation>();
		anim[die.name].layer = 2;
		anim[die.name].speed = .7f;
	}

	// Update is called once per frame
	void Update()
	{

		if (LockA == true)
		{
			if (counter < 100)
			{
				health -= 50.0f;
				++counter;
			}
			else
			{
				LockA = false;
				counter = 0;
			}
		}
		//Vector3 pos = transform.position;

		if(health < 0.0f && !isDead)
		{

			isDead = true;
			anim.CrossFade(die.name, 0.5f);
			//GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); ;
			//transform.position = pos;

			//Freeze all movement/rotations when NPC has been slain
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			GetComponent<SwarmGoblinBehavior> ().InformSwarmMembersOfDeath (this.gameObject);
			Destroy(this.gameObject, anim[die.name].length+3.0f);

		}

	}
	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
		case "RegularArrow":
			health -= other.GetComponent<Arrow>().damage;
			break;
		case "PoisionArrow":
			health -= other.GetComponent<Arrow>().damage;
			LockA = true;
			break;
		case "ExplosiveArrow":
			//No clue yet
			break;
		}


		Destroy(other.gameObject);
	}

}

