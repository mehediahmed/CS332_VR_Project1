// Author(s): Paul Calande
// This script is automatically attached to pooled objects.
// It caused the pooled objects to become unbooked upon disable.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolUnbooker : MonoBehaviour
{
    // Reference to the object pool component.
    public ObjectPool pool;
    // The integer ID of this object within the pool.
    public int id;

    private void OnDisable()
    {
        // When the object is disabled, unbook it from its corresponding pool.
        pool.UnbookObject(id);
    }
}