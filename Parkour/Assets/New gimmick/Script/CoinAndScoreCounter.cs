using UnityEngine;

public class CoinAndScoreCounter : MonoBehaviour
{
    public static CoinAndScoreCounter instance;
    public bool doubleCoins;
    int amount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int totalCoins;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (doubleCoins)
        {
            amount = 2;
        }

        else 
        {
            amount = 1;
        }
    }

    // Update is called once per frame
    public void AddCoins()
    {
        totalCoins = amount + totalCoins;
    }

}
