using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque_Chancla : MonoBehaviour
{
    private Animator anim;
    private bool isThrowing = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isThrowing)
        {
            isThrowing = true;
            anim.SetBool("isThrowing", true);
            StartCoroutine(ResetThrow());
        }
    }

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(1.0f); // Ajusta seg�n la duraci�n de la animaci�n
        anim.SetBool("isThrowing", false);
        isThrowing = false;
    }
}
