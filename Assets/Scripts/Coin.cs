using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    private bool isCollected = false;
    public int coinValue = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        // Play the collection animation
        if (anim != null)
        {
            anim.SetTrigger("CollectCoin");
        }
        AudioManager.Instance.Play("CoinPickUp");

        // Add coins to the inventory and trigger UI update
        Inventory.Instance.AddCoins(coinValue);

        // Destroy the coin object after animation
        Destroy(gameObject, 0.255f);
    }
}
