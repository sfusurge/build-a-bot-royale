using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class LobbyController : GamePhaseController
{
    [SerializeField] private TMPro.TMP_InputField CPUPlayersField = default;

    public void OnNextButtonClicked()
    {
        GameStateManager.Instance.ChangeState(GameStateManager.GameStates.BUILDING);
    }

    void Start()
    {
        FindObjectOfType<JoinGameUI>().GetComponent<CanvasGroup>().alpha = 1f;

        UpdateNumberOfCPUPlayers(CPUPlayersField.text);
        CPUPlayersField.onValueChanged.AddListener(UpdateNumberOfCPUPlayers);
    }

    private void UpdateNumberOfCPUPlayers(string inputVal)
    {
        bool validInput = int.TryParse(inputVal, out int cpuPlayers);
        if (validInput && cpuPlayers >= 0)
        {
            StartingGameStateSetuper.OverrideAddCPUPlayers = cpuPlayers;
        }
    }

    void Update()
    {
        
    }

    public override JSONObject ReturnDataForNextGamePhase()
    {
        // no data to transfer from lobby page
        return null;
    }

    public override void UseCarryOverData(JSONObject InputData)
    {
        // no data to use in lobby page
    }
}
