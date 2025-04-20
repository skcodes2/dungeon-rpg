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

    private Dictionary<string, string> buttonTooltips = new Dictionary<string, string>()
{
    { "kick", "Press I to use Kick Attack\nThis attacks increases weapon damage by 5 points" },
    { "pummel", "Press I to use Pummel Attack\nThis attacks increases weapon damage by 5 points" },
    { "slash", "Press O to use Slash\nThis attacks increases weapon damage by 10 points\n " },
    { "swipe", "Press O to use Swipe Attack\nThis attacks increases weapon\ndamage by 10 points and adds\nLifesteal +5 On-Hit" },
    { "spin", "Press P to use Spin Attack\nThis attacks increases weapon damage by 20 points\n Attack can only be used while running\n Increase Armour by 2 points" },
    { "slam", "Press P to use Sword Slam Attack\nThis attacks increases weapon\ndamage by 20 points and adds\n Lifesteal +10 On-Hit" },
    { "roll", "Press Space to Roll.\nRolling briefly increases speed by 0.5" },
    { "slide", "Press Space to Slide.\nSliding briefly increases speed by 1" },

    // Stat upgrade buttons
    { "atk1", "Increase base attack by 1 point." },
    { "atk5", "Increase base attack by 5 points." },
    { "atk10", "Increase base attack by 10 points." },
    { "speed1", "Increase walk speed by 1 unit.\nRun speed scales with it." },
    { "speed3", "Increase walk speed by 1.5 units.\nRun speed scales with it." },
    { "armour2", "Increase armour by 1 points to reduce incoming damage." },
    { "armour5", "Increase armour by 1 points to reduce damage." },
    { "hp20", "Boost your maximum health by 20" },
    { "hp25", "Boost your maximum health by 25" },
};


    private Label healthLabel;
    private Label damageLabel;
    private Label speedLabel;
    private Label armourLabel;

    private Label tooltipLabel;

    private VisualElement movementImage;
    private VisualElement specialImage;
    private VisualElement basic1Image;
    private VisualElement basic2Image;
    private List<Button> buttons;
    private UIDocument uiDoc;

    private UIDocument abilityBarDoc;
    private VisualElement movementImageCD;
    private VisualElement specialImageCD;
    private VisualElement basic1ImageCD;
    private VisualElement basic2ImageCD;
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

    void Awake()
    {
        // Make the UI documents persistent across scenes
        GameObject abilityBarObject = GameObject.FindGameObjectWithTag("AbilityBar");
        if (abilityBarObject != null)
        {
            DontDestroyOnLoad(abilityBarObject);
        }

        GameObject uiDocObject = gameObject;
        if (uiDocObject != null)
        {
            DontDestroyOnLoad(uiDocObject);
        }
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
                    movementImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                    movementImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
                else if (ability.getName() == "swipe" || ability.getName() == "slash")
                {
                    basic2ImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                    basic2Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
                else if (ability.getName() == "pummel" || ability.getName() == "kick")
                {
                    basic1ImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                    basic1Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
                else if (ability.getName() == "slam" || ability.getName() == "spin")
                {
                    specialImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                    specialImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[ability.getName()]);
                }
            }
        }

    }

    private void InitializeUI()
    {
        GameObject abilityBarObject = GameObject.FindGameObjectWithTag("AbilityBar");
        playerStats = PlayerStats.Instance;
        inventory = Inventory.Instance;
        uiDoc = GetComponent<UIDocument>();
        abilityBarDoc = abilityBarObject.GetComponent<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;
        VisualElement rightSide = root.Q<VisualElement>("RightSide");
        VisualElement leftSide = root.Q<VisualElement>("LeftSide");
        VisualElement stats = rightSide.Q<VisualElement>("Stats");

        tooltipLabel = root.Q<Label>("Tooltip");

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

        movementImageCD = abilityBarDoc.rootVisualElement.Q<VisualElement>("MovementImage");
        specialImageCD = abilityBarDoc.rootVisualElement.Q<VisualElement>("SpecialImage");
        basic1ImageCD = abilityBarDoc.rootVisualElement.Q<VisualElement>("Basic1Image");
        basic2ImageCD = abilityBarDoc.rootVisualElement.Q<VisualElement>("Basic2Image");

        buttons = root.Query<Button>().ToList();


    }
    private void RegisterButtonEvents()
    {
        foreach (Button btn in buttons)
        {
            btn.BringToFront();
            btn.RegisterCallback<ClickEvent>(OnClick);
            btn.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            btn.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);

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

    private void OnMouseEnter(MouseEnterEvent evt)
    {
        Button hoveredButton = evt.target as Button;
        if (hoveredButton == null || !buttonTooltips.ContainsKey(hoveredButton.name)) return;

        tooltipLabel.text = buttonTooltips[hoveredButton.name];
        tooltipLabel.style.display = DisplayStyle.Flex;

        Rect buttonBounds = hoveredButton.worldBound;

        // Tooltip Y: slightly above the button
        tooltipLabel.style.top = buttonBounds.y - 35;

        // Tooltip X: more to the left for certain buttons
        bool shouldShiftLeft = hoveredButton.name == "spin" ||
                               hoveredButton.name == "slam" ||
                               hoveredButton.name == "slash" ||
                               hoveredButton.name == "swipe" ||
                               hoveredButton.name == "hp20" ||
                               hoveredButton.name == "hp25" ||
                               hoveredButton.name == "armour2" ||
                               hoveredButton.name == "armour5";

        if (shouldShiftLeft)
        {
            tooltipLabel.style.left = buttonBounds.x - 180; // Well away from mouse
            tooltipLabel.style.top = buttonBounds.y - 80;
        }
        else
        {
            tooltipLabel.style.left = buttonBounds.xMax + 15; // Default: right side of button
        }
    }


    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        tooltipLabel.style.display = DisplayStyle.None;
    }


    private void OnClick(ClickEvent evt)
    {

        Button clickedButton = evt.target as Button;


        if (clickedButton == null) return;

        if (clickedButton.name == "kick")
        {
            AudioManager.Instance.Play("equip");
            inventory.AddSelectedAbility(new SkillsTreeButton(true, false, 20, "kick", null), "kick");
            basic1Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths["kick"]);
            basic1ImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths["kick"]);
            return;
        }


        SkillsTreeButton skillTreeButton = GetSkillTreeButton(clickedButton.name);


        if (skillTreeButton == null)
        {
            AudioManager.Instance.Play("error");
            return;
        }

        if (skillTreeButton.getNextButton() == null)
        {
            print("here1");
            HandleAbilityClicked(skillTreeButton, null);
            return;
        }


        Button nextUXMLButton = GetNextButton(skillTreeButton.getNextButton()?.getName());
        
        if (inventory.getCoins() < skillTreeButton.getPrice() && !skillTreeButton.getIsPurchased())
        {
            print("Not enough coins");
            AudioManager.Instance.Play("error");
            return;
        }

        if (!skillTreeButton.getIsActive())
        {
            print(" button is inactive");
            AudioManager.Instance.Play("error");
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
        if(inventory.getCoins()<skillTreeButton.getPrice() && !skillTreeButton.getIsPurchased()){
            print("Not enough coins");
            AudioManager.Instance.Play("error");
            return;
        }
        if (nextUXMLButton == null && !skillTreeButton.getIsPurchased())
        {
            
            inventory.RemoveCoins(skillTreeButton.getPrice());
            skillTreeButton.setIsPurchased(true);
        }

        else if (!skillTreeButton.getIsPurchased())
        {
            
            if (skillTreeButton.getName() == "Slash")
            {
                playerStats.IncreaseArmour(1f);
            }
            if (skillTreeButton.getName() == "SpinAttack")
            {
                playerStats.IncreaseArmour(2f);
            }
            AudioManager.Instance.Play("buy");
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
                    movementImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }
                else if (skillTreeButton.getName() == "swipe" || skillTreeButton.getName() == "slash")
                {
                    basic2Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                    basic2ImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }
                else if (skillTreeButton.getName() == "pummel" || skillTreeButton.getName() == "kick")
                {
                    basic1ImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                    basic1Image.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }
                else if (skillTreeButton.getName() == "slam" || skillTreeButton.getName() == "spin")
                {
                    specialImageCD.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                    specialImage.style.backgroundImage = Resources.Load<Texture2D>(imageAbilityPaths[skillTreeButton.getName()]);
                }

                inventory.AddSelectedAbility(skillTreeButton, skillTreeButton.getName());
                AudioManager.Instance.Play("equip");
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
            AudioManager.Instance.Play("buy");
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
                playerStats.IncreaseArmour(1f);
                armourLabel.text = playerStats.armour.ToString();
                break;
            case "armour5":
                playerStats.IncreaseArmour(1f);
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
