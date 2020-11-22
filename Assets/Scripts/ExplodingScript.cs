using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingScript : MonoBehaviour
{
    public GameObject explosion;
    public Vector3 explosionOffset;

    public GameObject smoke;
    public int maxSmokes;
    public float minForce;
    public float maxForce;
    public float radius;

    public void Start()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
        Explode();
    }

    public void Explode()
    {
        int smokeCounter = 0;
        if (explosion != null)
        {
            GameObject explosionFx = Instantiate(explosion, transform.position + explosionOffset, Quaternion.identity) as GameObject;
            Destroy(explosionFx, 5f);
        }
        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius, 0f, ForceMode.Impulse);
            }

            if (smoke != null && smokeCounter < maxSmokes)
            {
                if (Random.Range(1, 4) == 1)
                {
                    GameObject smokeFx = Instantiate(smoke, t.transform) as GameObject;
                    smokeCounter++;
                    Destroy(smokeFx, 5);
                }
                
            }
        }
    }
}
