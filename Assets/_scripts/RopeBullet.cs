// Author(s): Paul Calande
// Script for the bullet fired by the rope gun.

using UnityEngine;

public class RopeBullet : MonoBehaviour
{
    // Reference to the rope object.
    public GameObject rope;

    // Number of seconds remaining before being deactivated.
    private float life;

    private void Update()
    {
        // If the bullet's life runs out, disable it.
        life -= Time.deltaTime;
        if (life <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this bullet collides with an object with the Environment tag...
        if (collision.gameObject.tag == "Environment")
        {
            // Deactivate the bullet.
            gameObject.SetActive(false);
            // Activate the rope.
            rope.SetActive(true);
        }
    }
}