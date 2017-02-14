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

    private bool isActive;
    private float charges;

    // Division is an expensive operation, so calculating this constant ahead of time will help performance. -Paul
    private float intensityFactor = 1f / 12.5f;

    void Start()
    {
        charges = maxCharges;
        ResetLight();
    }

    protected override void Update()
    {
        base.Update();

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
        if (!isActive)
        {
            charges += RechargeRate * Time.deltaTime;
        }
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
            isActive = true;
            ActiveLight.gameObject.SetActive(true);
            PassiveLight.gameObject.SetActive(false);
        }
    }

    public void ResetLight()
    {
        isActive = false;
        ActiveLight.gameObject.SetActive(false);
        PassiveLight.gameObject.SetActive(true);
    }

    public float getCharges()
    {
        return charges;
    }
}

