using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimation : MonoBehaviour {
    public Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.Play("active");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
