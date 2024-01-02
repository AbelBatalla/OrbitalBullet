using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public GameObject player;
    public float detectionRadius = 40f;
    bool awake = false;
    float distanceY;
    float distanceX;
    float levelDifference = 5f;
    float frontierMoveOrStay = 3f;
    LevelCounter playerScript;
    public int level = 0;
    PlayerHealth p_health;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null) Debug.Log("playerNotFound");
        else
        {
            Debug.Log("playerFound");
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelCounter>();
            if (playerScript == null) Debug.Log("SCRIPT NOT FOUND");
            InvokeRepeating("CheckDistance", 0f, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (awake) {
            transform.LookAt(player.transform.position);
            if (distanceX > frontierMoveOrStay) { } //go to him
            else { } //shoot
        }
        else {
            if (distanceX <= detectionRadius && playerScript?.getLevel() == level)
            {
                awake = true;
                Debug.Log("awaken");
            }
        }
    }

    private void CheckDistance()
    {
        Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPositionXZ = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        distanceX = Vector3.Distance(enemyPositionXZ, playerPositionXZ);
        distanceY = Mathf.Abs(transform.position.y - player.transform.position.y);
    }
}
