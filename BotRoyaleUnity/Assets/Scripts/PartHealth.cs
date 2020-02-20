using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHealth : MonoBehaviour
{
    public int health;
    public Vector2Int relPos { get; private set; } = Vector2Int.zero;
    // Start is called before the first frame update
    void Start()
    {
        health = 3;
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
                GameObject.Find("Arena").GetComponent<ShrinkArena>().removeRobot();
            }
            else
            {
                transform.parent.gameObject.GetComponent<PartHandler>().partDestroyed(relPos.x, relPos.y);
                Destroy(gameObject);
            }
        }
    }

    public void setRelPos(int x, int z){
        relPos = new Vector2Int(x,z);
    }
}
