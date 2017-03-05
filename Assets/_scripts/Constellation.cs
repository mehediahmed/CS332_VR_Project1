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
    // How much alpha is needed before the game goes to the credits scene.
    public float alphaNeededForTransition;

    // True if the player has entered the trigger.
    private bool activated = false;
    // The alpha of the overlay.
    private float alpha = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
        }
    }

    private void Update()
    {
        if (activated)
        {
            IncrementAlpha();
            // Calculate the new color based on the alpha.
            Color fadeColor = new Color(1f, 1f, 1f, alpha);
            // Update the UICanvas image for the white overlay.
            WhiteFade.img.color = fadeColor;
        }
    }

    private void IncrementAlpha()
    {
        alpha += fadeSpeed * Time.deltaTime;
        if (alpha > alphaNeededForTransition)
        {
            GoToCredits();
        }
    }

    private void GoToCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }
}