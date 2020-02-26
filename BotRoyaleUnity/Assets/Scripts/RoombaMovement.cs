using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed;

    public GameObject attack = null;

    public string navigationMode;

    private int robotsRemaining;

    private float stuckTimer;

    void Start()
    {
        stuckTimer = Time.time;
        SetClosestTarget();
        SetNavigationMode("target");
        rigidBody = gameObject.GetComponent<Rigidbody>();
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
                int strongest = gameObject.GetComponent<PartHandler>().greatestDirectionStrength();
                switch(strongest){
                    case 0:
                        rigidBody.AddForce(-transform.right * 15 * rigidBody.mass);
                        break;
                    case 1:
                        rigidBody.AddForce(-transform.forward * 15 * rigidBody.mass);
                        break;
                    case 2:
                        rigidBody.AddForce(transform.right * 15 * rigidBody.mass);
                        break;
                    case 3:
                        rigidBody.AddForce(transform.forward * 15 * rigidBody.mass);
                        break;

                }
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


    private IEnumerator target()
    {
        while (true)
        {
            if (attack != null)
            {
                Vector3 direction = attack.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3f * Time.deltaTime);
            }
            else if (robotsRemaining > 1)
            {
                SetClosestTarget();
            }
            yield return null;
        }
    }

    private IEnumerator random(){
        while(true){
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
    }

    private IEnumerator reverse(){
        while(true){
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
