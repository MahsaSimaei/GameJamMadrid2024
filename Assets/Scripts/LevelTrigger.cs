using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public string nextLevelName;

    void Start()
    {
        Debug.Log("Script started. Ready to trigger with scene name: " + nextLevelName);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name + ", Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger. Attempting to load scene: " + nextLevelName);
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.Log("The object that entered is not a player, it is tagged as: " + other.tag);
        }
    }
}
