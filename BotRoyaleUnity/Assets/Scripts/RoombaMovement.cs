using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 2f;

    void Start()
    {
        StartCoroutine(ChangeDirectionCoroutine());
    }

    private void Update()
    {
        // move in the forward direction
        transform.position += transform.up * MovementSpeed * Time.deltaTime;
    }

    private IEnumerator ChangeDirectionCoroutine()
    {
        // never stop this coroutine.
        while (true)
        {
            float newDirection = Random.Range(-180f,180f);
            float rotationSpeed = Random.Range(0.5f,1.5f);
            float totalRotation = 0;
            if(newDirection < 0){
                while(totalRotation > newDirection){
                    transform.Rotate(Vector3.forward, -rotationSpeed);
                    totalRotation -= rotationSpeed;
                    yield return new WaitForSeconds(0.01f);
                }
            }else{
                while(totalRotation < newDirection){
                    transform.Rotate(Vector3.forward, rotationSpeed);
                    totalRotation += rotationSpeed;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
    }
}
