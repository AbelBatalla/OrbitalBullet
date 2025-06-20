using UnityEngine;
using UnityEngine.UIElements;

public class Flyer : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPlace;
    public GameObject Player;
    public float detectionRadius = 30f;
    bool awake = false;
    float distanceX;
    float frontierMoveOrStay = 10f;
    public float rotationSpeed = 15f;
    bool canShoot = true;
    private bool alive = true;
    public AudioClip deathAudio;
    public AudioClip hitAudio;
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
                    if (Mathf.Abs(oldDist) < Mathf.Abs(newDist))
                    {
                        transform.RotateAround(Vector3.zero, Vector3.up, -2 * rotationSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (canShoot)
                    {
                        canShoot = false;
                        if (bullet != null) Instantiate(bullet, shootPlace.position, shootPlace.rotation);
                        Invoke("resetShot", 2f);
                    }
                }
                transform.LookAt(Player.transform.position);
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
