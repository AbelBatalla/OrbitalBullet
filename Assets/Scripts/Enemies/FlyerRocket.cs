using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerRocket : MonoBehaviour
{
    float life = 1.5f;
    bool stop = false;
    public Renderer myRenderer;
    private Light myLight;
    private SphereCollider mySphereCollider;
    private Rigidbody myRigidbody;

    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Damage
    public float explosionDamage = 10f;
    public float explosionRange;

    PlayerHealth p_health;
    public float speed = 6f;

    private AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        myLight = GetComponent<Light>();
        mySphereCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        p_health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            // Calculate the new position
            Vector3 newPosition = myRigidbody.position + transform.forward * Time.fixedDeltaTime * speed;

            // Move the Rigidbody to the new position
            myRigidbody.MovePosition(newPosition); life -= Time.deltaTime;
            if (life <= 0) Explode();
        }
    }

    private void Explode() {
        audioPlayer.Play();
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < players.Length; i++)
        {
            p_health.TakeDamage(explosionDamage);
            break;
        }
        StopRender();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!stop) Explode();
    }

    private void StopRender()
    {
        stop = true;
        if (myRenderer != null)
        {
            myRenderer.enabled = false;
        }
        if (mySphereCollider != null)
        {
            mySphereCollider.enabled = false;
        }
        if (myLight != null)
        {
            myLight.enabled = false;
        }
        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
