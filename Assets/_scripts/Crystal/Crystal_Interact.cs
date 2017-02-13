using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Crystal_Interact : VRTK_InteractableObject
{

    public Light ActiveLight;
    
    public Light PassiveLight;
    public float charges;
    public bool isActive;
    public float decayRate;
    public float RechargeRate;

    void Start()
    {
        charges = 100;
        resetLight();
        

    }
    void Update() {
        ActiveLight.intensity = (float)(charges / 12.5);
        if (charges > 100)
        {

            charges = 100f;
        }
        if (charges < 0)
        {

            charges = 0;

        }
        if (isActive)
        {
            charges -= decayRate * Time.deltaTime;
            ActiveLight.intensity = (float)(charges/12.5f);

        }
         if (!isActive)
        {
            charges += RechargeRate * Time.deltaTime;
             

        }
     
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        ActviateLight();
       //StartCoroutine(decayCharges());
       // StopCoroutine(recharge());

    }
    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        resetLight();
        //StartCoroutine(recharge());
        //StopCoroutine(decayCharges());
    }
    IEnumerator decayCharges() {
        charges = charges - 20f;
        yield return new WaitForSeconds(1f);
    }
    IEnumerator recharge()
    {
        charges = charges + 5f;
        yield return new WaitForSeconds(1f);

    }
    public void ActviateLight()
    {
        isActive = true;
        ActiveLight.gameObject.SetActive(true);
        PassiveLight.gameObject.SetActive(false);



    }
    public void resetLight()
    {
        isActive = false;
        ActiveLight.gameObject.SetActive(false);
        PassiveLight.gameObject.SetActive(true);



    }

}

