using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public string triggerID;
    public Dialogue dialogue;


    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning("DialogueManager not found in the scene.");
        }
    }
}