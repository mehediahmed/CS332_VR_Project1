// Author(s): Paul Calande
// Shadow enemy AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class ShadowEnemy_Movement : MonoBehaviour
{
    // The distance away from the player at which the enemy will despawn.
    public float despawnDistance;

    // Reference to the player object.
    private GameObject playerObject;
    // Reference to the NavMeshAgent component.
    private NavMeshAgent agent;

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
}
