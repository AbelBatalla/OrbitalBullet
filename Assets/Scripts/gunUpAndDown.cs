using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunUpAndDown : MonoBehaviour
{

    // Speed of the movement
    public float speed = 5.0f;

    // Height of the movement
    public float height = 0.5f;

    // Original Y position
    private float originalY;

    void Start()
    {
        // Store the original Y position
        originalY = transform.position.y;
    }

    void Update()
    {
        // Calculate the new Y position
        float newY = originalY + height * Mathf.Sin(Time.time * speed);

        // Apply the new Y position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

