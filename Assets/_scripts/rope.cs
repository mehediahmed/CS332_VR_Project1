using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope : MonoBehaviour {

    // Use this for initialization
    public GameObject light;
	void Start () {
        GetComponent<CharacterJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
        
         GameObject k = Instantiate(light, transform.position, transform.rotation);
        k.transform.parent = transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
