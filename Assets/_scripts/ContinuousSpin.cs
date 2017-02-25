// Author(s): Paul Calande
// Make an object spin continuously.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSpin : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

    private void Update()
    {
        Vector3 rotation = new Vector3(xSpeed, ySpeed, zSpeed);
        transform.Rotate(rotation * Time.deltaTime);
    }
}