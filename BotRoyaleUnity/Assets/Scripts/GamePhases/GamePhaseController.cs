using UnityEngine;
using System.Collections;
using SimpleJSON;

public abstract class GamePhaseController : MonoBehaviour
{
    public abstract void UseCarryOverData(JSONObject InputData);
    public abstract JSONObject ReturnDataForNextGamePhase();
}
