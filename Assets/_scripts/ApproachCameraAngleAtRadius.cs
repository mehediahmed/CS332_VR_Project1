// Author(s): Paul Calande
// Makes the attached object move slowly towards where the main camera is looking.
// The attached object stays a certain radius away from the main camera's position.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCameraAngleAtRadius : MonoBehaviour
{
    // How far away the object stays from the main camera's position.
    public float radius;

    private void Update()
    {
        Vector3 cameraDirection = Camera.main.transform.eulerAngles;

        //transform.position =
    }
}