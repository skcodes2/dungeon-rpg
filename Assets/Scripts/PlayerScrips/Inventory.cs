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
    private int coins = 0;

    public List<SkillsTreeButton> skillTreeButtons = new List<SkillsTreeButton>();

    private bool firstCoin = true;
    public UnityEvent<int> OnCoinsUpdated = new UnityEvent<int>(); // Event for UI updates
    public SkillsTreeButton[] selectedAbilities;

    //special-0 basic2-1 basic1-2 movement-3
    public void AddSelectedAbility(SkillsTreeButton ability, string name)
    {
        if (name == "roll" || name == "slide")
        {
            this.selectedAbilities[3] = ability;
        }
        else if (name == "swipe" || name == "slash")
        {
            this.selectedAbilities[1] = ability;
        }
        else if (name == "pummel" || name == "kick")
        {
            this.selectedAbilities[2] = ability;
        }
        else if (name == "slam" || name == "spin")
        {
            this.selectedAbilities[0] = ability;
        }
        for (int i = 0; i < this.selectedAbilities.Length; i++)
        {
            if (this.selectedAbilities[i] == null)
            {
                continue;
            }
            Debug.Log("Selected Ability Added: " + this.selectedAbilities[i].getName());
        }
    }

    public bool ContainsAbility(string name)
    {
        for (int i = 0; i < this.selectedAbilities.Length; i++)
        {
            if (this.selectedAbilities[i] == null)
            {
                continue;
            }
            if (this.selectedAbilities[i].getName() == name)
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
        // Attack Line
        SkillsTreeButton pummel = new SkillsTreeButton(true, false, 20, "pummel", null);
        SkillsTreeButton atk10 = new SkillsTreeButton(false, false, 15, "atk10", pummel);
        SkillsTreeButton atk5 = new SkillsTreeButton(false, false, 10, "atk5", atk10);
        SkillsTreeButton atk1 = new SkillsTreeButton(false, true, 5, "atk1", atk5);

        // Speed Line
        SkillsTreeButton slide = new SkillsTreeButton(true, false, 20, "slide", null);
        SkillsTreeButton roll = new SkillsTreeButton(true, false, 15, "roll", slide);
        SkillsTreeButton speed3 = new SkillsTreeButton(false, false, 10, "speed3", roll);
        SkillsTreeButton speed1 = new SkillsTreeButton(false, true, 5, "speed1", speed3);

        // Defense Line
        SkillsTreeButton spin = new SkillsTreeButton(true, false, 20, "spin", null);
        SkillsTreeButton slash = new SkillsTreeButton(true, false, 15, "slash", spin);
        SkillsTreeButton armour5 = new SkillsTreeButton(false, false, 10, "armour5", slash);
        SkillsTreeButton armour2 = new SkillsTreeButton(false, true, 5, "armour2", armour5);

        // Health Line
        SkillsTreeButton slam = new SkillsTreeButton(true, false, 20, "slam", null);
        SkillsTreeButton swipe = new SkillsTreeButton(true, false, 15, "swipe", slam);
        SkillsTreeButton hp25 = new SkillsTreeButton(false, false, 10, "hp25", swipe);
        SkillsTreeButton hp20 = new SkillsTreeButton(false, true, 5, "hp20", hp25);


        skillTreeButtons.AddRange(new List<SkillsTreeButton>
        {
            pummel, atk10, atk5, atk1, slide, roll, speed3, speed1,
            spin, slash, armour5, armour2, slam, swipe, hp25, hp20
        });
        selectedAbilities = new SkillsTreeButton[4];
        initializeSelectedAbilites();

    }

    public void initializeSelectedAbilites()
    {
        selectedAbilities[2] = new SkillsTreeButton(true, false, 20, "kick", null);

    }
    public int getCoins()
    {
        return this.coins;
    }

    public void AddCoins(int amount)
    {
        if (firstCoin)
        {
            DialogueManager.TriggerDialogue("FirstCoin", dialogueTriggers);
            firstCoin = false;
        }
        this.coins += amount;

        // Trigger UI update event
        OnCoinsUpdated.Invoke(coins);

        Debug.Log("Coins: " + coins);
    }

    public void RemoveCoins(int amount)
    {
        this.coins -= amount;
        if (coins < 0)
        {
            coins = 0;
        }

        // Trigger UI update event
        OnCoinsUpdated.Invoke(coins);
    }
}