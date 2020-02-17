﻿using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
using UnityEngine.Assertions;

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
        { Servers.LOCAL, "localhost:9000" },
        { Servers.PRODUCTION, "build-a-bot-royale.herokuapp.com" }
    };

    [Header("Connection configuration")]
    [SerializeField] private Servers server = Servers.LOCAL;

    [Header("Required references")]
    [SerializeField] private GameObject SocketIOComponentPrefab;

    private Dictionary<string, List<Action<JSONObject>>> GameMessageListeners;
    private SocketIOComponent socket;
    private bool didConnect = false;

    void Awake()
    {
        GameMessageListeners = new Dictionary<string, List<Action<JSONObject>>>();
        socket = ConnectToSocketAPI(server);

        socket.On("game-message", HandleGameMessageReceived); // handle the main events that happen in the game

        socket.On("open", OnOpenMessage); // handles message that confirms connection

        socket.On("connect", (_) =>
        {
            didConnect = true;
        });

        // console log when beep-boop message is received
        socket.On("boop", (_) =>
        {
            Debug.Log("boop received");
        });
	}

    private IEnumerator WaitForConnection()
    {
        while (didConnect == false)
        {
            yield return null;
        }
    }

    
    private IEnumerator startGame()
    {
        yield return WaitForConnection();
        Debug.Log("here");

        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(5f);
            string m = "{ \"yeet\": \"" + i++ + "\" }";
            Debug.Log("Sending: " + m);
            EmitGameMessage(new JSONObject(m), null);
        }
    }

    private SocketIOComponent ConnectToSocketAPI(Servers server)
    {
        if (ServerDomains.ContainsKey(server) == false)
        {
            throw new NotImplementedException("Cannot connect to " + server + " because there is no domain associated with it");
        }
        string serverURL = "ws://" + ServerDomains[server] + "/socket.io/?EIO=4&transport=websocket";

        Assert.IsNotNull(SocketIOComponentPrefab);

        SocketIOComponentPrefab.GetComponent<SocketIOComponent>().url = serverURL;

        var socketGO = Instantiate(SocketIOComponentPrefab, transform);
        return socketGO.GetComponent<SocketIOComponent>();
    }

    #region Message handlers
    private void OnOpenMessage(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    private void HandleGameMessageReceived(SocketIOEvent socketEvent)
    {
        // todo: parse event data to get the type. Call all actions in GameMessageListeners for that type
    }
    #endregion

    #region Public methods
    public void StartNewGame(Action<JSONObject> onGameCreated)
    {
        socket.Emit("newgame", onGameCreated);
    }

    public void EmitGameMessage(JSONObject data, Action<JSONObject> onMessageSent = null)
    {
        if (onMessageSent == null)
        {
            socket.Emit("game-message", data);

        }
        else
        {
            socket.Emit("game-message", data, onMessageSent);
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
