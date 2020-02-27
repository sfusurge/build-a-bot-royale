using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkArena : MonoBehaviour
{
    private float scale = 1f;
    public int numRobots;
    float minArenaScale;
    private float shrinkSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        minArenaScale = 0.1f + 0.3f * numRobots;
        if (scale > 0)
        {
            transform.localScale = new Vector3(scale, 0.5f + scale / 2, scale);
            scale -= shrinkSpeed * Time.deltaTime;
        }

    }

    public void addRobot()
    {
        numRobots++;
    }

    public void removeRobot()
    {
        numRobots--;
    }
}
