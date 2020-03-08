using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] private List<GameObject> ObjectList;
    [SerializeField] private GameStateManager.GameStates StateOn;
    [SerializeField] private GameStateManager.GameStates StateOff;

    [SerializeField] private TextMeshPro time;
    [SerializeField] private float BuildTime = 0.0f;
    private bool timeUp = true;

    // Start is called before the first frame update
    void Awake()
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
