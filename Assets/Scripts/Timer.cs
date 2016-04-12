using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private Text timerText;
    private const int startTime = 300;
    private int currentTime;

	void Start () {
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        timerText.text = convertToTime(startTime);
        currentTime = startTime;
        
        InvokeRepeating("countDown", 0.0f, 1.0f);
	}

    private string convertToTime(int seconds) 
    {
        int m = seconds / 60;
        int s = seconds % 60;
        string mins = (m < 10 ? "0" + m.ToString() : m.ToString());
        string secs = (s < 10 ? "0" + s.ToString() : s.ToString());
        return mins + ":" + secs;
    }

    private void countDown()
    {
        timerText.text = convertToTime(--currentTime);
    }

    public int getStartTime() { return startTime; }

    public int getCurrentTime() { return currentTime; }
}
