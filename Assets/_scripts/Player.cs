// Author(s): Paul Calande
// Player script. To be attached to the player object. In this case, that's the [CameraRig] object.

// Comment out the following line to stop console spam about damage taken from enemies.
//#define DAMAGE_MESSAGES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public static reference to the player object.
    public static GameObject playerObject;
    // The maximum health that the player can have.
    public float maxHealth;
    // How quickly the player's health regenerates, in health per second.
    public float healthRegenRate;

    // A baked calculation that helps determine how intense the watch's light should be.
    private float intensityFactor;
    // How much health the player has.
    private float health;

    private void Awake()
    {
        // Set the playerObject to this gameObject automatically.
        playerObject = gameObject;
        // Calculate the intensity factor.
        intensityFactor = 8f / maxHealth;
        // Initialize the health.
        health = maxHealth;
    }

    private void Update()
    {
        // Regenerate health.
        health += healthRegenRate * Time.deltaTime;
        // Cap the health.
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        // Update the watch light's intensity.
        Watch_Light.watchLight.intensity = health * intensityFactor;
        // If the player runs out of health...
        if (health < 0f)
        {
            // Game over.
            //Debug.Log("Player has died.");
        }
    }

    public void TakeDamage(float amount)
    {
#if DAMAGE_MESSAGES
        Debug.Log("Took " + amount + " damage!");
#endif

        health -= amount;
    }
}