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
    // How close the enemy must be to the player to deal damage. Smaller number = closer.
    public float damageDistance;
    // How much damage the enemy deals per second.
    public float damageRate;
    // The time (in seconds) between each check for nearby crystals. Raising this number decreases accuracy but increases performance.
    public float crystalCheckFrequency;
    // The time (in seconds) before the enemy despawns inside of a crystal's light.
    public float lightDespawnTime;

    // Reference to the player object.
    private GameObject playerObject;
    // Reference to the NavMeshAgent component.
    private NavMeshAgent agent;
    // Whether the enemy is feeding on light.
    private bool isFeeding = false;
    // The last moment in which the enemy was inside of a crystal's passive light.
    private float lastTimeInLight = 0f;
    // The amount of continuous time the enemy has spent inside of a crystal's passive light.
    private float continuedTimeInLight = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Get the player position.
        Vector3 playerpos = playerObject.transform.position;
        // Set the NavMesh destination to the player's position.
        agent.destination = playerpos;
        // Calculate the distance from the enemy to the player.
        float distanceToPlayer = Vector3.Distance(playerpos, transform.position);
        // If the enemy is too far away from the player...
        if (distanceToPlayer > despawnDistance)
        {
            // Disable (despawn) the enemy.
            gameObject.SetActive(false);
        }
        // If the enemy is close enough to the player to deal damage...
        if (distanceToPlayer < damageDistance)
        {
            // DO MURDER!
            DealPlayerDamage(damageRate * Time.deltaTime);
        }
    }

    private void DealPlayerDamage(float damage)
    {
        // TODO: Interface with the player's life code.
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
                float radiusFeeding = ci.GetFeedingTotalRadius();
                float radiusPassiveLight = ci.GetPassiveLightRadius();
                if (distance < radiusFeeding)
                {
                    ci.AddEnemy(gameObject);
                    if (ci.getCharges() > ci.minimumThreshold)
                    {
                        // Only be in the "feeding" state when there's actually something to feed on.
                        isFeeding = true;
                    }
                }
                else
                {
                    ci.RemoveEnemy(gameObject);
                }
                // Check if the enemy has been inside of the passive light for too long.
                // If the enemy has been inside of the passive light for too long, despawn it.
                // On the other hand, if it has been outside of the passive light for a while, reset its continuous timer.
                if (distance < radiusPassiveLight)
                {
                    float currentTime = Time.time;
                    float timeSinceLastCheck = currentTime - lastTimeInLight;
                    continuedTimeInLight += timeSinceLastCheck;
                    lastTimeInLight = currentTime;
                    if (continuedTimeInLight > lightDespawnTime)
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    continuedTimeInLight = 0f;
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