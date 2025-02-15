using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Inventory>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("Inventory");
                    _instance = obj.AddComponent<Inventory>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    public List<string> items = new List<string>();
    public int coins = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        print("Coins: " + coins);
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;
        if (coins < 0)
        {
            coins = 0;
        }
    }

    public void AddItem(string item)
    {
        items.Add(item);
    }

    public void RemoveItem(string item)
    {
        items.Remove(item);
    }

    public bool HasItem(string item)
    {
        return items.Contains(item);
    }
}