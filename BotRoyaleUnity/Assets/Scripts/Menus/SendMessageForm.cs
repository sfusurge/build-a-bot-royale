using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageForm : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField InputElement = default;

    private SocketConnectionHandler socketIO;

    void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();
    }

    public void OnSubmitButtonClicked()
    {
        JSONObject dataObject = new JSONObject(InputElement.text);
        socketIO.EmitGameMessage(dataObject);
    }
}
