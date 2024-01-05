using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossController : MonoBehaviour
{

    bool awake = true;
    private LevelCounter level_counter;

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

    float timeCounter; // timer to help to track the life lost in some period of time
    float healthLost;

    public float bulletSpeed, spread, bulletLife, damage;
    //(bulletSpeed, MovePlayer.lookRight, ySpread, bulletLife, damage) (-spread, spread);

    

    float frontierMoveOrStay = 20f;
    // Start is called before the first frame update
    void Start()
    {
        level_counter = Player.GetComponent<LevelCounter>();
        anim = gameObject.GetComponent<Animator>();
        //gunBoss = gameObject.GetComponent<GunBoss>();
        //Debug.Log(gunBoss);
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.x = 95.0f;
        startDirection.Normalize();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        dead = anim.GetBool("dead");
        attacking = anim.GetBool("attack");
        if(Input.GetKeyDown(KeyCode.K)) {
            anim.SetTrigger("attack");
            //gameObject.GetComponent<GunBoss>().Shoot();
        }
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
                anim.SetFloat("Blend", 1.0f, 0.1f, Time.deltaTime);
            }
            else
            {
                if (canShoot)
                {
                    canShoot = false;
                    anim.SetTrigger("attack");
                    attacking = true;    
                    var bulletNew = Instantiate(bullet, shootPlace.position, Quaternion.Euler(90f, shootPlace.eulerAngles.y, 0f));
                    float coeficientSpread = Random.Range(0, 1);
                    float ySpread = Random.Range(-(spread + coeficientSpread*spread), spread + coeficientSpread*spread);
                    float coeficient = Random.Range(0, 1);
                    Projectile Projectile = bulletNew.GetComponent<Projectile>();

                    float bSpeedRand = Random.Range(bulletSpeed - coeficient*bulletSpeed, bulletSpeed + coeficient*bulletSpeed);
                    if(Projectile != null) Projectile.InitializeBullet(bSpeedRand, true, ySpread, bulletLife, damage);
                    Invoke("resetShot", 1f);
                }
                anim.SetFloat("Blend", 0.0f, 0.1f, Time.deltaTime);
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