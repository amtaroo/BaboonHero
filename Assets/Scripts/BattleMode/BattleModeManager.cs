using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleModeManager : MonoBehaviour
{
    public static BattleModeManager Instance { get; private set; }

    public List<GameObject> trashPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public float gameDuration = 60f;
    private float elapsedTime = 0f;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI monkeyScoreText;
    public TextMeshProUGUI finalPlayerScoreText;
    public TextMeshProUGUI finalMonkeyScoreText;
    public GameObject endGamePanel;

    public GameObject startPanel; 
    public Button startButton; 

    public bool isDoublePointsActive = false;
    public float doublePointsDuration = 10f;

    public PlayerScoreManager playerScoreManager;
    public MonkeyScoreManager monkeyScoreManager;

    private List<Transform> occupiedSpawnPoints = new List<Transform>();

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

        startPanel.SetActive(true);
        endGamePanel.SetActive(false);

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void OnStartButtonClicked()
    {
        startPanel.SetActive(false);
        UpdateScoreText();
        StartCoroutine(SpawnTrashItems());
        StartCoroutine(GameTimer());
    }

    IEnumerator SpawnTrashItems()
    {
        while (elapsedTime < gameDuration)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnTrash();
        }
    }

    void SpawnTrash()
    {
        if (trashPrefabs.Count > 0 && spawnPoints.Length > 0)
        {
            Transform spawnPoint = GetAvailableSpawnPoint();

            if (spawnPoint != null)
            {
                GameObject trashToSpawn = trashPrefabs[Random.Range(0, trashPrefabs.Count)];
                Instantiate(trashToSpawn, spawnPoint.position, spawnPoint.rotation);
                occupiedSpawnPoints.Add(spawnPoint);
            }
        }
    }

    Transform GetAvailableSpawnPoint()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (var occupiedPoint in occupiedSpawnPoints)
        {
            availableSpawnPoints.Remove(occupiedPoint);
        }

        if (availableSpawnPoints.Count > 0)
        {
            return availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
        }

        return null;
    }

    public void CollectTrash(bool correctDisposal, bool isMonkey)
    {
        int points = correctDisposal ? 10 : -20;

        if (isMonkey)
        {
            monkeyScoreManager.AddScore(points);
        }
        else
        {
            playerScoreManager.AddScore(points);
        }

        UpdateScoreText();
    }

    IEnumerator GameTimer()
    {
        while (elapsedTime < gameDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndGame();
    }

    void UpdateScoreText()
    {
        playerScoreText.text = "Player Score : " + playerScoreManager.GetScore();
        monkeyScoreText.text = "Monkey Score : " + monkeyScoreManager.GetScore();
    }

    void EndGame()
    {
        finalPlayerScoreText.text = "Player Final Score: " + playerScoreManager.GetScore();
        finalMonkeyScoreText.text = "Monkey Final Score: " + monkeyScoreManager.GetScore();

        endGamePanel.SetActive(true);
    }
}
