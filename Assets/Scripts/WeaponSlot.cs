using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    private bool active;

    public GameObject[] weaponModels;
    public GameObject selector;
    public int currentWeapon;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EquipWeapon(int weaponIndex)
    {
        Debug.Log("RECIEVED");
        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i] != null)
            {
                weaponModels[i].SetActive(i == weaponIndex);
            }
        }
        currentWeapon = weaponIndex;
    }

    public void activeToggle(bool activeSlot) {
        active = activeSlot;
        selector.SetActive(activeSlot);
    }
}
