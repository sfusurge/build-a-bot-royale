using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PartHandler : MonoBehaviour
{
    //keeps track of the positions where there are alive parts.
    public string[,] robotParts = new string[9, 9];

    public int[] directionStrength = new int[4];

    private Vector2Int centerPos;


    //array to calculate all attached parts.

    public bool[,] attachedParts;

    private Rigidbody rb;

    private SocketConnectionHandler socketConnectionHandler;

    public void setParts()
    {
         for(int row = 0; row < 9; row++){
            for(int column = 0; column < 9; column++){
                robotParts[row,column] = "none";
            }
        }
        foreach (Transform child in transform)
        {
            int x = (int)child.position.x;
            int z = (int)child.position.z;
            robotParts[x + 4, z + 4] = child.gameObject.name;
        }
    }

    public void delUnattachedParts()
    {
        attachedParts = new bool[9, 9];
        recursiveAttached(4, 4);
        foreach (Transform child in transform)
        {
            int x = child.gameObject.GetComponent<PartHealth>().relPos.x;
            int z = child.gameObject.GetComponent<PartHealth>().relPos.y;
            if (robotParts[x + 4, z + 4] != "none" && !attachedParts[x + 4, z + 4])
            {
                child.gameObject.GetComponent<PartHealth>().subtractDirectionStrength();
                Destroy(child.gameObject);
                rb.mass--;
                partDestroyed(x,z);
            }
        }
        //Just to make sure - sometimes it shows parts that are not connected.
        EmitCurrentParts();
    }

    private void recursiveAttached(int x, int z)
    {
        if (x >= 0 && x <= 8 && z >= 0 && z <= 8)
        {
            if (!attachedParts[x, z] && robotParts[x, z] != "none")
            {
                attachedParts[x, z] = true;
                if(robotParts[x,z] == "Center(Clone)" || robotParts[x,z] == "Block(Clone)"){
                    recursiveAttached(x + 1, z);
                    recursiveAttached(x - 1, z);
                    recursiveAttached(x, z + 1);
                    recursiveAttached(x, z - 1);
                }
            }
        }

    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        socketConnectionHandler = FindObjectOfType<SocketConnectionHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void partDestroyed(int x, int z)
    {
        robotParts[x+4, z+4] = "none";
        delUnattachedParts();
        EmitCurrentParts();
    }

    public void changeDirectionStrength(string direction, int change)
    {
        switch (direction)
        {
            case "north":
                directionStrength[0] += change;
                break;
            case "west":
                directionStrength[1] += change;
                break;
            case "south":
                directionStrength[2] += change;
                break;
            case "east":
                directionStrength[3] += change;
                break;
        }
    }

    public int greatestDirectionStrength()
    {
        int index = 0;
        for (int a = 1; a < 4; a++)
        {
            if (directionStrength[a] > directionStrength[index])
            {
                index = a;
            }
        }
        return index;
    }

    public void SetCenterPos(int x, int y)
    {
        centerPos = new Vector2Int(x, y);
    }

    public Vector2Int GetCenterPos()
    {
        return centerPos;
    }

    public void EmitCurrentParts()
    {
        JSONObject data = new JSONObject();
        data["action"] = "currentParts";
        data["name"] = gameObject.name;
        JSONArray parts = new JSONArray();
        foreach (Transform part in gameObject.transform)
        {
            var partHealth = part.gameObject.GetComponent<PartHealth>();
            JSONObject newPart = new JSONObject();
            newPart["type"] = partHealth.GetPartType();
            newPart["x"] = (partHealth.GetRelPos().x + centerPos.x);
            newPart["y"] = (partHealth.GetRelPos().y + centerPos.y);
            newPart["direction"] = partHealth.GetPartDirection();
            newPart["health"] = partHealth.GetHealth();
            if(robotParts[partHealth.GetRelPos().x + 4, partHealth.GetRelPos().y + 4] != "none"){
                parts.Add(newPart);
            }
        }
        data["parts"] = parts;
        socketConnectionHandler.EmitGameMessage(data);
    }
    public void EmitEmptyParts()
    {
        JSONObject data = new JSONObject();
        data["action"] = "currentParts";
        data["name"] = gameObject.name;
        JSONArray parts = new JSONArray();
        data["parts"] = parts;
        socketConnectionHandler.EmitGameMessage(data);
    }
}
