using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación del mapa

    // Update is called once per frame
    void Update()
    {
        // Obtener la entrada horizontal (teclas laterales)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Rotar el mapa en respuesta a la entrada horizontal
        RotateMap(horizontalInput);
    }

    void RotateMap(float horizontalInput)
    {
        // Calcular el ángulo de rotación basado en la entrada horizontal
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        // Aplicar la rotación al mapa alrededor del eje vertical (Y)
        transform.Rotate(Vector3.up, rotationAmount);
    }
}

