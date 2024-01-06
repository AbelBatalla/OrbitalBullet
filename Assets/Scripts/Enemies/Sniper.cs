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
    public float rotationSpeed = 15f;
    bool canShoot = true;
    public float bulletRotationSpeed = 120f;
    public float life = 1.5f;
    public float damage = 20f;
    bool lookRight;
    private bool alive = true;
    public AudioClip deathAudio;
    public AudioClip hitAudio;
    public AudioClip shootAudio;
    private AudioSource audioPlayer;

    void Start()
    {
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) Debug.Log("playerNotFound");
        audioPlayer = GetComponent<AudioSource>();
        CheckDistance();

    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
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
                        audioPlayer.PlayOneShot(shootAudio);
                        var bulletActual = Instantiate(bullet, shootPlace.position, Quaternion.Euler(90f, shootPlace.eulerAngles.y, 0f));
                        SniperBullet bulletScript = bulletActual.GetComponent<SniperBullet>();
                        bulletScript.InitializeBullet(bulletRotationSpeed, lookRight, life, damage, 300f / distanceX);
                        Invoke("resetShot", 2f);
                    }
                }
            }
            else
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

    public void death()
    {
        alive = false;
        audioPlayer.PlayOneShot(deathAudio);
        Destroy(gameObject, 2f);
    }

    public void hit()
    {
        audioPlayer.PlayOneShot(hitAudio);
    }
}
