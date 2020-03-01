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
            Debug.Log(i);
            if (i % 5 == 0)
            {
                robotJSON.Add(JSONRobotPart(1, 5, "block"));
                robotJSON.Add(JSONRobotPart(1, 4, "block"));
                robotJSON.Add(JSONRobotPart(1, 3, "block"));
                robotJSON.Add(JSONRobotPart(1, 2, "block"));
                robotJSON.Add(JSONRobotPart(1, 1, "center"));

                robotJSON.Add(JSONRobotPart(5, 5, "block"));
                robotJSON.Add(JSONRobotPart(5, 4, "block"));
                robotJSON.Add(JSONRobotPart(5, 3, "block"));
                robotJSON.Add(JSONRobotPart(5, 2, "block"));
                robotJSON.Add(JSONRobotPart(5, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 1, "block"));
                robotJSON.Add(JSONRobotPart(3, 1, "block"));
                robotJSON.Add(JSONRobotPart(2, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 5, "block"));
                robotJSON.Add(JSONRobotPart(3, 5, "block"));
                robotJSON.Add(JSONRobotPart(2, 5, "block"));
            }
            else if (i % 5 == 1)
            {
                robotJSON.Add(JSONRobotPart(1, 5, "center"));
                robotJSON.Add(JSONRobotPart(1, 4, "block"));
                robotJSON.Add(JSONRobotPart(1, 3, "block"));
                robotJSON.Add(JSONRobotPart(1, 2, "block"));
                robotJSON.Add(JSONRobotPart(1, 1, "block"));

                robotJSON.Add(JSONRobotPart(5, 5, "block"));
                robotJSON.Add(JSONRobotPart(5, 4, "block"));
                robotJSON.Add(JSONRobotPart(5, 3, "block"));
                robotJSON.Add(JSONRobotPart(5, 2, "block"));
                robotJSON.Add(JSONRobotPart(5, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 1, "block"));
                robotJSON.Add(JSONRobotPart(3, 1, "block"));
                robotJSON.Add(JSONRobotPart(2, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 5, "block"));
                robotJSON.Add(JSONRobotPart(3, 5, "block"));
                robotJSON.Add(JSONRobotPart(2, 5, "block"));
            }
            else if (i % 5 == 2)
            {
                robotJSON.Add(JSONRobotPart(1, 5, "block"));
                robotJSON.Add(JSONRobotPart(1, 4, "block"));
                robotJSON.Add(JSONRobotPart(1, 3, "block"));
                robotJSON.Add(JSONRobotPart(1, 2, "block"));
                robotJSON.Add(JSONRobotPart(1, 1, "block"));

                robotJSON.Add(JSONRobotPart(5, 5, "block"));
                robotJSON.Add(JSONRobotPart(5, 4, "block"));
                robotJSON.Add(JSONRobotPart(5, 3, "block"));
                robotJSON.Add(JSONRobotPart(5, 2, "block"));
                robotJSON.Add(JSONRobotPart(5, 1, "center"));

                robotJSON.Add(JSONRobotPart(4, 1, "block"));
                robotJSON.Add(JSONRobotPart(3, 1, "block"));
                robotJSON.Add(JSONRobotPart(2, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 5, "block"));
                robotJSON.Add(JSONRobotPart(3, 5, "block"));
                robotJSON.Add(JSONRobotPart(2, 5, "block"));
            }
            else if (i % 5 == 3)
            {
                robotJSON.Add(JSONRobotPart(1, 5, "block"));
                robotJSON.Add(JSONRobotPart(1, 4, "block"));
                robotJSON.Add(JSONRobotPart(1, 3, "block"));
                robotJSON.Add(JSONRobotPart(1, 2, "block"));
                robotJSON.Add(JSONRobotPart(1, 1, "block"));

                robotJSON.Add(JSONRobotPart(5, 5, "center"));
                robotJSON.Add(JSONRobotPart(5, 4, "block"));
                robotJSON.Add(JSONRobotPart(5, 3, "block"));
                robotJSON.Add(JSONRobotPart(5, 2, "block"));
                robotJSON.Add(JSONRobotPart(5, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 1, "block"));
                robotJSON.Add(JSONRobotPart(3, 1, "block"));
                robotJSON.Add(JSONRobotPart(2, 1, "block"));

                robotJSON.Add(JSONRobotPart(4, 5, "block"));
                robotJSON.Add(JSONRobotPart(3, 5, "block"));
                robotJSON.Add(JSONRobotPart(2, 5, "block"));
            }
            else if (i % 5 == 4)
            {
                robotJSON.Add(JSONRobotPart(3, 3, "center"));

                robotJSON.Add(JSONRobotPart(1, 1, "block"));
                robotJSON.Add(JSONRobotPart(1, 2, "block"));
                robotJSON.Add(JSONRobotPart(1, 3, "block"));
                robotJSON.Add(JSONRobotPart(1, 4, "block"));
                robotJSON.Add(JSONRobotPart(1, 5, "block"));

                robotJSON.Add(JSONRobotPart(2, 1, "block"));
                robotJSON.Add(JSONRobotPart(2, 2, "block"));
                robotJSON.Add(JSONRobotPart(2, 3, "block"));
                robotJSON.Add(JSONRobotPart(2, 4, "block"));
                robotJSON.Add(JSONRobotPart(2, 5, "block"));

                robotJSON.Add(JSONRobotPart(3, 1, "block"));
                robotJSON.Add(JSONRobotPart(3, 2, "block"));
                robotJSON.Add(JSONRobotPart(3, 4, "block"));
                robotJSON.Add(JSONRobotPart(3, 5, "block"));

                robotJSON.Add(JSONRobotPart(4, 1, "block"));
                robotJSON.Add(JSONRobotPart(4, 2, "block"));
                robotJSON.Add(JSONRobotPart(4, 3, "block"));
                robotJSON.Add(JSONRobotPart(4, 4, "block"));
                robotJSON.Add(JSONRobotPart(4, 5, "block"));

                robotJSON.Add(JSONRobotPart(5, 1, "block"));
                robotJSON.Add(JSONRobotPart(5, 2, "block"));
                robotJSON.Add(JSONRobotPart(5, 3, "block"));
                robotJSON.Add(JSONRobotPart(5, 4, "block"));
                robotJSON.Add(JSONRobotPart(5, 5, "block"));

            }

            sampleRobots.Add(robotJSON);
        }
        return sampleRobots;
    }

    public static JSONObject JSONRobotPart(int x, int y, string type, string direction = "north")
    {
        var jsonObj = new JSONObject();
        jsonObj["x"] = x;
        jsonObj["y"] = y;
        jsonObj["type"] = type;
        jsonObj["direction"] = direction;
        return jsonObj;
    }
}
