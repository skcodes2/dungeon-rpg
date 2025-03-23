using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject canvas;

    private Queue<string> sentences;

    private PlayerStats playerStats;

    // Typing speed variable
    public float typingSpeed = 0.05f;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
        playerStats = PlayerStats.Instance;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        playerStats.StopPlayer();
        canvas.SetActive(true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        canvas.SetActive(false);
        playerStats.ResumePlayer();
    }

    public static void TriggerDialogue(string triggerID, List<DialogueTrigger> dialogueTriggers)
    {

        DialogueTrigger trigger = dialogueTriggers.Find(t => t.triggerID == triggerID);
        if (trigger != null)
        {

            trigger.TriggerDialogue();
        }
        else
        {
            Debug.LogWarning("DialogueTrigger with ID " + triggerID + " not found.");
        }
    }
}