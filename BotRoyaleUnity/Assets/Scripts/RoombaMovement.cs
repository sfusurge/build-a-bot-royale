using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoombaMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed;

    public GameObject attack = null;

    public GameObject arena = null;
    public string navigationMode;

    private int robotsRemaining;

    private float stuckTimer;

    private float switchTimer;

    private float boostTimer;

    private int strongest;

    private bool isActivated = false;
    public bool ActivateOnStart = true;

    private PartHandler handler;


    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        handler = gameObject.GetComponent<PartHandler>();
        if (ActivateOnStart)
        {
            Activate();
        }
    }

    public void Activate()
    {
        arena = GameObject.Find("Arena");
        isActivated = true;

        stuckTimer = Time.time;
        switchTimer = Time.time;
        boostTimer = Time.time;
        SetClosestTarget();
        StartCoroutine("target");
        if(UnityEngine.Random.value > 0.5f){
            SetNavigationMode("defend");
        }else{
            SetNavigationMode("attack");
        }
    }

    private void Update()
    {
        if (isActivated)
        {
            // move in the forward direction
            if (transform.position.y < -5)
            {
                handler.EmitEmptyParts();
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
                float arenaScale = arena.GetComponent<ShrinkArena>().GetLocalScale().x;
                strongest = gameObject.GetComponent<PartHandler>().greatestDirectionStrength();
                if(navigationMode == "attack"){
                    Vector3 direction = Quaternion.AngleAxis(strongest * -90 - 90, Vector3.up) * transform.forward;
                    rigidBody.AddForce(direction * (rigidBody.mass * 15));
                }else if(navigationMode == "defend" && attack != null){
                    float distanceFromClosest = (attack.transform.position - transform.position).magnitude;
                    Vector3 direction = Quaternion.AngleAxis(strongest * -90 - 90, Vector3.up) * transform.forward;
                    rigidBody.AddForce(-direction * (rigidBody.mass * (30 / (distanceFromClosest) + 5)));
                    float distanceFromCenter = transform.position.magnitude;
                    Vector3 centerDirection = -transform.position;
                    centerDirection.Normalize();
                    rigidBody.AddForce(centerDirection * (rigidBody.mass * (distanceFromCenter * 20/arenaScale)));
                    
                }
            }
        }
    }


    private IEnumerator target()
    {
        while (true)
        {
            if (attack != null && Time.time - switchTimer < 0.5)
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

    public void SetNavigationMode(string mode)
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
        attack = closestBot;
    }

}
