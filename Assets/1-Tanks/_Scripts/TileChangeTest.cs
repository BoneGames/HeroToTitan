using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChangeTest : MonoBehaviour
{
    public List<Vector3Int> blastRadius;
    public Tile test;
    Tilemap tm;
    public Tile[] up = new Tile[9];
    public Tile[] down = new Tile[9];
    public Tile[] right = new Tile[9];
    public Tile[] leftt = new Tile[9];

    private void Start()
    {
        tm = GameObject.FindObjectOfType<Tilemap>();
    }

    // NOTE: Attached to Bullet
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("hey");
        Tilemap tilemap = col.GetComponentInChildren<Tilemap>();
        
        // Is a tilemap attached to the thing we hit?
        if (tilemap)
        {
            
            // Convert bullet's position to tilemap's cell
            Vector3Int hitPos = tilemap.WorldToCell(transform.position);
            
            GetTiles(hitPos, tilemap);
            //Debug.Log("pPos:" + hitPos);
            // If there is a tile at that position
            if (tilemap.GetTile(hitPos) != null)
            {
                // Remove that tile
                //tilemap.SetTile(hitPos, null);
                SetNewTiles(direction: up);
            }
            //tm.SetTile(hitPos, null);
        }
        
    }

    void GetTiles(Vector3Int hitPos, Tilemap tilemap)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector3Int point = new Vector3Int(hitPos.x - 1 + x, hitPos.y - 1 + y, 0);
                blastRadius.Add(point);
            }
        }
    }

    void SetNewTiles(Tile[] direction)
    {
        
        int counter = 0;
        foreach(Vector3Int pos in blastRadius)
        {
            if(counter == 0 || counter == 4 || counter == 6)
            {
                tm.SetTile(pos, null);
            }
            
            if (direction[counter] != null)
            {
                if(tm.GetTile(pos) != null)
                {
                     tm.SetTile(pos, up[counter]);
                }
               
            }
            counter++;
        }
    }
}
