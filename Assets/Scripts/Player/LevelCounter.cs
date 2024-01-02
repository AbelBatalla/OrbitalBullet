using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{

    public int level;
    
    void Start()
    {
        level = 0;
        Debug.Log("Current Level: " + level);
    }

    public void addLevel()
    {
        level++;
        Debug.Log("Current Level: " + level);
    }

    public int getLevel() { return level; }
}
