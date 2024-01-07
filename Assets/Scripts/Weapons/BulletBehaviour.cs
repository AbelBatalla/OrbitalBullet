using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject bulletCollision;

    Vector3 point = Vector3.zero;
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
            Quaternion rotation = Quaternion.AngleAxis((rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime, rotationAxis);

            // Apply the rotation
            myRigidbody.MoveRotation(rotation);

            // Manually move the Rigidbody to maintain distance from the point
            Vector3 offset = myRigidbody.position - point;
            myRigidbody.MovePosition(point + (rotation * offset));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Map"))
        {
            Instantiate(bulletCollision, collision.contacts[0].point, Quaternion.Euler(0, 0, 180));
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemyScript = collision.collider.GetComponent<Enemy>();
            if (enemyScript != null) enemyScript.takeDamage(damage);
            else { //Boss Case
                bossDamage bossScript = collision.collider.GetComponent<bossDamage>();
                if (bossScript != null) bossScript.takeDamage(damage);
                else Debug.Log("Null Component");
            } 
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
        myRenderer = GetComponent<MeshRenderer>();
        myLight = GetComponent<Light>();
        mySphereCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.velocity = Vector3.up * ySpread;
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
