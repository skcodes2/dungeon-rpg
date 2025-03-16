using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ButtonTrigger : MonoBehaviour
{
    private List<SkillsTreeButton> skillTreeButtons = new List<SkillsTreeButton>();
    private List<Button> buttons;
    private UIDocument uiDoc;
    private PlayerStats playerStats;
    private Inventory inventory;

    void Awake()
    {
        InitializeSkillsTree();
        InitializeUI();
        RegisterButtonEvents();
    }

    private void OnDisable()
    {
        UnregisterButtonEvents();
    }

    private void InitializeSkillsTree()
    {
        // Attack Line
        SkillsTreeButton pummel = new SkillsTreeButton(true, false, 20, "pummel", null);
        SkillsTreeButton atk10 = new SkillsTreeButton(false, false, 15, "atk10", pummel);
        SkillsTreeButton atk5 = new SkillsTreeButton(false, false, 10, "atk5", atk10);
        SkillsTreeButton atk1 = new SkillsTreeButton(false, true, 5, "atk1", atk5);

        // Speed Line
        SkillsTreeButton slide = new SkillsTreeButton(true, false, 20, "slide", null);
        SkillsTreeButton roll = new SkillsTreeButton(true, false, 15, "roll", slide);
        SkillsTreeButton speed5 = new SkillsTreeButton(false, false, 10, "speed5", roll);
        SkillsTreeButton speed3 = new SkillsTreeButton(false, true, 5, "speed3", speed5);

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
            pummel, atk10, atk5, atk1, slide, roll, speed5, speed3,
            spin, slash, armour5, armour2, slam, swipe, hp25, hp20
        });
    }

    private void InitializeUI()
    {
        uiDoc = GetComponent<UIDocument>();
        buttons = uiDoc.rootVisualElement.Query<Button>().ToList();
        print(buttons.Count);
        playerStats = PlayerStats.Instance;
        inventory = Inventory.Instance;
    }

    private void RegisterButtonEvents()
    {
        foreach (Button btn in buttons)
        {
            btn.BringToFront();
            btn.RegisterCallback<ClickEvent>(OnClick);
        }
    }

    private void UnregisterButtonEvents()
    {
        foreach (Button btn in buttons)
        {
            btn.UnregisterCallback<ClickEvent>(OnClick);
        }
    }

    private void OnClick(ClickEvent evt)
    {
        print("Button clicked");
        Button clickedButton = evt.target as Button;
        if (clickedButton == null) return;

        SkillsTreeButton skillTreeButton = GetSkillTreeButton(clickedButton.name);
        if (skillTreeButton == null) return;

        Button nextUXMLButton = GetNextButton(skillTreeButton.getNextButton()?.getName());

        if (!skillTreeButton.getIsActive() || inventory.getCoins() < skillTreeButton.getPrice())
        {
            return;
        }

        if (skillTreeButton.getIsAbilityUpgrade())
        {
            HandleAbilityClicked(skillTreeButton, nextUXMLButton);
        }
        else
        {
            HandlStatUpgrade(skillTreeButton, nextUXMLButton);
        }
    }

    private void HandleAbilityClicked(SkillsTreeButton skillTreeButton, Button nextUXMLButton)
    {
        if (!skillTreeButton.getIsPurchased())
        {
            inventory.RemoveCoins(skillTreeButton.getPrice());
            skillTreeButton.setIsPurchased(true);
            UnlockNextButton(skillTreeButton, nextUXMLButton);
            print("Ability purchased: " + skillTreeButton.getName());
        }
        else
        {
            if (!inventory.ContainsAbility(skillTreeButton.getName()))
            {
                inventory.AddSelectedAbility(skillTreeButton, skillTreeButton.getName());
            }
        }
    }

    private void HandlStatUpgrade(SkillsTreeButton skillTreeButton, Button nextUXMLButton)
    {
        if (!skillTreeButton.getIsPurchased())
        {
            inventory.RemoveCoins(skillTreeButton.getPrice());
            skillTreeButton.setIsPurchased(true);
            UnlockNextButton(skillTreeButton, nextUXMLButton);
            ApplyUpgrade(skillTreeButton);
        }
    }

    private void UnlockNextButton(SkillsTreeButton skillTreeButton, Button nextUXMLButton)
    {
        if (nextUXMLButton != null)
        {
            nextUXMLButton.RemoveFromClassList("InactiveButton");
            nextUXMLButton.AddToClassList("activeButton");
            skillTreeButton.getNextButton()?.setIsActive(true);
        }
    }

    private void ApplyUpgrade(SkillsTreeButton skillTreeButton)
    {
        switch (skillTreeButton.getName())
        {
            case "atk1":
                playerStats.baseDamage += 1;
                break;
            case "atk5":
                playerStats.baseDamage += 5;
                break;
            case "atk10":
                playerStats.baseDamage += 10;
                break;
            case "speed5":
                playerStats.walkSpeed += 3;
                playerStats.runSpeed += 3;
                break;
            case "speed3":
                playerStats.walkSpeed += 1;
                playerStats.runSpeed += 1;
                break;
            case "armour2":
                playerStats.armour += 2;
                break;
            case "armour5":
                playerStats.armour += 5;
                break;
            case "hp25":
                playerStats.health += 25;
                break;
            case "hp20":
                playerStats.health += 20;
                break;
        }
    }

    private SkillsTreeButton GetSkillTreeButton(string name)
    {
        return skillTreeButtons.Find(skill => skill.getName() == name);
    }

    private Button GetNextButton(string name)
    {
        return buttons.Find(button => button.name == name);
    }
}
