// Author(s): Paul Calande, Mehedi Ahmed, Josiah Erikson

// Comment out the following line to prevent the crystal's charge from being printed every frame.
//#define PRINT_CHARGES

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

    // The distance away from the light that the enemies must be in order to feed on it.
    public float enemyFeedingRadius;
    // The rate at which one enemy drains the crystal's charge.
    public float enemyFeedingRate;

    // AudioSource references.
    private AudioSource[] sounds;
    public AudioSource crystalThrum;
    public AudioSource activatedCrystal;

    // Reference to the FeedingArea object's feeding area script component.
    //private Crystal_FeedingArea feedingArea;

    // Whether the crystal is currently in its "used" state.
    private bool isActive;
    // The quantity of charge that the crystal has left before burning out.
    private float charges;
    // Division is an expensive operation, so calculating this constant ahead of time will help performance. -Paul
    private float intensityFactor;

    // A static list of the active crystals in the scene.
    private static List<GameObject> activeCrystals = new List<GameObject>();
    // A collection of the enemies that are feeding on the light.
    private HashSet<GameObject> enemiesFeeding = new HashSet<GameObject>();
    // The combined radius of the light with the feeding radius.
    private float enemyFeedingTotalRadius;

    void Start()
    {
        // Create the feeding area object.
        /*
        GameObject feedingAreaObject = new GameObject();
        feedingArea = feedingAreaObject.AddComponent<Crystal_FeedingArea>();
        feedingArea.PassVariables(ActiveLight, PassiveLight, gameObject, enemyFeedingRadius, enemyFeedingRate);
        feedingArea.name = name + "_FeedingArea";
        */

        // Initialize charges.
        intensityFactor = 8f / maxCharges;
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
            //Debug.Log("Charges have dipped below zero!");
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
        charges -= GetChargeDrain() * Time.deltaTime;

#if PRINT_CHARGES
        Debug.Log("Crystal charges: " + charges + " with " + enemiesFeeding.Count + " enemies feeding.");
#endif

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

    protected override void OnEnable()
    {
        base.OnEnable();
        // When the crystal is enabled, add it to the active crystals list.
        activeCrystals.Add(gameObject);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // When the crystal is disabled, remove it from the active crystals list.
        activeCrystals.Remove(gameObject);
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
            UpdateFeedingRadius();
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
        UpdateFeedingRadius();
    }

    public float getCharges()
    {
        return charges;
    }

    // Get the charge drain rate based on the number of enemies in the feeding radius.
    private float GetChargeDrain()
    {
        return enemiesFeeding.Count * enemyFeedingRate;
    }

    public static List<GameObject> GetActiveCrystals()
    {
        return activeCrystals;
    }

    // The following two functions respectively add and remove an enemy from the enemies feeding list.
    // This helps with keeping track of how many enemies are feeding on the light.
    public void AddEnemy(GameObject enemy)
    {
        enemiesFeeding.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemiesFeeding.Remove(enemy);
    }

    // Update the radius of the feeding area.
    private void UpdateFeedingRadius()
    {
        if (isActive)
        {
            // Set the feeding area's base radius to that of the active light.
            //feedingArea.radius = woeActive.GetRadius() + enemyFeedingRadius;
            enemyFeedingTotalRadius = ActiveLight.range + enemyFeedingRadius;
        }
        else
        {
            // Set the feeding area's base radius to that of the passive light.
            //feedingArea.radius = woePassive.GetRadius() + enemyFeedingRadius;
            enemyFeedingTotalRadius = PassiveLight.range + enemyFeedingRadius;
        }
    }

    // Get the radius of the current light combined with the radius of the feeding area.
    public float GetFeedingTotalRadius()
    {
        return enemyFeedingTotalRadius;
    }

    /*
    // Get the radius of the current light.
    public float GetCurrentLightRadius()
    {
        if (isActive)
        {
            return ActiveLight.range;
        }
        else
        {
            return PassiveLight.range;
        }
    }
    */

    // Get the radius of the passive light.
    public float GetPassiveLightRadius()
    {
        return PassiveLight.range;
    }
}