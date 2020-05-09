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

    public string FirstPlaceName => placeScoreboard[0].Item1;

    public void ResetStats(){
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


        JSONObject data = new JSONObject();
        JSONObject scoreBoard = new JSONObject();
        JSONArray topKills = new JSONArray();
        for(var a = 0; a < killScoreboard.Count; a++){
            JSONObject kills = new JSONObject();
            kills["name"] = killScoreboard[a].Item1;
            kills["kills"] = killScoreboard[a].Item2;
            topKills.Add(kills);
        }
        JSONArray topDamage = new JSONArray();
        for(var a = 0; a < damageScoreboard.Count; a++){
            JSONObject damage = new JSONObject();
            damage["name"] = damageScoreboard[a].Item1;
            damage["damage"] = damageScoreboard[a].Item2;
            topDamage.Add(damage);
        }
        JSONArray topPlacements = new JSONArray();
        for(var a = 0; a < placeScoreboard.Count; a++){
            JSONObject place = new JSONObject();
            place["name"] = placeScoreboard[a].Item1;
            place["place"] = placeScoreboard[a].Item2;
            topPlacements.Add(place);
        }
        data["action"] = "gameStats";
        scoreBoard["topKills"] = topKills;
        scoreBoard["topDamage"] = topDamage;
        scoreBoard["topPlacements"] = topPlacements;
        data["results"] = scoreBoard;
        socketConnectionHandler.EmitGameMessage(data);
    }
}