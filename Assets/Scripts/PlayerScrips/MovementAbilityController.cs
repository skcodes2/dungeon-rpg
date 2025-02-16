using UnityEngine;
using System.Collections;
[System.Serializable]
public class MovementAbilityController : AbilityController
{

    private float additionalSpeed;
    private PlayerStats playerStats;

    private MonoBehaviour coroutineRunner;

    public MovementAbilityController(MonoBehaviour runner, Animator anim, KeyCode keyBind, float abilityDuration, float abilityCooldown, float additionalSpeed)
        : base(anim, keyBind, abilityDuration, abilityCooldown)
    {
        this.additionalSpeed = additionalSpeed;
        this.playerStats = PlayerStats.Instance;
        this.coroutineRunner = runner;
    }

    public override void Update()
    {
        base.UpdateCooldownTimer();
        CheckKeyBoardInput();
    }

    protected IEnumerator CheckDurationTimer()
    {
        isUsingAbility = true;
        playerStats.IncreaseSpeed(additionalSpeed);
        yield return new WaitForSeconds(abilityDuration);
        playerStats.DecreaseSpeed(additionalSpeed);
        isUsingAbility = false;
    }

    protected override void CheckKeyBoardInput()
    {
        string animationTrigger = abilityDuration == 1f ? "Slide" : "Roll";
        if (Input.GetKeyDown(keyBind) && cooldownTimer <= 0 && isRunning)
        {
            animator.SetTrigger(animationTrigger);
            coroutineRunner.StartCoroutine(CheckDurationTimer());
            cooldownTimer = abilityCooldown;
        }
    }

}