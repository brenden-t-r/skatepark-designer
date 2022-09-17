using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParkDataSaves : ScriptableObject
{
    public static ParkData parkData;
    
    public static Dictionary<string, List<ParkData.SavedTile>> TilesToDict(List<Tilemap> tilemaps)
    {
        var dictionary = new Dictionary<string, List<ParkData.SavedTile>>();
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
            dictionary.Add(tilemap.name, tiles);
        }
        return dictionary;
    }
    
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
    
}