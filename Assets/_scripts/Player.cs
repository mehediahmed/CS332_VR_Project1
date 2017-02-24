// Author(s): Paul Calande
// Player script. To be attached to the player object.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public static reference to the player object.
    public static GameObject playerObject;
    // Reference to the watch.
    public GameObject watch;

	private void Awake()
    {
        // Set the playerObject to this gameObject automatically.
        playerObject = gameObject;
    }
}
