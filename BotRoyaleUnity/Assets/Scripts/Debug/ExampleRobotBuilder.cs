using System;
using System.Collections.Generic;
using SimpleJSON;

public static class ExampleRobotBuilder
{
    public static List<JSONArray> ExampleRobotsJSON(int numberOfRobots)
    {
        List<JSONArray> sampleRobots = new List<JSONArray>();
        for (int i = 0; i < numberOfRobots; i++)
        {
            var robotJSON = new JSONArray();
            robotJSON.Add(JSONRobotPart(3, 3, "center"));
            robotJSON.Add(JSONRobotPart(2, 3, "spike"));
            robotJSON.Add(JSONRobotPart(3, 2, "spike"));
            robotJSON.Add(JSONRobotPart(4, 3, "spike"));
            robotJSON.Add(JSONRobotPart(3, 4, "spike"));

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
