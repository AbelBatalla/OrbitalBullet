using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{

    public int level;
    
    void Start()
    {
        level = 0;
    }

    public void addLevel()
    {
        level++;
    }

    public int getLevel() { return level; }
}
