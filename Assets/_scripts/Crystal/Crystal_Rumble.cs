using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Crystal_Rumble : VRTK_InteractHaptics
{
    private Crystal_Interact interact;


    public override void HapticsOnUse(VRTK_ControllerActions controllerActions)
    {
        base.HapticsOnUse(controllerActions);

    }
    // Use this for initialization
    void Awake () {
        interact= gameObject.GetComponent<Crystal_Interact>();
	}
	
	// Update is called once per frame
	void Update () {
        strengthOnUse = interact.charges / 100f;
	}



}
