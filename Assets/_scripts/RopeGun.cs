// Author(s): Paul Calande
// Script for a gun that shoots a rope bullet.

using UnityEngine;
using VRTK;

public class RopeGun : VRTK_InteractableObject
{
    // Reference to the rope gun bullet prefab.
    public GameObject bulletPrefab;
    // The object from which the rope gun spawns.
    public GameObject bulletSpawner;
    // Speed of the bullet.
    public float bulletSpeed;
    // Lifetime of the bullet (in seconds).
    // If the bullet goes this long without colliding with anything, it will be destroyed.
    public float bulletLifetime;

    // Reference to the bullet GameObject.
    private GameObject bullet;
    // The bullet's Rigidbody component.
    private Rigidbody rb;

    // Called when the use trigger is pressed on the held object.
    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        FireBullet();
    }

    private void Start()
    {
        bullet = Instantiate(bulletPrefab);
        rb = bullet.GetComponent<Rigidbody>();
        bullet.SetActive(false);
    }

    private void FireBullet()
    {
        // If a bullet already exists, don't fire a bullet.
        // This makes sure that only one bullet exists at a time.
        if (bullet.activeInHierarchy)
        {
            return;
        }
        // Move the bullet to the position of the bullet spawner. Rotation, too.
        Transform bulletTransform = bullet.transform;
        Transform bulletSpawnerTransform = bulletSpawner.transform;
        bulletTransform.position = bulletSpawnerTransform.position;
        bulletTransform.rotation = bulletSpawnerTransform.rotation;
        // Activate the bullet.
        bullet.SetActive(true);
        // Send the bullet on its merry way!
        rb.AddForce(-bullet.transform.forward * bulletSpeed);
    }
}
