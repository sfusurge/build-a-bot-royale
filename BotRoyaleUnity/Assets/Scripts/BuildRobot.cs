using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System;
using UnityEngine.Events;

public class BuildRobot : MonoBehaviour
{
    public GameObject block, center, spike, robotParent;
    public Boolean buildSampleBots = true;
    // Start is called before the first frame update
    void Awake()
    {
        if(buildSampleBots){
            GameStateManager.Instance.RegisterActionToState(GameStateManager.GameStates.BATTLE, BuildSampleRobots);
         
        }
    }

    private void BuildSampleRobots()
    {
        string jsonString = File.ReadAllText("Assets/Scripts/robot1.json");
        GameObject robot1 = build(jsonString, "robot1");
        robot1.transform.position = new Vector3(0f, 0.5f, 5f);
        jsonString = File.ReadAllText("Assets/Scripts/robot2.json");
        GameObject robot2 = build(jsonString, "robot2");
        robot2.transform.position = new Vector3(4f, 0.5f, -5f);
        jsonString = File.ReadAllText("Assets/Scripts/robot3.json");
        GameObject robot3 = build(jsonString, "robot3");
        robot3.transform.position = new Vector3(-4f, 0.5f, -5f);
        GameObject robot4 = build(jsonString, "robot3");
        robot4.transform.position = new Vector3(-4f, 0.5f, -5f);
    }

    void setParent(GameObject parent, GameObject child)
    {
        child.transform.parent = parent.transform;
    }

    private void addDirectionStrength(GameObject parent, Vector3 pos, string direction, string type){
        if(type != "spike"){
            if(pos.x < 0){
                parent.GetComponent<PartHandler>().changeDirectionStrength("west",1);
            }
            if(pos.x > 0){
                parent.GetComponent<PartHandler>().changeDirectionStrength("east",1);
            }
            if(pos.z > 0){
                parent.GetComponent<PartHandler>().changeDirectionStrength("north",1);
            }
            if(pos.z < 0){
                parent.GetComponent<PartHandler>().changeDirectionStrength("south",1);
            }
        }else{
            parent.GetComponent<PartHandler>().changeDirectionStrength(direction,3);
        }
    }

    public GameObject build(string jsonString, string name)
    {
        var json = JSON.Parse(jsonString);
        int index = 0;
        GameObject parent = Instantiate(robotParent);
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        rb.mass = 0;
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
            Vector3 pos = new Vector3((float)(json[index]["x"] - centerX), 1f, (float)(json[index]["y"] - centerY));
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
            childPart.GetComponent<PartHealth>().setRelPos(json[index]["x"] - centerX, json[index]["y"] - centerY, type, direction);
            addDirectionStrength(parent,pos,direction,type);
            setParent(parent, childPart);
            rb.mass++;
            index++;
        }
        parent.GetComponent<PartHandler>().setParts();
        parent.GetComponent<PartHandler>().delUnattachedParts();
        GameStateManager.Instance.addRobot(parent);
        return parent;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
