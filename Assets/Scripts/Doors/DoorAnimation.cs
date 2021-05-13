using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator anim;
    public bool HorizontalDoor = false;
    public bool DebugOpen = false;

    void Awake() //For some reason putting this on Start gives errors when returning already visited room  
    {
        anim = gameObject.GetComponent<Animator>();
        if (DebugOpen)
            OpenDoor();
    }

    public void OpenDoor()
    {
        if (HorizontalDoor) 
            anim.Play("Door_Horizontal");
        else
            anim.Play("Door_Vertical");
    }
}