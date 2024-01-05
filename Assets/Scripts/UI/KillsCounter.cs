using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KillsCounter : MonoBehaviour
{
    private int souls = 0;
    public TextMeshProUGUI text;
    void Start()
    {
        text.text = souls.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) addKill();
    }

    public int getSouls() { return souls; }

    public void consumeSoul() { 
        --souls;
        text.text = souls.ToString();
    }

    public void addKill() { 
        ++souls;
        text.text = souls.ToString();
    }
    
}
