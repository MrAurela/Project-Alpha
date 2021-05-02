using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    //Does the room has connection to another room in certain direction:
    public bool HasDoorUp { get; set; }
    public bool HasDoorDown { get; set; }
    public bool HasDoorRight { get; set; }
    public bool HasDoorLeft { get; set; }

    //Speacial rooms
    public bool IsStartRoom{ get; set; }
    public bool IsBossRoom { get; set; }
    public bool IsSafeRoom { get; set; }

    //Room states. Enemies are not spawned when entering to a cleared room
    //public bool IsCleared { get; set; }
    private int roomsCleared = 0;

    public void SetRoomCleared(int addition = 1)
    {
        roomsCleared += addition;
    }

    public bool IsCleared()
    {
        return roomsCleared >= 2;
    }

    //Room coordinates at world level. These are the values that this room can be fetched with GetRoom() from GenerateLevel.
    public int x { get; set; }
    public int y { get; set; }

    //Room seed. This value should be set only once and then used as seed every time this room is load to prevent room from generating again
    public int Seed { get; set; }
    

    //Returns the number of doors the room has.
    private int NumberOfDoors()
    {
        int doors = 0;
        if (HasDoorUp) doors += 1;
        if (HasDoorDown) doors += 1;
        if (HasDoorRight) doors += 1;
        if (HasDoorLeft) doors += 1;
        return doors;
    }

    //Boolean functions describing the shape of the room.
    //Used to select from appropriate room layouts when randomizing the rooms in GenerateLevel.
    public bool IsX() { return NumberOfDoors() == 4; }
    public bool IsTVertical() { return NumberOfDoors() == 3 && HasDoorRight && HasDoorRight; }
    public bool IsTHorizontal() { return NumberOfDoors() == 3 && HasDoorUp && HasDoorDown; }
    public bool IsL() { return NumberOfDoors() == 2 && !IsIHorizontal() && !IsIVertical(); }
    public bool IsIVertical() { return NumberOfDoors() == 2 && HasDoorDown && HasDoorUp; }
    public bool IsIHorizontal() { return NumberOfDoors() == 2 && HasDoorRight && HasDoorRight; }
    public bool IsDEVertical() { return NumberOfDoors() == 1 && (HasDoorDown || HasDoorUp); }
    public bool IsDEHorizontal() { return NumberOfDoors() == 1 && (HasDoorRight || HasDoorLeft); }


    //Textual representation of the room. Used for debugging purposes.
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

