using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {
    //GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(spiderScore);
    public int score;
    public int totalScore;
    public int highScore;
    // Use this for initialization
    void Start () {
        score = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().score;
        highScore = PlayerPrefs.GetInt("High Score");
    }
	
	// Update is called once per frame
	void Update () {
        score = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().score;

        if(score > highScore)
        {
            highScore = score;
           
            PlayerPrefs.SetInt("High Score", highScore);
            PlayerPrefs.Save();
            Debug.Log("High Score is: " + highScore);
        }

    }
}
