using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
[System.Serializable]
public class MovementAbilityController : AbilityController
{

    private float additionalSpeed;
    private PlayerStats playerStats;

    private MonoBehaviour coroutineRunner;

    private float maxCooldownHeight = 160f;

    private bool isOnCooldown = false;

    private UIDocument uiDocument;
    private VisualElement cooldownOverlay;

    private Inventory inventory;


    public MovementAbilityController(MonoBehaviour runner, Animator anim, KeyCode keyBind, float abilityDuration, float abilityCooldown, float additionalSpeed)
        : base(anim, keyBind, abilityDuration, abilityCooldown)
    {
        this.additionalSpeed = additionalSpeed;
        this.playerStats = PlayerStats.Instance;
        this.coroutineRunner = runner;
        this.inventory = Inventory.Instance;
        this.isOnCooldown = false;
        GameObject abilityBarObject = GameObject.FindGameObjectWithTag("AbilityBar");
        this.uiDocument = abilityBarObject.GetComponent<UIDocument>();
        this.cooldownOverlay = uiDocument.rootVisualElement.Q<VisualElement>("MovementImageCD");
    }

    public override void Update()
    {
        if (this.isOnCooldown)
        {
            base.cooldownTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(base.cooldownTimer / base.abilityCooldown);
            float currentHeight = fillAmount * maxCooldownHeight;

            cooldownOverlay.style.height = new Length(currentHeight, LengthUnit.Pixel);

            if (base.cooldownTimer <= 0f)
            {
                this.isOnCooldown = false;
                cooldownOverlay.style.height = new Length(0, LengthUnit.Pixel); // overlay fully disappears
            }
        }

        string abilityName = abilityDuration == 1f ? "slide" : "roll";

        if (inventory.ContainsAbility(abilityName))
        {
            base.UpdateCooldownTimer();
            CheckKeyBoardInput();
        }
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
        if (Input.GetKeyDown(keyBind) && base.cooldownTimer <= 0 && isRunning)
        {
            animator.SetTrigger(animationTrigger);
            coroutineRunner.StartCoroutine(CheckDurationTimer());
            base.cooldownTimer = base.abilityCooldown;
            this.isOnCooldown = true;
            cooldownOverlay.style.height = new Length(maxCooldownHeight, LengthUnit.Pixel);
        }
    }

}