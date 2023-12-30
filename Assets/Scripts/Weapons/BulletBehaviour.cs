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
    private float damage = 10f;
    private Renderer myRenderer;
    private Light myLight;
    private SphereCollider mySphereCollider;
    private Rigidbody myRigidbody;

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

        if (collision.collider.CompareTag("Wall"))
        {
            Instantiate(bulletCollision, collision.contacts[0].point, Quaternion.Euler(0, 180, 0));
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemyScript = collision.collider.GetComponent<Enemy>();
            if (enemyScript != null) enemyScript.takeDamage(damage);
            else Debug.Log("Null Component");
        }      
        StopRender();
    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float ySpreadValue, float lifeValue, float damageValue)
    {
        damage = damageValue;
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        ySpread = (rotateRight ? 1 : -1) * ySpreadValue;
        Invoke("StopRender", life);
        Debug.Log("Bullet Created with values: ySpread: " + ySpread + "speed: " + rotationSpeed + "life: " + life);
        myRenderer = GetComponent<MeshRenderer>();
        myLight = GetComponent<Light>();
        mySphereCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void StopRender()
    {
        stop = true;
        myRenderer.enabled = false;
        if (myRigidbody != null)
        {
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;
            myRigidbody.useGravity = false;
            myRigidbody.isKinematic = true;
        }
        if (mySphereCollider != null)
        {
            mySphereCollider.enabled = false;
        }
        if (myLight != null)
        {
            myLight.enabled = false;
        }
        Destroy(gameObject, 1.5f);
    }   
}
