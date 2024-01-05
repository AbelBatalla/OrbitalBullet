using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossDamage : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    public float maxShield = 50;
    public float shield;
    private bool dead = false;

    public GameObject EndPannel;

    Animator anim;
    void Start()
    {
        health = maxHealth;
        shield = maxShield;
        anim = gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            takeDamage(10f);
        }

        if (health <= 0)
        {
            anim.SetBool("dead", true);
            Invoke("EndGame", 5.0f);
           
        }
    }

    void EndGame(){
        EndPannel.SetActive(true);
        Invoke("ChangeScene",  1.7f);
    }

    void ChangeScene() {
        SceneManager.LoadScene("Credits");
    }
    public void takeDamage(float damage) {
        shield -= damage;
        if (shield < 0) {
            health += shield;
            shield = 0;
        }
    }

    public float getHealth() {
        return health;
    }

}
