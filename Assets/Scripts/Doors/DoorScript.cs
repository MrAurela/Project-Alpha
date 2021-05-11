using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator anim;
    public bool HorizontalDoor = false;
    public bool DebugOpen = false;

    void Start()
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