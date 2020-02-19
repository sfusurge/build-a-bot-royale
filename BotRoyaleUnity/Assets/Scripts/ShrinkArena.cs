using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkArena : MonoBehaviour
{
    private float scale = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(scale > 0.3){
            transform.localScale = new Vector3(scale,1,scale);
            scale-=0.00005f;
        }
    
    }
}
