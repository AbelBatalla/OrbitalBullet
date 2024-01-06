using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    public int itemCode = 0;
    private bool active;
    //public GameObject text;
    private uiSingleton singleton_ui;
    private GameObject text_object;

    void Start()
    {
        active = false;
        Invoke("activate", 1f);
        GameObject singletonObject = GameObject.Find("UISingleton");
        singleton_ui = singletonObject.GetComponent<uiSingleton>();
        text_object = singleton_ui.getObjectText();
       
    }

    private void setup(){
        text_object.SetActive(false);
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
                text_object.SetActive(true);
                if (Input.GetKey(KeyCode.R))
                {
                    text_object.SetActive(false);
                    other.gameObject.GetComponent<InventoryController>()?.getWeapon(itemCode);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text_object.SetActive(false);
        }
    }
}
