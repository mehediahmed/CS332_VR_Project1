using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class RopeSoundsScript : MonoBehaviour
{

    private VRTK_InteractableObject rope;
    private AudioSource source;
    public AudioClip ropeGrabbedSound;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        rope = GetComponent<VRTK_InteractableObject>();
        source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        //Subscribe to the grabbed event
        rope.InteractableObjectGrabbed += GrabRock;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //What to do when the grabbed event is triggered
    void GrabRock(object thisrock, VRTK.InteractableObjectEventArgs somearguments)
    {
        source.PlayOneShot(ropeGrabbedSound);
        Debug.Log("Rope Grabbed");
    }
}

