// Author(s): Paul Calande
// Game over scene script.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
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