using UnityEngine;

[System.Serializable]
public class WeaponController : AbilityController
{
    private float attackDurationTimer = 0f;
    private Inventory inventory;


    public WeaponController(GameObject weapon, Animator anim, KeyCode keyBind, float attackDuration, float attackCooldown)
        : base(weapon, anim, keyBind, attackDuration, attackCooldown)
    {
        this.inventory = Inventory.Instance;
    }

    public override void Update()
    {
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
