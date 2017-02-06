// Author(s): Paul Calande
// Shadow enemy AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class ShadowEnemy_Movement : MonoBehaviour
{
    // The name of the player object.
    public string playerObjectName;

    // Reference to the NavMeshAgent component.
    private NavMeshAgent agent;
    // Reference to the player object.
    private GameObject playerObject;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerObject = GameObject.Find(playerObjectName);
    }

    private void Update()
    {
        agent.destination = playerObject.transform.position;
    }
}
