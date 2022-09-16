using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataPark : ScriptableObject
{
    private string title;
    private string author;
    private string data;
    
    /*
     * {
     *     "tiles": [
     *          "tileId": BLOCK,
     *          "x": 0,
     *          "y": 0,
     *          "elevation": 0
     *     ]
     * }
     */

    public string TilesToJson(List<Tilemap> tilemaps)
    {
        
        foreach (var tilemap in tilemaps)
        {
            
        }
        return null;
    }

    public List<Tilemap> JsonToTilemaps(string json)
    {
        return null;
    }
}
