using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject PausePannel;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(){
        PausePannel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue(){
        PausePannel.SetActive(false);
        Time.timeScale = 1;
    }
}
