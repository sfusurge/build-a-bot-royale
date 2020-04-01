using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    SocketConnectionHandler SocketIO;

    void Start()
    {
        SocketIO = FindObjectOfType<SocketConnectionHandler>();
        if (SocketIO == null)
        {
            throw new MissingComponentException("TitleController needs socket");
        }
    }

    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        // TODO: need to make sure we're connected to the socket before we can do this
        SocketIO.StartNewGame(responseString =>
        {
            var responseJSON = JSON.Parse(responseString);
            FindObjectOfType<JoinGameUI>().ShowGameID(responseJSON["gameID"]);

            GameStateManager.Instance.ChangeState(GameStateManager.GameStates.LOBBY);
            Destroy(gameObject);
        });
    }
}
