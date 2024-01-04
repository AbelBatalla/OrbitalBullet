using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    public int itemCode = 0;
    private bool active;

    void Start()
    {
        active = false;
        Invoke("activate", 1f);
    }

    private void activate()
    {
        active = true;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (active)
        {
            if (other.gameObject.tag == "Player")
            {
                if (Input.GetKey(KeyCode.R))
                {
                    other.gameObject.GetComponent<InventoryController>()?.getWeapon(itemCode);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
