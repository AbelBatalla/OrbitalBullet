using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación del mapa
    bool recoiling = false;
    public float recoilMax, recoilAccum, recoilSpeed;
    bool recoilDirection;

    // Update is called once per frame
    void Update()
    {
        // Obtener la entrada horizontal (teclas laterales)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Rotar el mapa en respuesta a la entrada horizontal
        if (!recoiling) RotateMap(horizontalInput);
        else continueRecoil();
    }

    void RotateMap(float horizontalInput)
    {
        // Calcular el ángulo de rotación basado en la entrada horizontal
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        // Aplicar la rotación al mapa alrededor del eje vertical (Y)
        transform.Rotate(Vector3.up, rotationAmount);
    }

    public void giveRecoil(float recoilMaxValue, float recoilSpeedValue, bool recoilDirectionValue)
    {
        recoiling = true;
        recoilMax = recoilMaxValue;
        recoilDirection = recoilDirectionValue;
        recoilSpeed = (recoilDirection ? -1 : 1) * recoilSpeedValue;
        recoilAccum = 0f;
    }

    void continueRecoil()
    {
        float rotationAmount = recoilSpeed * Time.deltaTime;
        recoilAccum += rotationAmount;

        transform.Rotate(Vector3.up, rotationAmount);

        if (Math.Abs(recoilAccum) >= recoilMax)
        {
            recoiling = false;
        }
    }
}

