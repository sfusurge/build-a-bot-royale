﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed;

    public GameObject target = null;

    public string navigationMode;

    private int robotsRemaining;

    void Start()
    {
        SetClosestTarget();
        navigationMode = "target";
        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(ChangeDirectionCoroutine());
    }

    private void Update()
    {
        // move in the forward direction
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
            GameObject.Find("Arena").GetComponent<ShrinkArena>().removeRobot();
        }
    }


    void FixedUpdate()
    {
        speed = rigidBody.velocity.magnitude;
        if (speed < 8)
        {
            if (navigationMode != "reverse")
            {
                rigidBody.AddForce(transform.forward * 15 * rigidBody.mass);
            }
            else
            {
                rigidBody.AddForce(-transform.forward * 15 * rigidBody.mass);
            }
        }
    }


    private IEnumerator ChangeDirectionCoroutine()
    {
        while (navigationMode == "straight" || navigationMode == "reverse")
        {
            yield return null;
        }
        while (navigationMode == "left")
        {
            float rotationSpeed = 130f;
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationThisFrame);
            yield return null;
        }
        while (navigationMode == "right")
        {
            float rotationSpeed = 130f;
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, -rotationThisFrame);
            yield return null;
        }
        while (navigationMode == "target")
        {
            if (target != null)
            {
                Vector3 direction = target.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3f * Time.deltaTime);
            }
            else if (robotsRemaining > 1)
            {
                SetClosestTarget();
            }
            yield return null;
        }
        while (navigationMode == "random")
        {
            float newDirection = Random.Range(-180f, 180f);
            float rotationSpeed = Random.Range(80f, 150f);
            float totalRotation = 0;
            if (newDirection < 0)
            {
                while (totalRotation > newDirection)
                {
                    var rotationThisFrame = rotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.forward, -rotationThisFrame);
                    totalRotation -= rotationThisFrame;
                    yield return null;
                }
            }
            else
            {
                while (totalRotation < newDirection)
                {
                    var rotationThisFrame = rotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.forward, rotationThisFrame);
                    totalRotation += rotationThisFrame;
                    yield return null;
                }
            }
        }
        yield return null;
    }

    public void setNavigationMode(string mode)
    {
        navigationMode = mode;
    }

    public void SetClosestTarget()
    {
        GameObject[] robots = GameObject.FindGameObjectsWithTag("robot");
        robotsRemaining = robots.Length;
        GameObject closestBot = null;
        foreach (GameObject robot in robots)
        {
            if (closestBot == null && robot != gameObject)
            {
                closestBot = robot;
                //if the robot is closer that the previous closest
            }
            else if (robot != gameObject && (transform.position - robot.transform.position).magnitude < (transform.position - closestBot.transform.position).magnitude)
            {
                closestBot = robot;
            }
        }
        target = closestBot;
    }

}
