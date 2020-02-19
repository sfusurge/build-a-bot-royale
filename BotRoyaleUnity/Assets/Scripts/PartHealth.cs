using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHealth : MonoBehaviour
{
    public int health;
    public int[] relPos = new int[2];
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
            if (gameObject.CompareTag("Center"))
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
                transform.parent.gameObject.GetComponent<PartHandler>().delUnattachedParts();
            }
        }
    }

    public void setRelPos(int x, int z){
        relPos[0] = x;
        relPos[1] = z;
    }
}
