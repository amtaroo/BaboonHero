using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CompetitionModeManager : MonoBehaviour
{
    public List<GameObject> trashPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public float gameDuration = 60f;
    private float elapsedTime = 0f;
    private List<Transform> occupiedSpawnPoints = new List<Transform>();

    public int playerScore = 0;
    public int monkeyScore = 0;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI monkeyScoreText;
    public float competitionDuration = 60f;
    private float timeRemaining;
    public TextMeshProUGUI timerText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;


    void Start()
    {
        timeRemaining = competitionDuration;
        UpdateScoreUI();
        StartCoroutine(SpawnTrashItems());
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            EndCompetition();
        }
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

    public void AddPlayerScore(int points)
    {
        playerScore += points;
        UpdateScoreUI();
    }

    public void AddMonkeyScore(int points)
    {
        monkeyScore += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        playerScoreText.text = "Player: " + playerScore.ToString();
        monkeyScoreText.text = "Monkey: " + monkeyScore.ToString();
    }

    private void EndCompetition()
    {
        // หยุดเกมและแสดงผลลัพธ์
        Time.timeScale = 0; // หยุดการเคลื่อนไหวทั้งหมด
        resultPanel.SetActive(true);

        if (playerScore > monkeyScore)
        {
            resultText.text = "Player Wins!";
        }
        else if (monkeyScore > playerScore)
        {
            resultText.text = "Monkey Wins!";
        }
        else
        {
            resultText.text = "It's a Tie!";
        }
    }
}
