using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // Expose a field in the Inspector to set the scene name
    public string sceneName = "GameScene";

    public void OnPlayButtonClick()
    {
        // Load the scene specified in the Inspector
        SceneManager.LoadScene(sceneName);
    }
}
