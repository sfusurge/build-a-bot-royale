﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameStates InitialGameState = GameStates.NONE;
    private static GameStateManager instance;
    public static GameStateManager Instance{
        get{
            if (instance != null){
                return instance;
            }
            throw new System.InvalidOperationException("Missing instance of GameManager");
        }
    }

    private static List<GameObject> robotList;

    public enum GameStates{
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
        ChangeState(GameStates.GAME_OVER);
    }

    public void RegisterActionToState(GameStates stateToListenFor, Action onStateChange){
        if (StateActions.ContainsKey(stateToListenFor) == false){
            StateActions.Add(stateToListenFor, new List<Action>());
        }
        
        StateActions[stateToListenFor].Add(onStateChange);  
        Debug.Log("register: " + StateActions[stateToListenFor].Count);
    }

   private void ChangeState(GameStates newState){
        
        bool isCurrentState = (GameState == newState);
        GameState = newState;
        if (StateActions.ContainsKey(newState) && isCurrentState == false){
            Debug.Log("action: " + StateActions[newState].Count);
            foreach (Action action in StateActions[newState]){
                
                action.Invoke();
            }
        }
    }

    public void addRobot(GameObject robot){
        robotList.Add(robot);
        if (GameState == GameStates.BATTLE){
                if (robotList.Count <= 3){
                ChangeState(GameStates.CHAMP_BATTLE);
            }
        }
    }

    public void killRobot(GameObject robot){
        robotList.Remove(robot);
            if (GameState == GameStates.BATTLE){
                if (robotList.Count <= 3){
                    ChangeState(GameStates.CHAMP_BATTLE);
                if (robotList.Count <= 1){
                    ChangeState(GameStates.GAME_OVER);
                }
            }
        }
    }
}