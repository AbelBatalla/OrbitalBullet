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
    bool god_mode = false;
    public AudioClip deathAudio;
    public AudioClip hitAudio;
    public AudioClip regenAudio;
    private AudioSource audioPlayer;
    bool dead = false;
    bool poisoned = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameOver_menu.SetActive(false);
        playerObject = GameObject.Find("T-Pose_new");
        anim = playerObject.GetComponentInChildren<Animator>();
        audioPlayer = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            giveHealth();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            god_mode = !god_mode;
        }
        if(health <= 0) {
            anim.SetBool("Death",true);
            gameOver_menu.SetActive(true);
            if (!dead)
            {
                audioPlayer.PlayOneShot(deathAudio);
                dead = true;
            }
        }
        if(playerObject.transform.position.y < -10f) TakeDamage(100f);
    }

    public void TakeDamageContinuous(){
        if(!poisoned){
             health -= 10.0f;
             poisoned = true;
             StartCoroutine("damageForceField");
        }
    }

    IEnumerator damageForceField() {
        if(poisoned){
            health -= 3.0f;
        }
        yield return new WaitForSeconds(2);
        poisoned = false;
    }
    public void TakeDamage(float damage)
    {
        if (anim.GetBool("Slide") == false && !god_mode)
        {
            health -= damage;
            if (!dead) audioPlayer.PlayOneShot(hitAudio);
        }
    }

    public void giveHealth() {
        health = maxHealth;
        audioPlayer.PlayOneShot(regenAudio);
    }


}
