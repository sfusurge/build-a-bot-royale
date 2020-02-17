using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System;

public class BuildRobot : MonoBehaviour
{
    public GameObject block, center, spike, pare;


    // Start is called before the first frame update
    void Start()
    {
        string jsonString = File.ReadAllText("Assets/Scripts/robot1.json");
        GameObject robot1 = build(jsonString);
        jsonString = File.ReadAllText("Assets/Scripts/robot2.json");
        GameObject robot2 = build(jsonString);
        robot2.transform.position = new Vector3(3,3,3);
    }

    void setParent(GameObject parent, GameObject child)
    {
        child.transform.parent = parent.transform;
    }

    GameObject build(string jsonString)
    {
        var json = JSON.Parse(jsonString);
        int index = 0;
        GameObject parent = new GameObject("Parent");
        parent.transform.position = new Vector3(0, 0.5f, 0);
        parent.transform.rotation = Quaternion.Euler(90, 0, -90);
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
            setParent(parent, childPart);
            index++;
        }
        parent.AddComponent<RoombaMovement>();
        Rigidbody parentRigidbody = parent.AddComponent<Rigidbody>();
        parentRigidbody.mass = 5;
        return parent;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
