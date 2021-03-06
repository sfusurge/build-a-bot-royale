using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class StatsTracker : MonoBehaviour
{
    private int kills;

    private int boostsRemaining;

    private float damageDealt;
    private SocketConnectionHandler socketConnectionHandler;

    private GameObject lastTouched;

    void Start(){
        kills = 0;
        damageDealt = 0;
        boostsRemaining = 3;
        socketConnectionHandler = FindObjectOfType<SocketConnectionHandler>();
        lastTouched = null;

        socketConnectionHandler.OnGameMessage("useBoost", jsonObject =>{
            try{
                if(jsonObject["username"] == gameObject.name){
                    UseBoost();
                }
            }catch{
                //The robot died, so can't process no need to do the check.
            }
        });
    }

    public void IncrementKills(){
        kills++;
        boostsRemaining++;
        EmitCurrentBoosts();
    }

    public int GetKills(){
        return kills;
    }

    public void AddDamageDealt(float damage){
        damageDealt += damage;
    }

    public float GetDamageDealt(){
        return damageDealt;
    }

    
    public int GetBoosts(){
        return boostsRemaining;
    }
    public void UseBoost(){
        if(boostsRemaining > 0){
            boostsRemaining--;
            gameObject.GetComponent<RoombaMovement>().Boost();
            EmitCurrentBoosts();
        }
    }

    private void EmitCurrentBoosts(){
        JSONObject data = new JSONObject();
        data["action"] = "currentBoosts";
        data["name"] = gameObject.name;
        data["boosts"] = boostsRemaining;
        socketConnectionHandler.EmitGameMessage(data);
    }

    public void SetLastTouched(GameObject robot){
        lastTouched = robot;
    }

    public GameObject GetLastTouched(){
        return lastTouched;
    }




}