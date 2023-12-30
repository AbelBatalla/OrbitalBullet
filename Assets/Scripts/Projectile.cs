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

    //Assignables
    private Rigidbody myRigidbody;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public float explosionDamage;
    public float explosionRange;

    //Lifetime
    public int maxCollisions;
    public bool explodeOnTouch = true;

    int collisions = 0;
    PhysicMaterial physics_mat;
    bool hitWall = false;
    Vector3 gravity = new Vector3(0, -9.81f, 0); // Earth-like gravity
    Vector3 velocity = new Vector3(0, 0, 0); // Earth-like gravity

    private void Start()
    {
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
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        Debug.Log("Number of enemies detected: " + enemies.Length);
        Debug.Log(enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyScript = enemies[i].GetComponent<Enemy>();
            if (enemyScript != null) enemyScript.takeDamage(explosionDamage);
            else Debug.Log("Null Component");
        }
        StopRender();
    }

    private void StopRender()
    {
        myRigidbody.velocity = new Vector3(0, 0, 0);
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
            myRigidbody.useGravity = false;
        }
        Destroy(gameObject, 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rotationSpeed = rotationSpeed * 0.7f;
        //Count up collisions
        collisions++;

        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Enemy"))
        {
            hitWall = true;
            // Get current velocity
            Vector3 currentVelocity = myRigidbody.velocity;

            // Set x and z components of the velocity to 0
            currentVelocity.x = 0f;
            currentVelocity.z = 0f;

            // Apply the modified velocity back to the Rigidbody
            myRigidbody.velocity = currentVelocity;
            rotationSpeed = -rotationSpeed / 2;
        }

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
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
        currentVelocity.y = 6+ySpread;
        myRigidbody.velocity = currentVelocity;

    }

    public void InitializeBullet(float rotationSpeedValue, bool rotateRightValue, float ySpreadValue, float lifeValue)
    {
        rotationSpeed = rotationSpeedValue;
        rotateRight = rotateRightValue;
        life = lifeValue;
        ySpread = ySpreadValue;
        Debug.Log("Grenade Created with values: ySpread: " + ySpread + " speed: " + rotationSpeed + " life: " + life);

    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
