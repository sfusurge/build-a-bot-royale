using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeGamestateButton : MonoBehaviour
{
    [SerializeField] private GameStateManager.GameStates GameStateToChangeTo = GameStateManager.GameStates.TITLE;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameStateManager.Instance.ChangeState(GameStateToChangeTo);
        });
    }
}
