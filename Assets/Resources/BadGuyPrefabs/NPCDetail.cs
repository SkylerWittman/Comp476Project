﻿using UnityEngine;
using System.Collections;

public class NPCDetail : MonoBehaviour {
    bool LockA = false;
    Animation anim;
    public AnimationClip die;
    public AnimationClip hit;
    public float health;
    public float damage;
    int counter = 0;
    public int deathScore = 10;
    private bool addedScore = false;

    private AudioSource audioSource;

    private AudioClip hitSound;

    private bool triedSpawning = false;
    private PowerupSpawner powerupSpawner;

    public bool isAlive;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
        anim[die.name].layer = 2;
        anim[die.name].speed = .7f;
        anim[hit.name].layer = 3;

        hitSound = Resources.Load("Sounds/GeneralSounds/Hit") as AudioClip;

        powerupSpawner = GameObject.FindGameObjectWithTag("PowerupSpawner").GetComponent<PowerupSpawner>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (LockA == true)
        {
            if (counter > 10)
            {
                health -= 10.0f;
                counter = 0;
            }
            counter++; 
        }

        Vector3 pos = transform.position;
      
		if(health <= 0.0f && isAlive)
        {
			isAlive = false;
			anim.Play(die.name);
         
            transform.position = pos;

            //Freeze all movement/rotations when NPC has been slain
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			GetComponent<Collider> ().enabled = false;


			if (!triedSpawning)
            {
                powerupSpawner.trySpawn(pos);
                triedSpawning = true;
            }

            if (!addedScore)
            {
                GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(deathScore);
                addedScore = true;
            }

			StatePattern goblinFSM = GetComponent<StatePattern>(); //checks to see if the prefab this script is attached to is a goblin, if it is, disable the script so it doesnt respond to any more damage

			if (goblinFSM != null)
			{
				goblinFSM.enabled = false;
			}
           
			Destroy(this.gameObject, anim[die.name].length+.5f);
        }

    }
    void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
		case "RegularArrow":
			health -= other.GetComponent<Arrow> ().damage;
				anim.CrossFade(hit.name, 0.5f);
                playHitSound();
                Destroy(other.gameObject);
                break;
            case "PoisonArrow":
                health -= other.GetComponent<Arrow>().damage;
                anim.CrossFade(hit.name, 0.5f);
                LockA = true;
                break;
            case "ExplosiveArrow":
             
                anim.CrossFade(hit.name, 0.5f);
                break;
            case "Gas":
                LockA = true;
                break;
        }


        
    }

    private void playHitSound()
    {
        audioSource.PlayOneShot(hitSound, 0.1f);
    }

}
