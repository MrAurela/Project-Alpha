using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room: MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float unitSize;
    [SerializeField] int doorSize;
    [SerializeField] GameObject origin;
    [SerializeField] GameObject wall;

    private bool doorUp;
    private bool doorDown;
    private bool doorRight;
    private bool doorLeft;

    public void SelectDoors()
    {
        doorUp = Random.value > 0.5f;
        doorDown = Random.value > 0.5f;
        doorRight = Random.value > 0.5f;
        doorLeft = Random.value > 0.5f;
    }

    public Vector2 GetSize()
    {
        return new Vector2(width * unitSize, height * unitSize);
    }
    
    public bool hasDoorUp()
    {
        return doorUp;
    }

    public bool hasDoorDown()
    {
        return doorDown;
    }

    public bool hasDoorRight()
    {
        return doorRight;
    }

    public bool hasDoorLeft()
    {
        return doorLeft;
    }

}
