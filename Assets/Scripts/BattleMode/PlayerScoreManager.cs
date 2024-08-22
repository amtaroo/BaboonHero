using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private int playerScore = 0;

    public void AddScore(int points)
    {
        if (points > 0)
        {
            playerScore += points;
            PlayerPrefs.SetInt("PlayerScore", playerScore);
            PlayerPrefs.Save();
        }
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void LoadScore()
    {
        playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
    }
}
