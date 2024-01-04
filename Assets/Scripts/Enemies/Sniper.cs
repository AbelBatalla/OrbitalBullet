using Unity.VisualScripting;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPlace;
    public GameObject Player;
    public float detectionRadius = 30f;
    bool awake = false;
    float distanceX;
    float frontierMoveOrStay = 50f;
    float frontierEscape = 30f;
    LevelCounter playerScript;
    public int level = 0;
    public float rotationSpeed = 15f;
    bool canShoot = true;
    public float bulletRotationSpeed = 120f;
    public float life = 1.5f;
    public float damage = 20f;
    bool lookRight;

    void Start()
    {
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) Debug.Log("playerNotFound");
        else
        {
            playerScript = Player.GetComponent<LevelCounter>();
            if (playerScript == null) Debug.Log("SCRIPT NOT FOUND");
        }
        CheckDistance();

    }

    // Update is called once per frame
    void Update()
    {
        if (awake)
        {
            CheckDistance();
            if (Mathf.Abs(distanceX) > frontierMoveOrStay)
            {
                float oldDist = distanceX;
                transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
                Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 playerPositionXZ = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
                float newDist = Vector3.Distance(enemyPositionXZ, playerPositionXZ);
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.zero - transform.position, Vector3.up));
                lookRight = false;
                if (Mathf.Abs(oldDist) < Mathf.Abs(newDist))
                {
                    lookRight = true;
                    transform.RotateAround(Vector3.zero, Vector3.up, -2 * rotationSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, Vector3.zero - transform.position));
                }
            }
            else
            {
                if (Mathf.Abs(distanceX) < frontierEscape)
                {
                    float oldDist = distanceX;
                    transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
                    Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
                    Vector3 playerPositionXZ = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
                    float newDist = Vector3.Distance(enemyPositionXZ, playerPositionXZ);
                    transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, Vector3.zero - transform.position));
                    lookRight = true;
                    if (Mathf.Abs(oldDist) > Mathf.Abs(newDist))
                    {
                        lookRight = false;
                        transform.RotateAround(Vector3.zero, Vector3.up, -2 * rotationSpeed * Time.deltaTime);
                        transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.zero - transform.position, Vector3.up));
                    }
                }
                if (canShoot)
                {
                    canShoot = false;
                    var bulletActual = Instantiate(bullet, shootPlace.position, Quaternion.Euler(90f, shootPlace.eulerAngles.y, 0f));
                    SniperBullet bulletScript = bulletActual.GetComponent<SniperBullet>();
                    bulletScript.InitializeBullet(bulletRotationSpeed, lookRight, life, damage);
                    Invoke("resetShot", 2f);
                }
            }
        }
        else
        {
            if (playerScript.getLevel() == level)
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
    }

    private void resetShot() { canShoot = true; }
}
