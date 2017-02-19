// Author(s): Mehedi Ahmed, Paul Calande
// Primary script for the rope gun.

// Comment out the following line of code to disable raycast debugging.
//#define RAYCAST_DEBUG

using UnityEngine;
using VRTK;

public class RopeGun : VRTK_InteractableObject
{
    // Reference to the "bullet" child prefab (the bullet opening of the gun).
    public GameObject bullet;
    // Layer mask for raycasting.
    public LayerMask targetLayer;
    // Reference to the marker prefab.
    public GameObject marker;
    // Reference to the rope prefab.
    public GameObject ropePrefab;
    //Sounds for shooting and hitting
    public AudioClip shootSound;
    public AudioClip hitSound;
    //source of the sounds this gun will make
    private AudioSource source;

    // Position of the raycast hit.
    private Vector3 pos;
    // The direction in which the gun will fire.
    private Vector3 fireDirection;

    // The following variables are static to ensure that one rope is shared between all rope guns.
    private static bool isTargetSet = false;
    private static GameObject origin;
    private static GameObject target;
    private static GameObject rope;



    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }

    public override void StartUsing(GameObject usingObject)
    {
#if RAYCAST_DEBUG
        Debug.Log("StartUsing()");
#endif
        base.StartUsing(usingObject);
        Raycasting();
        source.PlayOneShot(shootSound);
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
#if RAYCAST_DEBUG
        Debug.Log("Grabbed()");
#endif
        base.Grabbed(currentGrabbingObject);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
#if RAYCAST_DEBUG
        Debug.Log("StopUsing()");
#endif
        base.StopUsing(previousUsingObject);
    }

    protected override void Update()
    {
        base.Update();
        fireDirection = -transform.right;

#if RAYCAST_DEBUG
        Debug.DrawRay(bullet.transform.position, fireDirection);
#endif
    }

    private void Raycasting()
    {
        Ray ray = new Ray(bullet.transform.position, fireDirection);

#if RAYCAST_DEBUG
        Debug.Log("bullet.transform.position: " + bullet.transform.position);
        Debug.DrawRay(bullet.transform.position, fireDirection);
#endif

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000f, targetLayer))
        {
#if RAYCAST_DEBUG
            Debug.DrawLine(transform.position, hit.point);
#endif
            {
#if RAYCAST_DEBUG
                Debug.Log("hit");
                Debug.DrawRay(bullet.transform.position, fireDirection);
#endif
                pos = hit.point;
                SpawnRope();
                source.PlayOneShot(hitSound);
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
            rope = Instantiate(ropePrefab, pos, Quaternion.identity);
            RopeCylinder rc = rope.GetComponent<RopeCylinder>();
            rc.origin = origin;
            rc.target = target;
            /*
            origin_rope_tube = origin.AddComponent<Rope_Tube>();
            origin_rope_tube.target = target.transform;
            origin_rope_tube.BuildRope();
            */
            ResetTarget();
        }
    }

    public void ResetTarget()
    {
        isTargetSet = false;
    }

    public void DestroyOldRope()
    {
        if (origin)
        {
            //origin_rope_tube.DestroyRope();
            Destroy(origin);
            Destroy(target);
            Destroy(rope);
        }
    }

#if RAYCAST_DEBUG
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pos, 1f);
    }
#endif

}
