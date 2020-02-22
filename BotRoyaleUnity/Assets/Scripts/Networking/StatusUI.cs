using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Shows is the application is connected to the server or not
/// </summary>
public class StatusUI : MonoBehaviour
{
    [SerializeField] private float elipsesInterval = 0.3f;
    [SerializeField] private float connectedMessageDisappearAfter = 2f;

    private SocketConnectionHandler socketIO;
    private TMPro.TMP_Text textElement;

    void Start()
    {
        textElement = GetComponent<TMPro.TMP_Text>();
        Assert.IsNotNull(textElement);

        socketIO = FindObjectOfType<SocketConnectionHandler>();
        Assert.IsNotNull(socketIO);

        StartCoroutine(ConnectingAnimation());

        socketIO.OnSocketEvent("connect", (_) =>
        {
            StopAllCoroutines();
            StartCoroutine(ShowConnectionStatus(true));
        });

        socketIO.OnSocketEvent("disconnect", (_) =>
        {
            StopAllCoroutines();
            StartCoroutine(ShowConnectionStatus(false));
        });
    }

    private IEnumerator ConnectingAnimation()
    {
        textElement.color = Color.white;
        while (true)
        {
            textElement.text = "Connecting";
            yield return new WaitForSeconds(elipsesInterval);
            textElement.text = "Connecting.";
            yield return new WaitForSeconds(elipsesInterval);
            textElement.text = "Connecting..";
            yield return new WaitForSeconds(elipsesInterval);
            textElement.text = "Connecting...";
            yield return new WaitForSeconds(elipsesInterval);
        }
    }

    private IEnumerator ShowConnectionStatus(bool connected)
    {
        if (connected)
        {
            textElement.enabled = true;
            textElement.text = "Connected";
            textElement.color = Color.green;

            // hide connected message after some time
            yield return new WaitForSeconds(connectedMessageDisappearAfter);
            textElement.enabled = false;
        }
        else
        {
            textElement.enabled = true;
            textElement.text = "Disconnected";
            textElement.color = Color.red;
        }
    }

}
