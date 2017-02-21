// Author(s): Paul Calande
// Script for a crystal's feeding area.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_FeedingArea : MonoBehaviour
{
    // The creator object of this feeding area.
    private GameObject creator;

    // References to the active and passive lights.
    private Light ActiveLight;
    private Light PassiveLight;

    // The distance away from the light that the enemies must be in order to feed on it.
    private float enemyFeedingRadius;
    // The rate at which one enemy drains the crystal's charge.
    private float enemyFeedingRate;

    // References to the Light_WardOffEnemies components of the active and passive lights.
    private Light_WardOffEnemies woeActive, woePassive;
    // The number of enemies currently feeding on the light.
    private int enemyFeedingCount = 0;
    // Reference to the trigger in which enemies can feed on the light.
    private CapsuleCollider feedingArea;

    private void Update()
    {
        // Simulate the effect of parenting.
        transform.position = creator.transform.position;
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

    public void UpdateRadius(bool isPassiveLight)
    {
        if (isPassiveLight)
        {
            // Set the feeding area's base radius to that of the passive light.
            feedingArea.radius = woePassive.GetRadius() + enemyFeedingRadius;
        }
        else
        {
            // Set the feeding area's base radius to that of the active light.
            feedingArea.radius = woeActive.GetRadius() + enemyFeedingRadius;
        }
    }

    public float GetChargeDrain()
    {
        //Debug.Log("Crystal_FeedingArea GetChargeDrain(): " + enemyFeedingCount * enemyFeedingRate);
        return enemyFeedingCount * enemyFeedingRate;
    }

    // Initialize this object with the appropriate variables.
    public void PassVariables(Light active, Light passive, GameObject stalk, float feedRadius, float feedRate)
    {
        ActiveLight = active;
        PassiveLight = passive;
        creator = stalk;
        enemyFeedingRadius = feedRadius;
        enemyFeedingRate = feedRate;

        // Create a trigger slightly larger than the light's radius that counts the number of enemies inside.
        // This will determine how quickly the crystal's charge should drain.
        // The trigger's radius will be determined by the current light.
        woeActive = ActiveLight.GetComponent<Light_WardOffEnemies>();
        woePassive = PassiveLight.GetComponent<Light_WardOffEnemies>();
        feedingArea = gameObject.AddComponent<CapsuleCollider>();
        feedingArea.isTrigger = true;
    }
}