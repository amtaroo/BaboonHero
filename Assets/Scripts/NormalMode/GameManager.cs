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
    }

    public void CollectTrash()
    {
        collectedTrashItems++;
        if (collectedTrashItems >= trashItems.Count)
        {
            EndGame();
        }
    }
    public void GameOver()
    {

        Time.timeScale = 0; 
        GameOverPanel.SetActive(true);
    }

    void EndGame()
    {
        endGamePanel.SetActive(true);
        UnlockNewLevel();
        UnityEngine.Debug.Log("Next Level Unlock");
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
