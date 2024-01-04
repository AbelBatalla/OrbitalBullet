using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    //public GameObject bulletCollision;

    Vector3 rotationAxis = Vector3.up;
    private float rotationSpeed = 120f;
    private float life = 1.5f;
    bool stop = false;
    private float damage = 20f;
    private bool rotateRight;
    private Renderer myRenderer;
    private Light myLight;
    private SphereCollider mySphereCollider;
    private Rigidbody myRigidbody;

    void Update()
    {
        if (!stop)
        {
            transform.RotateAround(Vector3.zero, rotationAxis, (rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime);
            transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Map"))
        {
            //Instantiate(bulletCollision, collision.contacts[0].point, Quaternion.Euler(0, 180, 0));
        }
        else if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();
            if (player != null) player.TakeDamage(damage);
            else Debug.Log("Null Component");
        }
        StopRender();
    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float lifeValue, float damageValue)
    {
        damage = damageValue;
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        Invoke("StopRender", life);
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
