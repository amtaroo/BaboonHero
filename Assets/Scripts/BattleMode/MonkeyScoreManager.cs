using UnityEngine;

public class MonkeyScoreManager : MonoBehaviour
{
    private int monkeyScore = 0;

    public void AddScore(int points)
    {
        monkeyScore += points;
    }

    public int GetScore()
    {
        return monkeyScore;
    }
}
