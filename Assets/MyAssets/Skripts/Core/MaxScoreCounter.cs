using TMPro;
using UnityEngine;

public class MaxScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text maxScoreCount;

    public void UpdateMaxScore(int score)
    {
        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);

        if (score > maxScore)
        {
            maxScore = score;
            PlayerPrefs.SetInt("MaxScore", maxScore);
        }

        maxScoreCount.text = maxScore.ToString();
    }

    private void Start()
    {
        maxScoreCount.text = PlayerPrefs.GetInt("MaxScore", 0).ToString();
    }
}