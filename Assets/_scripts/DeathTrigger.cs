// Author(s): Paul Calande
// Script for death trigger. The player will die upon entry.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger region...
        if (other.name == "[CameraRig]")
        {
            GameOver.message = "You fell to your death.";
            other.GetComponent<Player>().Die();
        }
    }
}