using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    bool awake = false;
    private LevelCounter level_counter;

    GunBoss gunBoss;

    Vector3 startDirection;

    public GameObject Player;

    private Animator anim;

    public float detectionRadius = 30f;
    float distanceY;
    float distanceX;

    public float rotationSpeed = 20f;
    bool rotated = false;

    bool attacking = false;

    float lastTime_attack = 0.0f;

    public GameObject bullet;

    bool canShoot = true;

    bool dead = false;

    public Transform shootPlace;

    

    float frontierMoveOrStay = 20f;
    // Start is called before the first frame update
    void Start()
    {
        level_counter = Player.GetComponent<LevelCounter>();
        anim = gameObject.GetComponentInChildren<Animator>();
        gunBoss = gameObject.GetComponent<GunBoss>();
        Debug.Log(gunBoss);
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.x = 95.0f;
        startDirection.Normalize();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        dead = anim.GetBool("killed");
        if(Input.GetKeyDown(KeyCode.K)) gameObject.GetComponent<GunBoss>().Shoot();
        if (awake && !dead)
        {
            CheckDistance();
            if (Mathf.Abs(distanceX) > frontierMoveOrStay && !attacking)
            {
                float oldDist = distanceX;
                transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
                Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 playerPositionXZ = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
                float newDist = Vector3.Distance(enemyPositionXZ, playerPositionXZ);
                if (Mathf.Abs(oldDist) < Mathf.Abs(newDist))
                {
                    Debug.Log("walking");
                    if(rotated) {
                        transform.Rotate(Vector3.up, 180f);
                        rotated = false;
                    }
                    transform.RotateAround(Vector3.zero, Vector3.up, -2 * rotationSpeed * Time.deltaTime);
                } else if(Mathf.Abs(oldDist) > Mathf.Abs(newDist)){
                    Debug.Log("walking reverse");
                    if(!rotated) {
                        transform.Rotate(Vector3.up, 180f);
                        rotated = true;
                    }
                }
                Debug.Log(Mathf.Abs(newDist));
                anim.SetFloat("Blend", 0.5f, 0.1f, Time.deltaTime);
            }
            else
            {
                float value = Random.value;
                if(lastTime_attack != 0) lastTime_attack += Time.deltaTime;
                if(lastTime_attack >= 10.0f){
                    lastTime_attack = 0;
                    attacking = false;
                } 
                else if(lastTime_attack >= 3.0f) anim.SetBool("attack", false);
                //Debug.Log(lastTime_attack);
                if(value > 0.5f){
                    if(!attacking){
                        anim.SetBool("attack", true);
                        lastTime_attack = Time.deltaTime;
                        Debug.Log(lastTime_attack);
                        attacking = true;
                        gameObject.GetComponent<GunBoss>().Shoot();
                        gameObject.GetComponent<GunBoss>().Shoot();
                        
                    } 
                }
                anim.SetFloat("Blend", 0.0f, 0.1f, Time.deltaTime);
                if (false)
                {
                    //canShoot = false;
                    //if (bullet != null) Instantiate(bullet, shootPlace.position, shootPlace.rotation);
                    //Invoke("resetShot", 2f);
                }
            }
            // Correct orientation of player
            // Compute current direction
            Vector3 currentDirection = transform.position - transform.parent.position;
            currentDirection.y = 0.0f;
            currentDirection.Normalize();
            // Change orientation of player accordingly
            Quaternion orientation;
            if ((startDirection - currentDirection).magnitude < 1e-3)
                orientation = Quaternion.AngleAxis(0.0f, Vector3.up);
            else if ((startDirection + currentDirection).magnitude < 1e-3)
                orientation = Quaternion.AngleAxis(180.0f, Vector3.up);
            else
                orientation = Quaternion.FromToRotation(startDirection, currentDirection);
            transform.rotation = orientation;
        }
        else
        {
            if (level_counter.getLevel() == 5)
            {
                CheckDistance();
                if (distanceX <= detectionRadius)
                {
                    awake = true;
                }
            }
        }
        
    }

private void CheckDistance()
    {
        Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPositionXZ = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);

        distanceX = Vector3.Distance(enemyPositionXZ, playerPositionXZ);
        distanceY = Mathf.Abs(transform.position.y - Player.transform.position.y);
    }




    
}
