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
        if (scale > minArenaScale)
        {
            transform.localScale = new Vector3(scale, 0.5f + scale / 2, scale);
            scale -= shrinkSpeed * Time.deltaTime;
        }
        if (numRobots <= 3){
            GameObject.Find("GroupCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 30;
            GameObject.Find("OrbitalCamera").SetActive(false);
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
