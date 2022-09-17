using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Build : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap tilemapL1;
    [SerializeField] private Tilemap tilemapL2;
    [SerializeField] private Tilemap tilemapL3;
    [SerializeField] private Tilemap tilemapExtras;
    private List<Tilemap> tilemaps;
    private Tilemap activeTilemap;
    private Renderer tilemapExtrasRenderer;
    private Tile highlightedTile;
    private Vector3Int highlightedTilePos;

    private void Start()
    {
        tilemapExtrasRenderer = tilemapExtras.GetComponent<Renderer>();
        activeTilemap = tilemap;
        tilemaps = new List<Tilemap>
            { tilemap, tilemapL1, tilemapL2, tilemapL3 };
        ClearMap();
        LoadMap();
    }

    private void Update()
    {
        Vector3Int mousePos = GetMousePosition();
        Vector3Int hoverPos = new Vector3Int(mousePos.x, mousePos.y, 0);
        HoverHighlight(hoverPos);
    }

    private Vector3Int GetMousePosition () {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return grid.WorldToCell(mouseWorldPos);
    }

    private void ClickEvent(Vector3Int gridPos)
    {
        SetActiveTilemap();
        activeTilemap.SetTile(gridPos, BuildSettings.selectedPiece);
    }
    
    private void RightClickEvent(Vector3Int gridPos)
    {
        SetActiveTilemap();
        activeTilemap.SetTile(gridPos, null);
    }

    private void SetActiveTilemap()
    {
        switch (BuildSettings.elevation)
        {
            case 0:
                activeTilemap = tilemap;
                break;
            case 1:
                activeTilemap = tilemapL1;
                break;
            case 2:
                activeTilemap = tilemapL2;
                break;
            case 3:
                activeTilemap = tilemapL3;
                break;
            default:
                Debug.LogError("Invalid elevation");
                break;
        }
    }

    public void OnPointerClick (PointerEventData eventData) {
        Vector3Int mousePos = GetMousePosition();
        Vector3Int gridPos = new Vector3Int(mousePos.x, mousePos.y, 0);

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                ClickEvent(gridPos);
                break;
            case PointerEventData.InputButton.Right:
                RightClickEvent(gridPos);
                break;
            case PointerEventData.InputButton.Middle:
            default:
                return;
        }
    }
    
    private void HoverHighlight(Vector3Int hoverPos)
    {
        if (hoverPos == highlightedTilePos) return;
        tilemapExtrasRenderer.sortingOrder = BuildSettings.elevation;
        if (highlightedTile != null)
        {
            ResetHoverHighlight();
        }
        highlightedTilePos = hoverPos;
        highlightedTile = BuildSettings.selectedPiece;
        tilemapExtras.SetTile(hoverPos, highlightedTile);
    }

    private void ResetHoverHighlight()
    {
        tilemapExtras.SetTile(highlightedTilePos, null);
    }

    public void SaveMap()
    {
        ParkData parkData = ScriptableObject.CreateInstance<ParkData>();
        parkData.title = "TestPark";
        parkData.author = "TestAuthor";
       // parkData.data = ParkDataSaves.TilesToDict(tilemaps);
        parkData.maps = ParkDataSaves.TilesToList(tilemaps);
        string json = JsonUtility.ToJson(parkData);
        Debug.Log(json);
        PlayerPrefs.SetString(parkData.title, json);
        PlayerPrefs.Save();
    }

    public void LoadMap()
    {
        foreach (var map in ParkDataSaves.parkData.maps)
        {
            int index = tilemaps.FindIndex((tilemap) => tilemap.name.Equals(map.name));
            if (index == -1) continue;
            Tilemap tilemap = tilemaps[index];
            foreach (var tile in map.tiles)
            {
                tilemap.SetTile(tile.Position, tile.Tile);
            }
        } 
        // foreach (var map in tilemaps)
       // {
       // if (ParkDataSaves.parkData.data.TryGetValue(map.name, out List<ParkData.SavedTile> tiles))
            // {
            //     foreach (var tile in tiles)
            //     {
            //         map.SetTile(tile.Position, tile.Tile);
            //     }
            // }
       // }
    }
    
    public void ClearMap() {
        var maps = FindObjectsOfType<Tilemap>();
        foreach (var map in maps) {
            map.ClearAllTiles();
        }
    }
    
    
}