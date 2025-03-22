using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

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
    [SerializeField]
    private List<DialogueTrigger> dialogueTriggers; // List of DialogueTriggers
    public int coins = 100;
    public UnityEvent<int> OnCoinsUpdated = new UnityEvent<int>(); // Event for UI updates
    private SkillsTreeButton[] selectedAbilities = new SkillsTreeButton[4];

    //special-0 basic2-1 basic1-2 movement-3
    public void AddSelectedAbility(SkillsTreeButton ability, string name)
    {
        if (name == "roll" || name == "slide")
        {
            selectedAbilities[3] = ability;
        }
        else if (name == "swipe" || name == "slash")
        {
            selectedAbilities[1] = ability;
        }
        else if (name == "pummel" || name == "kick")
        {
            selectedAbilities[2] = ability;
        }
        else if (name == "slam" || name == "spin")
        {
            selectedAbilities[0] = ability;
        }
    }

    public bool ContainsAbility(string name)
    {
        for (int i = 0; i < selectedAbilities.Length; i++)
        {
            if (selectedAbilities[i] == null)
            {
                continue;
            }
            if (selectedAbilities[i].getName() == name)
            {
                return true;
            }
        }
        return false;
    }

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

    public int getCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        if (coins == 0)
        {
            DialogueManager.TriggerDialogue("FirstCoin", dialogueTriggers);
        }
        coins += amount;

        // Trigger UI update event
        OnCoinsUpdated.Invoke(coins);

        Debug.Log("Coins: " + coins);
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;
        if (coins < 0)
        {
            coins = 0;
        }

        // Trigger UI update event
        OnCoinsUpdated.Invoke(coins);
    }
}