using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookInteractable : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    public GameObject interactPrompt;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range — showing prompt!");
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactPrompt?.SetActive(false);
        }
    }
}
