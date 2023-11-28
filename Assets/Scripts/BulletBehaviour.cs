using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Define the axis (up, right, forward, etc.)
    public float rotationSpeed = 180f; // Speed of rotation
    public float life = 1.5f;
    public bool rotateRight = true;
    public float ySpread = 0;
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, rotationAxis, (rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime);
        transform.Translate(Vector3.up * ySpread * Time.deltaTime);
    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float ySpreadValue, float lifeValue)
    {
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        ySpread = ySpreadValue;
    }

}
