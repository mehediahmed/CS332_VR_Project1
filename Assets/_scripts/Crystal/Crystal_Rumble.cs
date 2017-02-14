using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Crystal_Rumble : VRTK_InteractHaptics
{
    private Crystal_Interact interact;
    private float maxChargesFactor;


    public override void HapticsOnUse(VRTK_ControllerActions controllerActions)
    {
        base.HapticsOnUse(controllerActions);
        strengthOnUse = interact.getCharges() * maxChargesFactor;

    }
    // Use this for initialization
    void Awake ()
    {
        interact = gameObject.GetComponent<Crystal_Interact>();
        maxChargesFactor = 1f / interact.maxCharges;
	}
	
	// Update is called once per frame
	void Update ()
    {
        strengthOnUse = interact.getCharges() * maxChargesFactor;
	}   
}
