using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSoundScript : MonoBehaviour {
    private AudioSource source;
    public AudioClip walkingSound;
    private int count; //The velocity seems finicky, so we want the player to come to a complete stop before it stops playing the walking sound
    public int framesBeforeStop;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("I'm running");
        float playerVelocity = GetComponent<Rigidbody>().velocity.magnitude;
        if (playerVelocity > 0.05)
        {
            if (source.isPlaying == false)
            {
                source.Play();
                source.loop = true;
                Debug.Log("Playing walking sound because player velocity is " + playerVelocity);
            }
        }
        else
        {
            if (source.isPlaying == true)
            {
                if (count > framesBeforeStop)
                {
                    count = 0;
                    source.Stop();
                    Debug.Log("Stopping walking sound because player velocity is " + playerVelocity);
                }
                else
                {
                    count++;
                }
            }
        }
		
	}
}
