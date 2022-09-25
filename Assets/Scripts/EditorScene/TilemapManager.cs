using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace EditorScene
{
    public class TilemapManager : MonoBehaviour
    {
        public static TilemapManager _instance;
        private static readonly string PLAYER_PREFS_PARKS = "Parks";

        [SerializeField] public Grid grid;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tilemap tilemapL1;
        [SerializeField] private Tilemap tilemapL2;
        [SerializeField] private Tilemap tilemapL3;
        [SerializeField] private Tilemap tilemapExtras;
        [SerializeField] private List<GameTile> gameTiles;
        private List<Tilemap> tilemaps;

        private void Start()
        {
            _instance = this;
            tilemaps = new List<Tilemap>
                { tilemap, tilemapL1, tilemapL2, tilemapL3 };
        }

        public Tilemap GetTilemapFromElevation(int elevation)
        {
            switch (elevation)
            {
                case 0:
                    return tilemap;
                case 1:
                    return tilemapL1;
                case 2:
                    return tilemapL2;
                case 3:
                    return tilemapL3;
                default:
                    Debug.LogError("Invalid elevation");
                    return null;
            }
        }

        public Tilemap GetTilemapExtras()
        {
            return tilemapExtras;
        }
        
        public void SaveMap()
        {
            // Save park
            ParkData parkData = ParkDataSaves.parkData;
            parkData.maps = ParkDataSaves.TilesToList(tilemaps);
            string json = JsonUtility.ToJson(parkData);
            PlayerPrefs.SetString(parkData.title, json);
        
            // Update Parks list
            string parksJson = PlayerPrefs.GetString(PLAYER_PREFS_PARKS);
            ParkDataSaves.ListWrapper<string> parks = parksJson != "" 
                ? JsonUtility.FromJson<ParkDataSaves.ListWrapper<string>>(parksJson) 
                : new ParkDataSaves.ListWrapper<string>();
            if (!parks.list.Contains(parkData.title))
            {
                parks.list.Add(parkData.title);
            }
            parksJson = JsonUtility.ToJson(parks);
            PlayerPrefs.SetString(PLAYER_PREFS_PARKS, parksJson);
            PlayerPrefs.Save();
        }

        public void LoadMap()
        {
            foreach (var map in ParkDataSaves.parkData.maps)
            {
                int index = tilemaps.FindIndex((x) => x.name.Equals(map.name));
                if (index == -1) continue;
                Tilemap mapToPopulate = tilemaps[index];
                foreach (var tile in map.tiles)
                {
                    int tileIndex = gameTiles.FindIndex(x => x.type.Equals(tile.Tile.type));
                    if (tileIndex == -1) continue;
                    GameTile tileToPlace = gameTiles[tileIndex];
                    Debug.Log("setTile, " + tile.Position.x + " , " +  tile.Position.y);
                    mapToPopulate.SetTile(tile.Position, tileToPlace);
                }
                mapToPopulate.RefreshAllTiles();
            }
        }
        
        public void ClearMap() {
            var maps = FindObjectsOfType<Tilemap>();
            foreach (var map in maps) {
                map.ClearAllTiles();
            }
        }
    }
}