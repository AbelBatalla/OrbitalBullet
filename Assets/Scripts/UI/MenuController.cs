using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(string scene_name){
        SceneManager.LoadScene(scene_name);
    }
}
