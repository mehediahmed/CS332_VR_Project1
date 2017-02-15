// Author(s): Paul Calande
// Cylindrical rope that extends between two GameObjects.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCylinder : MonoBehaviour
{
    // The two GameObjects that the cylinder will connect.
    public GameObject origin;
    public GameObject target;

    private void Start()
    {
        Vector3 pos1 = origin.transform.position;
        Vector3 pos2 = target.transform.position;
        Vector3 difference = pos2 - pos1;
        // Position the cylinder between the two GameObjects.
        transform.position = pos1 + difference * 0.5f;
        // Scale the cylinder.
        Vector3 newScale = transform.localScale;
        newScale.y = difference.magnitude * 0.5f;
        transform.localScale = newScale;
        // Rotate the cylinder.
        transform.rotation = Quaternion.FromToRotation(Vector3.up, difference);
    }
}
