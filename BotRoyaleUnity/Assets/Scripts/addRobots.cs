﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addRobots : MonoBehaviour
{
    public Cinemachine.CinemachineTargetGroup cinemachine;
    private List<Cinemachine.CinemachineTargetGroup.Target> targets = new List<Cinemachine.CinemachineTargetGroup.Target>();
    private GameObject[] robotObject = null;
    private List<Transform> robots = new List<Transform>();

    void Start()
    {
        robotObject = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject robot in robotObject){
            robots.Add(robot.GetComponent<Transform>());
        }
    
        foreach (Transform robot in robots){
            targets.Add(new Cinemachine.CinemachineTargetGroup.Target 
            { target = robot, weight = 1, radius = 1f});
        }
        cinemachine.m_Targets = targets.ToArray();
        targets.Clear();
        robots.Clear();
    }
}