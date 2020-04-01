using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BuildController : GamePhaseController
{
    public TMPro.TMP_Text StatusUIElement;

    SocketConnectionHandler socketIO;

    List<JSONObject> submitedRobots;

    void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();
        if (socketIO == null)
        {
            throw new MissingComponentException("BuildController needs socket");
        }

        submitedRobots = new List<JSONObject>();
        socketIO.OnGameMessage("submitrobot", jsonObject =>
        {
            submitedRobots.Add(jsonObject);
        });
    }

    private void Update()
    {
        StatusUIElement.text = "Robots received: " + submitedRobots.Count;
    }

    public void OnNextButtonClicked()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameStates.BATTLE);
    }

    public override JSONObject ReturnDataForNextGamePhase()
    {
        // put all submitted robots into a json object for the build phase to use
        JSONObject submitedRobotsJSON = new JSONObject();
        for (int i = 0; i < submitedRobots.Count; i++)
        {
            submitedRobotsJSON[i.ToString()] = submitedRobots[i];
        }
        Debug.Log(submitedRobotsJSON);
        return submitedRobotsJSON;
    }

    public override void UseCarryOverData(JSONObject InputData)
    {
        // Do nothing. Nothing in this phase uses any carry over data from previous phase
    }
}
