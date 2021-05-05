using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    // Tiles
    public Tile floorTile;
    public RuleTile wallTile; // Rule Tiles are from unity 2d extras, they implement the autotiling behaviour

    void Start()
    {
        // Get the tilemaps. The testing script is in their parent but that can't be assumed when generating the map
        var floorTilemap = GetComponentsInChildren<Tilemap>()[0];
        var wallTilemap = GetComponentsInChildren<Tilemap>()[1];

        // Setting a normal floortile at tilemap coordinates 0, 0
        floorTilemap.SetTile(new Vector3Int(0, 0, 0), floorTile);

        // Setting rule tiles. They behave the same as normal tiles, nothing special to worry about
        // NOTE: remember to use wallTilemap, it has collisions while floorTilemap doesn't.
        wallTilemap.SetTile(new Vector3Int(1, -1, 0), wallTile);
        wallTilemap.SetTile(new Vector3Int(2, -1, 0), wallTile);
        wallTilemap.SetTile(new Vector3Int(1, -2, 0), wallTile);
        wallTilemap.SetTile(new Vector3Int(2, -2, 0), wallTile);
    }
}
