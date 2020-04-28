using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class StatsTracker : MonoBehaviour
{
    public int kills;

    public int boostsRemaining;

    public float damageDealt;
    private SocketConnectionHandler socketConnectionHandler;

    void Start(){
        kills = 0;
        damageDealt = 0;
        boostsRemaining = 1;
        socketConnectionHandler = FindObjectOfType<SocketConnectionHandler>();

        socketConnectionHandler.OnGameMessage("currentBoosts", jsonObject =>{
            try{
                if(jsonObject["name"] == gameObject.name){
                    boostsRemaining = jsonObject["boosts"];
                }
            }catch{
                //The robot died, so can't process no need to do the check.
            }
        });
    }

    void OnDestroy(){
        socketConnectionHandler.UnsubscribeOnGameMessage("currentBoosts", jsonObject =>{
            if(jsonObject["name"] == gameObject.name){
                boostsRemaining = jsonObject["boosts"];
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
    public void useBoost(){
        if(boostsRemaining > 0){
            boostsRemaining--;
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




}