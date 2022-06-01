using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegtableSplash : MonoBehaviour
{
    public GameObject splash;
    public GameObject sprite;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impactForceSum.y < -6.5f || collision.impactForceSum.y > 6.5f)
        {
            sprite.GetComponent<SpriteRenderer>().enabled = false;
            splash.SetActive(true);
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
