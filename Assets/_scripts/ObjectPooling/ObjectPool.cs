// Author(s): Paul Calande
// Object pool featuring functionality for booking objects.

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Reference to the object to populate the pool with.
    public GameObject pooledObject;

    // The object pool.
    private List<GameObject> objects = new List<GameObject>();
    // A corresponding list of boolean variables that tell whether the object has been booked.
    // Booking an object means reserving it.
    // Once an object is booked, it cannot be returned from the BookObject function until it is unbooked.
    private List<bool> booked = new List<bool>();
    // The maximum number of objects that can be enabled at once.
    private int maxObjectCount;

    // Populate the object pool.
    public void Populate(int count)
    {
        // Set the maximum object count equal to the population count.
        maxObjectCount = count;
        // Populate the object pool.
        for (int i = 0; i < count; ++i)
        {
            // Instantiate the object.
            GameObject obj = Instantiate(pooledObject);
            // Add the object pool unbooker component to the object.
            ObjectPoolUnbooker opu = obj.AddComponent<ObjectPoolUnbooker>();
            opu.pool = this;
            opu.id = i;
            // Unbook the corresponding entry.
            booked.Add(false);
            // Deactive the object.
            obj.SetActive(false);
            // Add the object to the pool.
            objects.Add(obj);
        }
    }

    // Get an object from the pool, but don't activate it yet. Returns false if no object is available.
    // If an object is found, it is booked and assigned to obj.
    public bool BookObject(out GameObject obj)
    {
        obj = null;
        // Try to find an unbooked object in the pool.
        for (int i = 0; i < maxObjectCount; ++i)
        {
            // If we've found a viable object...
            if (booked[i] == false)
            {
                // Mark the object as booked.
                booked[i] = true;
                // Keep a reference to the found object.
                obj = objects[i];
                // Return with success!
                return true;
            }
        }
        // If we didn't find an inactive object, the pool is currently exhausted.
        // Hence, we can't get any objects from the pool at this time, so we return false.
        return false;
    }

    // Unbook an object based on its index in the pool.
    public void UnbookObject(int indexWithinPool)
    {
        booked[indexWithinPool] = false;
    }
}