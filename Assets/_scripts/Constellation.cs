// Author(s): Paul Calande
// Script for the constellation.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Constellation : MonoBehaviour
{
    // The name of the credits scene.
    public string creditsScene;
    // How quickly the overlay fades in. Larger = faster.
    public float fadeSpeed;
    // The texture that covers the screen during the fade out.
    public Texture2D fadeOverlay;
    // How much alpha is needed before the game goes to the credits scene.
    public float alphaNeededForTransition;

    // True if the player has entered the trigger.
    private bool activated = false;
    // The alpha of the overlay.
    private float alpha = 0f;
    // The draw depth of the overlay.
    private int drawDepth = -1000000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
        }
    }

    private void OnGUI()
    {
        if (activated)
        {
            alpha += fadeSpeed * Time.deltaTime;
            if (alpha > alphaNeededForTransition)
            {
                GoToCredits();
            }
            Color fadeColor = new Color(1f, 1f, 1f, alpha);
            GUI.color = fadeColor;
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeOverlay);
        }
    }

    private void GoToCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }
}