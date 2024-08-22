/*using System.Collections;
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

    private bool isTimeFrozen = true;

    void Start()
    {
        timerText.gameObject.SetActive(false);

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void Update()
    {
        if (!isTimeFrozen)
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


    public void SetTimeFrozen(bool frozen)
    {
        isTimeFrozen = frozen;
    }

    void OnStartButtonClicked()
    {
        isTimeFrozen = false; 
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
