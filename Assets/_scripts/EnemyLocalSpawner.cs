// Author(s): Paul Calande
// Local spawner class for shadow enemies.
// All child GameObjects of this component's GameObject in the hierarchy will be used as shadow monster spawn points.

// Comment out the following line to disable various debug items.
//#define SHOW_SPAWN_VECTORS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocalSpawner : MonoBehaviour
{
    // Reference to the player object.
    public GameObject playerObject;
    // The maximum number of shadow enemies from this object's pool that can be enabled at once.
    public int maxNumberOfEnemies = 1;
    // The number of seconds before the first enemy spawn.
    public float timeBeforeFirstSpawn = 1.0f;
    // The number of seconds between each enemy spawn.
    public float timeBetweenSpawns = 3.0f;
    // The minimum distance away from the spawner the player must be before the enemy can spawn.
    public float spawnDistance = 10.0f;

    private List<Vector3> spawnLocations = new List<Vector3>();
    private ObjectPool pool;
    private int numberOfSpawnLocations;

#if SHOW_SPAWN_VECTORS
    private float debugDirection = 0.0f;
    private Vector3 debugPosition = new Vector3(0f, 0f, 0f);
#endif

    private void Awake()
    {
        pool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        // Populate the enemy pool.
        pool.Populate(maxNumberOfEnemies);
        // Get the spawn locations.
        // The following code makes sure that only the children game objects are counted.
        Transform[] spawnLocationsArray = GetComponentsInChildren<Transform>();
        foreach (Transform comp in spawnLocationsArray)
        {
            if (comp.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                spawnLocations.Add(comp.position);
            }
        }
        // Get the number of spawn locations.
        numberOfSpawnLocations = spawnLocations.Count;
        //Debug.Log("Spawn location count: " + numberOfSpawnLocations);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by the following object: " + other.name + " with tag " + other.tag);
        // If the player enters the trigger region...
        if (other.name == "VRTK_HeadsetColliderContainer")
        {
            //Debug.Log("OMG!!!!!!! KICK THE PLAYER'S ASS");
            StartCoroutine(SpawnEnemies());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "VRTK_HeadsetColliderContainer")
        {
            //Debug.Log("That's enough.");
            //StopCoroutine(SpawnEnemies());
            StopAllCoroutines();
            pool.UnbookAllDeactivatedObjects();
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(timeBeforeFirstSpawn);
        // Loop until the coroutine is stopped.
        while (true)
        {
            GameObject enemy;
            if (pool.BookObject(out enemy))
            {
                // Choose a spawn point randomly.
                int randomLocation = Random.Range(0, numberOfSpawnLocations);
                // Use this spawn point in the coroutine.
                StartCoroutine(SpawnEnemy(enemy, randomLocation));
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    IEnumerator SpawnEnemy(GameObject enemy, int randomLocation)
    {
        //Debug.Log("SpawnEnemy");
        // The loop will become exitable when this variable becomes false.
        bool needsToSpawn = true;
        // Get the spawn position.
        Vector3 spawnPosition = spawnLocations[randomLocation];
        // Let's enter loop purgatory!
        while (needsToSpawn)
        {
            //Debug.Log("SpawnEnemy needsToSpawn loop");
            // Calculate the angle between the camera and the spawn point.
            float z_component = spawnPosition.z - Camera.main.transform.position.z;
            float x_component = spawnPosition.x - Camera.main.transform.position.x;
            // Atan2 returns degree values within the -180 to 180 range.
            float direction = Mathf.Atan2(x_component, z_component) * Mathf.Rad2Deg;
#if SHOW_SPAWN_VECTORS
            debugDirection = direction;
            debugPosition = spawnPosition;
#endif
            // If the angle is outside the camera view...
            if (!CameraView.DegreeAngleN180180IsInFOV(direction))
            {
                //Debug.Log("Outside of view indeed, my friend!");
                // Attempt to move the agent to the NavMesh.
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPosition, out hit, 4f, NavMesh.AllAreas))
                {
                    //Debug.Log("NavMeshHit successful!");
                    Vector3 desiredPosition = hit.position;
                    // Check that each light is sufficiently far away from the desired enemy spawn point.
                    bool ok = true;
                    // Get all of the anti-shadow light components in the scene.
                    Light_WardOffEnemies[] woes = (Light_WardOffEnemies[])FindObjectsOfType(typeof(Light_WardOffEnemies));
                    foreach (Light_WardOffEnemies woe in woes)
                    {
                        if (Vector3.Distance(desiredPosition, woe.gameObject.transform.position) < (woe.GetRadius() + 1f))
                        {
                            ok = false;
                            break;
                        }
                    }
                    // Also check that the player is sufficiently far away from the desired enemy spawn point.
                    if (Vector3.Distance(desiredPosition, playerObject.transform.position) < spawnDistance)
                    {
                        ok = false;
                    }
                    if (ok)
                    {
                        // Move the enemy.
                        enemy.transform.position = desiredPosition;
                        // Activate the enemy.
                        enemy.GetComponent<NavMeshAgent>().enabled = true;
                        enemy.GetComponent<ShadowEnemy_Movement>().SetPlayerObject(playerObject);
                        enemy.SetActive(true);
                        // Now we can get the heck out of this loop.
                        needsToSpawn = false;
                    }
                }
            }
            // If the spawn isn't yet successful...
            if (needsToSpawn)
            {
                // Keep running this coroutine until the camera view is appropriate.
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    // Helpful editor visuals.
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform trans in transforms)
        {
            if (trans.gameObject.name != gameObject.name)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawSphere(trans.position, 0.2f);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(trans.position, 0.2f);
            }
        }

#if SHOW_SPAWN_VECTORS
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(debugPosition, new Vector3(1f, 1f, 1f));
#endif
    }

#if SHOW_SPAWN_VECTORS
    private void Update()
    {
        Vector3 vec = new Vector3(Mathf.Sin(Mathf.Deg2Rad * debugDirection), 0.0f, Mathf.Cos(Mathf.Deg2Rad * debugDirection));
        vec *= 100f;
        Debug.DrawRay(Camera.main.transform.position, vec, Color.blue);
        CameraView.DebugDrawCameraRegion();
    }
#endif

}