using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionBasedOnArenaScale : MonoBehaviour
{
    private Transform arena;

    void Start()
    {
        arena = FindObjectOfType<ShrinkArena>().transform;
    }

    private void Update()
    {
        transform.position = Vector3.up * arena.localScale.x;
    }
}
