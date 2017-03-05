using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatortest : MonoBehaviour {
    public Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.Play("Attacking");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
