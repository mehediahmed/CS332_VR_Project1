// Author(s): Paul Calande
// Global spawner class for shadow enemies. Functions on a global scale throughout the scene.

// Comment out the following line to disable the debug camera rays.
//#define SHOW_CAMERA_FIELD
// Comment out the following line to disable spawning milestone debug logs.
//#define LOG_SPAWNING_MILESTONES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGlobalSpawner : MonoBehaviour
{
    // The maximum number of shadow enemies that can be enabled at once.
    public int maxNumberOfEnemies;
    // The distance away from the player at which the enemy will spawn.
    public float spawnDistance;
    // The time between each attempted enemy spawn, measured in seconds.
    public float timeBetweenEnemySpawns;

    // Reference to the player object.
    private GameObject playerObject;
    // Reference to the enemy object pool.
    private ObjectPool pool;

#if SHOW_CAMERA_FIELD
    // Debug vector for showing the direction from which enemies spawn.
    private Vector3 spawnline = new Vector3(0f, 0f, 0f); // spawnPoint (blue ray)
#endif

    private void Start()
    {
        // Get the player object.
        playerObject = Player.playerObject;
        // Get and populate the enemy pool.
        pool = GetComponent<ObjectPool>();
        pool.Populate(maxNumberOfEnemies);
        // Start the timer-based enemy spawning coroutine.
        StartCoroutine(SpawnTimer());
    }

#if SHOW_CAMERA_FIELD

    private void Update()
    {
        // Debug stuff for showing the camera's field of view.
        float camrotx = Camera.main.transform.eulerAngles.y;
        float camrotx1 = camrotx + halffov;
        float camrotx2 = camrotx - halffov;
        camrotx1 *= Mathf.Deg2Rad;
        camrotx2 *= Mathf.Deg2Rad;
        Vector3 camrotxvec1 = new Vector3(Mathf.Sin(camrotx1), 0.0f, Mathf.Cos(camrotx1));
        Vector3 camrotxvec2 = new Vector3(Mathf.Sin(camrotx2), 0.0f, Mathf.Cos(camrotx2));
        camrotxvec1 *= 100f;
        camrotxvec2 *= 100f;
        Vector3 campos = Camera.main.transform.position;
        Debug.DrawRay(campos, camrotxvec1, Color.red);
        Debug.DrawRay(campos, camrotxvec2, Color.red);
        Debug.DrawRay(campos, spawnline, Color.blue);
    }

#endif

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
        // Attempt to get an enemy from the pool.
        GameObject enemy;
        // If we found an enemy...
        if (pool.BookObject(out enemy))
        {
            // Calculate a random direction to spawn the enemy in.
            // This calculation makes sure the enemy spawns outside of the camera view.
            //float lowerBound = CameraView.DegreeAngleToRangeN180180(CameraView.outsideLowerBound);
            //float upperBound = CameraView.DegreeAngleToRangeN180180(CameraView.outsideUpperBound);
            float direction = Random.Range(CameraView.outsideLowerBound, CameraView.outsideUpperBound);
            // Get all of the anti-shadow light components in the scene.
            Light_WardOffEnemies[] woes = (Light_WardOffEnemies[])FindObjectsOfType(typeof(Light_WardOffEnemies));
            // Enter the jaws of darkness: a big loop!
            while (direction < CameraView.outsideUpperBound)
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
                        // Move the enemy.
                        enemy.transform.position = desiredPosition;
                        // Activate the enemy.
                        enemy.GetComponent<NavMeshAgent>().enabled = true;
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
        }

        // At this point, we have ultimately failed to spawn an agent.

#if SHOW_CAMERA_FIELD
            spawnline = new Vector3(0f, 0f, 0f);
#endif

        return null;
    }
}