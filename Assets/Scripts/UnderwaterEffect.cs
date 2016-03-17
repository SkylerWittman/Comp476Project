﻿using UnityEngine;
using System.Collections;

public class UnderwaterEffect : MonoBehaviour {

    private Transform playerPosition;
    private float waterLevel = -3.0f;
    private bool currentlyUnderwater = false;
    private Color underwaterColor = new Color(0.22f, 0.65f, 0.9f, 0.5f);

	void Start () {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update () {
        checkIfUnderwater();
	}

    void checkIfUnderwater()
    {
        if((playerPosition.position.y < waterLevel) != currentlyUnderwater) {
            currentlyUnderwater = playerPosition.position.y < waterLevel;
            if (currentlyUnderwater) { setInWater(); }
            if (!currentlyUnderwater) { setOutOfWater(); }
        }
    }

    void setOutOfWater()
    {
        RenderSettings.fog = false;
    }

    void setInWater()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.03f;
    }
}
