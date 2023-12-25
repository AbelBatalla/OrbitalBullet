using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody Physics;
    public float Speed = 1.0f;
    public float JumpForce = 1.0f;

    //public Transform cylinderCenter; // Centro del cilindro
    public float cylinderRadius = 10f; // Radio del cilindro

    void Start()
    {
        Physics = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        // Obtener posición relativa del jugador con respecto al cilindro
        //Vector3 relativePosition = transform.position - cylinderCenter.position;

        // Convertir a coordenadas cilíndricas
        //float theta = Mathf.Atan2(relativePosition.z, relativePosition.x);
        //float radius = relativePosition.magnitude;

        // Actualizar el ángulo y el radio
        //theta += h * Time.deltaTime * Speed;
        //radius = Mathf.Clamp(radius + v * Time.deltaTime * Speed, 0f, cylinderRadius);

        // Actualizar la posición del jugador en coordenadas cartesianas
        //Vector3 newPosition = new Vector3(radius * Mathf.Cos(theta), relativePosition.y, radius * Mathf.Sin(theta)) + cylinderCenter.position;

        // Establecer la nueva posición del jugador
        //transform.position = newPosition;
        //Physics.MovePosition(newPosition);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Physics.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
        }
    }
}
