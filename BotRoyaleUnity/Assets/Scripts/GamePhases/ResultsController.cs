using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class ResultsController : GamePhaseController
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnNextButtonClicked()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameStates.LOBBY);
    }

    public override void UseCarryOverData(JSONObject InputData)
    {
        // TODO: use the carryover data to display the results in the ui
    }

    public override JSONObject ReturnDataForNextGamePhase()
    {
        return null;
    }
}
