using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Shows is the application is connected to the server or not
/// </summary>
public class StatusUI : MonoBehaviour
{
    private SocketConnectionHandler socketIO;
    private TMPro.TMP_Text textElement;

    void Start()
    {
        textElement = GetComponent<TMPro.TMP_Text>();
        Assert.IsNotNull(textElement);

        socketIO = FindObjectOfType<SocketConnectionHandler>();
        Assert.IsNotNull(socketIO);

        socketIO.OnSocketEvent("connect", (_) =>
        {
            textElement.text = "Connected";
            textElement.color = Color.green;
        });

        socketIO.OnSocketEvent("disconnect", (_) =>
        {
            textElement.text = "Disconnected";
            textElement.color = Color.red;
        });
    }
}
