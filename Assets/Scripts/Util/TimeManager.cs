using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static bool isTimeStopped;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        isTimeStopped = false;
    }

    public void StopTime() {
        Time.timeScale = 0f;
        isTimeStopped = true;
    }

    public void ResumeTime() {
        Time.timeScale = 1f;
        isTimeStopped = false;
    }
}
