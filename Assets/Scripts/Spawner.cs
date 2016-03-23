using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    //WaveEngine vars
    private WaveEngine waveEngine;

    //Wave vars
    private int currentWaveCount = -1;

    //WaveCountText vars
    private TextMesh waveCountText;

    //Timer vars
    private Timer timer;


    void Start()
    {
        StartCoroutine(initialize());
    }

    //Spawns occur:
    //Every 60 seconds and not at startTime, nor when the game ends
    private void trySpawn()
    {
        if (timer.getCurrentTime() % 60 == 0 && timer.getCurrentTime() != 600 && timer.getCurrentTime() != 0)
        {
            waveEngine.spawnBadGuys(new Wave(++currentWaveCount));
            StartCoroutine(showWaveText());
        }
    }

    private IEnumerator initialize()
    {
        yield return new WaitForSeconds(1.0f);
        
        waveEngine = GetComponent<WaveEngine>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        waveCountText = GameObject.FindGameObjectWithTag("WaveCount").GetComponent<TextMesh>();

        //This is our initial spawn of 50 bad guys
        waveEngine.spawnBadGuys(new Wave(++currentWaveCount));

        //Try every second to see if we can spawn bad guys
        InvokeRepeating("trySpawn", 0.0f, 1.0f);
    }

    private IEnumerator showWaveText()
    {
        waveCountText.GetComponent<Renderer>().enabled = true;
        waveCountText.text = "WAVE " + currentWaveCount;
        yield return new WaitForSeconds(5.0f);
        waveCountText.GetComponent<Renderer>().enabled = false;
    }
}
