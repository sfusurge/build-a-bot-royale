using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Assertions;

public class StartingGameStateSetuper : MonoBehaviour
{
    [Header("Spawn position")]
    [SerializeField] private float SpawnDistanceAboveGround = 0.5f;
    [SerializeField] private float DistanceFromEdge = 9f;
    [SerializeField] private float ArenaScaleIncrementPerRobot = 1f;
    [SerializeField] private float ArenaBaseSize = 25f;

    [Header("Timings")]
    [SerializeField] private float SetupSequenceRobotSpawnInterval = 0.01f;
    [SerializeField] private float DelayBeforeSpawning = 1f;
    [SerializeField] private float DelayAfterSpawning = 1f;

    [Header("Settings")]
    [SerializeField] private int MinNumberOfRobots = 5;

    [Header("Debug")]
    [SerializeField] private int TEST_number_of_robots = 5;
    [SerializeField] private bool TEST_SetupOnStart = false;
    [SerializeField] private bool TEST_SetupOnQ = false;


    private void Start()
    {
        if (TEST_SetupOnStart)
        {
            StartSampleGame();
        }
    }

    private void Update()
    {
        if (TEST_SetupOnQ && Input.GetKeyDown(KeyCode.Q))
        {
            StartSampleGame();
        }
    }

    private void StartSampleGame()
    {
        var exampleRobots = ExampleRobotBuilder.ExampleRobotsJSON(TEST_number_of_robots);
        SetupGame(exampleRobots, () => Debug.Log("Setup done"));
    }

    public void SetupGame(List<JSONObject> Robots, Action onDoneSetup)
    {
        StopAllCoroutines();
        StartCoroutine(GameSetupSequence(Robots, onDoneSetup));
    }

    private IEnumerator GameSetupSequence(List<JSONObject> Robots, Action onDone)
    {
        //reset stats
        FindObjectOfType<AllRobotStats>().ResetStats();
        
        // pad out robots list if there are too few robots
        MinNumberOfRobots = 8;
        if (Robots.Count < MinNumberOfRobots)
        {
            Robots.AddRange(ExampleRobotBuilder.ExampleRobotsJSON(MinNumberOfRobots - Robots.Count));
        }

        // delete existing robots
        foreach (RoombaMovement robot in FindObjectsOfType<RoombaMovement>())
        {
            Destroy(robot.gameObject);
        }

        // get references to objects in the scene
        var RobotBuilder = FindObjectOfType<BuildRobot>();
        Assert.IsNotNull(RobotBuilder, "BuildRobot needed");

        var Arena = FindObjectOfType<ShrinkArena>();
        Assert.IsNotNull(Arena, "Arena needed");

        // set the arena size
        yield return Arena.GrowToSize(ArenaSizeForRobots(Robots.Count));

        // dramatic pause
        yield return new WaitForSeconds(DelayBeforeSpawning);

        // spawn robots
        Vector3 arenaCenter = Arena.transform.position;
        float spawnDistanceFromCenter = (ArenaSizeForRobots(Robots.Count) * 0.45f) - DistanceFromEdge; // 0.45 because 90% of half the diameter for hex shape

        float angle = 0f;
        float angleInterval = (Mathf.PI * 2f) / Robots.Count;
        int robotNumber = 1;

        foreach (var robotJSONObject in Robots)
        {
            JSONArray robotParts = robotJSONObject["parts"].AsArray;
            string robotName = robotJSONObject["username"];

            // build robot from json
            var robot = RobotBuilder.build(robotParts.ToString(), robotName);
            robot.GetComponent<RoombaMovement>().ActivateOnStart = false;

            // place robot in circle on arena
            Vector3 spawnPosition =
                arenaCenter + // spawn relative to center
                new Vector3( // spawn in a circle
                    -Mathf.Cos(angle) * spawnDistanceFromCenter, // negative because the first one goes on the left of the screen
                    0f,
                    Mathf.Sin(angle) * spawnDistanceFromCenter
                ) +
                Vector3.up * SpawnDistanceAboveGround; // go a bit above the arena floor

            robot.transform.position = spawnPosition;

            // look at center of arena
            robot.transform.LookAt(new Vector3(arenaCenter.x, robot.transform.position.y, arenaCenter.z));

            // increment values for next robot
            angle += angleInterval;
            robotNumber += 1;

            // wait so robots appear one at a time
            yield return new WaitForSeconds(SetupSequenceRobotSpawnInterval);
        }

        // pause before battle
        yield return new WaitForSeconds(DelayAfterSpawning);

        // battle starts TODO: maybe this code goes somewhere else
        Arena.StartShrinkSequence();
        foreach (RoombaMovement robot in FindObjectsOfType<RoombaMovement>())
        {
            robot.Activate();
        }
        onDone();
    }

    private float ArenaSizeForRobots(int numberOfRobots)
    {
        if (numberOfRobots < 1)
        {
            return ArenaBaseSize;
        }
        return ArenaBaseSize + (numberOfRobots * ArenaScaleIncrementPerRobot);
    }
}
