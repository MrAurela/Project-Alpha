using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    //Has to be public so Instantiation copies these properties?
    public bool HasDoorUp { get; set; }
    public bool HasDoorDown { get; set; }
    public bool HasDoorRight { get; set; }
    public bool HasDoorLeft { get; set; }
    public bool IsStartRoom{ get; set; }
    public bool IsBossRoom { get; set; }



}

