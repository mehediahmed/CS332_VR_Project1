
using UnityEngine;

using VRTK;

public class RopeGun : VRTK_InteractableObject
{
    public GameObject bullet;
    public GameObject selectionSphere;
    public float bulletSpeed;
    public LayerMask targetLayer;
    public Rope_Tube rope;
    public bool isTargetSet = false;

    private Vector3 pos;
    private Vector3 pos2;

    private GameObject origin;
    private GameObject target;



    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        //      selectionSphere.SetActive(true);
        Raycasting();
        spawnRope(isTargetSet);

    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
    }

    protected override void Update()
    {
        base.Update();
        Debug.DrawRay(bullet.transform.position, -transform.forward);
    }

    protected void Start()
    {
        bullet.SetActive(false);
    }

    private void FireBullet()
    {
        GameObject bulletClone = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation) as GameObject;

        bulletClone.SetActive(true);
        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        rb.AddForce(-bullet.transform.forward * bulletSpeed);
        //     Destroy(bulletClone, bulletLife);

    }



    private void Raycasting()
    {
        Ray ray = new Ray(bullet.transform.position, -transform.forward);
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(bullet.transform.position, -transform.forward);
        Debug.Log("raycasting");


        if (Physics.Raycast(ray, out hit, 10000f, targetLayer))
        {
            Debug.DrawLine(transform.position, hit.point);

            {
                Debug.Log("hit");
                //   Debug.Log(hit.collider.gameObject.name);
                //   selectionSphere.SetActive(true);
                //     selectionSphere.transform.Translate(hit.point);
                pos = hit.point;
                //  origin = hit.rigidbody.gameObject;
                Debug.DrawRay(bullet.transform.position, -transform.forward);


            }

        }



    }

    public void spawnRope(bool haveTarget)
    {
        if (!haveTarget)
        {
            origin = Instantiate(new GameObject(), pos, new Quaternion(0, 0, 0, 0));
            origin.AddComponent<Rope_Tube>();
            origin.AddComponent<SphereCollider>();
            origin.GetComponent<Rigidbody>().useGravity = false;
            //     Material newMat = Resources.Load("Assets/_Models/Materials/rope.mat",typeof(Material)) as Material;
            //   Debug.Log(newMat.ToString());

            isTargetSet = true;



        }
        else
        {
            target = Instantiate(new GameObject(), pos, new Quaternion(0, 0, 0, 0));
            target.AddComponent<SphereCollider>();

            origin.GetComponent<Rope_Tube>().target = target.transform;

            origin.GetComponent<Rope_Tube>().BuildRope();
            reset();

        }



    }

    public void reset()
    {
        origin = null;
        target = null;
        // pos = Vector3.zero;
        //  pos2 = Vector3.zero;
        isTargetSet = false;

    }

    void OnDrawGizmos()
    {

        Gizmos.DrawSphere(pos, 1f);

        //Debug.Log("giz");

    }

}

