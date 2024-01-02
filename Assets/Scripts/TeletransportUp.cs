using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeletransportUp : MonoBehaviour
{
    public Transform Target;
    public GameObject Player;
    public GameObject text;
    private bool tp_active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true);
            tp_active = true;
            Debug.Log("Active");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
            tp_active = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && tp_active)
        {
            Player.transform.position = Target.transform.position;
            if (Mathf.Abs(transform.position.y - Target.transform.position.y) > 5f) Player.GetComponent<LevelCounter>()?.addLevel();
            Physics.SyncTransforms();
            text.SetActive(false);
        }
    }
}
