using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ButtonTrigger : MonoBehaviour
{
    private List<SkillsTreeButton> skillTreeButtons;

    private Dictionary<string, string> imageAbilityPaths = new Dictionary<string, string>()
    {
        {"kick", "Icon23"},
        {"pummel", "Icon19"},
        {"slash", "Icon33"},
        {"swipe", "Icon25"},
        {"spin", "Icon5"},
        {"slam", "Icon4"},
        {"roll", "Icon7"},
        {"slide", "Icon43"},

    };

    private Label healthLabel;
    private Label damageLabel;
    private Label speedLabel;
    private Label armourLabel;

    private VisualElement movementImage;
    private VisualElement specialImage;
    private VisualElement basic1Image;
    private VisualElement basic2Image;
    private List<Button> buttons;
    private UIDocument uiDoc;
    private PlayerStats playerStats;
    private Inventory inventory;

    void OnEnable()
    {
        InitializeUI();
        RegisterButtonEvents();
    }

    private void OnDisable()
    {
        UnregisterButtonEvents();
    }

    private void InitializeSkillsTree()
    {
        skillTreeButtons = inventory.skillTreeButtons;
        //loop through and check active states
        foreach (SkillsTreeButton skill in skillTreeButtons)
        {
            if (skill.getIsActive())
            {
                Button button = GetButton(skill.getName());
                button.RemoveFromClassList("InactiveButton");
                button.AddToClassList("activeButton");
            }

            if (!skill.getIsAbilityUpgrade() && skill.getIsPurchased())
            {
                Button btn = GetButton(skill.getName());
                btn.style.backgroundImage = Resources.Load<Texture2D>("Lock");
                btn.SetEnabled(false);
                btn.RemoveFromClassList("InactiveButton");
                btn.AddToClassList("activeButton");
            }
        }

        SkillsTreeButton[] selectedAbilities = inventory.selectedAbilities;

        foreach (SkillsTreeButton ability in selectedAbilities)
        {
            if (ability != null)
            {
                if (ability.getName() == "roll" || ability.getName() == "slide")
                {
                    movementImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
                else if (ability.getName() == "swipe" || ability.getName() == "slash")
                {
                    basic2Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
                else if (ability.getName() == "pummel" || ability.getName() == "kick")
                {
                    basic1Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
                else if (ability.getName() == "slam" || ability.getName() == "spin")
                {
                    specialImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
            }
        }

    }

    private void InitializeUI()
    {
        playerStats = PlayerStats.Instance;
        inventory = Inventory.Instance;
        uiDoc = GetComponent<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;
        VisualElement rightSide = root.Q<VisualElement>("RightSide");
        VisualElement leftSide = root.Q<VisualElement>("LeftSide");
        VisualElement stats = rightSide.Q<VisualElement>("Stats");

        healthLabel = stats.Query<Label>("Health");
        damageLabel = stats.Query<Label>("Attack");
        speedLabel = stats.Query<Label>("Speed");
        armourLabel = stats.Query<Label>("Armour");

        healthLabel.text = playerStats.health.ToString();
        damageLabel.text = playerStats.baseDamage.ToString();
        speedLabel.text = playerStats.walkSpeed.ToString();
        armourLabel.text = playerStats.armour.ToString();

        movementImage = leftSide.Q<VisualElement>("MovementImage");
        specialImage = leftSide.Q<VisualElement>("SpecialImage");
        basic1Image = leftSide.Q<VisualElement>("Basic1Image");
        basic2Image = leftSide.Q<VisualElement>("Basic2Image");

        buttons = root.Query<Button>().ToList();


    }
    private void RegisterButtonEvents()
    {
        foreach (Button btn in buttons)
        {
            btn.BringToFront();
            btn.RegisterCallback<ClickEvent>(OnClick);

        }
        InitializeSkillsTree();
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

        if (clickedButton.name == "kick")
        {
            inventory.AddSelectedAbility(new SkillsTreeButton(true, false, 20, "kick", null), "kick");
            basic1Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths["kick"]);
            return;
        }


        SkillsTreeButton skillTreeButton = GetSkillTreeButton(clickedButton.name);


        if (skillTreeButton == null) return;

        if (skillTreeButton.getNextButton() == null)
        {
            HandleAbilityClicked(skillTreeButton, null);
            return;
        }


        Button nextUXMLButton = GetNextButton(skillTreeButton.getNextButton()?.getName());


        if (!skillTreeButton.getIsActive() && inventory.getCoins() < skillTreeButton.getPrice())
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
            skillTreeButton.setIsActive(true);
            UnlockNextButton(skillTreeButton, nextUXMLButton);
            print("Ability purchased: " + skillTreeButton.getName());
        }
        else
        {
            if (!inventory.ContainsAbility(skillTreeButton.getName()))
            {
                if (skillTreeButton.getName() == "roll" || skillTreeButton.getName() == "slide")
                {
                    movementImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }
                else if (skillTreeButton.getName() == "swipe" || skillTreeButton.getName() == "slash")
                {
                    basic2Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }
                else if (skillTreeButton.getName() == "pummel" || skillTreeButton.getName() == "kick")
                {
                    basic1Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }
                else if (skillTreeButton.getName() == "slam" || skillTreeButton.getName() == "spin")
                {
                    specialImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }

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
            Button btn = GetButton(skillTreeButton.getName());
            btn.style.backgroundImage = Resources.Load<Texture2D>("Lock");
            btn.SetEnabled(false);
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
                playerStats.walkSpeed += 1.5f;
                playerStats.runSpeed = playerStats.walkSpeed * 1.5f;
                speedLabel.text = playerStats.walkSpeed.ToString();
                break;
            case "speed1":
                playerStats.walkSpeed += 1f;
                playerStats.runSpeed = playerStats.walkSpeed * 1.5f;
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
                playerStats.SetMaxHealth(playerStats.maxHealth + 25);
                healthLabel.text = playerStats.maxHealth.ToString();
                break;
            case "hp20":
                playerStats.SetMaxHealth(playerStats.maxHealth + 20);
                healthLabel.text = playerStats.maxHealth.ToString();
                break;
        }
    }

    private SkillsTreeButton GetSkillTreeButton(string name)
    {

        return skillTreeButtons.Find(skill => skill.getName() == name);
    }

    private Button GetButton(string name)
    {
        return buttons.Find(button => button.name == name);
    }

    private Button GetNextButton(string name)
    {

        return buttons.Find(button => button.name == name);
    }
}
