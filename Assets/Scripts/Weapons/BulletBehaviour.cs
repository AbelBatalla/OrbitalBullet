using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject bulletCollision;

    Vector3 rotationAxis = Vector3.up; // Define the axis (up, right, forward, etc.)
    float rotationSpeed;// = 180f; // Speed of rotation
    float life = 1.5f;
    bool rotateRight;// = true;
    float ySpread;// = 0f;
    bool stop = false;
    private Renderer myRenderer;
    private Light myLight;
    private SphereCollider mySphereCollider;

    void Update()
    {
        if (!stop)
        {
            transform.RotateAround(Vector3.zero, rotationAxis, (rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime);
            transform.Translate(Vector3.forward * ySpread * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(bulletCollision, collision.contacts[0].point, Quaternion.Euler(0, 180, 0));
        StopRender();
    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float ySpreadValue, float lifeValue)
    {
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        ySpread = (rotateRight ? 1 : -1) * ySpreadValue;
        Invoke("StopRender", life);
        Debug.Log("Bullet Created with values: ySpread: " + ySpread + "speed: " + rotationSpeed + "life: " + life);
        myRenderer = GetComponent<MeshRenderer>();
        myLight = GetComponent<Light>();
        mySphereCollider = GetComponent<SphereCollider>();
    }

    private void StopRender()
    {
        stop = true;
        myRenderer.enabled = false;
        if (mySphereCollider != null)
        {
            mySphereCollider.enabled = false;
        }
        if (myLight != null)
        {
            myLight.enabled = false;
        }
        Destroy(gameObject, 2.0f);
    }   
}
