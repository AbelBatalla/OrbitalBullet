using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoIcon : MonoBehaviour
{
    public int ammoType = 0;
    InventoryController ammoController;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
        GameObject Player = GameObject.FindWithTag("Player");
        if (Player != null)
        {
            ammoController = Player.GetComponent<InventoryController>();
            ammoController.giveAmmo(ammoType);
        }
    }
}
