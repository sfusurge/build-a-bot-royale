using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
