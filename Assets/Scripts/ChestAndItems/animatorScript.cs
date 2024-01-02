using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorScript : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void activa()
    {
        animator.enabled = true;
        Invoke("desactiva", 1.5f);
    }

    private void desactiva()
    {
        animator.enabled = false;
    }
}
