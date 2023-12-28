using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject[] weapons;
    public WeaponSlot Slot1, Slot2;
    private Vector2Int inventoryWeapons;
    public int currentWeapon = 0;
    public bool slotWeapon = true; //weapon slot 1 or 2 in use

    void Start()
    {
        inventoryWeapons.x = 0;
        inventoryWeapons.y = -1;
        currentWeapon = 0;
        slotWeapon = true;
        Slot1.activeToggle(true);
        Slot1.EquipWeapon(0);
        Slot2.activeToggle(false);
        EquipWeapon(0);
    }

    void Update()
    {
        //SWAP WEAPON SLOTS
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!slotWeapon)
            {
                EquipWeapon(inventoryWeapons.x);
                Slot1.activeToggle(true);
                Slot2.activeToggle(false);
            }
            slotWeapon = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slotWeapon)
            {
                EquipWeapon(inventoryWeapons.y);
                Slot2.activeToggle(true);
                Slot1.activeToggle(false);
            }
            slotWeapon = false;
            
        }

        //OBTAIN WEAPONS
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inventoryWeapons.x != 0 && inventoryWeapons.y != 0)
            {
                ObtainWeapon(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (inventoryWeapons.x != 1 && inventoryWeapons.y != 1)
            {
                ObtainWeapon(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (inventoryWeapons.x != 2 && inventoryWeapons.y != 2)
            {
                ObtainWeapon(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (inventoryWeapons.x != 3 && inventoryWeapons.y != 3)
            {
                ObtainWeapon(3);
            }
        }
    }

    void EquipWeapon(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].SetActive(i == weaponIndex);
            }
        }
        currentWeapon = weaponIndex;
        printStatus();
    }

    void ObtainWeapon(int weaponIndex)
    {
        if (slotWeapon)
        { 
            inventoryWeapons.x = weaponIndex;
            Slot1.EquipWeapon(weaponIndex);
        }
        else
        {
            inventoryWeapons.y = weaponIndex;
            Slot2.EquipWeapon(weaponIndex);
        }
        EquipWeapon(weaponIndex);
    }

    private void printStatus() {
        Debug.Log("Slots - 1: " + inventoryWeapons.x + "   2: " + inventoryWeapons.y + "\n" + "Slot: " + slotWeapon + "   Current Weapon: " + currentWeapon);
    }
}
