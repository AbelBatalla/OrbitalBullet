using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossController : MonoBehaviour
{

    bool awake = false;
    private LevelCounter level_counter;

    public GameObject force_field;

   // GunBoss gunBoss;

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

    public GameObject faceGun;

    float timeCounter = 0.0f; // timer to help to track the life lost in some period of time
    float health = 300.0f;
    float timeShoot = 2.9f;
    int fase_actual = 1;

    bossDamage scriptBossDamage;

    float counterForce_field = 0.0f;
    float timer_force_field = 0.0f;

    bool field_activate = false;

    public float bulletSpeed, spread, bulletLife, damage;
    //(bulletSpeed, MovePlayer.lookRight, ySpread, bulletLife, damage) (-spread, spread);

    

    float frontierMoveOrStay = 20f;
    // Start is called before the first frame update
    void Start()
    {
        level_counter = Player.GetComponent<LevelCounter>();
        anim = gameObject.GetComponent<Animator>();
        scriptBossDamage = gameObject.GetComponentInChildren<bossDamage>();
        Debug.Log(scriptBossDamage);
        //gunBoss = gameObject.GetComponent<GunBoss>();
        //Debug.Log(gunBoss);
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.x = 95.0f;
        startDirection.Normalize();
        first_fase();
    }



    void first_fase(){
        bulletSpeed = 10.0f;
        bulletLife = 6.0f;
        damage = 20.0f;
        spread = 0.7f;
        rotationSpeed = 40.0f;
        fase_actual = 1;
        Debug.Log("First Fase");
    }
     
    void shooter(){
        canShoot = false;
        attacking = true;    
        var bulletNew = Instantiate(bullet, shootPlace.position, Quaternion.Euler(90f, shootPlace.eulerAngles.y, 0f));
        float coeficientSpread = Random.Range(0, 1);
        float ySpread = Random.Range(-(spread + coeficientSpread*spread), spread + coeficientSpread*spread);
        float coeficient = Random.Range(0, 1);
        ProjectileBoss Projectile = bulletNew.GetComponent<ProjectileBoss>();

        float bSpeedRand = Random.Range(bulletSpeed - coeficient*bulletSpeed, bulletSpeed + coeficient*bulletSpeed);
        if(Projectile != null) Projectile.InitializeBullet(bSpeedRand, !rotated, ySpread, bulletLife, damage);
        Invoke("resetShot", 3.3f);
    }

    // Update is called once per frame
    void Update()
    {
        dead = anim.GetBool("dead");
        if (awake && !dead)
        {
            if((timeCounter > 40.0f || health <= 150.0f) && fase_actual == 1){
                faceGun.SetActive(true);
            }

            if(field_activate){
                faceGun.SetActive(true);
            } else {
                faceGun.SetActive(false);
            }

            if(counterForce_field >= Random.Range(8, 18) && !field_activate){
                force_field.SetActive(true);
                counterForce_field = 0.0f;
                field_activate = true;
            } else if(counterForce_field >= Random.Range(6, 16) && field_activate) {
                force_field.SetActive(false);
                counterForce_field = 0.0f;
                field_activate = false;
            }
            counterForce_field += Time.deltaTime;

            timeCounter += Time.deltaTime;
            health = scriptBossDamage.getHealth();
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
                        Debug.Log("ROTATED");
                        transform.Rotate(Vector3.up, 180f);
                        rotated = false;
                    }
                    transform.RotateAround(Vector3.zero, Vector3.up, -2 * rotationSpeed * Time.deltaTime);
                } else if(Mathf.Abs(oldDist) > Mathf.Abs(newDist)){
                    Debug.Log("walking reverse");
                    if(!rotated) {
                        Debug.Log("NOT ROTATED");
                        transform.Rotate(Vector3.up, 180f);
                        rotated = true;
                    }
                }
                Debug.Log(Mathf.Abs(newDist));
                anim.SetFloat("Blend", 1.0f, 0.1f, Time.deltaTime);
            }
            else
            {
                if (canShoot && !field_activate)
                {
                    anim.SetTrigger("attack");
                    Invoke("shooter", 1.7f);
                }
                else {anim.SetFloat("Blend", 0.0f, 0.1f, Time.deltaTime);}
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


private void resetShot() { 
    canShoot = true; 
    attacking = false;
}

    
}