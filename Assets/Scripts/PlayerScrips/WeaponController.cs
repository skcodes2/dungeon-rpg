using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WeaponController : AbilityController
{
    private float attackDurationTimer = 0f;

    private float maxCooldownHeight = 160f; // Maximum height of the cooldown overlay

    private Inventory inventory;

    private bool isOnCooldown;

    private UIDocument uiDocument;


    private VisualElement cooldownOverlay;


    public WeaponController(GameObject weapon, Animator anim, KeyCode keyBind, float attackDuration, float attackCooldown, GameObject abilityBarUI)
        : base(weapon, anim, keyBind, attackDuration, attackCooldown)
    {
        this.inventory = Inventory.Instance;
        this.isOnCooldown = false;
        this.uiDocument = abilityBarUI.GetComponent<UIDocument>();
        this.cooldownOverlay = uiDocument.rootVisualElement.Q<VisualElement>(getAbilityType(base.ability.name));

    }

    private string getAbilityType(string abilityName)
    {
        return abilityName switch
        {
            "SwordSlam" => "SpecialImageCD",
            "SpinAttack" => "SpecialImageCD",
            "Pummel" => "Basic1ImageCD",
            "Kick" => "Basic1ImageCD",
            "Swipe" => "Basic2ImageCD",
            "Slash" => "Basic2ImageCD",
        };
    }
    

    public override void Update()
    {
        
        if (this.isOnCooldown)
        {
            base.cooldownTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(cooldownTimer / base.abilityCooldown);
            float currentHeight = fillAmount * maxCooldownHeight;
            if (cooldownOverlay != null)
            {
                Debug.Log($"CooldownOverlay initialized successfully for ability: {base.ability.name}");
            }
            else
            {
                Debug.LogError($"CooldownOverlay is null for ability: {base.ability.name}");
            }
            cooldownOverlay.style.height = new Length(currentHeight, LengthUnit.Pixel);

            if (base.cooldownTimer <= 0f)
            {
                this.isOnCooldown = false;
                cooldownOverlay.style.height = new Length(0, LengthUnit.Pixel); // overlay fully disappears
            }
        }

        string abilityName = "";
        if (base.ability.name == "SpinAttack")
        {
            abilityName = "spin";
        }
        else if (base.ability.name == "SwordSlam")
        {
            abilityName = "slam";
        }

        else
        {
            abilityName = base.ability.name.ToLower();
        }

        if (inventory.ContainsAbility(abilityName))
        {
            CheckDurationTimer();
            base.UpdateCooldownTimer();
            CheckKeyBoardInput();
        }

    }

    protected override void CheckKeyBoardInput()
    {

        bool running = base.ability.name.Contains("SpinAttack") ? base.isRunning : !base.isRunning;
        if (Input.GetKeyDown(base.keyBind) && base.cooldownTimer <= 0 && running)
        {
            base.animator.SetTrigger(base.ability.name);
            onAttack();
            base.cooldownTimer = base.abilityCooldown;
            this.isOnCooldown = true;
            Debug.Log(cooldownOverlay.name);
            cooldownOverlay.style.height = new Length(maxCooldownHeight, LengthUnit.Pixel); // overlay fully appears
        }
    }

    protected void CheckDurationTimer()
    {
        if (base.isUsingAbility)
        {
            attackDurationTimer += Time.deltaTime;

            if (attackDurationTimer >= base.abilityDuration * (3 / 5f))
            {
                base.ability.SetActive(true);
            }

            if (attackDurationTimer >= base.abilityDuration)
            {
                base.isUsingAbility = false;
                attackDurationTimer = 0;
                base.ability.SetActive(false);
            }
        }
    }

    private void onAttack()
    {
        if (!base.isUsingAbility)
        {
            base.isUsingAbility = true;

        }
    }
}
