using UnityEngine;
using System.Collections;

public class Wave {

    //SAMPLE WAVE BAD GUYS COUNTS
    //(WE INITIALLY START WITH 50 BAD GUYS)
    //WAVE 1: 10
    //WAVE 2: 16
    //WAVE 3: 22
    //WAVE 4: 30
    //. . .
    //WAVE 10: 100 

    public int waveNumber { get; set; }
    public int badGuyCount { get; set; }

    public Wave(int num)
    {
        waveNumber = num;
        if (waveNumber == 0)
        {
            badGuyCount = 50;
        }
        else
        {
            badGuyCount = (waveNumber * ((waveNumber - 1) + 2) + 10);
        }
    }
}
