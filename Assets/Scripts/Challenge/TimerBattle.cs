
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerBattle : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;
    private float timeElapsed;

    public Button startButton;

    void Start()
    {
        timerText.gameObject.SetActive(false);

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

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
            if (!BattleModeManager.Instance.endGamePanel.activeSelf)
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

    void OnStartButtonClicked()
    {
        timerText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerBattle : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    private bool isTimerRunning = false;

    public Button startButton;

    void Start()
    {
        timerText.gameObject.SetActive(false);
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void Update()
    {
        if (isTimerRunning && timeRemaining > 0) // ตรวจสอบว่าตัวจับเวลาถูกเริ่มหรือไม่
        {
            timeElapsed += Time.deltaTime;
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else if (timeRemaining <= 0 && isTimerRunning) // ตรวจสอบว่าเวลาหมดแล้วและตัวจับเวลากำลังทำงานอยู่หรือไม่
        {
            isTimerRunning = false; // หยุดการจับเวลาเมื่อเวลาหมด
            if (!BattleModeManager.Instance.endGamePanel.activeSelf)
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

    void OnStartButtonClicked()
    {
        timerText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        isTimerRunning = true; // เริ่มจับเวลาเมื่อกดปุ่ม Start
    }
}
