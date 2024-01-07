using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    //public GameObject bulletCollision;

    Vector3 rotationAxis = Vector3.up;
    private float rotationSpeed = 120f;
    private float life = 2.5f;
    bool stop = false;
    private float damage = 20f;
    private float height = 2f;
    private bool rotateRight;
    private Renderer myRenderer;
    private Light myLight;
    private SphereCollider mySphereCollider;
    private Rigidbody myRigidbody;
    Vector3 point = Vector3.zero;


    void Update()
    {
        if (!stop)
        {
            Quaternion rotation = Quaternion.AngleAxis((rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime, rotationAxis);

            // Apply the rotation
            myRigidbody.MoveRotation(rotation);

            // Manually move the Rigidbody to maintain distance from the point
            Vector3 offset = myRigidbody.position - point;
            myRigidbody.MovePosition(point + (rotation * offset));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stop && other.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player != null) player.TakeDamage(damage);
            else Debug.Log("Null Component");
            StopRender();
        }
        if (!stop && other.gameObject.CompareTag("Map"))
        {
            StopRender();
        }
    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float lifeValue, float damageValue, float heightValue)
    {
        damage = damageValue;
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        height = heightValue;
        Invoke("StopRender", life);
        myRenderer = GetComponent<MeshRenderer>();
        myLight = GetComponent<Light>();
        mySphereCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.velocity = Vector3.up * -heightValue;
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
