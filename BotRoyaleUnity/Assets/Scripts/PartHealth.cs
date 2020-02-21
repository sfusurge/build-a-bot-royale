using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartHealth : MonoBehaviour
{
    public float maxHealth = 20;

    public Color initialColor;
    public float health;
    public Vector2Int relPos { get; private set; } = Vector2Int.zero;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        initialColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubtractHealth(int damage)
    {
        health -= damage;
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
        UpdateColor();
    }

    private void UpdateColor(){
        GetComponent<Renderer>().material.color = Color.Lerp(initialColor, Color.red, 1-(health/maxHealth));
    }
    public void setRelPos(int x, int z)
    {
        relPos = new Vector2Int(x, z);
    }
}
