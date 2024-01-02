using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Flyer : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPlace;
    public GameObject Player;
    public float detectionRadius = 30f;
    bool awake = false;
    float distanceY;
    float distanceX;
    float levelDifference = 5f;
    float frontierMoveOrStay = 10f;
    LevelCounter playerScript;
    public int level = 0;
    public float rotationSpeed = 15f;
    bool canShoot = true;


    void Start()
    {
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) Debug.Log("playerNotFound");
        else
        {
            Debug.Log("playerFound");
            playerScript = Player.GetComponent<LevelCounter>();
            if (playerScript == null) Debug.Log("SCRIPT NOT FOUND");

        }
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
                if (Mathf.Abs(oldDist) < Mathf.Abs(newDist))
                {
                    transform.RotateAround(Vector3.zero, Vector3.up, -2 * rotationSpeed * Time.deltaTime);
                }
            }
            else {
                if (canShoot) {
                    canShoot = false;
                    Debug.Log("Shooting");
                    if (bullet != null) Instantiate(bullet, shootPlace.position, shootPlace.rotation);
                    Invoke("resetShot", 2f);
                }
            }
            transform.LookAt(Player.transform.position);
        }
        else
        {
            if (distanceX <= detectionRadius && playerScript.getLevel() == level)
            {
                awake = true;
                Debug.Log("awaken");
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

    private void resetShot() { canShoot = true; }
}
