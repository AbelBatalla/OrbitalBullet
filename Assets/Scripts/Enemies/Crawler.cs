using UnityEngine;

public class Crawler : MonoBehaviour
{
    public GameObject Player;
    public float detectionRadius = 30f;
    bool awake = false;
    float distanceX;
    float levelDifference = 5f;
    LevelCounter playerScript;
    PlayerHealth playerHealth;
    public int level = 0;
    public float rotationSpeed = 15f;
    int status = 0; //0 idle, -1 move left, 1 move right;
    public float damage = 1f;


    void Start()
    {
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) Debug.Log("playerNotFound");
        else
        {
            Debug.Log("playerFound");
            playerScript = Player.GetComponent<LevelCounter>();
            if (playerScript == null) Debug.Log("LEVEL SCRIPT NOT FOUND");
            playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerScript == null) Debug.Log("HEALTH SCRIPT NOT FOUND");
        }
        randomizeIdle();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        //if (distanceX <= detectionRadius) awake = true;
        //else awake = false;
        awake = distanceX <= detectionRadius;


        if (awake)
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
            transform.LookAt(Player.transform.position);
        }
        else
        {
            if (status != 0) transform.RotateAround(Vector3.zero, Vector3.up, status * rotationSpeed * 0.8f * Time.deltaTime);
        }
    }

    private void CheckDistance()
    {
        Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPositionXZ = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);

        distanceX = Vector3.Distance(enemyPositionXZ, playerPositionXZ);
    }

    private void randomizeIdle() {
        status = Random.Range(0, 3) - 1;
        float time = Random.Range(1.5f, 2.5f);
        Debug.Log("time: " + time);
        Invoke("randomizeIdle", time);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (playerHealth != null) playerHealth.TakeDamage(damage);
        }
    }
    */
}
