using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BuildRobotFromNetwork : MonoBehaviour
{
    SocketConnectionHandler socketIO;

    void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();

        socketIO.OnGameMessage("submitrobot", jsonObject =>
        {
            /*JSONObject partsArray = jsonObject["parts"];

            var robot = GetComponent<BuildRobot>().build(partsArray.ToString(), "new-robot");

            var randomPosition = Random.onUnitSphere * 3f;
            robot.transform.position =  new Vector3(randomPosition.x, 1f, randomPosition.z);*/
        });
    }
}
