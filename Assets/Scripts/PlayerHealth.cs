using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float health;
    public float maxHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
