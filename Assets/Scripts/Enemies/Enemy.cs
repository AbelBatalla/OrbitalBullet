using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    public float maxShield = 50;
    public float shield;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        shield = maxShield;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            takeDamage(10f);
        }
        if (health <= 0) {
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetBool("killed", true);
        }
    }
    public void takeDamage(float damage) { 
        shield -= damage;
        if (shield < 0) {
            health += shield;
            shield = 0;
        }
    }

}
