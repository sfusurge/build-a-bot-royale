using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 4f;

    void Start()
    {
        StartCoroutine(ChangeDirectionCoroutine());
    }

    private void Update()
    {
        // move in the forward direction
        transform.position += transform.up * MovementSpeed * Time.deltaTime;
        if(transform.position.y < -5){
            Destroy(gameObject);
        }
    }

    private IEnumerator ChangeDirectionCoroutine()
    {
        // never stop this coroutine.
        while (true)
        {
            float newDirection = Random.Range(-180f, 180f);
            float rotationSpeed = Random.Range(30f, 80f);
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
}
