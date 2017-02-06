// Author(s): Paul Calande
// Example script for showing the effects of pulsating light on the shadow monsters.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_ExamplePulse : MonoBehaviour
{
    private Light lightComponent;
    private float asdf;
	// Use this for initialization
	void Start () {
        lightComponent = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        asdf += Time.deltaTime;
        lightComponent.range = 6f + 4f * Mathf.Sin(asdf);
	}
}
