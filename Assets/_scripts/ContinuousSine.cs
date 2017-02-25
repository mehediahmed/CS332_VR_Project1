// Author(s): Paul Calande
// Make an object bob up and down continuously in a sine pattern.
// Modifies transform.position directly.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSine : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
    public float xMagnitude;
    public float yMagnitude;
    public float zMagnitude;

    private float xProgress = 0f;
    private float yProgress = 0f;
    private float zProgress = 0f;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Increment progress variables.
        xProgress += xSpeed * Time.deltaTime;
        yProgress += ySpeed * Time.deltaTime;
        zProgress += zSpeed * Time.deltaTime;
        // Keep all of the progress variables within the range of 0 to 2*pi.
        while (xProgress > PaulUtility.TWO_PI)
        {
            xProgress -= PaulUtility.TWO_PI;
        }
        while (yProgress > PaulUtility.TWO_PI)
        {
            yProgress -= PaulUtility.TWO_PI;
        }
        while (zProgress > PaulUtility.TWO_PI)
        {
            zProgress -= PaulUtility.TWO_PI;
        }
        while (xProgress < 0f)
        {
            xProgress += PaulUtility.TWO_PI;
        }
        while (yProgress < 0f)
        {
            yProgress += PaulUtility.TWO_PI;
        }
        while (zProgress < 0f)
        {
            zProgress += PaulUtility.TWO_PI;
        }
        // Calculate the offset coordinates.
        float xOff = xMagnitude * Mathf.Sin(xProgress);
        float yOff = yMagnitude * Mathf.Sin(yProgress);
        float zOff = zMagnitude * Mathf.Sin(zProgress);
        // Apply the offset to the base position.
        Vector3 positionalOffset = new Vector3(xOff, yOff, zOff);
        transform.position = originalPosition + positionalOffset;
    }
}