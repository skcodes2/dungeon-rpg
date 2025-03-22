using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coinText; // Assign this in Inspector
    [SerializeField] private Image coinImage; // Assign your coin sprite UI here

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnCoinsUpdated.AddListener(UpdateCoinUI);
            UpdateCoinUI(Inventory.Instance.getCoins()); // Initialize UI
        }
        else
        {
            Debug.LogError("Inventory.Instance is NULL!");
        }
    }

    public void UpdateCoinUI(int coins)
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString(); // Update coin count
        }
        else
        {
            Debug.LogError("CoinText is NOT assigned in UIManager! Assign it in the Inspector.");
        }
    }
}
