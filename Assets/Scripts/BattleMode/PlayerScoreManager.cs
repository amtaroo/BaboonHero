using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private int playerScore = 0;

    public void AddScore(int points)
    {
        playerScore += points;
    }

    public int GetScore()
    {
        return playerScore;
    }
}
