using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    bool up = false;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!up && transform.position.y < 2.62) {
            transform.position += new Vector3 (0, 0.15f, 0);
            if(transform.position.y >= 2.62) up = true;
        }
        if(up && transform.position.y > 0.97) {
            transform.position += new Vector3 (0, -0.15f, 0);
            if(transform.position.y <= 0.97) up = false;
        }
        
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(30.0f);
        }
    }
}
