using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update

    private float lastDirectionChange;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Collider current = contact.thisCollider;
            Collider other = contact.otherCollider;
            if (other.gameObject.GetComponent<PartHealth>() != null)
            {
                if (current.CompareTag("Spike"))
                {
                    other.gameObject.GetComponent<PartHealth>().SubtractHealth(3);
                }
                else if (current.CompareTag("Block"))
                {
                    other.gameObject.GetComponent<PartHealth>().SubtractHealth(1);
                }
                else if (current.CompareTag("Center"))
                {
                    other.gameObject.GetComponent<PartHealth>().SubtractHealth(1);
                }
            }
        }
    }
}
