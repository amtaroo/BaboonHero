using UnityEngine;

public class MonkeyScoreManager : MonoBehaviour
{
    private int monkeyScore = 0;

    public void AddScore(int points)
    {
        if (points > 0)
        {
            monkeyScore += points;
            PlayerPrefs.SetInt("MonkeyScore", monkeyScore);
            PlayerPrefs.Save();
        }
    }

    public int GetScore()
    {
        return monkeyScore;
    }

    public void LoadScore()
    {
        monkeyScore = PlayerPrefs.GetInt("MonkeyScore", 0);
    }
}
