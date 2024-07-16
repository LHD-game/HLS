using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellcomeW : MonoBehaviour
{
    public Animator Animator;

    public void ClickWc()
    {
        Animator.SetTrigger("Login");
    }
}
