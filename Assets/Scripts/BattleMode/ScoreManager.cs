using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public int playerScore = 0;
    public int monkeyScore = 0;

    public void AddScore(int points, bool isMonkey)
    {
        if (isMonkey)
        {
            monkeyScore += points;
        }
        else
        {
            playerScore += points;
        }
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetMonkeyScore()
    {
        return monkeyScore;
    }
}

