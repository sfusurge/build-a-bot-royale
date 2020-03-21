using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using SimpleJSON;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameStates InitialGameState = GameStates.TITLE;
    private static GameStateManager instance;
    public static GameStateManager Instance{
        get{
            if (instance != null){
                return instance;
            }
            throw new System.InvalidOperationException("Missing instance of GameManager");
        }
    }

    public static bool appIsQuitting { get; private set; } = false;

    private static List<GameObject> robotList;
    private SocketConnectionHandler SocketIO;

    public enum GameStates{
        /*
        NONE,
        //receive JSON build
        //displays count down timer for building
        //have the camera circle in before the battle
        BUILDING,
        //build robot
        //robot fights
        //camera action
        //...
        BATTLE,
        //camera action (zooms in?)
        CHAMP_BATTLE,
        //end game scene
        GAME_OVER
        */
        TITLE,
        LOBBY,
        BUILDING,
        BATTLE,
        RESULTS
    }
    public GameStates GameState {get; private set;}

    private Dictionary<GameStates, List<Action>> StateActions;

    void Awake(){
        if (instance != null){
            Destroy(this);
        }
        instance = this;

        robotList = new List<GameObject>();

        StateActions = new Dictionary<GameStates, List<Action>>();
        foreach (GameStates State in Enum.GetValues(typeof(GameStates)))
        {
            StateActions.Add(State, new List<Action>());
        }

        GameState = InitialGameState;

        Application.quitting += () => appIsQuitting = true;

        SocketIO = FindObjectOfType<SocketConnectionHandler>();
        Assert.IsNotNull(SocketIO, "Game state manager needs reference to socket handler");
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameStates.BUILDING);   
    }

    // have a signal from countdown scene to call this function 
    public void BuildTimeUp(){
        ChangeState(GameStates.BATTLE);
    }

    public void EndGame(){
        ChangeState(GameStates.RESULTS);
    }

    public void RegisterActionToState(GameStates stateToListenFor, Action onStateChange){
        if (StateActions.ContainsKey(stateToListenFor) == false){
            StateActions.Add(stateToListenFor, new List<Action>());
        }
        
        StateActions[stateToListenFor].Add(onStateChange);  
        Debug.Log("register: " + StateActions[stateToListenFor].Count);
    }

   public void ChangeState(GameStates newState)
   {
        
        bool isCurrentState = (GameState == newState);
        if (!isCurrentState)
        {

            // send new state to server. Only change state in Unity app once server responds
            SocketIO.ChangeGameState(newState, response =>
            {
                // check for error in the response
                var responseJSON = JSONObject.Parse(response);
                if (responseJSON["error"] != null)
                {
                    Debug.LogError("Server error when changing game state: " + responseJSON["error"]);
                    return;
                }

                // change game state and call listeners
                GameState = newState;
                if (StateActions.ContainsKey(newState))
                {
                    Debug.Log("action: " + StateActions[newState].Count);
                    foreach (Action action in StateActions[newState])
                    {
                        action.Invoke();
                    }
                }
            });
        }
    }


    public void addRobot(GameObject robot){
        robotList.Add(robot);
        /*if (GameState == GameStates.BATTLE){
                if (robotList.Count <= 3){
                ChangeState(GameStates.CHAMP_BATTLE);
            }
        }*/
    }

    public void killRobot(GameObject robot)
    {
        robotList.Remove(robot);
        /*
           if (GameState == GameStates.BATTLE){
                if (robotList.Count <= 3){
                    ChangeState(GameStates.CHAMP_BATTLE);
                if (robotList.Count <= 1){
                    ChangeState(GameStates.GAME_OVER);
                }
           }
        }*/
    }
}
