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

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool speedUpTyping = false;

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
        isTyping = true;
        speedUpTyping = false;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // If player is holding to speed up, use faster delay
            float delay = speedUpTyping ? typingSpeed * 0.1f : typingSpeed;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }

    void Update()
    {
        if (!canvas.activeInHierarchy) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                speedUpTyping = true;
            }
            else
            {
                DisplayNextSentence();
            }
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