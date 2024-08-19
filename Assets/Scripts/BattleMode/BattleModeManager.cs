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

    public int playerScore = 0;
    public int monkeyScore = 0;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI monkeyScoreText;
    public TextMeshProUGUI finalPlayerScoreText;
    public TextMeshProUGUI finalMonkeyScoreText;

    public bool isDoublePointsActive = false;
    public float doublePointsDuration = 10f;

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
    }

    void Start()
    {
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

        if (isDoublePointsActive)
        {
            points *= 2;
        }

        if (isMonkey)
        {
            monkeyScore += points;
        }
        else
        {
            playerScore += points;
        }

        UpdateScoreText();
    }

    public void ActivateDoublePoints()
    {
        if (!isDoublePointsActive)
        {
            isDoublePointsActive = true;
            StartCoroutine(DoublePointsTimer());
        }
    }

    private IEnumerator DoublePointsTimer()
    {
        yield return new WaitForSeconds(doublePointsDuration);
        isDoublePointsActive = false;
    }

    void UpdateScoreText()
    {
        playerScoreText.text = "Player Score : " + playerScore;
        monkeyScoreText.text = "Monkey Score : " + monkeyScore;
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

    void EndGame()
    {
        // ปิดการทำงานทั้งหมดในเกมและแสดงผลคะแนน
    }
}
