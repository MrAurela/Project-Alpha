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
    private int seed;

    private int NumberOfDoors()
    {
        int doors = 0;
        if (HasDoorUp) doors += 1;
        if (HasDoorDown) doors += 1;
        if (HasDoorRight) doors += 1;
        if (HasDoorLeft) doors += 1;
        return doors;
    }

    public bool IsX() { return NumberOfDoors() == 4; }
    public bool IsTVertical() { return NumberOfDoors() == 3 && HasDoorRight && HasDoorRight; }
    public bool IsTHorizontal() { return NumberOfDoors() == 3 && HasDoorUp && HasDoorDown; }
    public bool IsL() { return NumberOfDoors() == 2 && !IsIHorizontal() && !IsIVertical(); }
    public bool IsIVertical() { return NumberOfDoors() == 2 && HasDoorDown && HasDoorUp; }
    public bool IsIHorizontal() { return NumberOfDoors() == 2 && HasDoorRight && HasDoorRight; }
    public bool IsDEVertical() { return NumberOfDoors() == 1 && (HasDoorDown || HasDoorUp); }
    public bool IsDEHorizontal() { return NumberOfDoors() == 1 && (HasDoorRight || HasDoorLeft); }

    public string ToString()
    {
        if (IsX()) return "X";
        else if (IsTVertical()) return "T vertical";
        else if (IsTHorizontal()) return "T horizontal";
        else if (IsL()) return "L";
        else if (IsIVertical()) return "I vertical";
        else if (IsIHorizontal()) return "I horizontal";
        else if (IsDEVertical()) return "DE vertical";
        else if (IsDEHorizontal()) return "DE horizontal";
        else return "Unknown room type! Up: "+HasDoorUp+", Right: "+HasDoorRight+", Down: "+HasDoorDown+", Left:"+HasDoorLeft;
    }
}

