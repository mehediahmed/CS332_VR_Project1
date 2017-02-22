// Author(s): Paul Calande
// Shadow enemy AI.

// Comment out the following line to prevent console messages every time an enemy is enabled or disabled.
//#define ENABLE_DISABLE_LOGGING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class ShadowEnemy_Movement : MonoBehaviour
{
    // The distance away from the player at which the enemy will despawn.
    public float despawnDistance;
    // The time (in seconds) between each check for nearby crystals. Raising this number decreases accuracy but increases performance.
    public float crystalCheckFrequency;

    // Reference to the player object.
    private GameObject playerObject;
    // Reference to the NavMeshAgent component.
    private NavMeshAgent agent;
    // Whether the enemy is feeding on light.
    private bool isFeeding = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Vector3 playerpos = playerObject.transform.position;
        // Set the NavMesh destination to the player's position.
        agent.destination = playerpos;
        // If the enemy is too far away from the player...
        if (Vector3.Distance(playerpos, transform.position) > despawnDistance)
        {
            // Disable (despawn) the enemy.
            gameObject.SetActive(false);
        }
    }

    public void SetPlayerObject(GameObject obj)
    {
        playerObject = obj;
    }

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    private void OnEnable()
    {
#if ENABLE_DISABLE_LOGGING
        Debug.Log("Shadow enemy enabled.");
#endif
        StartCoroutine(CheckForCrystals());
    }

    private void OnDisable()
    {
#if ENABLE_DISABLE_LOGGING
        Debug.Log("Shadow enemy disabled.");
#endif
        StopCoroutine(CheckForCrystals());
    }

    // Check for nearby crystals to feed on.
    IEnumerator CheckForCrystals()
    {
        while (true)
        {
            isFeeding = false;
            List<GameObject> crystals = Crystal_Interact.GetActiveCrystals();
            foreach (GameObject crystal in crystals)
            {
                Crystal_Interact ci = crystal.GetComponent<Crystal_Interact>();
                float distance = Vector3.Distance(transform.position, crystal.transform.position);
                float radius = ci.GetFeedingTotalRadius();
                if (distance < radius)
                {
                    if (ci.getCharges() > ci.minimumThreshold)
                    {
                        // Only be in the "feeding" state when there's actually something to feed on.
                        isFeeding = true;
                    }
                    ci.AddEnemy(gameObject);
                }
                else
                {
                    ci.RemoveEnemy(gameObject);
                }
            }
            yield return new WaitForSeconds(crystalCheckFrequency);
        }
    }

    // Returns whether the enemy is feeding on light.
    public bool GetIsFeeding()
    {
        return isFeeding;
    }
}