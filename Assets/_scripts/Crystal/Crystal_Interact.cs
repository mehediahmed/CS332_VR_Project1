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

    // The distance away from the light that the enemies must be in order to feed on it.
    public float enemyFeedingRadius;
    // The rate at which one enemy drains the crystal's charge.
    public float enemyFeedingRate;

    private AudioSource[] sounds;
    public AudioSource crystalThrum;
    public AudioSource activatedCrystal;

    // Reference to the FeedingArea object's feeding area script component.
    private Crystal_FeedingArea feedingArea;

    // Whether the crystal is currently in its "used" state.
    private bool isActive;
    // The quantity of charge that the crystal has left before burning out.
    private float charges;

    // Division is an expensive operation, so calculating this constant ahead of time will help performance. -Paul
    private float intensityFactor = 1f / 12.5f;

    void Start()
    {
        // Create the feeding area object.
        GameObject feedingAreaObject = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        feedingArea = feedingAreaObject.AddComponent<Crystal_FeedingArea>();
        feedingArea.PassVariables(ActiveLight, PassiveLight, gameObject, enemyFeedingRadius, enemyFeedingRate);
        feedingArea.name = name + "_FeedingArea";

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
        charges -= feedingArea.GetChargeDrain() * Time.deltaTime;
        //Debug.Log("Crystal charges: " + charges);
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
            feedingArea.UpdateRadius(false);
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
        feedingArea.UpdateRadius(true);
    }

    public float getCharges()
    {
        return charges;
    }
}

