﻿using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
    public RenderTexture MiniMapTexture;
    public Material MiniMapMaterial;
    private float offset;

	// Use this for initialization
	void Awake () {
        offset = 10;
	}
	
	// Update is called once per frame
	void OnGUI () {
        if(Event.current.type == EventType.Repaint)
            Graphics.DrawTexture(new Rect(0, 0, 256, 256), MiniMapTexture, MiniMapMaterial);

	}
}
