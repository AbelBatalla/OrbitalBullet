using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación del mapa
    private PlayerMovement playerScript;
    Animator anim;
    GameObject playerObject = null; 
    float rotationOld = 0;
    // Update is called once per frameç

    void Start()
    {
        playerObject = GameObject.Find("T-Pose");
        anim = playerObject.GetComponentInChildren<Animator>();
        playerScript = playerObject.GetComponentInChildren<PlayerMovement>();
    }

    void Update()
    {
        // Obtener la entrada horizontal (teclas laterales)
        float horizontalInput = Input.GetAxis("Horizontal");
        bool playerHit = playerScript.GetCollisionEnv();
        // Rotar el mapa en respuesta a la entrada horizontal
        if(!playerHit) RotateMap(horizontalInput);
       
    }

    void RotateMap(float horizontalInput)
    {
        // Calcular el ángulo de rotación basado en la entrada horizontal
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        
        //Aplicar la rotación al mapa alrededor del eje vertical (Y)
        transform.Rotate(Vector3.up, rotationAmount);
        
        if(rotationAmount - rotationOld != 0){
        anim.SetFloat("Blend", 0.5f, 0.15f, Time.deltaTime);
        rotationOld = rotationAmount;
        } else anim.SetFloat("Blend", 0.0f, 0.15f, Time.deltaTime);
        
    }

    bool IsGrounded()
    {
        return playerObject.GetComponentInChildren<Rigidbody>().velocity.y == 0;
    }



}

