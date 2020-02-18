using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHealth : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 1;
    }

    // Update is called once per frame
    void Update()
    {   

    }

    public void hit()
    {
        health--;
        if (health <= 0)
        {
            if (gameObject.tag == "Center")
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
