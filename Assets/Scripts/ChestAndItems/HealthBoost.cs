using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    PlayerHealth healthController;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
        GameObject Player = GameObject.FindWithTag("Player");
        if (Player != null)
        {
            healthController = Player.GetComponent<PlayerHealth>();
            healthController.giveHealth();
        }
    }
}
