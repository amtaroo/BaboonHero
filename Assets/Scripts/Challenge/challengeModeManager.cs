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
            GameObject trashToSpawn = trashPrefabs[Random.Range(0, trashPrefabs.Count)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            Instantiate(trashToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void CollectTrash(bool correctDisposal)
    {
        collectedTrashItems++;
        if (correctDisposal)
        {
            score += 10; 
        }
        else
        {
            score -= 20; 
        }
        UpdateScoreText(); 
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
