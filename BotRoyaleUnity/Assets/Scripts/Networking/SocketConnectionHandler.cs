using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using SimpleJSON;
using UnitySocketIO;
using UnitySocketIO.Events;

/// <summary>
/// Connects to the Socket.IO API.
/// Provides public methods for other scripts to call.
/// Allows scripts to listen for game-messages.
/// </summary>
public class SocketConnectionHandler : MonoBehaviour
{
    [Header("Required references")]
    [SerializeField] private GameObject SocketIOComponentPrefab = default;

    private Dictionary<string, List<Action<JSONObject>>> GameMessageListeners;
    private SocketIOController socket;

    void Awake()
    {
        GameMessageListeners = new Dictionary<string, List<Action<JSONObject>>>();

        StartCoroutine(ConnectionSequence());
	}

    #region Initialization
    private IEnumerator ConnectionSequence()
    {
        // initialize the socket object
        socket = InitializeSocket();

        // wait some time so that the Javascript part can run in the webgl build. Not sure if there's any way to detect when it's ready
        yield return new WaitForSeconds(1);

        // connect to the server
        socket.Connect();

        InitializeMessageHandlers();
    }

    private SocketIOController InitializeSocket()
    {
        Assert.IsNotNull(SocketIOComponentPrefab);

        // configure socket url settings
        var prefabSocketController = SocketIOComponentPrefab.GetComponent<SocketIOController>();
        prefabSocketController.settings.url = StaticNetworkSettings.ServerURL;
        prefabSocketController.settings.port = StaticNetworkSettings.ServerPort;
        prefabSocketController.settings.sslEnabled = StaticNetworkSettings.UseSSL;

        // instantitate
        var socketGO = Instantiate(SocketIOComponentPrefab, transform);
        return socketGO.GetComponent<SocketIOController>();
    }

    private void InitializeMessageHandlers()
    {
        socket.On("game-message", HandleGameMessageReceived); // handle the main events that happen in the game

        socket.On("open", OnOpenMessage); // handles message that confirms connection

        socket.On("boop", (_) => Debug.Log("boop received")); // console log when beep-boop message is received
    }
    #endregion

    #region Message handlers
    private void OnOpenMessage(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }
    
    private void HandleGameMessageReceived(SocketIOEvent socketEvent)
    {
        Assert.AreEqual(socketEvent.name, "game-message");

        // parse data
        var jsonData = JSON.Parse(socketEvent.data);
        JSONObject dataObject = jsonData.AsObject;
        string actionName = dataObject["action"];

        // call listeners
        if (GameMessageListeners.ContainsKey(actionName))
        {
            foreach (Action<JSONObject> action in GameMessageListeners[actionName])
            {
                action.Invoke(dataObject);
            }
        }
        else
        {
            Debug.LogWarning("A game-message with action " + actionName + " was received, but there are no listeners for this action");
        }
    }
    
    #endregion

    #region Public methods
    public void StartNewGame(Action<string> onGameCreated)
    {
        socket.Emit("newgame", onGameCreated);
    }

    public void ChangeGameState(string newState, Action<string> onStateChanged = null)
    {
        var messageData = new JSONObject();
        messageData["gameState"] = newState;
        if (onStateChanged == null)
        {
            socket.Emit("updateGameState", messageData.ToString());
        }
        else
        {
            socket.Emit("updateGameState", messageData.ToString(), onStateChanged);
        }
    }

    public void EmitGameMessage(JSONObject data, Action<string> onMessageSent = null)
    {
        if (onMessageSent == null)
        {
            socket.Emit("game-message", data.ToString());
        }
        else
        {
            socket.Emit("game-message", data.ToString(), onMessageSent);
        }
    }

    public void EmitCurrentParts(GameObject robot){
        Vector2Int centerPos = robot.GetComponent<PartHandler>().GetCenterPos();
        JSONObject data = new JSONObject();
        data["action"] = "currentParts";
        data["name"] = robot.name;
        JSONArray parts = new JSONArray();
        foreach(Transform part in robot.transform){
            var partHealth = part.gameObject.GetComponent<PartHealth>();
            JSONObject newPart = new JSONObject();
            newPart["type"] = partHealth.GetPartType();
            newPart["x"] = (partHealth.GetRelPos().x + centerPos.x);
            newPart["y"] = (partHealth.GetRelPos().y + centerPos.y);
            newPart["direction"] = partHealth.GetPartDirection();
            newPart["health"] = partHealth.GetHealth();
            parts.Add(newPart);
        }
        data["parts"] = parts;
        socket.Emit("game-message", data.ToString());
        
    }

    public void EmitEmptyParts(string name){
        JSONObject data = new JSONObject();
        data["action"] = "currentParts";
        data["name"] = name;
        JSONArray parts = new JSONArray();
        data["parts"] = parts;
        socket.Emit("game-message", data.ToString());
    }

    public void OnGameMessage(string messageType, Action<JSONObject> onMessageReceived)
    {
        if (GameMessageListeners.ContainsKey(messageType) == false)
        {
            GameMessageListeners.Add(messageType, new List<Action<JSONObject>>());
        }
        GameMessageListeners[messageType].Add(onMessageReceived);
    }

    public void OnSocketEvent(string eventName, Action<SocketIOEvent> onEventAction)
    {
        socket.On(eventName, onEventAction);
    }

    public SocketIOController GetSocket(){
        return socket;
    }

    #endregion
}
