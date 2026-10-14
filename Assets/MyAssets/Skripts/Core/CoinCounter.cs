using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private int coins;

    private void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coins.ToString();
    }

    public void AddCoin()
    {
        coins++;
        PlayerPrefs.SetInt("Coins", coins);
        coinText.text = coins.ToString();
    }
}