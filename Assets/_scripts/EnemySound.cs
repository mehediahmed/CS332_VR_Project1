// Author(s): Josiah Erikson, Paul Calande
// Enemy sound script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    // How close the enemy must be to the player for the sound to play.
    //public int soundDistance;
    // The time in seconds between each DoAudio call.
    // DoAudio changes the enemy sounds depending on whether the enemy is feeding or not.
    public float doAudioFrequency;

    private AudioSource[] myAudio;
    private AudioSource feedingSound;
    private AudioSource movingSound;
    // Reference to the player object.
    private GameObject player;
    private ShadowEnemy_Movement sem;

    private void Start()
    {
        myAudio = GetComponents<AudioSource>();
        feedingSound = myAudio[0];
        movingSound = myAudio[1];
        sem = GetComponent<ShadowEnemy_Movement>();
        //player = sem.GetPlayerObject();

        StartCoroutine(PeriodicAudioCheck());
    }

    // Calls DoAudio frequently but not every frame since a calculation in Update is unnecessary.
    IEnumerator PeriodicAudioCheck()
    {
        while (true)
        {
            DoAudio();
            yield return new WaitForSeconds(doAudioFrequency);
        }
    }

    public void DoAudio()
    {
        // Whether the enemy is feeding on light. This determines which sound to play.
        bool isFeeding = sem.GetIsFeeding();

        /*
        Vector3 playerpos = player.transform.position;
        float playerDistance = Vector3.Distance(playerpos, transform.position);
        //Debug.Log("Playerpos is " + playerpos);
        //Debug.Log("Transform position is " + transform.position);
        //Debug.Log("SoundDistance is " + soundDistance);
        //Debug.Log("Distance between them is " + playerDistance);
        // If the enemy is too far away from the player, stop playing audio
        if (playerDistance > soundDistance)
        {
            //Debug.Log("Player is too far away to play sound");
            movingSound.Stop();
            feedingSound.Stop();
        }
        //Otherwise, if it's not playing, start playing it
        else
        */
        {
            // Debug.Log("Player is within range for playing sound");
            if (isFeeding)
            {
                if (feedingSound.isPlaying == false)
                {
                    movingSound.Stop();
                    feedingSound.Play();
                    feedingSound.loop = true;
                }
            }
            else
            {
                if (movingSound.isPlaying == false)
                {
                    feedingSound.Stop();
                    movingSound.Play();
                    movingSound.loop = true;
                }
            }
        }
    }
}