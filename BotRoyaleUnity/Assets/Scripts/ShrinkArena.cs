using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkArena : MonoBehaviour
{
    private float scale = 1f;
    public int numRobots;
    float minArenaScale;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        minArenaScale = 0.1f + 0.3f*numRobots;
        if(scale > minArenaScale){
            transform.localScale = new Vector3(scale,0.5f + scale/2,scale);
            scale-=0.0001f;
        }
    
    }

    public void addRobot(){
        numRobots++;
    }

    public void removeRobot(){
        numRobots--;
    }
}
