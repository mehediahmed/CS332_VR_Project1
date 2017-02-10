using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replaceMesh : MonoBehaviour {

    // Use this for initialization
    public MeshFilter lowPolyMesh;
    public MeshFilter currentMesh;
    private string meshName;
    private string newMeshName;



    void Start() {
        currentMesh = GetComponent<MeshFilter>();
        meshName = currentMesh.mesh.ToString();
        Debug.Log(meshName);
        meshName.Insert(meshName.Length - 5, "C");
        Debug.Log(meshName);


    }

}
