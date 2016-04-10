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
        if (isAlive && health <= 0.0f)
        {
            isAlive = false;
            GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(deathScore);
        }
        if (LockA == true)
        {
            if (counter > 5)
            {
                health -= 10.0f;
                counter = 0;
            }
            counter++; 
        }
        Vector3 pos = transform.position;
        if(health <= 0.0f)
        {
            anim.Play(die.name);
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); ;
            transform.position = pos;
            //Freeze all movement/rotations when NPC has been slain
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (!triedSpawning)
            {
                powerupSpawner.trySpawn(pos);
                triedSpawning = true;
            }

            GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(deathScore);
            isAlive = false;
			Destroy(this.gameObject, anim[die.name].length+.5f);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        DinoBehavior dinoBehave = GetComponent<DinoBehavior>();
        if (dinoBehave != null)
        {
            dinoBehave.gotHit();
        }
        

        switch (other.gameObject.tag)
        {
		case "RegularArrow":
			health -= other.GetComponent<Arrow> ().damage;
				anim.CrossFade(hit.name, 0.5f);
                playHitSound();
                Destroy(other.gameObject);
                break;
                //misspelled poison...
            case "PoisonArrow":
                health -= other.GetComponent<Arrow>().damage;
                anim.CrossFade(hit.name, 0.5f);
                LockA = true;
                break;
            case "ExplosiveArrow":
                Destroy(this.gameObject, anim[die.name].length + .5f);
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
