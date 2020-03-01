using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoombaMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed;

    public GameObject attack = null;

    public string navigationMode;

    private int robotsRemaining;

    private float stuckTimer;

    private float switchTimer;

    private float boostTimer;

    private int strongest;

    private bool isActivated = false;
    public bool ActivateOnStart = true;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        if (ActivateOnStart)
        {
            Activate();
        }
    }

    public void Activate()
    {
        isActivated = true;

        stuckTimer = Time.time;
        switchTimer = Time.time;
        boostTimer = Time.time;
        SetClosestTarget();
        SetNavigationMode("target");
    }

    private void Update()
    {
        if (isActivated)
        {
            // move in the forward direction
            if (transform.position.y < -5)
            {
                Destroy(gameObject);
            }
            if (Input.GetButtonDown("Jump"))
            {
                strongest = gameObject.GetComponent<PartHandler>().greatestDirectionStrength();
                Vector3 direction = Quaternion.AngleAxis(strongest * -90 - 90, Vector3.up) * transform.forward;
                rigidBody.AddForce(direction * (rigidBody.mass * 850));
            }
        }
    }


    void FixedUpdate()
    {
        if (isActivated)
        {
            speed = rigidBody.velocity.magnitude;
            if (speed < 10)
            {
                if (navigationMode != "reverse")
                {
                    strongest = gameObject.GetComponent<PartHandler>().greatestDirectionStrength();
                    Vector3 direction = Quaternion.AngleAxis(strongest * -90 - 90, Vector3.up) * transform.forward;
                    rigidBody.AddForce(direction * (rigidBody.mass * 15));
                    if (speed > 1)
                    {
                        stuckTimer = Time.time;
                    }
                    else if (Time.time - stuckTimer > 1)
                    {
                        SetNavigationMode("reverse");
                        stuckTimer = Time.time;
                    }
                }
                else
                {
                    rigidBody.AddForce(-transform.forward * 10 * rigidBody.mass);
                    if (Time.time - stuckTimer > 0.55)
                    {
                        SetNavigationMode("target");
                        stuckTimer = Time.time;
                    }
                }
            }
        }
    }


    private IEnumerator target()
    {
        while (true)
        {
            if (attack != null && Time.time - switchTimer < 5)
            {

                Vector3 direction = Quaternion.AngleAxis(strongest * 90 + 90, Vector3.up) * (attack.transform.position - transform.position);
                Quaternion rotation = Quaternion.LookRotation(direction);
                float angle = Quaternion.Angle(transform.rotation,rotation);
                float rotationSpeed = 30f/(0.1f + (float)Math.Sqrt(Math.Abs(angle)));
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
            else if (robotsRemaining > 1)
            {
                SetClosestTarget();
                switchTimer = Time.time;
            }
            yield return null;
        }
    }

    private IEnumerator random()
    {
        while (true)
        {
            float newDirection = UnityEngine.Random.Range(-180f, 180f);
            float rotationSpeed = UnityEngine.Random.Range(80f, 150f);
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
    }

    private IEnumerator reverse()
    {
        while (true)
        {
            yield return null;
        }
    }

    public void SetNavigationMode(string mode)
    {
        StopAllCoroutines();
        StartCoroutine(mode);
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
        attack = closestBot;
    }

}
