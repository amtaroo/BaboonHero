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


    public Button startButton; // Reference to the Start Button

    private bool isTimeFrozen = true; // Initially frozen until Start is pressed

    void Start()
    {
        // Initially hide the timer text
        timerText.gameObject.SetActive(false);

        // Add a listener to the start button
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
                    // Trigger the game over panel or other end-game logic

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

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    // Time Freeze control
    public void SetTimeFrozen(bool frozen)
    {
        isTimeFrozen = frozen;
    }

    // Called when the start button is clicked
    void OnStartButtonClicked()
    {
        isTimeFrozen = false; // Unfreeze the timer to start counting down
        timerText.gameObject.SetActive(true); // Show the timer text
        startButton.gameObject.SetActive(false); // Hide the start button after it's pressed
    }
}
