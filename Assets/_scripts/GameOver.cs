// Author(s): Paul Calande
// Game over scene script.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // The message to be displayed on the game over screen.
    public static string message = "If you're reading this particular string after dying, yell at Paul immediately.";

    private void Awake()
    {
        // Set the game over text to the appropriate string.
        message += "\n\nRestarting...";
        gameObject.GetComponent<Text>().text = message;
        // Start the respawn timer.
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(5.0f);
        ReturnToGame();
    }

    // Return to the last scene that the player was in before they died.
    private void ReturnToGame()
    {
        string sceneName = Player.GetLastSceneBeforeDeath();
        SceneManager.LoadScene(sceneName);
    }
}