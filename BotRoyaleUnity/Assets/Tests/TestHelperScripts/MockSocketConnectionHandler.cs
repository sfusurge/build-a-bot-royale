using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

namespace BuildABotRoyale.Testing
{
    /// <summary>
    /// A class with the same public interface as the SocketConnectionHandler, but doesn't actually connect to the server.
    /// Responds to method calls with canned responses.
    /// Provides counters to see if and how many times a public method has been called
    /// </summary>
    public class MockSocketConnectionHandler : SocketConnectionHandler
    {
        public int StartNewGame_Called { get; private set; } = 0;
        public override void StartNewGame(Action<string> onGameCreated)
        {
            StartNewGame_Called += 1;
            onGameCreated.Invoke("{ \"gameID\": \"TSTID\" }");
        }

        public int ChangeGameState_Called { get; private set; } = 0;
        public override void ChangeGameState(string newState, Action<string> onStateChanged = null)
        {
            ChangeGameState_Called += 1;
            if (onStateChanged != null)
            {
                onStateChanged.Invoke("{ \"message\": \"ok\" }");
            }
        }

        public int EmitGameMessage_Called { get; private set; } = 0;
        public override void EmitGameMessage(JSONObject data, Action<string> onMessageSent = null)
        {
            EmitGameMessage_Called += 1;
            if (onMessageSent != null)
            {
                onMessageSent.Invoke("{ \"message\": \"ok\" }");
            }
        }
    }
}
