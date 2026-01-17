using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    

    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            CoinAndScoreCounter.instance.AddCoins();
        }
    }
}

