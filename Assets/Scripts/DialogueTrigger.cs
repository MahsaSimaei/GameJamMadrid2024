using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public List<GameObject> objectsToDisappear;
    public List<GameObject> objectsToAppear;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.7f;

    // Define the dialogue lines directly within the script
    public string[] dialogueLines = new string[]
    {
        "Future Jimmy: Jimmy! Don’t freak out, it’s me… you, from the future!",
        "Jimmy: I can't believe I go bald... This is awful!",
        "Future Jimmy: Trust me, losing hair is the least of our issues. We're dealing with what I call 'chronos chaos.",
        "Jimmy: Chronos chaos?",
        "Future Jimmy: Yeah, it's not a common term, but think of it like this—'chronos' is the Greek for time, and 'chaos' means total confusion. So, 'chronos chaos'? It's a total mess with time itself, like disruptions in how time should flow.",
        "Jimmy: Why are you here? What happened?",
        "Future Jimmy: I'm here because you made a big mess by opening portals to the past. You opened the Chronos Chaos chest. You connected two timelines together. We need to find the people who slipped through them and send them back. And we have to do it when they're vulnerable and not attacking.",
        "Jimmy: What happens if they catch me? Or worse, kill me?",
        "Future Jimmy: Then you go back to their time, and they'll just kill you again. You’d disappear forever. So, let’s avoid that.",
        "Jimmy: Great! No pressure then.",
        "Future Jimmy: Run you have no time to waste."
    };

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player GameObject has the "Player" tag
        {
            dialogueManager.StartDialogue(dialogueLines);
            ToggleObjects();
        }
    }

    private void ToggleObjects()
    {
        // Disable all objects in the objectsToDisappear list
        foreach (var item in objectsToDisappear)
        {
            if (item != null)
                item.SetActive(false);
        }

        // Enable all objects in the objectsToAppear list
        foreach (var item in objectsToAppear)
        {
            if (item != null)
                item.SetActive(true);
        }
    }
}
