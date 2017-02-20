// Author(s): Paul Calande, Mehedi Ahmed, Josiah Erikson

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Crystal_Interact : VRTK_InteractableObject
{
    // References to the active and passive lights.
    public Light ActiveLight;
    public Light PassiveLight;

    // The rate at which the crystal charge decays when in use.
    public float decayRate;
    // The rate at which the crystal recharges when not in use.
    public float RechargeRate;
    // The maximum charge of the crystal.
    public float maxCharges;
    // The minimum charge of the crystal required before using it.
    // This prevents the player from simply holding the trigger button forever and being safe as a result.
    public float minimumThreshold;

    public AudioSource[] sounds;
    public AudioSource crystalThrum;
    public AudioSource activatedCrystal;

    // The distance away from the light that the enemies must be in order to feed on it.
    public float enemyFeedingRadius;
    // The rate at which one enemy drains the crystal's charge.
    public float enemyFeedingRate;

    // Whether the crystal is currently in its "used" state.
    private bool isActive;
    // The quantity of charge that the crystal has left before burning out.
    private float charges;
    // References to the Light_WardOffEnemies components of the active and passive lights.
    private Light_WardOffEnemies woeActive, woePassive;
    // The number of enemies currently feeding on the light.
    private int enemyFeedingCount = 0;
    // Reference to the trigger in which enemies can feed on the light.
    private CapsuleCollider feedingArea;

    // Division is an expensive operation, so calculating this constant ahead of time will help performance. -Paul
    private float intensityFactor = 1f / 12.5f;

    void Start()
    {
        // Create a trigger slightly larger than the light's radius that counts the number of enemies inside.
        // This will determine how quickly the crystal's charge should drain.
        // The trigger's radius will be determined by the current light.
        woeActive = ActiveLight.GetComponent<Light_WardOffEnemies>();
        woePassive = PassiveLight.GetComponent<Light_WardOffEnemies>();
        feedingArea = gameObject.AddComponent<CapsuleCollider>();
        feedingArea.isTrigger = true;

        // Initialize charges.
        charges = maxCharges;
        ResetLight();

        // Sound stuff.
        sounds = GetComponents<AudioSource>();
        crystalThrum = sounds[0];
        activatedCrystal = sounds[1];
        crystalThrum.Play();
        crystalThrum.loop = true;
    }

    protected override void Update()
    {
        base.Update();

        // The light will become dimmer with less charges.
        // When the charges hit 0, the light will have an intensity of 0.
        ActiveLight.intensity = charges * intensityFactor;
        PassiveLight.intensity = ActiveLight.intensity;
        if (charges > maxCharges)
        {
            charges = maxCharges;
        }
        if (charges < 0f)
        {
            charges = 0f;
        }
        // If the charge runs out, automatically disable the light.
        if (charges == 0f)
        {
            ResetLight();
        }
        if (isActive)
        {
            charges -= decayRate * Time.deltaTime;
        }
        else
        {
            charges += RechargeRate * Time.deltaTime;
        }
        // Drain the charge based on the number of enemies that are feeding.
        charges -= enemyFeedingCount * enemyFeedingRate * Time.deltaTime;
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        ActivateLight();
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        ResetLight();
    }

    public void ActivateLight()
    {
        // Only activate the light if the charge is above the minimum threshold.
        if (charges > minimumThreshold)
        {
            // Enable the active light and disable the passive light.
            isActive = true;
            ActiveLight.gameObject.SetActive(true);
            PassiveLight.gameObject.SetActive(false);
            // Play the activated crystal sound if it isn't already playing.
            if (activatedCrystal.isPlaying == false)
            {
                activatedCrystal.Play();
                activatedCrystal.loop = true;
            }
            // Set the feeding area's base radius to that of the active light.
            feedingArea.radius = woeActive.GetRadius() + enemyFeedingRadius;
        }
    }

    public void ResetLight()
    {
        // Disable the active light and enable the passive light.
        isActive = false;
        ActiveLight.gameObject.SetActive(false);
        PassiveLight.gameObject.SetActive(true);
        // Stop the activated crystal sound.
        activatedCrystal.Stop();
        // Set the feeding area's base radius to that of the passive light.
        feedingArea.radius = woePassive.GetRadius() + enemyFeedingRadius;
    }

    public float getCharges()
    {
        return charges;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyFeedingCount += 1;
            other.gameObject.GetComponent<EnemySound>().SetIsFeeding(true);
            Debug.Log("Crystal enemyFeedingCount: " + enemyFeedingCount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyFeedingCount -= 1;
            other.gameObject.GetComponent<EnemySound>().SetIsFeeding(false);
            Debug.Log("Crystal enemyFeedingCount: " + enemyFeedingCount);
        }
    }
}

