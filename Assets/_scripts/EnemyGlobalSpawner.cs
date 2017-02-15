// Author(s): Paul Calande
// Spawner object for shadow enemies. Functions on a global scale throughout the scene.

// Comment out the following line to disable the debug camera rays.
#define SHOW_CAMERA_FIELD
// Comment out the following line to disable spawning milestone debug logs.
//#define LOG_SPAWNING_MILESTONES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGlobalSpawner : MonoBehaviour
{
    // Reference to the player object.
    public GameObject playerObject;
    // Reference to the enemy to spawn.
    public GameObject enemyObject;
    // The maximum number of shadow enemies that can be enabled at once.
    public int maxNumberOfEnemies;
    // The distance away from the player at which the enemy will spawn.
    public float spawnDistance;
    // The distance away from the player at which the enemy will despawn.
    public float despawnDistance;
    // The time between each attempted enemy spawn, measured in seconds.
    public float timeBetweenEnemySpawns;

    // Use object pooling to store the enemies.
    private List<GameObject> enemies = new List<GameObject>();
    // A parallel pool for storing the NavMeshAgent components.
    private List<NavMeshAgent> agents = new List<NavMeshAgent>();
    // The main camera's horizontal field of view divided by 2. To be used for calculations.
    private float halffov;

#if SHOW_CAMERA_FIELD
    // Debug vector for showing the direction from which enemies spawn.
    private Vector3 spawnline = new Vector3(0f, 0f, 0f); // spawnPoint (blue ray)
#endif

    private void Start()
    {
        // Add the enemies to the object pool.
        for (int i=0; i<maxNumberOfEnemies; ++i)
        {
            GameObject enemy = Instantiate(enemyObject);
            ShadowEnemy_Movement component = enemy.GetComponent<ShadowEnemy_Movement>();
            component.SetPlayerObject(playerObject);
            component.SetDespawnDistance(despawnDistance);
            enemy.SetActive(false);
            enemies.Add(enemy);
            agents.Add(enemy.GetComponent<NavMeshAgent>());
        }
        // Calculate the main camera's horizontal field of view.
        float verticalfov = Camera.main.fieldOfView * Mathf.Deg2Rad;
        float horizontalfov = 2 * Mathf.Atan(Mathf.Tan(verticalfov / 2) * Camera.main.aspect);
        // Convert to degrees and divide by 2 for use in calculations.
        halffov = horizontalfov * Mathf.Rad2Deg * 0.5f;
        // Add a few extra degrees to prevent enemies from spawning directly on the edge of the camera's vision.
        halffov += 10f;
        // Start the timer-based enemy spawning coroutine.
        StartCoroutine("SpawnTimer");
    }

    private void Update()
    {
#if SHOW_CAMERA_FIELD
        // Debug stuff for showing the camera's field of view.
        float camrotx = Camera.main.transform.eulerAngles.y;
        float camrotx1 = camrotx + halffov;
        float camrotx2 = camrotx - halffov;
        camrotx1 *= Mathf.Deg2Rad;
        camrotx2 *= Mathf.Deg2Rad;
        Vector3 camrotxvec1 = new Vector3(Mathf.Sin(camrotx1), 0.0f, Mathf.Cos(camrotx1));
        Vector3 camrotxvec2 = new Vector3(Mathf.Sin(camrotx2), 0.0f, Mathf.Cos(camrotx2));
        camrotxvec1 *= 100;
        camrotxvec2 *= 100;
        Vector3 campos = Camera.main.transform.position;
        Debug.DrawRay(campos, camrotxvec1, Color.red);
        Debug.DrawRay(campos, camrotxvec2, Color.red);
        Debug.DrawRay(campos, spawnline, Color.blue);
#endif
    }

    // Repeated spawning coroutine.
    IEnumerator SpawnTimer()
    {
        // Loop indefinitely.
        while (true)
        {
            // Wait, then spawn an enemy.
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
            SpawnEnemy();
        }
    }

    // Spawn an enemy.
    public GameObject SpawnEnemy()
    {
        GameObject enemy = null;
        NavMeshAgent agent = null;
        // Try to find an inactive enemy in the pool.
        for (int i=0; i<maxNumberOfEnemies; ++i)
        {
            // If we've found an inactive enemy...
            if (enemies[i].activeInHierarchy == false)
            {
                // Keep a reference to the found enemy and its NavMeshAgent component.
                enemy = enemies[i];
                agent = agents[i];
                // End the loop prematurely.
                break;
            }
        }
        // If we didn't find an inactive enemy, the pool is currently exhausted.
        // Hence, we can't spawn any more enemies at this time, so we exit this function.
        if (enemy == null)
        {
            return null;
        }
        // Otherwise, we get the enemy ready and activate it.
        else
        {
            // Calculate a random direction to spawn the enemy in.
            // This calculation makes sure the enemy spawns outside of the camera view.
            float camrot = Camera.main.transform.eulerAngles.y;
            float lowerBound = camrot + halffov;
            float upperBound = camrot + 360f - halffov;
            //Debug.Log("lowerBound/upperBound: " + lowerBound + "/" + upperBound);
            float direction = Random.Range(lowerBound, upperBound);
            // Get all of the anti-shadow light components in the scene.
            Light_WardOffEnemies[] woes = (Light_WardOffEnemies[])FindObjectsOfType(typeof(Light_WardOffEnemies));
            // Enter the jaws of darkness: a big loop!
            while (direction < upperBound)
            {
                // Convert the direction from degrees to radians for use in trig functions.
                direction *= Mathf.Deg2Rad;
                // Use trigonometry magic to determine the spawning position of the enemy.
                float x = spawnDistance * Mathf.Sin(direction);
                float z = spawnDistance * Mathf.Cos(direction);
                float y = 0.0f;
                Vector3 baseSpawnPoint = new Vector3(x, y, z);
                // Make the spawn point relative to the player's position.
                Vector3 spawnPoint = playerObject.transform.position + baseSpawnPoint;
                // Try to find the closest point on the navmesh to the spawn point.
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPoint, out hit, 4f, NavMesh.AllAreas))
                {
                    Vector3 desiredPosition = hit.position;
#if SHOW_CAMERA_FIELD
                    spawnline = baseSpawnPoint;
#endif
#if LOG_SPAWNING_MILESTONES
                    Debug.Log("NavMesh.SamplePosition check satisfied.");
#endif
                    // Check that each light is sufficiently far away from the desired enemy spawn point.
                    bool ok = true;
                    foreach (Light_WardOffEnemies woe in woes)
                    {
                        if (Vector3.Distance(desiredPosition, woe.gameObject.transform.position) < (woe.GetRadius() + 1f))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
#if LOG_SPAWNING_MILESTONES
                        Debug.Log("Light_WardOffEnemies check satisfied.");
#endif
                        // Move the enemy to the position.
                        // Since it's a navmesh agent, we need to disable its agent component briefly.
                        // Otherwise, it will revert back to its old position.
                        //agent.enabled = false;
                        // Move the enemy.
                        enemy.transform.position = desiredPosition;
                        // Activate the enemy.
                        agent.enabled = true;
                        enemy.SetActive(true);
                        // Return the reference to this enemy.
                        return enemy;
                    }
                }
                // If the function has made it this far, the enemy has failed to spawn.
                // This loop will repeat until success is reached or too much time has been wasted.
                // Slightly increase the direction for different results next iteration.
                direction *= Mathf.Rad2Deg;
                direction += 4f;
            }
            // Failed to spawn an agent.
#if SHOW_CAMERA_FIELD
            spawnline = new Vector3(0f, 0f, 0f);
#endif
            return null;
        }
    }
}