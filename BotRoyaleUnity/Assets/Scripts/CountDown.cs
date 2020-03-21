using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] private List<GameObject> ObjectList = default;
    [SerializeField] private GameStateManager.GameStates StateOn = GameStateManager.GameStates.BUILDING;
    [SerializeField] private GameStateManager.GameStates StateOff = GameStateManager.GameStates.BATTLE;

    [SerializeField] private TextMeshPro time = default;
    [SerializeField] private float BuildTime = 0.0f;
    private bool timeUp = true;

    void Start()
    {
        GameStateManager.Instance.RegisterActionToState(StateOn, turnOnScene);
        GameStateManager.Instance.RegisterActionToState(StateOff, turnOffScene);
        GameStateManager.Instance.RegisterActionToState(StateOn, ()=> timeUp = false);
    
    }

    private void turnOnScene(){
        foreach (GameObject gameObject in ObjectList)
        {
            gameObject.SetActive(true);
        }
            
        
    }
    private void turnOffScene(){
        foreach (GameObject gameObject in ObjectList)
        {
            gameObject.SetActive(false);
        }
    }

    void Update(){
        if (!timeUp){
            if (BuildTime >= 0){
                BuildTime -= Time.deltaTime;
            } else{
                BuildTime = 0;
                timeUp = true;
                GameStateManager.Instance.BuildTimeUp();
            }
            time.text = BuildTime.ToString("#");
        }
    }
}
