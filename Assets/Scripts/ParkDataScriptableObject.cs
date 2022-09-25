using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParkData : ScriptableObject
{
    public string title;
    public string author;
    public List<SavedTileMap> maps = new List<SavedTileMap>();

    [Serializable]
    public class SavedTile {
        public Vector3Int Position;
        public GameTile.TileTypes TileType;
    }

    [Serializable]
    public class SavedTileMap
    {
        public string name;
        public List<SavedTile> tiles;
    }
}
