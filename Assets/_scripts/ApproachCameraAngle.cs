// Author(s): Paul Calande
// Makes the attached object's angle move towards matching the main camera's angle.
// Also makes the attached object moves towards the main camera's position.
// Works well for worldspace UI canvases in VR.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCameraAngle : MonoBehaviour
{
    // How quickly the object's angle changes.
    public float rotationSpeedMultiplier;
    // How quickly the object's position changes.
    public float movementSpeedMultiplier;

    // Change the speed of movement and rotation based on how far away the destination is.
    private void Update()
    {
        Quaternion qMe = transform.rotation;
        Quaternion qCamera = Camera.main.transform.rotation;
        float rotationSpeed = Quaternion.Angle(qMe, qCamera) * rotationSpeedMultiplier * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(qMe, qCamera, rotationSpeed);

        Vector3 pMe = transform.position;
        Vector3 pCamera = Camera.main.transform.position;
        float movementSpeed = Vector3.Distance(pMe, pCamera) * movementSpeedMultiplier * Time.deltaTime;
        transform.position = Vector3.MoveTowards(pMe, pCamera, movementSpeed);
    }
}