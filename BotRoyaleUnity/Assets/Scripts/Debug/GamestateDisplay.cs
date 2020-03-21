using UnityEngine;
using System.Collections;
using System;

public class GamestateDisplay : MonoBehaviour
{
    private TMPro.TMP_Text TextElement;
    void Start()
    {
        TextElement = GetComponent<TMPro.TMP_Text>();

        var states = Enum.GetValues(typeof(GameStateManager.GameStates)) as GameStateManager.GameStates[];
        foreach (var state in states)
        {
            GameStateManager.Instance.RegisterActionToState(state, () =>
            {
                TextElement.text = "Current game state: " + state.ToString();
            });
        }
    }
}
