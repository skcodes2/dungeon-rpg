using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ButtonTrigger : MonoBehaviour
{
    private List<SkillsTreeButton> skillTreeButtons = new List<SkillsTreeButton>();

    private Label healthLabel;
    private Label damageLabel;
    private Label speedLabel;
    private Label armourLabel;
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


    }

    private void InitializeUI()
    {
        playerStats = PlayerStats.Instance;
        inventory = Inventory.Instance;
        uiDoc = GetComponent<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;
        VisualElement rightSide = root.Q<VisualElement>("RightSide");
        VisualElement stats = rightSide.Q<VisualElement>("Stats");

        healthLabel = stats.Query<Label>("Health");

        damageLabel = stats.Query<Label>("Attack");
        speedLabel = stats.Query<Label>("Speed");
        armourLabel = stats.Query<Label>("Armour");



        healthLabel.text = playerStats.health.ToString();

        damageLabel.text = playerStats.baseDamage.ToString();
        speedLabel.text = playerStats.walkSpeed.ToString();
        armourLabel.text = playerStats.armour.ToString();

        buttons = root.Query<Button>().ToList();


    }

    private void RegisterButtonEvents()
    {
        foreach (Button btn in buttons)
        {
            btn.BringToFront();
            btn.RegisterCallback<ClickEvent>(OnClick);
            print("Registered ClickEvent for button: " + btn.name);
        }
    }

    private void UnregisterButtonEvents()
    {
        print("Unregistering button events");
        foreach (Button btn in buttons)
        {
            btn.UnregisterCallback<ClickEvent>(OnClick);
        }
    }

    private void OnClick(ClickEvent evt)
    {

        Button clickedButton = evt.target as Button;

        if (clickedButton == null) return;


        SkillsTreeButton skillTreeButton = GetSkillTreeButton(clickedButton.name);


        if (skillTreeButton == null) return;

        if (skillTreeButton.getNextButton() == null)
        {
            HandleAbilityClicked(skillTreeButton, null);
            return;
        }


        Button nextUXMLButton = GetNextButton(skillTreeButton.getNextButton()?.getName());


        if (!skillTreeButton.getIsActive() || inventory.getCoins() < skillTreeButton.getPrice())
        {
            print("Not enough coins or button is inactive");
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
        if (nextUXMLButton == null && !skillTreeButton.getIsPurchased())
        {
            inventory.RemoveCoins(skillTreeButton.getPrice());
            skillTreeButton.setIsPurchased(true);
        }

        else if (!skillTreeButton.getIsPurchased())
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
                damageLabel.text = playerStats.baseDamage.ToString();
                break;
            case "atk5":
                playerStats.baseDamage += 5;
                damageLabel.text = playerStats.baseDamage.ToString();
                break;
            case "atk10":
                playerStats.baseDamage += 10;
                damageLabel.text = playerStats.baseDamage.ToString();
                break;
            case "speed3":
                playerStats.walkSpeed += 3;
                speedLabel.text = playerStats.walkSpeed.ToString();
                break;
            case "speed1":
                playerStats.walkSpeed += 1;
                speedLabel.text = playerStats.walkSpeed.ToString();
                break;
            case "armour2":
                playerStats.armour += 2;
                armourLabel.text = playerStats.armour.ToString();
                break;
            case "armour5":
                playerStats.armour += 5;
                armourLabel.text = playerStats.armour.ToString();
                break;
            case "hp25":
                playerStats.health += 25;
                healthLabel.text = playerStats.health.ToString();
                break;
            case "hp20":
                playerStats.health += 20;
                healthLabel.text = playerStats.health.ToString();
                break;
        }
    }

    private SkillsTreeButton GetSkillTreeButton(string name)
    {

        return skillTreeButtons.Find(skill => skill.getName() == name);
    }

    private Button GetNextButton(string name)
    {
        print("Getting button: " + name);
        return buttons.Find(button => button.name == name);
    }
}
