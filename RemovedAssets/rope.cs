using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope : MonoBehaviour
{
    public GameObject my_light;

	void Start()
    {
        GetComponent<CharacterJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
        
        GameObject k = Instantiate(my_light, transform.position, transform.rotation);
        k.transform.parent = transform;
    }
}
