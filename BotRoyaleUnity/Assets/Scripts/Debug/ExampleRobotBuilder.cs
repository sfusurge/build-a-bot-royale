using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public static class ExampleRobotBuilder
{
    public static List<JSONArray> ExampleRobotsJSON(int numberOfRobots)
    {
        List<JSONArray> sampleRobots = new List<JSONArray>();
        for (int i = 0; i < numberOfRobots; i++)
        {
            var robotJSON = new JSONArray();
            if(i % 3 == 0){
                robotJSON.Add(JSONRobotPart(3,3,"center", "north"));
                robotJSON.Add(JSONRobotPart(4,3,"block", "north"));
                robotJSON.Add(JSONRobotPart(2,3,"block", "north"));
                robotJSON.Add(JSONRobotPart(3,2,"block", "north"));
                robotJSON.Add(JSONRobotPart(3,4,"block", "north"));
                robotJSON.Add(JSONRobotPart(5,3,"spike", "east"));
                robotJSON.Add(JSONRobotPart(1,3,"spike", "west"));
                robotJSON.Add(JSONRobotPart(3,1,"spike", "south"));
                robotJSON.Add(JSONRobotPart(3,5,"spike", "north"));
            }else if(i % 3 == 1){
                robotJSON.Add(JSONRobotPart(3,3,"center", "north"));
                robotJSON.Add(JSONRobotPart(4,3,"block", "north"));
                robotJSON.Add(JSONRobotPart(2,3,"block", "north"));
                robotJSON.Add(JSONRobotPart(3,2,"block", "north"));
                robotJSON.Add(JSONRobotPart(3,4,"block", "north"));
                robotJSON.Add(JSONRobotPart(5,3,"shield", "east"));
                robotJSON.Add(JSONRobotPart(1,3,"shield", "west"));
                robotJSON.Add(JSONRobotPart(3,1,"shield", "south"));
                robotJSON.Add(JSONRobotPart(3,5,"shield", "north"));
            }else if(i % 3 == 2){
                robotJSON.Add(JSONRobotPart(3,3,"center", "north"));
                robotJSON.Add(JSONRobotPart(4,3,"block", "north"));
                robotJSON.Add(JSONRobotPart(2,3,"block", "north"));
                robotJSON.Add(JSONRobotPart(3,2,"block", "north"));
                robotJSON.Add(JSONRobotPart(3,4,"block", "north"));
                robotJSON.Add(JSONRobotPart(4,4,"block", "north"));
                robotJSON.Add(JSONRobotPart(2,2,"block", "north"));
                robotJSON.Add(JSONRobotPart(4,2,"block", "north"));
                robotJSON.Add(JSONRobotPart(2,4,"block", "north"));
                robotJSON.Add(JSONRobotPart(5,3,"spike", "east"));
                robotJSON.Add(JSONRobotPart(1,3,"spike", "west"));
                robotJSON.Add(JSONRobotPart(3,1,"spike", "south"));
                robotJSON.Add(JSONRobotPart(3,5,"spike", "north"));
                robotJSON.Add(JSONRobotPart(5,2,"shield", "east"));
                robotJSON.Add(JSONRobotPart(5,4,"shield", "east"));
                robotJSON.Add(JSONRobotPart(1,2,"shield", "west"));
                robotJSON.Add(JSONRobotPart(1,4,"shield", "west"));
                robotJSON.Add(JSONRobotPart(2,1,"shield", "south"));
                robotJSON.Add(JSONRobotPart(4,1,"shield", "south"));
                robotJSON.Add(JSONRobotPart(2,5,"shield", "north"));
                robotJSON.Add(JSONRobotPart(4,5,"shield", "north"));
            }
            sampleRobots.Add(robotJSON);
        }
        return sampleRobots;
    }

    public static JSONObject JSONRobotPart(int x, int y, string type, string direction)
    {
        var jsonObj = new JSONObject();
        jsonObj["x"] = x;
        jsonObj["y"] = y;
        jsonObj["type"] = type;
        jsonObj["direction"] = direction;
        return jsonObj;
    }
}
