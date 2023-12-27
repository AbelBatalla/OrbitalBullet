using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Slider shieldSlider;
    public Slider easeShieldSlider;
    public Enemy playerHealth;
    private float lerpSpeed = 0.03f;
    private float oldEase;
    private float oldEaseShield;

    void Start()
    {
        oldEase = healthSlider.maxValue = healthSlider.value = easeHealthSlider.maxValue = easeHealthSlider.value = playerHealth.maxHealth;
        oldEaseShield = shieldSlider.maxValue = shieldSlider.value = easeShieldSlider.maxValue = easeShieldSlider.value = playerHealth.maxShield;
    }

    void Update()
    {
        if (healthSlider.value != playerHealth.health)
        {
            healthSlider.value = playerHealth.health;
        }
        if (shieldSlider.value != playerHealth.shield)
        {
            shieldSlider.value = playerHealth.shield;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerHealth.health, lerpSpeed);
            if (oldEase == easeHealthSlider.value) easeHealthSlider.value = playerHealth.health;
            oldEase = easeHealthSlider.value;
        }

        if (shieldSlider.value != easeShieldSlider.value)
        {
            easeShieldSlider.value = Mathf.Lerp(easeShieldSlider.value, playerHealth.shield, lerpSpeed);
            if (oldEaseShield == easeShieldSlider.value) easeShieldSlider.value = playerHealth.shield;
            oldEaseShield = easeShieldSlider.value;
        }
    }
}
