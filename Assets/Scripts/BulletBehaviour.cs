using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Define the axis (up, right, forward, etc.)
    public float rotationSpeed = -180f; // Speed of rotation
    public float life = 1.5f;
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
