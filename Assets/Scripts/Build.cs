using System;
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
    private Tilemap activeTilemap;
    private Renderer tilemapExtrasRenderer;
    private Tile highlightedTile;
    private Vector3Int highlightedTilePos;

    private void Start()
    {
        tilemapExtrasRenderer = tilemapExtras.GetComponent<Renderer>();
        activeTilemap = tilemap;
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
}
