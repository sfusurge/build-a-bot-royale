using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System;

public class buildRobot : MonoBehaviour
{
    public GameObject block,center,spike;
    public Vector3 pos,rot;
    

// Start is called before the first frame update
void Start()
    {   
        string jsonString = File.ReadAllText("Assets/Scripts/test.json");
        var N = JSON.Parse(jsonString);
        string test = N[0]["type"];
        int a = 0;
        while(N[a] != null){
            pos = new Vector3((float)N[a]["x"],0.5f,(float)N[a]["y"]);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            string type = N[a]["type"];
            switch(type){
                case "block":
                    Instantiate(block, pos, rot);  
                    break;
                case "center":
                    Instantiate(center, pos, rot);
                    break;
                case "spike":
                    Instantiate(spike, pos, rot);
                    break;
            }
            a++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
