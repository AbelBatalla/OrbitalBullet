using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    PlayerHealth p_health;
    public float speed = 6f;
    public float timelife = 1.5f;
    // Start is called before the first frame update
    public GameObject explosionEffect;
    void Start()
    {
        p_health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        Destroy(gameObject, timelife);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Bullet");
        if(other.CompareTag("Player") || other.CompareTag("Level")){
            if(other.CompareTag("Player")) p_health.TakeDamage(10.0f);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
