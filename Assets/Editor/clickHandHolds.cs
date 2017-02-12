using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[ExecuteInEditMode]
public class clickHandHolds : Editor {
    public GameObject handHolds;
    private int count;
	// Use this for initialization
	void Start () {
        count = 0;

    }
	void OnSceneGUI()
    {
       

    }
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 clickPos = hit.point;
            Instantiate(handHolds, hit.point, hit.rigidbody.transform.rotation);

        }


    }
}
