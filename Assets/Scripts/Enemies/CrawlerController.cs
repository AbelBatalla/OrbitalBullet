using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CrawlerController : MonoBehaviour
{
    public GameObject Player;
    public float detectionRadius = 30f;
    bool awake = true;
    float distanceX;
    float levelDifference = 5f;
    LevelCounter playerScript;
    PlayerHealth playerHealth;
    public int level = 0;
    public float rotationSpeed = 15f;
    int status = 0; //0 idle, -1 move left, 1 move right;
    public float damage = 1f;
    bool faceRight;
    private float tolerance = 0.1f;
    public Transform CrawlerBody;
    public Animator anim;
    int IdleOne;
    int IdleAlert;
    int Sleeps;
    int AngryReaction;
    int Hit;
    int AnkleBite;
    int CrochBite;
    int Dies;
    int HushLittleBaby;
    int Run;

    public CharacterController charControl;

    // Use this for initialization
    void Start()
    {
        charControl = GetComponent<CharacterController>();

        if (anim == null) Debug.Log("ANIMATOR NOT FOUND");
        IdleOne = Animator.StringToHash("IdleOne");
        IdleAlert = Animator.StringToHash("IdleAlert");
        Sleeps = Animator.StringToHash("Sleeps");
        AngryReaction = Animator.StringToHash("AngryReaction");
        Hit = Animator.StringToHash("Hit");
        AnkleBite = Animator.StringToHash("AnkleBite");
        CrochBite = Animator.StringToHash("CrochBite");
        Dies = Animator.StringToHash("Dies");
        HushLittleBaby = Animator.StringToHash("HushLittleBaby");
        Hit = Animator.StringToHash("Hit");
        Run = Animator.StringToHash("Run");

        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) Debug.Log("playerNotFound");
        else
        {
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
        if (playerScript.getLevel() == level)
        {
            CheckDistance();
            awake = distanceX <= detectionRadius;
        }

        if (awake)
        {
            float oldDist = distanceX;

            Vector3 position = transform.position;
            float angle = rotationSpeed * Time.deltaTime;
            Vector3 target = Quaternion.AngleAxis(angle, Vector3.up) * position;
            if (charControl.Move(target - position) != CollisionFlags.None)
            {
                Physics.SyncTransforms();
            }

            Vector3 enemyPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 playerPositionXZ = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
            float newDist = Vector3.Distance(enemyPositionXZ, playerPositionXZ);

            if (Mathf.Abs(oldDist) < Mathf.Abs(newDist))
            {
                position = transform.position;
                angle = rotationSpeed * Time.deltaTime;
                target = Quaternion.AngleAxis(-2*angle, Vector3.up) * position;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    Physics.SyncTransforms();
                }
            }

            //Face Player
            Vector3 directionPl = (Player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionPl.x, 0, directionPl.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            //Animation
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleOne"))
            {
                anim.SetBool(IdleAlert, false);
                anim.SetBool(IdleOne, false);
                anim.SetBool(Sleeps, false);
                anim.SetBool(AngryReaction, false);
                anim.SetBool(Hit, false);
                anim.SetBool(AnkleBite, false);
                anim.SetBool(CrochBite, false);
                anim.SetBool(Dies, false);
                anim.SetBool(HushLittleBaby, false);
                anim.SetBool(Run, true);
            }
        }
        
        else
        {
            if (status != 0)
            {
                Vector3 position = transform.position;
                float angle = rotationSpeed * Time.deltaTime;
                Vector3 target = Quaternion.AngleAxis(status*angle*0.7f, Vector3.up) * position;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    Physics.SyncTransforms();
                }

                if (status == -1) { transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, Vector3.zero - CrawlerBody.position)); }
                else { transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.zero - CrawlerBody.position, Vector3.up)); }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleOne"))
                {
                    anim.SetBool(IdleAlert, false);
                    anim.SetBool(IdleOne, false);
                    anim.SetBool(Sleeps, false);
                    anim.SetBool(AngryReaction, false);
                    anim.SetBool(Hit, false);
                    anim.SetBool(AnkleBite, false);
                    anim.SetBool(CrochBite, false);
                    anim.SetBool(Dies, false);
                    anim.SetBool(HushLittleBaby, false);
                    anim.SetBool(Run, true);
                }
            }
            else
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    anim.SetBool(IdleAlert, false);
                    anim.SetBool(IdleOne, true);
                    anim.SetBool(Sleeps, false);
                    anim.SetBool(AngryReaction, false);
                    anim.SetBool(Hit, false);
                    anim.SetBool(AnkleBite, false);
                    anim.SetBool(CrochBite, false);
                    anim.SetBool(Dies, false);
                    anim.SetBool(HushLittleBaby, false);
                    anim.SetBool(Run, false);
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

    private void randomizeIdle()
    {
        status = Random.Range(0, 3) - 1;
        float time = Random.Range(1.5f, 2.5f);
        Invoke("randomizeIdle", time);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            attack();
        }
        else if (collision.collider.CompareTag("Map"))
        {

            foreach (ContactPoint contact in collision.contacts)
            {
                // Check if the normal of the contact point is approximately horizontal
                if (Mathf.Abs(contact.normal.y) < tolerance) // 'tolerance' is a small value like 0.1
                {
                    Debug.Log("COLLISION WITH WALL");
                    status = status * -1;
                    break;
                }
            }
        }
    }
    private void attack()
    {
        if (playerHealth != null) playerHealth.TakeDamage(damage);
    }
}
