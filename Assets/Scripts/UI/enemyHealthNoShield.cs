using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthNoShield : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Enemy playerHealth;
    private float lerpSpeed = 0.03f;
    private float oldEase;

    void Start()
    {
        oldEase = healthSlider.maxValue = healthSlider.value = easeHealthSlider.maxValue = easeHealthSlider.value = playerHealth.maxHealth;
    }

    void Update()
    {
        if (healthSlider.value != playerHealth.health)
        {
            healthSlider.value = playerHealth.health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerHealth.health, lerpSpeed);
            if (oldEase == easeHealthSlider.value) easeHealthSlider.value = playerHealth.health;
            oldEase = easeHealthSlider.value;
        }
    }
}
