using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WalkingSound_VRTK : MonoBehaviour
{
    private VRTK_MoveInPlace playerMovement;
    private AudioSource source; //Where this audio will be coming from
    private GameObject player; //We want to attach the audio source to the player 
                               // Use this for initialization
    void Start()
    {

    }
    //Get the script that we can reference to get speed
    void Awake()
    {
        playerMovement = GetComponent<VRTK_MoveInPlace>();
        player = GameObject.FindWithTag("MainCamera");
        source = player.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerMovement.GetSpeed() > 0.0)
        {
            if (source.isPlaying == false)
            {
                source.Play();
                source.loop = true;
                //Debug.Log("Playing walking sound because player velocity is greater than 0");
            }
        }
        else
        {
            if (source.isPlaying == true)
            {
                source.Stop();
                //Debug.Log("Stopping walking sound because player speed is 0");

            }
        }
    }
}