// Author(s): Paul Calande
// Local spawner class for shadow enemies.
// All child GameObjects of this component's GameObject in the hierarchy will be used as shadow monster spawn points.

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocalSpawner : MonoBehaviour
{
    // Reference to the player object.
    public GameObject playerObject;
    // Reference to the enemy to spawn.
    public GameObject enemyObject;
    // Whether to make the helpful editor mesh invisible during gameplay.
    public bool makeMeshInvisible = true;
    // The maximum number of shadow enemies from this object's pool that can be enabled at once.
    public int maxNumberOfEnemies;
    // The number of seconds before the first enemy spawn.
    public float timeBeforeFirstSpawn = 1.0f;
    // The number of seconds between each enemy spawn.
    public float timeBetweenSpawns = 3.0f;

    private Transform[] spawnLocations;
    private ObjectPool pool;
    private int numberOfSpawnLocations;

    private void Awake()
    {
        if (makeMeshInvisible)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        pool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        // Populate the enemy pool.
        pool.Populate(maxNumberOfEnemies);
        // Get the spawn locations and take note of how many there are.
        spawnLocations = GetComponentsInChildren<Transform>();
        numberOfSpawnLocations = spawnLocations.Length;
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
            StopCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(timeBeforeFirstSpawn);
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

    IEnumerator SpawnEnemy(GameObject enemy, int randomLocation)
    {
        bool needsToSpawn = true;
        Vector3 spawnPosition = spawnLocations[randomLocation].position;
        while (needsToSpawn)
        {
            // Calculate the angle between the camera and the spawn point.
            float y_component = spawnPosition.z - Camera.main.transform.position.z;
            float x_component = spawnPosition.x - Camera.main.transform.position.x;
            float direction = Mathf.Atan2(y_component, x_component) * Mathf.Rad2Deg;
            // If the angle is within the camera view...
            if (CameraView.lowerBound < direction && direction < CameraView.upperBound)
            {
                // Keep running this coroutine until the camera view is appropriate.
                yield return new WaitForSeconds(0.1f);
            }
            // Otherwise, if the angle is outside the camera view...
            else
            {
                // Move the enemy to the spawn position.
                enemy.transform.position = spawnPosition;
                // Activate the enemy.
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                enemy.SetActive(true);
                // Now we can get the heck out of this loop.
                needsToSpawn = false;
            }
        }
    }
}