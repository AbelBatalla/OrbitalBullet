using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        if (cam == null)
        {
            GameObject mainCameraObj = GameObject.FindWithTag("MainCamera");
            if (mainCameraObj != null)
            {
                cam = mainCameraObj.transform;
            }
            else Debug.Log("Camera Not Found");
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
