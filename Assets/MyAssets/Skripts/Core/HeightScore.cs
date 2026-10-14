using UnityEngine;
using UnityEngine.UI;

public class HeightScore : MonoBehaviour
{
    [SerializeField] private MaxScoreCounter maxScoreManager;


    [SerializeField] private Transform player;
    [SerializeField] private float heightPerPoint = 30f;

    [SerializeField] private Image[] digitImages = new Image[4];
    [SerializeField] private Sprite[] digitSprites = new Sprite[10];

    private float startY;
    private float highestY;
    private int score;

    private void Start()
    {
        startY = player.position.y;
        highestY = startY;
        score = 0;

        UpdateDisplay();
    }

    private void Update()
    {
        if (player.position.y > highestY)
        {
            highestY = player.position.y;

            int newScore = Mathf.FloorToInt((highestY - startY) / heightPerPoint * 10f);

            newScore = Mathf.Clamp(newScore, 0, 9999);

            if (newScore != score)
            {
                score = newScore;
                UpdateDisplay();
            }
        }
    }

    private void UpdateDisplay()
    {
        string scoreText = score.ToString();

        int firstVisibleIndex = digitImages.Length - scoreText.Length;

        for (int i = 0; i < digitImages.Length; i++)
        {
            bool showDigit = i >= firstVisibleIndex;

            if (showDigit)
            {
                int digit = scoreText[i - firstVisibleIndex] - '0';
                digitImages[i].sprite = digitSprites[digit];
            }

            digitImages[i].gameObject.SetActive(showDigit);
        }
    }

    public void SaveMaxScore()
    {
        maxScoreManager.UpdateMaxScore(score);
    }
}
