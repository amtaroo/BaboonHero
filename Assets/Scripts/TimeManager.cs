using System.Collections;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float timeLimit = 10f;
    public TextMeshProUGUI warningText;
    private bool isCounting; 

    void Start()
    {
        isCounting = false;
        warningText.gameObject.SetActive(false);
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
