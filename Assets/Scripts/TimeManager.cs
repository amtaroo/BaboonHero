using System.Collections;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float timeLimit = 10f; // Time limit in seconds
    public TextMeshProUGUI warningText; // Reference to the TextMeshProUGUI component for displaying messages
    private bool isCounting; // Flag to check if counting time

    void Start()
    {
        isCounting = false;
        warningText.gameObject.SetActive(false); // Hide the warning text initially
    }

    public void StartTrashSorting()
    {
        if (!isCounting)
        {
            StartCoroutine(CountdownTimer());
        }
    }

    IEnumerator CountdownTimer()
    {
        isCounting = true;
        float timer = 0f;
        while (timer < timeLimit)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // Time's up logic here
        ShowWarningMessage("Time's up!", 1f, Color.white);
        isCounting = false;
    }

    public void ShowWarningMessage(string message, float duration, Color textColor)
    {
        StartCoroutine(ShowMessage(message, duration, textColor));
    }

    IEnumerator ShowMessage(string message, float duration, Color textColor)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
        warningText.color = textColor;
        yield return new WaitForSeconds(duration);
        warningText.gameObject.SetActive(false);
    }
}
