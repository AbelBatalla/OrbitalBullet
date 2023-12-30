using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class InventoryController : MonoBehaviour
{
    public GameObject[] weapons;
    private Gun[] GunScripts;
    private int[] ammo; //index is type, value is quantity
    public int[] ammoInitialQuantity;
    public WeaponSlot Slot1, Slot2;
    private Vector2Int inventoryWeapons;
    public int currentWeapon = 0; //Deletable, just used for debugging
    public bool slotWeapon = true; //weapon slot 1 or 2 in use
    private AudioSource audioSource;
    public AudioClip swapAudio, obtainAudio, failedToShootAudio, reloadAudio;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GunScripts = new Gun[weapons.Length];
        ammo = new int[ammoInitialQuantity.Length];
        Array.Copy(ammoInitialQuantity, ammo, ammoInitialQuantity.Length);
        inventoryWeapons.x = 0;
        inventoryWeapons.y = -1;
        currentWeapon = 0;
        slotWeapon = true;
        Slot1.activeToggle(true);
        Slot1.EquipWeapon(0);
        Slot2.activeToggle(false);
        EquipWeapon(0, true);
    }

    void Update()
    {
        //SWAP WEAPON SLOTS
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!slotWeapon)
            {
                slotWeapon = true;
                EquipWeapon(inventoryWeapons.x, false);
                if (currentWeapon != -1) playSwapAudio();
                Slot1.activeToggle(true);
                Slot2.activeToggle(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slotWeapon)
            {
                slotWeapon = false;
                EquipWeapon(inventoryWeapons.y, false);
                if (currentWeapon != -1) playSwapAudio();
                Slot2.activeToggle(true);
                Slot1.activeToggle(false);
            }
        }

        //RELOAD WEAPONS
        if (Input.GetKeyDown(KeyCode.M))
        {
            Array.Copy(ammoInitialQuantity, ammo, ammoInitialQuantity.Length);
            foreach (Gun gun in GunScripts) {
                if (gun != null)
                {
                    int type = gun.getAmmoType();
                    gun.setAmmo(ammo[type]);
                }
            }
            Slot1.setAmmo(GunScripts[inventoryWeapons.x].getAmmo());
            if (inventoryWeapons.y != -1) Slot2.setAmmo(GunScripts[inventoryWeapons.y].getAmmo());
            playReloadAudio();
        }
        //OBTAIN WEAPONS
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inventoryWeapons.x != 0 && inventoryWeapons.y != 0)
            {
                ObtainWeapon(0);
                playObtainAudio();
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (inventoryWeapons.x != 1 && inventoryWeapons.y != 1)
            {
                ObtainWeapon(1);
                playObtainAudio();
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (inventoryWeapons.x != 2 && inventoryWeapons.y != 2)
            {
                ObtainWeapon(2);
                playObtainAudio();
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (inventoryWeapons.x != 3 && inventoryWeapons.y != 3)
            {
                ObtainWeapon(3);
                playObtainAudio();
            }
        }
    }

    public void getWeapon(int weaponIndex)
    {
        if (weaponIndex < weapons.Length && inventoryWeapons.x != weaponIndex && inventoryWeapons.y != weaponIndex)
        {
            if(inventoryWeapons.y == -1)
            {
                slotWeapon = false;
                Slot2.activeToggle(true);
                Slot1.activeToggle(false);
            }
            playObtainAudio();
            ObtainWeapon(weaponIndex);
        }
    }

    void EquipWeapon(int weaponIndex, bool first)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].SetActive(i == weaponIndex);
            }
        }
        currentWeapon = weaponIndex;
        if (first) GunScripts[weaponIndex] = weapons[weaponIndex].GetComponent<Gun>();
        if (weaponIndex != -1)
        {
            int type = GunScripts[weaponIndex].getAmmoType();
            GunScripts[weaponIndex].setAmmo(ammo[type]);
            updateAmmo(ammo[type], GunScripts[weaponIndex].getAmmo(), type);
        }
    }

    void ObtainWeapon(int weaponIndex)
    {
        if (slotWeapon)
        {
            if(inventoryWeapons.x != -1) GunScripts[inventoryWeapons.x].dropHolder();
            inventoryWeapons.x = weaponIndex;
            Slot1.EquipWeapon(weaponIndex);
        }
        else
        {
            if (inventoryWeapons.y != -1) GunScripts[inventoryWeapons.y].dropHolder();
            inventoryWeapons.y = weaponIndex;
            Slot2.EquipWeapon(weaponIndex);
        }
        EquipWeapon(weaponIndex, true);
    }

    private void printStatus() {
        Debug.Log("Slots - 1: " + inventoryWeapons.x + "   2: " + inventoryWeapons.y + "\n" + "Slot: " + slotWeapon + "   Current Weapon: " + currentWeapon);
    }

    private void playSwapAudio() {
        if (audioSource != null && swapAudio != null)
        {
            audioSource.PlayOneShot(swapAudio);
        }
    }
    private void playObtainAudio() {
        if (audioSource != null && obtainAudio != null)
        {
            audioSource.PlayOneShot(obtainAudio);
        }
    }

    private void playReloadAudio() {
        if (audioSource != null && reloadAudio != null)
        {
            audioSource.PlayOneShot(reloadAudio);
        }
    }

    public void failedToShoot() {
        if (audioSource != null && failedToShootAudio != null)
        {
            audioSource.PlayOneShot(failedToShootAudio);
        }
    }
    public void updateAmmo(int realAmmoValue, int showAmmoValue, int type) {
        ammo[type] = realAmmoValue;

        if (slotWeapon)
        {
            Slot1.setAmmo(showAmmoValue);
            if (inventoryWeapons.y != -1)
            {
                if (weapons[inventoryWeapons.y].GetComponent<Gun>()?.getAmmoType() == type)
                {
                    Slot2.setAmmo(showAmmoValue);
                }
            }
        }
        else
        {
            Slot2.setAmmo(showAmmoValue);
            if (weapons[inventoryWeapons.x].GetComponent<Gun>()?.getAmmoType() == type)
            {
                Slot1.setAmmo(showAmmoValue);
            }
        }
    }



}
