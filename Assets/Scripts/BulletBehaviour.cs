using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    Vector3 rotationAxis = Vector3.up; // Define the axis (up, right, forward, etc.)
    float rotationSpeed;// = 180f; // Speed of rotation
    float life = 1.5f;
    bool rotateRight;// = true;
    float ySpread;// = 0f;
 
    void Update()
    {
        transform.RotateAround(Vector3.zero, rotationAxis, (rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime);
        transform.Translate(Vector3.back * ySpread * Time.deltaTime);
    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float ySpreadValue, float lifeValue)
    {
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        ySpread = (rotateRight ? 1 : -1) * ySpreadValue;
        Destroy(gameObject, life);
        Debug.Log("Bullet Created with values: ySpread: " + ySpread + "speed: " + rotationSpeed + "life: " + life);
    }

}
