using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using SimpleJSON;

public class GameStateManager : MonoBehaviour
{
    [Header("State controllers")]
    [SerializeField] private GameObject TitleController = default;
    [SerializeField] private GameObject LobbyController = default;
    [SerializeField] private GameObject BuildController = default;
    [SerializeField] private GameObject BattleController = default;
    [SerializeField] private GameObject ResultsController = default;

    [Header("Config")]
    [SerializeField] private GameStates InitialGameState = GameStates.NONE;

    private static GameStateManager instance;
    public static GameStateManager Instance{
        get{
            if (instance != null){
                return instance;
            }
            throw new InvalidOperationException("Missing instance of GameManager");
        }
    }

    public static bool appIsQuitting { get; private set; } = false;

    private GamePhaseController currentStateController;
    private SocketConnectionHandler SocketIO;
    public GameStates GameState { get; private set; }

    public enum GameStates{
        NONE,

        TITLE,
        LOBBY,
        BUILDING,
        BATTLE,
        RESULTS
    }

    private string GameStateToSocketMessageString(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.NONE:
                return "none";
            case GameStates.TITLE:
                return "title";
            case GameStates.LOBBY:
                return "lobby";
            case GameStates.BUILDING:
                return "build";
            case GameStates.BATTLE:
                return "battle";
            case GameStates.RESULTS:
                return "results";
            default:
                throw new NotImplementedException("No socket message string for " + gameState);
        }
    }

    private GameObject controllerPrefabForState(GameStates gameState)
    {
        switch(gameState)
        {
            case GameStates.NONE:
                return null;
            case GameStates.TITLE:
                return TitleController;
            case GameStates.LOBBY:
                return LobbyController;
            case GameStates.BUILDING:
                return BuildController;
            case GameStates.BATTLE:
                return BattleController;
            case GameStates.RESULTS:
                return ResultsController;
            default:
                throw new NotImplementedException("No controller prefab for " + gameState);
        }
    }

    private GamePhaseController InstantiateControllerForState(GameStates gameState)
    {
        GameObject controllerPrefab = controllerPrefabForState(gameState);
        if (controllerPrefab == null)
        {
            return null;
        }

        var newController = Instantiate(controllerPrefab);
        var newPhaseController = newController.GetComponent<GamePhaseController>();
        if (newPhaseController == null)
        {
            throw new MissingComponentException("State controller prefab must have a GamePhaseController component");
        }
        return newPhaseController;
    }

    public void Awake()
    {
        if (instance != null){
            Debug.LogWarning("There can only be one instance of GameStateManager. Deleting this one");
            Destroy(this);
        }
        instance = this;

        GameState = InitialGameState;

        Application.quitting += () => appIsQuitting = true;

        SocketIO = FindObjectOfType<SocketConnectionHandler>();
        Assert.IsNotNull(SocketIO, "Game state manager needs reference to socket handler");
    }

    // Start is called before the first frame update
    void Start()
    {
        //ChangeState(GameStates.BUILDING);   
    }

    public void ChangeState(GameStates newState)
    {    
        bool isCurrentState = (GameState == newState);
        if (!isCurrentState)
        {
            // send new state to server. Only change state in Unity app once server responds
            SocketIO.ChangeGameState(GameStateToSocketMessageString(newState), response =>
            {
                // check for error in the response
                var responseJSON = JSONObject.Parse(response);
                if (responseJSON["error"] != null)
                {
                    Debug.LogError("Server error when changing game state: " + responseJSON["error"]);
                    return;
                }

                // get any carry-over data from the current state controller and delete the controller
                JSONObject betweenStateControllerData = null;
                if (currentStateController != null)
                {
                    betweenStateControllerData = currentStateController.ReturnDataForNextGamePhase();
                    Destroy(currentStateController);
                }

                // instantiate a new state controller and pass it the carry-over data
                GamePhaseController newStateController = InstantiateControllerForState(newState);
                currentStateController = newStateController;
                if (currentStateController != null)
                {
                    currentStateController.UseCarryOverData(betweenStateControllerData);
                }
            });
        }
    }
}
