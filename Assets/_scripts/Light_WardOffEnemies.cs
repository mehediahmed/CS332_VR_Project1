// Author(s): Paul Calande
// Add this script to any point light to make its light ward off enemies.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (Light))]
public class Light_WardOffEnemies : MonoBehaviour
{
    // Reference to a NavMeshObstacle component.
    private NavMeshObstacle obstacle;
    // Reference to the Light component.
    private Light lightComponent;

    private void Awake()
    {
        // Get the light component.
        lightComponent = GetComponent<Light>();
        // Note that the shadow enemies move around based on Unity's navigation meshes.
        // Creating a navigation mesh obstacle will inspire the shadow enemies to stay away.
        obstacle = gameObject.AddComponent<NavMeshObstacle>();
        // Set the obstacle shape to a capsule (default is box).
        // Capsules are circular, just like the light emitted by point lights.
        // Hence, capsules are better for us than boxes in this case.
        obstacle.shape = NavMeshObstacleShape.Capsule;
        UpdateObstacleRadius();
    }

    private void Update()
    {
        // Update the obstacle radius every step.
        // This is useful for lights that change size over time.
        UpdateObstacleRadius();
    }

    public void UpdateObstacleRadius()
    {
        // Set the obstacle radius to 0 if the light has no intensity.
        if (lightComponent.intensity == 0)
        {
            obstacle.radius = 0;
        }
        else
        {
            // Make the obstacle the same radius as the light.
            obstacle.radius = lightComponent.range;
        }
    }

    public float GetRadius()
    {
        return obstacle.radius;
    }
}
