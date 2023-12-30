using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{

    public GameObject bullet;
    public float timer = 2f;
    private float timerCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     timerCounter += Time.deltaTime;
     if(timerCounter > timer) {
        Instantiate(bullet, transform.position, transform.rotation);
        timerCounter = 0f;
     }   
    }

}
