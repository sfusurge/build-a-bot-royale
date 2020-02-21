﻿using System;
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
    private enum Servers
    {
        LOCAL, PRODUCTION
    }
    private Dictionary<Servers, string> ServerDomains = new Dictionary<Servers, string>()
    {
        { Servers.LOCAL, "localhost" },
        { Servers.PRODUCTION, "build-a-bot-royale.herokuapp.com" }
    };
    private Dictionary<Servers, int> ServerPorts = new Dictionary<Servers, int>()
    {
        { Servers.LOCAL, 9000 },
        { Servers.PRODUCTION, 80 }
    };

    [Header("Connection configuration")]
    [SerializeField] private Servers server = Servers.LOCAL;

    [Header("Required references")]
    [SerializeField] private GameObject SocketIOComponentPrefab = default;

    private Dictionary<string, List<Action<JSONObject>>> GameMessageListeners;
    private SocketIOController socket;

    void Awake()
    {
        GameMessageListeners = new Dictionary<string, List<Action<JSONObject>>>();

        socket = ConnectToSocketAPI(server);

        socket.On("game-message", HandleGameMessageReceived); // handle the main events that happen in the game

        socket.On("open", OnOpenMessage); // handles message that confirms connection

        // console log when beep-boop message is received
        socket.On("boop", (_) =>
        {
            Debug.Log("boop received");
        });   
	}

    private SocketIOController ConnectToSocketAPI(Servers server)
    {
        if (ServerDomains.ContainsKey(server) == false)
        {
            throw new NotImplementedException("Cannot connect to " + server + " because there is no domain associated with it");
        }
        if (ServerPorts.ContainsKey(server) == false)
        {
            throw new NotImplementedException("Cannot connect to " + server + " because there is no port associated with it");
        }
        Assert.IsNotNull(SocketIOComponentPrefab);

        // configure socket url settings
        var prefabSocketController = SocketIOComponentPrefab.GetComponent<SocketIOController>();
        prefabSocketController.settings.url = ServerDomains[server];
        prefabSocketController.settings.port = ServerPorts[server];

        // instantitate and connect
        var socketGO = Instantiate(SocketIOComponentPrefab, transform);
        var instantiatedSocketController = socketGO.GetComponent<SocketIOController>();
        instantiatedSocketController.Connect();

        return socketGO.GetComponent<SocketIOController>();
    }
    
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
        string actionName = dataObject["action"].ToString();

        // call listeners
        if (GameMessageListeners.ContainsKey(actionName))
        {
            foreach (Action<JSONObject> action in GameMessageListeners[actionName])
            {
                action.Invoke(dataObject);
            }
        }
    }
    
    #endregion

    #region Public methods
    public void StartNewGame(Action<string> onGameCreated)
    {
        socket.Emit("newgame", onGameCreated);
    }

    public void EmitGameMessage(JSONObject data, Action<string> onMessageSent = null)
    {
        if (onMessageSent == null)
        {
            socket.Emit("game-message", data.ToString());

        }
        else
        {
            socket.Emit("game-message", data.ToString(), onMessageSent); ;
        }
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

    #endregion
}
