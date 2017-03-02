// Author(s): Paul Calande
// Player script. To be attached to the player object. In this case, that's the [CameraRig] object.

// Comment out the following line to stop console spam about damage taken from enemies.
//#define DAMAGE_MESSAGES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Public static reference to the player object.
    public static GameObject playerObject;
    // The maximum health that the player can have.
    public float maxHealth;
    // How quickly the player's health regenerates, in health per second.
    public float healthRegenRate;
    // The scene that the player goes to upon dying.
    public string deathScene;
    // The sound that plays when the player dies
    public AudioClip deathSound;

    // A baked calculation that helps determine how intense the watch's light should be.
    private float intensityFactor;
    // How much health the player has.
    private float health;
    // The name of the scene that the player was previously on before they died.
    private static string lastSceneBeforeDeath;
    // The source of audio that this script will play
    private AudioSource source;

    private void Awake()
    {
        // Set the playerObject to this gameObject automatically.
        playerObject = gameObject;
        // Calculate the intensity factor.
        intensityFactor = 8f / maxHealth;
        // Initialize the health.
        health = maxHealth;
        // Update the last scene before death.
        lastSceneBeforeDeath = SceneManager.GetActiveScene().name;
        //Get the Audio Source
        source = GetComponent<AudioSource>();
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
            GameOver.message = "You were caught by the Hollows.";
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
#if DAMAGE_MESSAGES
        Debug.Log("Took " + amount + " damage!");
#endif

        health -= amount;
    }

    public static string GetLastSceneBeforeDeath()
    {
        return lastSceneBeforeDeath;
    }

    public void Die()
    {
        // Game over.
        //Debug.Log("Player has died.");
        //Stop playing the background music and play the Death music
        source.Stop();
        source.PlayOneShot(deathSound);
        SceneManager.LoadScene(deathScene);
        
    }
}