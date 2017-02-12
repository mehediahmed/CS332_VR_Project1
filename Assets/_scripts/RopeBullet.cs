// Author(s): Paul Calande
// Script for the bullet fired by the rope gun.

using UnityEngine;

public class RopeBullet : MonoBehaviour
{
    // Reference to the rope object.
    public GameObject rope;


    // Number of seconds remaining before being deactivated.
    public float life;

    private void Update()
    {
        // If the bullet's life runs out, disable it.
        life -= Time.deltaTime;
     
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this bullet collides with an object with the Environment tag...
        if (collision.gameObject.tag == "Environment")
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            GameObject ropeClone = Instantiate(rope, transform.position, transform.rotation);
            ropeClone.SetActive(true);
            Destroy(this, life);
            Destroy(ropeClone, life);

        }
    }
}