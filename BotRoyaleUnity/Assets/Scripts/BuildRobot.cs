using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System;

public class BuildRobot : MonoBehaviour
{
    public GameObject block, center, spike, robotParent;


    // Start is called before the first frame update
    void Start()
    {
        BuildSampleRobots();
    }

    private void BuildSampleRobots()
    {
        string jsonString = File.ReadAllText("Assets/Scripts/robot1.json");
        GameObject robot1 = build(jsonString, "robot1");
        jsonString = File.ReadAllText("Assets/Scripts/robot2.json");
        GameObject robot2 = build(jsonString, "robot2");
        robot2.transform.position = new Vector3(4f, 1f, 4f);
        jsonString = File.ReadAllText("Assets/Scripts/robot3.json");
        GameObject robot3 = build(jsonString, "robot3");
        robot3.transform.position = new Vector3(-3f, 1f, -3f);
    }

    void setParent(GameObject parent, GameObject child)
    {
        child.transform.parent = parent.transform;
    }

    public GameObject build(string jsonString, string name)
    {
        var json = JSON.Parse(jsonString);
        int index = 0;
        GameObject parent = Instantiate(robotParent);
        int centerX = 0, centerY = 0;
        while (json[index] != null)
        {
            if (json[index]["type"] == "center")
            {
                centerX = json[index]["x"];
                centerY = json[index]["y"];
            }
            index++;
        }
        index = 0;
        while (json[index] != null)
        {
            string type = json[index]["type"];
            GameObject childPart = null;
            Vector3 pos = new Vector3((float)(json[index]["x"] - centerX), 0.5f, (float)(json[index]["y"] - centerY));
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
                    throw new NotImplementedException("Invalid JSON - direction");
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
                default:
                    throw new NotImplementedException("Invalid JSON - type");
            }
            childPart.name = name;
            childPart.GetComponent<PartHealth>().setRelPos(json[index]["x"] - centerX, json[index]["y"] - centerY);
            setParent(parent, childPart);
            index++;
        }
        parent.GetComponent<PartHandler>().setParts();
        parent.GetComponent<PartHandler>().delUnattachedParts();
        GameObject.Find("Arena").GetComponent<ShrinkArena>().addRobot();
        return parent;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
