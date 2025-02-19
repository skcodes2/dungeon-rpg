using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    Inventory inventory;
    private bool isCollected = false;
    public int coinValue = 1;

    void Awake()
    {
        inventory = Inventory.Instance;
    }

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
        anim.SetTrigger("CollectCoin");
        inventory.AddCoins(coinValue);

        // Destroy the coin object after the animation has played
        Destroy(gameObject, 0.255f);
    }
}