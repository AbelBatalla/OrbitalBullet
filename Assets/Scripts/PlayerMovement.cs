using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody physics;
    public float range = 3;
    public float JumpForce = 6.0f;
    public bool collisionEnv = false;

    //public Transform cylinderCenter; // Centro del cilindro
    public float cylinderRadius = 10f; // Radio del cilindro
    Animator anim;
    GameObject playerObject = null; 

    void Start()
    {
        physics = GetComponent<Rigidbody>();
        playerObject = GameObject.Find("T-Pose");
        anim = playerObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {   

        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(transform.position, transform.TransformDirection(direction*range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction*range));

        if(Physics.Raycast(theRay, out RaycastHit hit, range)){
            if(hit.collider.tag == "env") collisionEnv = true;
            else collisionEnv = false;
        }
        else collisionEnv = false;
         //print(collisionEnv);

        if (Input.GetKeyDown(KeyCode.Space))
        {   
            anim.SetFloat("Blend", 1.0f);
            physics.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            
        }
        print(IsGrounded());
    }


    bool IsGrounded()
    {
        return GetComponent<Rigidbody>().velocity.y == 0;
    }

    public bool GetCollisionEnv() {
        return collisionEnv;
    }
}
