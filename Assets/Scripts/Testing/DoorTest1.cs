using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest1 : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.Play("Door_Horizontal");
    }
}
