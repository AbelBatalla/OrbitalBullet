using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    public float maxShield = 50;
    public float shield;
    public int deathAnim = 0; //0=false, 1=Crawler, 2=Sniper, 3=Flyer
    private bool dead = false;
    private Sniper sniperScript;
    private Flyer flyerScript;
    private CrawlerController crawlerScript;
    public KillsCounter killsCounter;
    void Start()
    {
        health = maxHealth;
        shield = maxShield;
        if (deathAnim == 1) crawlerScript = GetComponentInParent<CrawlerController>();
        else if (deathAnim == 2) sniperScript = GetComponentInParent<Sniper>();
        else if (deathAnim == 3) flyerScript = GetComponentInParent<Flyer>();
        killsCounter = GameObject.FindWithTag("KillsCanvas").GetComponent<KillsCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            takeDamage(10f);
        }
        if (health <= 0 && !dead)
        {
            killsCounter?.addKill();
            if (deathAnim == 0) Destroy(transform.parent.gameObject);
            else
            {
                dead = true;
                if (deathAnim == 1)
                {
                    crawlerScript.death();
                    GetComponent<BoxCollider>().enabled = false;
                }
                else if (deathAnim == 2)
                {
                    sniperScript.death();
                    Destroy(gameObject);
                }
                else if (deathAnim == 3)
                {
                    flyerScript.death();
                    Destroy(gameObject);
                }
            }
        }
    }

    public void takeDamage(float damage) {
        if (deathAnim == 1) crawlerScript.hit();
        else if (deathAnim == 2) sniperScript.hit();
        else if (deathAnim == 3) flyerScript.hit();
        shield -= damage;
        if (shield < 0) {
            health += shield;
            shield = 0;
        }
    }

}
