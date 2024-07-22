using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer_Challenge : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    public GameObject GameOverPanel;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeElapsed += Time.deltaTime;
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            if (!ChallengeModeManager.Instance.endGamePanel.activeSelf)
            {
                Debug.Log("Time's up!");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }
}

