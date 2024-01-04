using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    public GameObject[] levels;
    public GameObject[] floors;
    public int level;
    
    void Start()
    {
        level = 0;
        DeactivateLevelsAndFloors();
    }

    public void addLevel()
    {
        level++;
        DeactivateLevelsAndFloors();
    }

    public int getLevel() { return level; }

    private void DeactivateLevelsAndFloors()
    {
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i] != null)
            {
                floors[i].SetActive(i == level);
            }
        }
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] != null)
            {
                levels[i].SetActive(i == level || i == level+1);
            }
        }
    }
}
