namespace VRTK.Examples
{
    using UnityEngine;

    public class Gun : VRTK_InteractableObject
    {
        public GameObject bullet;
        public float bulletSpeed;

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
            FireBullet();
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
    }
}