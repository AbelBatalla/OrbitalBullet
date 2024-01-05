using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CrawlerController : MonoBehaviour
{
    public GameObject Player;
    public float detectionRadius = 30f;
    bool awake = true;
    float distanceX;
    float distanceY;
    float levelDifference = 5f;
    LevelCounter playerScript;
    PlayerHealth playerHealth;
    public int level = 0;
    public float rotationSpeed = 15f;
    int status = 0; //0 idle, -1 move left, 1 move right;
    public float damage = 10f;
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
    public float verticalAttackRange = 2f;
    public float attackCooldown = 1.5f;
    private float timeSinceLastAttack = 0f;
    private bool canAttack = true;
    float frontierMoveOrStay = 2f;
    public CharacterController charControl;
    Vector3 vel;
    private bool alive = true;
    public AudioClip deathAudio;
    public AudioClip hitAudio;
    public AudioClip attackAudio;
    private AudioSource audioPlayer;

    // Use this for initialization
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        audioPlayer = GetComponent<AudioSource>();

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
         vel = new Vector3(0, 0, 0);
        randomizeIdle();
    }

    // Update is called once per frame

    void Update()
    {
        if (alive)
        {
            if (!canAttack)
            {
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack >= attackCooldown)
                {
                    canAttack = true;
                    timeSinceLastAttack = 0f;
                }
            }
            CheckDistance();
            awake = distanceX <= detectionRadius;

            if (awake)
            {
                if (Mathf.Abs(distanceX) > frontierMoveOrStay)
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
                        target = Quaternion.AngleAxis(-2 * angle, Vector3.up) * position;
                        if (charControl.Move(target - position) != CollisionFlags.None)
                        {
                            Physics.SyncTransforms();
                        }
                    }
                }
                else
                {
                    if (canAttack && distanceY <= verticalAttackRange)
                    {
                        attack();
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

                // Apply gravity
                if (charControl.isGrounded)
                {
                    vel.y = 0f; // Reset gravity effect when on the ground
                }
                else
                {
                    vel.y -= 20f * Time.deltaTime; // Apply gravity over time
                }

                // Move the character controller with the gravity effect
                charControl.Move(vel * Time.deltaTime);

            }

            else
            {
                // Apply gravity
                if (charControl.isGrounded)
                {
                    vel.y = 0f; // Reset gravity effect when on the ground
                }
                else
                {
                    vel.y -= 20f * Time.deltaTime; // Apply gravity over time
                }

                // Move the character controller with the gravity effect
                charControl.Move(vel * Time.deltaTime);
                if (status != 0)
                {
                    Vector3 position = transform.position;
                    float angle = rotationSpeed * Time.deltaTime;
                    Vector3 target = Quaternion.AngleAxis(status * angle * 0.7f, Vector3.up) * position;
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
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleOne"))
            {
                anim.SetBool(IdleAlert, false);
                anim.SetBool(IdleOne, false);
                anim.SetBool(Sleeps, false);
                anim.SetBool(AngryReaction, false);
                anim.SetBool(Hit, false);
                anim.SetBool(AnkleBite, false);
                anim.SetBool(CrochBite, false);
                anim.SetBool(Dies, true);
                anim.SetBool(HushLittleBaby, false);
                anim.SetBool(Run, false);
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

    private void randomizeIdle()
    {
        status = Random.Range(0, 3) - 1;
        float time = Random.Range(1.5f, 2.5f);
        Invoke("randomizeIdle", time);
    }

    private void attack()
    {
        if (playerHealth != null) playerHealth.TakeDamage(damage);
        canAttack = false;
        audioPlayer.PlayOneShot(attackAudio);
    }

    public void death()
    {
        alive = false;
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
        charControl.enabled = false;
        audioPlayer.PlayOneShot(deathAudio);
        Destroy(gameObject, 2f);
    }
    public void hit()
    {
        audioPlayer.PlayOneShot(hitAudio);
    }
}
