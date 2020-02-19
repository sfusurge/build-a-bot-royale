using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHandler : MonoBehaviour
{
    //keeps track of the positions where there are alive parts.
    public bool[,] parts = new bool[9, 9];

    //array to calculate all attached parts.

    public bool[,] attachedParts;

    public void setParts()
    {
        foreach (Transform child in transform)
        {
            int x = (int)child.position.x;
            int z = (int)child.position.z;
            parts[x + 4, z + 4] = true;
        }
    }

    public void delUnattachedParts()
    {
        attachedParts = new bool[9, 9];
        recursiveAttached(4,4);
        foreach (Transform child in transform)
        {
            int x = child.gameObject.GetComponent<PartHealth>().relPos[0];
            int z = child.gameObject.GetComponent<PartHealth>().relPos[1];
            if(parts[x+4,z+4] && !attachedParts[x+4,z+4]){
                Destroy(child.gameObject);
            }
        }
    }

    public void recursiveAttached(int x, int z)
    {
        if(x >= 0 && x <= 8 && z >=0 && z <= 8){
            if(!attachedParts[x,z]&& parts[x,z]){
                attachedParts[x,z] = true;
                recursiveAttached(x+1,z);
                recursiveAttached(x-1,z);
                recursiveAttached(x,z+1);
                recursiveAttached(x,z-1);
            }
        }

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
