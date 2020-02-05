using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 2f;
    [SerializeField] private float ChangeDirectionAfterTime = 5f;

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
            // wait some time between changing directions
            yield return new WaitForSeconds(ChangeDirectionAfterTime);

            // change rotation to a random direction
            transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
        }
    }
}
