using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    public GameObject GameOverPanel;

    private bool isTimeFrozen;

    void Update()
    {
        if (!GameManager.Instance.IsTimeFrozen()) // ตรวจสอบว่าเวลาถูกหยุดหรือไม่
        {
            if (timeRemaining > 0)
            {
                timeElapsed += Time.deltaTime;
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                if (!GameManager.Instance.endGamePanel.activeSelf)
                {
                    Debug.Log("Time's up!");
                    GameOverPanel.SetActive(true);
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("Time : " + "{0:00}:{1:00}", minutes, seconds);
    }

    public void SetTimeFrozen(bool isFrozen)
    {
        isTimeFrozen = isFrozen;
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
