using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts){
            Collider current = contact.thisCollider;
            Collider other = contact.otherCollider;
            if(other.tag != "Untagged"){
                Debug.Log(current.tag + " "+ current.name + " : " + other.tag + " " + other.name);
                if(current.tag == "Spike"){
                    other.gameObject.GetComponent<PartHealth>().hit();
                }
            }
        }
    }
}
