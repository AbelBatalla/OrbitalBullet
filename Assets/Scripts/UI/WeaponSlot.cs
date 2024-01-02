using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    private bool active;

    public GameObject[] weaponModels;
    public GameObject selector;
    public TextMeshProUGUI ammoDisplay;
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
        if (ammoDisplay != null)
        {
            ToggleBold(activeSlot);
        }
    }

    public void setAmmo(int ammo) { 
        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(ammo.ToString());
        }
    }

    private void ToggleBold(bool bold)
    {

        if (bold)
        {
            // Apply bold formatting
            ammoDisplay.text = "<b>" + ammoDisplay.text + "</b>";
        }
        else
        {
            // Remove bold formatting
            ammoDisplay.text = ammoDisplay.text.Replace("<b>", "").Replace("</b>", "");
        }
    }
}
