using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class MovePlayer : MonoBehaviour
{
    public float rotationSpeed, jumpSpeed, gravity;
    
    Vector3 startDirection;
    float speedY;
    Animator anim;
    GameObject playerObject = null; 
    Vector3 lastPosition = new Vector3(0,0,0);
    bool recoil = false;
    bool dash = false;
    float recoilMax, recoilAccum, recoilSpeed;
    float dashMax, dashAccum, dashSpeed;
    public bool lookRight = true;


    // Start is called before the first frame update
    void Start()
    {
        // Store starting direction of the player with respect to the axis of the level
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.x = 95.0f;
        startDirection.Normalize();
        speedY = 0;

        playerObject = GameObject.Find("T-Pose_new");
        anim = playerObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position;
    
        // Left-right movement
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !recoil)
        {
            float angle;
            startDirection.x = -95.0f;
            Vector3 direction, target;
            anim.SetFloat("Blend", 0.5f, 0.1f, Time.deltaTime);
            position = transform.position;


            lastPosition = transform.position;
            angle = rotationSpeed * Time.deltaTime;
            if (recoil) position = lastPosition * 2;
            direction = position - transform.parent.position;

            if (Input.GetKey(KeyCode.A))
            {
                target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
                lookRight = false;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {

                    transform.position = position;
                    Physics.SyncTransforms();
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                startDirection.x = 95.0f;
                target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                lookRight = true;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    transform.position = position;
                    Physics.SyncTransforms();
                }
            }
        }
        else if(Input.GetKey(KeyCode.S)) {
             giveDash();
        }
        else
        {
            anim.SetFloat("Blend", 0.0f, 0.1f, Time.deltaTime);
            if (recoil) continueRecoil();
            else if(dash) continueDash();
        }
        

        // Correct orientation of player
        // Compute current direction
        Vector3 currentDirection = transform.position - transform.parent.position;
        currentDirection.y = 0.0f;
        currentDirection.Normalize();
        // Change orientation of player accordingly
        Quaternion orientation;
        if ((startDirection - currentDirection).magnitude < 1e-3)
            orientation = Quaternion.AngleAxis(0.0f, Vector3.up);
        else if ((startDirection + currentDirection).magnitude < 1e-3)
            orientation = Quaternion.AngleAxis(180.0f, Vector3.up);
        else
            orientation = Quaternion.FromToRotation(startDirection, currentDirection);
        transform.rotation = orientation;

        // Apply up-down movement
        position = transform.position;
        if (charControl.Move(speedY * Time.deltaTime * Vector3.up) != CollisionFlags.None)
        {
            transform.position = position;
            Physics.SyncTransforms();
        }
        if (charControl.isGrounded)
        {
            if (speedY < 0.0f) {
                anim.SetBool("Jump",false);
                anim.SetBool("Grounded", true);
                speedY = 0.0f;
            }


            if (Input.GetKey(KeyCode.W)) {
                speedY = jumpSpeed;
                anim.SetBool("Jump",true);
                anim.SetBool("Grounded", false);
            }
                
        }
        else
            speedY -= gravity * Time.deltaTime;
    }

    public void giveRecoil(float recoilMaxValue, float recoilSpeedValue)
    {
        recoil = true;
        recoilMax = recoilMaxValue;
        recoilSpeed = recoilSpeedValue;
        recoilAccum = 0f;
        speedY = 0.0f;
        //continueRecoil();
    }

    void continueRecoil()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position, direction, target;
        float rotationAmount = recoilSpeed * Time.deltaTime;
        recoilAccum += rotationAmount;
        if (recoilAccum >= recoilMax)
        {
            recoil = false;
        }
        position = transform.position;
        direction = position - transform.parent.position;

        float dir_x = startDirection.x;
        if (dir_x == -95.0f) target = transform.parent.position + Quaternion.AngleAxis(-rotationAmount, Vector3.up) * direction;
        else if (dir_x == 95.0f) target = transform.parent.position + Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;
        else target = transform.parent.position + Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;

        if (charControl.Move(target - position) != CollisionFlags.None)
        {

            Debug.Log(position);
            transform.position = position;
            Physics.SyncTransforms();


        }
        anim.SetBool("Shoot", false);
    }

    void giveDash() {
        dash = true;
        dashMax = 40.0f;
        dashSpeed = 70.0f;
        dashAccum = 0f;
        speedY = 0.0f;
        anim.SetBool("Slide",true);
    }

    void continueDash()
    {   
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position, direction, target;
        float rotationAmount = dashSpeed * Time.deltaTime;
        dashAccum += rotationAmount;
        if (dashAccum >= dashMax)
        {
            dash = false;
            anim.SetBool("Slide", false);
        }
        
        position = transform.position;
        direction = position - transform.parent.position;

        float dir_x = startDirection.x;
        if (dir_x == -95.0f) target = transform.parent.position + Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;
        else if (dir_x == 95.0f) target = transform.parent.position + Quaternion.AngleAxis(-rotationAmount, Vector3.up) * direction;
        else target = transform.parent.position + Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;

        if (charControl.Move(target - position) != CollisionFlags.None)
        {

            Debug.Log(position);
            transform.position = position;
            Physics.SyncTransforms();


        }
        
    }

    /*
    public void giveRecoil(float recoilMaxValue, float recoilSpeedValue){
       
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position, direction, target;
        float angle;
        position = transform.position;
        angle = rotationSpeed * Time.deltaTime;
        direction = position - transform.parent.position;

        float dir_x = startDirection.x;
        if(dir_x == -95.0f) target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
        else if(dir_x == 95.0f) target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
        else target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
    
        Vector3 recoilPosition = transform.position - transform.forward * recoilMaxValue;
        anim.SetBool("Shoot", false);
        anim.SetBool("Shoot", true);
        // Mueve suavemente al jugador hacia atrás
        transform.position = Vector3.Lerp(transform.position, recoilPosition, recoilSpeedValue * Time.deltaTime);
        if (charControl.Move(target - position) != CollisionFlags.None)
        {
            
            Debug.Log(position);
            transform.position = position;
            Physics.SyncTransforms();
           
            
        }
         anim.SetBool("Shoot", false);
    }

    */
  

   
}


