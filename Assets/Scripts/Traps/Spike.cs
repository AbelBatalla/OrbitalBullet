using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    bool up = false;
    float posY_original;
    public float Yspeed = 0.035f;
    public float damage = 20f;
    void Start()
    {
        posY_original = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(!up && transform.position.y <= posY_original) {
            transform.position += new Vector3 (0, Yspeed, 0);
            if(transform.position.y >= posY_original) up = true;
        }
        if(up && transform.position.y > posY_original - 1.65) {
            transform.position += new Vector3 (0, -Yspeed, 0);
            if(transform.position.y <= posY_original - 1.65) up = false;
        }
        
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
