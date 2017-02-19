using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class RockClimbingSounds : MonoBehaviour {

    private VRTK_InteractableObject rock;
    private AudioSource source;
    public AudioClip rockGrabbedSound;

    // Use this for initialization
    void Start () {
		
	}

    void Awake ()
    {
        rock = GetComponent<VRTK_InteractableObject>();
        source = GetComponent<AudioSource>();
    }

    void OnEnable ()
    {
        //Subscribe to the grabbed event
         rock.InteractableObjectGrabbed += GrabRock;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //What to do when the grabbed event is triggered
    void GrabRock(object thisrock, VRTK.InteractableObjectEventArgs somearguments)
    {
        source.PlayOneShot(rockGrabbedSound);
    }
}
