using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private AudioSource myAudio;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
        //This is a comment
        myAudio.Play();
    }

    private void Update()
    {
        if (3 + 2 < 6)
        {
            myAudio.loop = true;
        }
    }
}
