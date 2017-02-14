// Author(s): Mehedi Ahmed, Paul Calande
// Primary script for the rope gun.

using UnityEngine;

using VRTK;

public class RopeGun : VRTK_InteractableObject
{
    // Reference to the "bullet" child prefab (the bullet opening of the gun).
    public GameObject bullet;
    // Layer mask for raycasting.
    public LayerMask targetLayer;
    // Reference to the marker object.
    public GameObject marker;

    private Vector3 pos;

    // The following variables are static to ensure that one rope is shared between all rope guns.
    private static bool isTargetSet = false;
    private static GameObject origin;
    private static GameObject target;
    private static Rope_Tube origin_rope_tube;



    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        Raycasting();
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
        //Debug.DrawRay(bullet.transform.position, -transform.forward);
    }

    private void Raycasting()
    {
        Ray ray = new Ray(bullet.transform.position, -transform.forward);

        //Debug.DrawRay(bullet.transform.position, -transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000f, targetLayer))
        {
            //Debug.DrawLine(transform.position, hit.point);
            {
                //Debug.Log("hit");
                pos = hit.point;
                //Debug.DrawRay(bullet.transform.position, -transform.forward);
                SpawnRope();
            }
        }
    }

    public void SpawnRope()
    {
        if (!isTargetSet)
        {
            DestroyOldRope();

            origin = Instantiate(marker, pos, Quaternion.identity);
            isTargetSet = true;
        }
        else
        {
            target = Instantiate(marker, pos, Quaternion.identity);
            origin_rope_tube = origin.AddComponent<Rope_Tube>();
            origin_rope_tube.target = target.transform;
            origin_rope_tube.BuildRope();
            ResetTarget();
        }
    }

    public void ResetTarget()
    {
        isTargetSet = false;
    }

    public void DestroyOldRope()
    {
        if (origin_rope_tube)
        {
            origin_rope_tube.DestroyRope();
            Destroy(origin);
            Destroy(target);
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pos, 1f);
    }
    */
}
