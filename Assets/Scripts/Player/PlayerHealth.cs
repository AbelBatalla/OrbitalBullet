using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float health;
    public float maxHealth = 100f;
    public GameObject gameOver_menu;
    Animator anim;
    GameObject playerObject = null; 

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameOver_menu.SetActive(false);
        playerObject = GameObject.Find("T-Pose_new");
        anim = playerObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10f);
        }
        if(health <= 0) {
            anim.SetBool("Death",true);
            gameOver_menu.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
