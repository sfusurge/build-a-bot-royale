﻿using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupForm : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text GameIDTextElement = default;

    private SocketConnectionHandler socketIO;

    private void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();

        // only show this menu when connected to the server
        gameObject.SetActive(false);
        socketIO.OnSocketEvent("connect", (_) =>
        {
            gameObject.SetActive(true);
        });

        socketIO.OnSocketEvent("disconnect", (_) =>
        {
            gameObject.SetActive(false);
        });
    }

    public void OnNewGameButtonClicked()
    {
        socketIO.StartNewGame(response =>
        {
            var jsonResponse = JSONObject.Parse(response);
            GameIDTextElement.text = "Game ID: <b>" + jsonResponse["gameID"] + "</b>";

            socketIO.ChangeGameState("build");
        });
    }
}
