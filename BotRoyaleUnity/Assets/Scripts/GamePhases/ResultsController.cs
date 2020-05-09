using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;
using UnityEngine;

public class ResultsController : GamePhaseController
{
    [SerializeField] private TMP_Text firstPlaceText = default;

    void Start()
    {
        var stats = FindObjectOfType<AllRobotStats>();
        firstPlaceText.text = stats.FirstPlaceName + " won!";
    }

    public void OnNextButtonClicked()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameStates.LOBBY);
    }

    public override void UseCarryOverData(JSONObject InputData)
    {
        // No carry over data
    }

    public override JSONObject ReturnDataForNextGamePhase()
    {
        return null;
    }
}
