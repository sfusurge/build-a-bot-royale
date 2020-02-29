using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkArena : MonoBehaviour
{
    private float scale = 1f;
    private float shrinkSpeed = 0.02f;

    public bool ShrinkActive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ShrinkActive)
        {
            if (scale > 0)
            {
                transform.localScale = new Vector3(scale, 0.5f + scale / 2, scale);
                scale -= shrinkSpeed * Time.deltaTime;
            }
        }
    }
}
