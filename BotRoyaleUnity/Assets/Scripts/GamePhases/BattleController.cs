using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BattleController : GamePhaseController
{
    private bool DoneBuildingRobots = false;

    SocketConnectionHandler socketIO;

    void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();
        if (socketIO == null)
        {
            throw new MissingComponentException("BattleController needs socket");
        }

        socketIO.OnGameMessage("changeBehaviour", jsonObject =>
        {
            foreach (GameObject robot in GameObject.FindGameObjectsWithTag("robot"))
            {
                if (robot.name == jsonObject["username"])
                {
                    robot.GetComponent<RoombaMovement>().SetNavigationMode(jsonObject["behaviour"]);
                }
            }
        });

        FindObjectOfType<JoinGameUI>().GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Update()
    {
        // if only one robot left, transition to results page
        if (DoneBuildingRobots)
        {
            int numberOfRobotsLeft = FindObjectsOfType<RoombaMovement>().Length;
            if (numberOfRobotsLeft < 2)
            {
                GameStateManager.Instance.ChangeState(GameStateManager.GameStates.RESULTS);
            }
        }
    }

    /// <summary>
    /// Use the carry over data to build the robots and start the battle
    /// </summary>
    /// <param name="InputData"></param>
    public override void UseCarryOverData(JSONObject InputData)
    {
        List<JSONObject> robotsList = new List<JSONObject>();

        int index = 0;
        while (InputData[index.ToString()] != null)
        {
            robotsList.Add(InputData[index.ToString()].AsObject);
            index += 1;
        }

        GetComponent<StartingGameStateSetuper>().SetupGame(robotsList, () => {
            DoneBuildingRobots = true;
        });
    }

    public override JSONObject ReturnDataForNextGamePhase()
    {
        // TODO: return any results and stats for the results page to use here
        return null;
    }
}
