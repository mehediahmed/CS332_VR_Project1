// Author(s): Paul Calande
// Add this script to the watch's light. It gets the light component and stores it in a public static variable.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch_Light : MonoBehaviour
{
    public static Light watchLight;

    private void Awake()
    {
        watchLight = gameObject.GetComponent<Light>();
    }
}
