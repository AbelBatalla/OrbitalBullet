using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    // Start is called before the first frame update
    bool poison;
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerHealth>().TakeDamageContinuous();
        }
    }
}
