using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class AllRobotStats : MonoBehaviour
{
    List<(string, int)> killScoreboard;
    List<(string, float)> damageScoreboard;
    List<(string, int)> placeScoreboard;

    private bool dataEmitted = true;

    private int robotsRemaining;

    private SocketConnectionHandler socketConnectionHandler; 

    void Start(){
        killScoreboard = new List<(string, int)>();
        damageScoreboard = new List<(string, float)>();
        placeScoreboard = new List<(string, int)>();
        robotsRemaining = 0;
        socketConnectionHandler = FindObjectOfType<SocketConnectionHandler>();
    }

    void Update(){
        if(robotsRemaining == 0 && dataEmitted == false){
            EmitGameStats();
            dataEmitted = true;
        }
    }
    public void IncrementRobotsRemaining(){
        robotsRemaining++;
        dataEmitted = false;
    }

    public int GetRobotsRemaining(){
        return robotsRemaining;
    }

    public void AddToList(GameObject robot){
        // This functions is called when a robots is killed to add the stats to the lists.
        string name = robot.name;
        var script = robot.GetComponent<StatsTracker>();
        killScoreboard.Add((name,script.GetKills()));
        damageScoreboard.Add((name,script.GetDamageDealt()));
        placeScoreboard.Add((name,robotsRemaining));
        robotsRemaining--;
    }

    void EmitGameStats(){
        // This function is called when there is only 1 robot remaining.
        
        killScoreboard.Sort((x,y) => y.Item2.CompareTo(x.Item2));
        damageScoreboard.Sort((x,y) => y.Item2.CompareTo(x.Item2));
        placeScoreboard.Sort((x,y) => x.Item2.CompareTo(y.Item2));

        foreach(var k in killScoreboard){
            Debug.Log(k.ToString());
        }
        foreach(var k in damageScoreboard){
            Debug.Log(k.ToString());
        }
        foreach(var k in placeScoreboard){
            Debug.Log(k.ToString());
        }

        JSONObject data = new JSONObject();
        JSONArray topKills = new JSONArray();
        for(var a = 0; a < Math.Min(3,killScoreboard.Count); a++){
            JSONObject kills = new JSONObject();
            kills["name"] = killScoreboard[a].Item1;
            kills["kills"] = killScoreboard[a].Item2;
            topKills.Add(kills);
        }
        JSONArray topDamage = new JSONArray();
        for(var a = 0; a < Math.Min(3,damageScoreboard.Count); a++){
            JSONObject damage = new JSONObject();
            damage["name"] = damageScoreboard[a].Item1;
            damage["damage"] = damageScoreboard[a].Item2;
            topDamage.Add(damage);
        }
        JSONArray topPlacements = new JSONArray();
        for(var a = 0; a < Math.Min(3,placeScoreboard.Count); a++){
            JSONObject place = new JSONObject();
            place["name"] = placeScoreboard[a].Item1;
            place["place"] = placeScoreboard[a].Item2;
            topPlacements.Add(place);
        }
        data["action"] = "gameStats";
        data["topKills"] = topKills;
        data["topDamage"] = topDamage;
        data["topPlacements"] = topPlacements;
        socketConnectionHandler.EmitGameMessage(data);
    }
}