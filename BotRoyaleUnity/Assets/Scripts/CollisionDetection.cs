using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    
    private float lastDirectionChange;
    void Start()
    {
        lastDirectionChange = Time.time;

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
            if (!other.CompareTag("Untagged"))
            {
                if (!other.CompareTag("Wall"))
                {
                    if (current.CompareTag("Spike"))
                    {
                        other.gameObject.GetComponent<PartHealth>().hit();
                    }
                }
                /*
                else if(Time.time - lastDirectionChange > 1)
                {
                    if (gameObject.GetComponent<RoombaMovement>().navigationMode == "reverse")
                    {
                        gameObject.GetComponent<RoombaMovement>().setNavigationMode("forward");
                    }
                    else
                    {
                        gameObject.GetComponent<RoombaMovement>().setNavigationMode("reverse");
                    }
                }
                */
            }
        }
    }
}
