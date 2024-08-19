using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<GameObject> trashItems;
    private int collectedTrashItems = 0;
    public GameObject endGamePanel;
    public GameObject GameOverPanel;
    private bool isTimeFrozen = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        endGamePanel.SetActive(false);
        collectedTrashItems = 0;

        trashItems = new List<GameObject>(GameObject.FindGameObjectsWithTag("Trash"));
    }

    public void SetTimeFrozen(bool frozen)
    {
        isTimeFrozen = frozen;
    }

    public bool IsTimeFrozen()
    {
        return isTimeFrozen;
    }

    public void CollectTrash()
    {
        collectedTrashItems++;
        Debug.Log("Collected Trash: " + collectedTrashItems);
        Debug.Log("Total Trash: " + trashItems.Count);

        if (collectedTrashItems >= trashItems.Count)
        {
            EndGame();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
        SetTimeFrozen(true);
        Timer.Instance.HideTimer();
    }

    void EndGame()
    {
        Debug.Log("EndGame called");
        endGamePanel.SetActive(true);
        
        // หยุดเวลาและซ่อนตัวจับเวลา
        SetTimeFrozen(true);
        Timer.Instance.HideTimer();

        UnlockNewLevel();
        Debug.Log("Next Level Unlock");
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("UnlockedLevel"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
