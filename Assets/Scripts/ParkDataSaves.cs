using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParkDataSaves : ScriptableObject
{
    public static ParkData parkData = CreateInstance<ParkData>();

    public static List<ParkData.SavedTileMap> TilesToList(List<Tilemap> tilemaps)
    {
        var list = new List<ParkData.SavedTileMap>();
        foreach (var tilemap in tilemaps)
        {
            var tiles = new List<ParkData.SavedTile>();
            foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
                if (tilemap.HasTile(pos))
                {
                    tiles.Add(new ParkData.SavedTile{
                        Position = pos,
                        Tile = tilemap.GetTile<Tile>(pos)
                    });
                }
            }
            ParkData.SavedTileMap map = new ParkData.SavedTileMap();
            map.name = tilemap.name;
            map.tiles = tiles;
            list.Add(map);
        }
        return list;
    }
    
    [System.Serializable]
    public class ListWrapper<T>
    {
        public List<T> list;
        public ListWrapper(List<T> list) => this.list = list;
        public ListWrapper() => this.list = new List<T>();
    }
    
}