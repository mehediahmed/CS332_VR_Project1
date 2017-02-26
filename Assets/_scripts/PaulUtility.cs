// Author(s): Paul Calande
// Useful things.

using UnityEngine;

public class PaulUtility
{
    public const float TWO_PI = 2 * Mathf.PI;

    // Quits the game regardless of whether it's in editor mode or if it's a release build.
    public static void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}