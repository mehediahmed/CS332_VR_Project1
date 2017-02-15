using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCylinderCreator : MonoBehaviour
{
    public GameObject origin;
    public GameObject target;
    public GameObject ropePrefab;

    private void Start()
    {
        GameObject obj = Instantiate(ropePrefab, transform);
        RopeCylinder rc = obj.GetComponent<RopeCylinder>();
        rc.origin = origin;
        rc.target = target;
    }
}
