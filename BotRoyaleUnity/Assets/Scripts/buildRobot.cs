using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System;

public class buildRobot : MonoBehaviour
{
    public GameObject block, center, spike;


    // Start is called before the first frame update
    void Start()
    {
        string jsonString = File.ReadAllText("Assets/Scripts/robot1.json");
        GameObject robot1  = build(jsonString);
        robot1.AddComponent<RoombaMovement>();
        jsonString = File.ReadAllText("Assets/Scripts/robot2.json");
        GameObject robot2  = build(jsonString);
        robot2.AddComponent<RoombaMovement>();
    }

    void setParent(GameObject parent, GameObject child){
        child.transform.parent = parent.transform;
    }

    GameObject build(string jsonString){
        var json = JSON.Parse(jsonString);
        int index = 0;
        GameObject centerParent = null;
        while (json[index] != null && centerParent == null)
        {
            if (json[index]["type"] == "center")
            {
                Vector3 pos = new Vector3((float)json[index]["x"], 0.5f, (float)json[index]["y"]);
                Quaternion rot = Quaternion.Euler(90, 0, -90);
                centerParent = Instantiate(center, pos, rot);
            }
            index++;
        }
        index = 0;
        while (json[index] != null)
        {
            string type = json[index]["type"];
            if (type != "center")
            {
                GameObject childPart = null;
                Vector3 pos = new Vector3((float)json[index]["x"], 0.5f, (float)json[index]["y"]);
                Quaternion rot;
                string direction = json[index]["direction"];
                switch (direction)
                {
                    case "north":
                        rot = Quaternion.Euler(90, 0, 0);
                        break;
                    case "south":
                        rot = Quaternion.Euler(-90, 0, 0);
                        break;
                    case "east":
                        rot = Quaternion.Euler(0, 0, -90);
                        break;
                    case "west":
                        rot = Quaternion.Euler(0, 0, 90);
                        break;
                    default:
                        rot = Quaternion.Euler(0, 0, 0);
                        break;
                }
                switch (type)
                {
                    case "block":
                        childPart = Instantiate(block, pos, rot);
                        break;
                    case "center":
                        childPart = Instantiate(center, pos, rot);
                        break;
                    case "spike":
                        childPart = Instantiate(spike, pos, rot);
                        break;
                }
                setParent(centerParent,childPart);
            }
            index++;

        }
        return centerParent;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
