using System.Drawing;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 rotationAxis = Vector3.up; // Define the axis (up, right, forward, etc.)
    Vector3 point = Vector3.zero;
    float rotationSpeed;// = 180f; // Speed of rotation
    float life = 1.5f;
    bool rotateRight;// = true;
    float ySpread;// = 0f;
    bool stop = false;
    public Renderer myRenderer;
    private Light myLight;
    private SphereCollider mySphereCollider;
    private float tolerance = 0.9f;

    //Assignables
    private Rigidbody myRigidbody;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsPlayer;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    private float explosionDamage = 10f;
    public float explosionRange;

    //Lifetime
    public int maxCollisions;
    public bool explodeOnTouch = true;

    int collisions = 0;
    PhysicMaterial physics_mat;
    bool hitWall = false;

    public AudioClip explodeAudio;
    private AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        Setup();
    }

    private void Update()
    {
        if (!stop)
        {
            //if (!hitWall)
            //{
                Quaternion rotation = Quaternion.AngleAxis((rotateRight ? -rotationSpeed : rotationSpeed) * Time.deltaTime, rotationAxis);
                Quaternion newRotation = rotation * Quaternion.LookRotation(myRigidbody.position - point);

                // Apply the rotation
                myRigidbody.MoveRotation(newRotation);

                // Manually move the Rigidbody to maintain distance from the point
                Vector3 offset = myRigidbody.position - point;
                myRigidbody.MovePosition(point + (rotation * offset));
            //}
            //When to explode:
            if (collisions >= maxCollisions) Explode();

            //Count down lifetime
            life -= Time.deltaTime;
            if (life <= 0) Explode();
        }
    }

    private void Explode()
    {
        audioPlayer.PlayOneShot(explodeAudio);
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyScript = enemies[i].GetComponent<Enemy>();
            if (enemyScript != null) enemyScript.takeDamage(explosionDamage);
            else Debug.Log("Null Component");
        }
        Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, whatIsPlayer);
        for (int i = 0; i < players.Length; i++)
        {
            PlayerHealth healthScript = players[i].GetComponent<PlayerHealth>();
            if (healthScript != null) healthScript.TakeDamage(explosionDamage/4);
            else Debug.Log("Null Component");
        }

        Collider[] boss = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < boss.Length; i++)
        {
            bossDamage bossScript = boss[i].GetComponent<bossDamage>();
            if (bossScript != null) bossScript.takeDamage(explosionDamage);
            else Debug.Log("Null Component");
        }

        StopRender();
    }

    private void StopRender()
    {
        stop = true;
        if (myRenderer != null) {
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
        if (myRigidbody != null)
        {
            myRigidbody.velocity = new Vector3(0, 0, 0);
            myRigidbody.useGravity = false;
        }
        Destroy(gameObject, 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rotationSpeed = rotationSpeed * 0.7f;
        //Count up collisions
        collisions++;

        if (collision.collider.CompareTag("Map")) Debug.Log("Collision");
            foreach (ContactPoint contact in collision.contacts)
            {
                // Check if the normal of the contact point is approximately horizontal
                if (Mathf.Abs(contact.normal.y) < tolerance) // 'tolerance' is a small value like 0.1
                {
                
                    // Get current velocity
                    Vector3 currentVelocity = myRigidbody.velocity;

                    // Set x and z components of the velocity to 0
                    currentVelocity.x = 0f;
                    currentVelocity.z = 0f;

                    // Apply the modified velocity back to the Rigidbody
                    myRigidbody.velocity = currentVelocity;
                    rotationSpeed = -rotationSpeed / 1.5f;
                }
            }

        //Explode if bullet hits an enemy directly and explodeOnTouch is activadted
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
        {
            Explode();
        }
            
    }

    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        myLight = GetComponent<Light>();
        mySphereCollider = GetComponent<SphereCollider>();
        myRigidbody = GetComponent<Rigidbody>();

        mySphereCollider.material = physics_mat;
        myRigidbody.useGravity = true;
        Vector3 currentVelocity = myRigidbody.velocity;
        currentVelocity.y = 15+ySpread;
        myRigidbody.velocity = currentVelocity;

    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float ySpreadValue, float lifeValue, float damageValue)
    {
        explosionDamage = damageValue;
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        ySpread = ySpreadValue;
    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
