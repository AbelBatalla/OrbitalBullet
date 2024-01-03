using UnityEngine;

public class CrawlerAttaclAndStomp : MonoBehaviour
{
    private bool isInCollider1 = false;
    private bool isInCollider2 = false;

    Enemy enemy;
    public PlayerHealth player;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            Debug.Log("PlayerCollision");
            isInCollider1 = true;
            /*
            if (other == collider1)
            {
                Debug.Log("Enter1");
                isInCollider1 = true;
                CheckAndTriggerBehavior();
            }
            else if (other == collider2)
            {
                Debug.Log("Enter2");
                isInCollider2 = true;
                CheckAndTriggerBehavior();
            }
            */
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Check if it's the player
        {
            Debug.Log("PlayerLeft");
            isInCollider1 = false;
            /*
            if (other == collider1)
            {
                isInCollider1 = false;
            }
            else if (other == collider2)
            {
                isInCollider2 = false;
            }
            */
        }
    }

    private void CheckAndTriggerBehavior()
    {
        if (isInCollider1 && !isInCollider2)
        {
            // Trigger behavior for being in Collider1 only
            TriggerBehavior1();
        }
        else if (isInCollider2 && !isInCollider1)
        {
            // Trigger behavior for being in Collider2 only
            TriggerBehavior2();
        }
    }

    private void TriggerBehavior1()
    {
        // Define behavior when in Collider1 only
        Debug.Log("In Collider1 only");
        enemy.takeDamage(80f);
    }

    private void TriggerBehavior2()
    {
        // Define behavior when in Collider2 only
        Debug.Log("In Collider2 only");
        player.TakeDamage(1f);
    }
}
