using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBulletBoss : MonoBehaviour
{
    public Transform target;
    public float detectionRange = 20f; // Define el rango de detección

    void Update()
    {
        // Calcula la dirección hacia el objetivo
        Vector3 targetDirection = target.position - transform.position;

        // Si el objetivo está dentro del rango de detección
        if (targetDirection.magnitude <= detectionRange)
        {
            // Dibuja un rayo verde para visualización
            Debug.DrawRay(transform.position, targetDirection, Color.green);

            // Calcula la rotación hacia el objetivo
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Aplica la rotación suavizada (Slerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 6*Time.deltaTime);
        }
        // Puedes agregar un "else" aquí si quieres manejar otro comportamiento cuando el objetivo no está en el rango.
    }
}
