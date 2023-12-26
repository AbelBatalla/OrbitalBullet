using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public PlayerHealth playerHealth;
    private float lerpSpeed = 0.03f;
    private float oldEase;

    void Start()
    {
        oldEase = healthSlider.maxValue = healthSlider.value = easeHealthSlider.maxValue = easeHealthSlider.value = playerHealth.maxHealth;
        Debug.Log("healthSlider: " + healthSlider.value);
        Debug.Log("easehealthSlider: " + easeHealthSlider.value);
        Debug.Log("STARTED");

    }

    void Update()
    {
        if(healthSlider.value != playerHealth.health)
        {
            healthSlider.value = playerHealth.health;
            Debug.Log("healthSlider UPDATE: " + healthSlider.value);
        }

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerHealth.health, lerpSpeed);
            if (oldEase == easeHealthSlider.value) easeHealthSlider.value = playerHealth.health;
            oldEase = easeHealthSlider.value;
            Debug.Log("easeHealthSlider UPDATE: " + easeHealthSlider.value + " to " + playerHealth.health);

        }
    }
}
