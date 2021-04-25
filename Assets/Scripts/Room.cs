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
    public bool IsCharacter1Start { get; set; }
    public bool IsCharacter2Start { get; set; }
    public bool IsBossRoom { get; set; }

    public void SetDoors(float probability = 0.5f)
    {
        SetDoors(probability, probability, probability, probability);
    }

    public void SetDoors(float up = 0.5f, float right = 0.5f, float down = 0.5f, float left = 0.5f)
    {
        HasDoorUp = Random.value < up;
        HasDoorDown = Random.value < down;
        HasDoorRight = Random.value < right;
        HasDoorLeft = Random.value < left;
    }

}

