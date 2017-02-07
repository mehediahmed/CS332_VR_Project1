using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Crystal_Interact : VRTK_InteractableObject
{
    public Light pointLight;
    public float charges;
    public float decayRate;
    public float rechargeRate;
    public float rangeIncrease;
    public float intensityIncrease;
	// Use this for initialization
	void Start () {
        charges = 100f;
        pointLight = GetComponentInChildren<Light>();
        resetLight();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        useLight();
        StartCoroutine(decayCharges());
        
    }
    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        StopCoroutine(decayCharges());
        resetLight();
    }
    public void useLight() {
        pointLight.intensity = +intensityIncrease;
        pointLight.range = + rangeIncrease;

    }
    IEnumerator decayCharges() {
        charges =- decayRate;
        yield return new WaitForSeconds(1);


    }
    IEnumerator recharge() {
        charges = +rechargeRate;
        yield return new WaitForSeconds(1);
    }

    public void resetLight() {

        pointLight.intensity = 1;
        pointLight.range = 10f;
    }


}

