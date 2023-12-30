using UnityEngine;

public class GrenadeRotate : MonoBehaviour
{
    public float rotationSpeedMin = 800f;
    public float rotationSpeedMax = 1500f;
    private float rotationSpeed;
    private Vector3 randomRotation;

    void Start()
    {
        rotationSpeed = Random.Range(rotationSpeedMin,rotationSpeedMax);
        randomRotation = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void Update()
    {
        transform.Rotate(randomRotation, rotationSpeed * Time.deltaTime);
    }
}
