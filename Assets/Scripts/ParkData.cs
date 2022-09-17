using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class ParkData : ScriptableObject
{
    public string title;
    public string author;
    //public Dictionary<string, List<SavedTile>> data = new Dictionary<string, List<SavedTile>>();
    public List<SavedTileMap> maps = new List<SavedTileMap>();

    [Serializable]
    public class SavedTile {
        public Vector3Int Position;
        public Tile Tile;
    }

    [Serializable]
    public class SavedTileMap
    {
        public string name;
        public List<SavedTile> tiles;
    }
}


