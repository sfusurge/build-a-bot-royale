using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageList : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text TextElement = default;

    private SocketConnectionHandler socketIO;

    void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();
        socketIO.OnSocketEvent("game-message", eventData =>
        {
            TextElement.text = "> " + eventData.data + "\n" + TextElement.text;
        });
    }
}
