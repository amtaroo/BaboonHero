using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<GameObject> trashItems;
    private int collectedTrashItems = 0;
    public GameObject endGamePanel;

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

    void EndGame()
    {
        endGamePanel.SetActive(true);
    }
}
