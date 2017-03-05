// Author(s): Paul Calande
// Get the raw image component of this object and set it to a static variable.
// To be used as part of the Constellation's fade out routine.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteFade : MonoBehaviour
{
    public static RawImage img;

    private void Awake()
    {
        img = GetComponent<RawImage>();
    }
}