using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Using TextMesh Pro components
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Assuming TextMesh Pro is used
    public GameObject dialoguePanel;
    public Button skipButton;

    private Queue<string> sentences;
    private bool isTyping = false;

    void Awake()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
        skipButton.onClick.AddListener(EndDialogue); // Adjust skip button to end dialogue instead
    }

    void Update()
    {
        // Checks if the spacebar is pressed and no typing is currently happening
        if (Input.GetKeyDown(KeyCode.Space) && dialoguePanel.activeInHierarchy && !isTyping)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach (string line in dialogueLines)
        {
            sentences.Enqueue(line);
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
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f); // Control the speed of the typing effect here
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
