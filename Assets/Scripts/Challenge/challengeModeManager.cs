using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class ChallengeModeManager : MonoBehaviour
{
    public static ChallengeModeManager Instance { get; private set; }

    public List<GameObject> trashPrefabs;
    private int collectedTrashItems = 0;
    public GameObject endGamePanel;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public float gameDuration = 60f;
    private float elapsedTime = 0f;

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

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
        endGamePanel.SetActive(false);
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

    Transform GetAvailableSpawnPoint() //ขยะจะไม่spawnในตำแหน่งที่มีขยะอยู่แล้ว
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

    public void CollectTrash(bool correctDisposal)
    {
        collectedTrashItems++;
        int points = correctDisposal ? 10 : -20;

        if (isDoublePointsActive) //Point x2
        {
            points *= 2;
        }

        score += points;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoublePointsItem"))
        {
            ActivateDoublePoints();
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator DoublePointsTimer()
    {
        yield return new WaitForSeconds(doublePointsDuration);
        isDoublePointsActive = false;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score : " + score;
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
        endGamePanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
        finalScoreText.text = "Score : " + score;
        StopAllCoroutines();
    }
}
