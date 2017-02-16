// Author(s): Paul Calande
// Static class for handling additional camera-related functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraView is a static class, hence allowing its public functions to be accessed from anywhere in the program.
static class CameraView
{
    // How many extra degrees are added to the angles at the edge of the camera view.
    private static float padding;
    // Vertical and horizontal fields of views respectively.
    private static float verticalfov;
    private static float horizontalfov;
    // The main camera's horizontal field of view divided by 2. To be used for calculations.
    private static float halffov;

    // The following are absolute angles that define the edges of the camera's regions, including padding.
    // insideLower -> insideUpper = range of the INSIDE of the camera region.
    // outsideLower -> outsideUpper = range of the OUTSIDE of the camera region.
    public static float insideLowerBound
    {
        get
        {
            return Camera.main.transform.eulerAngles.y - halffov;
        }
    }
    public static float insideUpperBound
    {
        get
        {
            return Camera.main.transform.eulerAngles.y + halffov;
        }
    }
    public static float outsideLowerBound
    {
        get
        {
            return Camera.main.transform.eulerAngles.y + halffov;
        }
    }
    public static float outsideUpperBound
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
        verticalfov = Camera.main.fieldOfView * Mathf.Deg2Rad;
        horizontalfov = 2 * Mathf.Atan(Mathf.Tan(verticalfov / 2) * Camera.main.aspect);
        // Set the default padding.
        SetPadding(10.0f);
    }

    public static void SetPadding(float newPadding)
    {
        padding = newPadding;
        // Convert to degrees and divide by 2 for use in calculations.
        halffov = horizontalfov * Mathf.Rad2Deg * 0.5f;
        // Add a few extra degrees to prevent "out-of-view" objects from spawning directly on the edge of the camera's vision.
        halffov += padding;
    }

    // Draws the boundaries of the camera region. Should be called from some object's Update function.
    public static void DebugDrawCameraRegion()
    {
        float camrotx1 = insideUpperBound;
        float camrotx2 = insideLowerBound;
        camrotx1 *= Mathf.Deg2Rad;
        camrotx2 *= Mathf.Deg2Rad;
        Vector3 camrotxvec1 = new Vector3(Mathf.Sin(camrotx1), 0.0f, Mathf.Cos(camrotx1));
        Vector3 camrotxvec2 = new Vector3(Mathf.Sin(camrotx2), 0.0f, Mathf.Cos(camrotx2));
        camrotxvec1 *= 100f;
        camrotxvec2 *= 100f;
        Vector3 campos = Camera.main.transform.position;
        Debug.DrawRay(campos, camrotxvec1, Color.red);
        Debug.DrawRay(campos, camrotxvec2, Color.red);
    }

    // Converts an angle to the -180 to 180 degree range.
    public static float DegreeAngleToRangeN180180(float angle)
    {
        return ((angle + 180f + 360f) % 360f - 180f);
    }
    
    // Returns true if the angle is within the field of view.
    public static bool DegreeAngleN180180IsInFOV(float angle)
    {
        float anglediff = DegreeAngleToRangeN180180(Camera.main.transform.eulerAngles.y - angle);
        return (anglediff <= halffov && anglediff >= -halffov);
    }
}