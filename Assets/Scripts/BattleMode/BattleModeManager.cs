
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BattleModeManager : MonoBehaviour
{
    public static BattleModeManager Instance { get; private set; }

    public List<GameObject> trashPrefabs;
    public Transform[] spawnPointsSet1;
    public Transform[] spawnPointsSet2;
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
    public TextMeshProUGUI previousPlayerScoreText;
    public TextMeshProUGUI previousMonkeyScoreText;

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

        playerScoreManager = GameObject.FindObjectOfType<PlayerScoreManager>();
        monkeyScoreManager = GameObject.FindObjectOfType<MonkeyScoreManager>();

        startPanel.SetActive(true);
        endGamePanel.SetActive(false);
        playerScoreText.gameObject.SetActive(false);
        monkeyScoreText.gameObject.SetActive(false);

        int previousPlayerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        int previousMonkeyScore = PlayerPrefs.GetInt("MonkeyScore", 0);
        previousPlayerScoreText.text = "Player Score: " + previousPlayerScore;
        previousMonkeyScoreText.text = "Monkey Score: " + previousMonkeyScore;

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void OnStartButtonClicked()
    {
        startPanel.SetActive(false);
        playerScoreText.gameObject.SetActive(true);
        monkeyScoreText.gameObject.SetActive(true);
        UpdateScoreText();
        StartCoroutine(SpawnTrashItems());
        StartCoroutine(GameTimer());
    }
//SpawnTrash
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
        if (trashPrefabs.Count > 0)
        {
            Transform spawnPointSet1 = GetAvailableSpawnPoint(spawnPointsSet1);
            Transform spawnPointSet2 = GetAvailableSpawnPoint(spawnPointsSet2);

            if (spawnPointSet1 != null)
            {
                SpawnTrashAtPoint(spawnPointSet1);
            }

            if (spawnPointSet2 != null)
            {
                SpawnTrashAtPoint(spawnPointSet2);
            }
        }
    }

    Transform GetAvailableSpawnPoint(Transform[] spawnPoints)
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

    void SpawnTrashAtPoint(Transform spawnPoint)
    {
        GameObject trashToSpawn = trashPrefabs[Random.Range(0, trashPrefabs.Count)];
        Instantiate(trashToSpawn, spawnPoint.position, spawnPoint.rotation);
        occupiedSpawnPoints.Add(spawnPoint);
    }
//EndSpawnTrash

    public void AddMonkeyScore(int points)
    {
        monkeyScoreManager.AddScore(points);
        UpdateScoreText();
    }

    public void AddPlayerScore(int points)
    {
        playerScoreManager.AddScore(points);
        UpdateScoreText();
    }

    public void CollectTrash(bool correctDisposal, bool isMonkey)
    {
        int points = 10;
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
        playerScoreText.text = "Player Score: " + playerScoreManager.GetScore();
        monkeyScoreText.text = "Monkey Score: " + monkeyScoreManager.GetScore();
    }

    void EndGame()
    {
        finalPlayerScoreText.text = "Player Final Score: " + playerScoreManager.GetScore();
        finalMonkeyScoreText.text = "Monkey Final Score: " + monkeyScoreManager.GetScore();

        PlayerPrefs.SetInt("PlayerScore", playerScoreManager.GetScore());
        PlayerPrefs.SetInt("MonkeyScore", monkeyScoreManager.GetScore());
        PlayerPrefs.Save();

        endGamePanel.SetActive(true);
        playerScoreText.gameObject.SetActive(false);
        monkeyScoreText.gameObject.SetActive(false);
    }
}
