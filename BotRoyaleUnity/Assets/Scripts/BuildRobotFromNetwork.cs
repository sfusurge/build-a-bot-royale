﻿using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BuildRobotFromNetwork : MonoBehaviour
{
    SocketConnectionHandler socketIO;

    void Awake()
    {

        //GameStateManager.Instance.RegisterActionToState(GameStateManager.GameStates.BATTLE, socketConnection);
    }

    void Start(){
        socketIO = FindObjectOfType<SocketConnectionHandler>();
         socketIO.OnGameMessage("changeBehaviour", jsonObject =>
        {
            foreach(GameObject robot in GameObject.FindGameObjectsWithTag("robot")){
                if(robot.name == jsonObject["username"]){
                    robot.GetComponent<RoombaMovement>().SetNavigationMode(jsonObject["behaviour"]);
                }
            }
        });

        socketIO.OnGameMessage("submitrobot", jsonObject =>
        {
            Debug.Log("Built");
            JSONArray partsArray = jsonObject["parts"].AsArray;
            var username = jsonObject["username"];

            var robot = GetComponent<BuildRobot>().build(partsArray.ToString(), username);

            var randomPosition = Random.onUnitSphere * 3f;
            robot.transform.position =  new Vector3(randomPosition.x, 1f, randomPosition.z);
        });

    }
}
