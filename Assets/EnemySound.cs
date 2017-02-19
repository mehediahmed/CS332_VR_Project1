//Author Josiah Erikson
//Enemy sound script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private AudioSource myAudio;
    public int soundDistance;
    // Reference to the player object.
    private GameObject player;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("MainCamera");
        Vector3 playerpos = player.transform.position;
        //Debug.Log("Playerpos is " + playerpos);
        //Debug.Log("Transform position is " + transform.position);
        //Debug.Log("SoundDistance is " + soundDistance);
        //Debug.Log("Distance between them is " + Vector3.Distance(playerpos, transform.position)); 
        // If the enemy is too far away from the player, stop playing audio
        if (Vector3.Distance(playerpos, transform.position) > soundDistance)
        {
          //  Debug.Log("Player is too far away to play sound");
            myAudio.Stop();
        }
    }

    private void Update()
    {
        Vector3 playerpos = player.transform.position;
        // If the enemy is too far away from the player, stop playing audio
        float playerDistance = Vector3.Distance(playerpos, transform.position);
        //Debug.Log("Playerpos is " + playerpos);
        //Debug.Log("Transform position is " + transform.position);
        //Debug.Log("SoundDistance is " + soundDistance);
        //Debug.Log("Distance between them is " + playerDistance);
        if (playerDistance > soundDistance)
        {
            //Debug.Log("Player is too far away to play sound");
            myAudio.Stop();
        }
        else
        //Otherwise, if it's not playing, start playing it
        {
           // Debug.Log("Player is within range for playing sound");
            if (myAudio.isPlaying == false)
            {
                myAudio.Play();
                myAudio.loop = true;
            }
        }
    }
}