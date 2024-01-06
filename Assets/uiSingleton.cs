using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiSingleton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject text_object;
    public GameObject text_kill_enemies;

    // Update is called once per frame
    public GameObject getObjectText(){
        return text_object;
    }

     public GameObject getChestEnemyText(){
        return text_kill_enemies;
    }
}
