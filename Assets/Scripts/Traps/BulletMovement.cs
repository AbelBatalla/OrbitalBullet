using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 6f;
    public float timelife = 1.5f;
    // Start is called before the first frame update
    public GameObject explosionEffect;
    void Start()
    {
        Destroy(gameObject, timelife);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") || other.CompareTag("Level")){
            
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
