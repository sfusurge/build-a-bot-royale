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

    public void OnSubmitButtonClicked()
    {
        JSONObject dataObject = new JSONObject(InputElement.text);
        socketIO.EmitGameMessage(dataObject);
    }
}
