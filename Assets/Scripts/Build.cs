using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Build : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap tilemapExtras;
    private Tile highlightedTile;
    private Vector3Int highlightedTilePos;
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
        //Debug.Log(tilemap.GetTile(gridPos).name);
        Debug.Log(gridPos);
        tilemap.SetTile(gridPos, BuildSettings.selectedPiece);
    }
    
    private void RightClickEvent(Vector3Int gridPos)
    {
        tilemap.SetTile(gridPos, null);
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
