// Author(s): Paul Calande
// Static class for handling additional camera-related functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraView is a static class, hence allowing its public functions to be accessed from anywhere in the program.
static class CameraView
{
    // How many extra degrees are added to the angles at the edge of the camera view.
    const float padding = 10f;

    // The main camera's horizontal field of view divided by 2. To be used for calculations.
    private static float halffov;

    // The absolute angles that define the edges of the camera's regions, including padding.
    public static float lowerBound
    {
        get
        {
            return Camera.main.transform.eulerAngles.y + halffov;
        }
    }
    public static float upperBound
    {
        get
        {
            return Camera.main.transform.eulerAngles.y + 360f - halffov;
        }
    }

    // Static class constructor.
    static CameraView()
    {
        // Calculate the main camera's horizontal field of view.
        float verticalfov = Camera.main.fieldOfView * Mathf.Deg2Rad;
        float horizontalfov = 2 * Mathf.Atan(Mathf.Tan(verticalfov / 2) * Camera.main.aspect);
        // Convert to degrees and divide by 2 for use in calculations.
        halffov = horizontalfov * Mathf.Rad2Deg * 0.5f;
        // Add a few extra degrees to prevent enemies from spawning directly on the edge of the camera's vision.
        halffov += padding;
    }
}