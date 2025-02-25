using UnityEngine;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    public GameObject secretStairs; // Assign this in the Inspector
    private FloorBtn[] buttons;
    private BoxCollider2D stairsCollider;
    private SpriteRenderer stairsRenderer;

    // public List<DialogueTrigger> dialogueTriggers; // Assign in the Inspector

    public Sprite stairsSprite; // Assign the staircase sprite in Inspector

    private void Start()
    {
        buttons = FindObjectsOfType<FloorBtn>(); // Find all buttons
        FloorBtn.OnButtonActivated += CheckAllButtonsActivated;

        // Get the stairs components
        if (secretStairs != null)
        {
            stairsCollider = secretStairs.GetComponent<BoxCollider2D>();
            stairsRenderer = secretStairs.GetComponent<SpriteRenderer>();

            // Hide the stairs at the start
            stairsRenderer.enabled = false;
            stairsCollider.enabled = false;
        }
    }

    private void CheckAllButtonsActivated()
    {
        foreach (FloorBtn button in buttons)
        {
            if (!button.IsActivated())
            {
                return; // If any button isn't activated, do nothing
            }
        }

        RevealSecretStairs(); // Reveal the stairs when all buttons are pressed
    }

    private void RevealSecretStairs()
    {
        Debug.Log("All buttons activated! Revealing the secret stairs.");

        if (stairsRenderer != null && stairsSprite != null)
        {
            stairsRenderer.enabled = true; // Make stairs visible
            stairsRenderer.sprite = stairsSprite;
            // DialogueManager.TriggerDialogue("ShowStairs", dialogueTriggers); // Trigger dialogue
        }

        if (stairsCollider != null)
        {
            stairsCollider.enabled = true; // Enable the collider so the player can enter
        }
    }

    private void OnDestroy()
    {
        FloorBtn.OnButtonActivated -= CheckAllButtonsActivated; // Prevent memory leaks
    }
}
