using UnityEngine;
[System.Serializable]
public abstract class AbilityController
{
    protected GameObject ability;
    protected Animator animator;

    protected bool isUsingAbility = false;

    protected float abilityDuration;
    [SerializeField]
    protected float abilityCooldown;
    protected float cooldownTimer = 0f;
    protected bool isRunning;
    [SerializeField]
    protected KeyCode keyBind;

    public AbilityController(GameObject ability, Animator anim, KeyCode keyBind, float abilityDuration, float abilityCooldown)
    {
        this.ability = ability;
        this.abilityDuration = abilityDuration;
        this.abilityCooldown = abilityCooldown;
        this.keyBind = keyBind;
        this.animator = anim;
        this.isRunning = false;
    }

    //Without ability
    public AbilityController(Animator anim, KeyCode keyBind, float abilityDuration, float abilityCooldown)
    {
        this.abilityDuration = abilityDuration;
        this.abilityCooldown = abilityCooldown;
        this.keyBind = keyBind;
        this.animator = anim;
        this.isRunning = false;

    }

    public abstract void Update();
    protected abstract void CheckKeyBoardInput();
    protected void UpdateCooldownTimer()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
    }

}

