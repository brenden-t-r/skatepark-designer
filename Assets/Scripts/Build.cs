using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Build : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile _tile;
    
    private Vector3Int GetMousePosition () {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return grid.WorldToCell(mouseWorldPos);
    }

    private void ClickEvent(Vector3Int gridPos)
    {
        //Debug.Log(tilemap.GetTile(gridPos).name);
        Debug.Log(gridPos);
        tilemap.SetTile(gridPos, _tile);
    }


    public void OnPointerClick (PointerEventData eventData) {
        Vector3Int mousePos = GetMousePosition();
        Vector3Int gridPos = new Vector3Int(mousePos.x, mousePos.y, 0);

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log("Left Mouse Clicked");
                ClickEvent(gridPos);
                break;
            case PointerEventData.InputButton.Right:
                Debug.Log("Right Mouse Clicked");
                break;
            case PointerEventData.InputButton.Middle:
            default:
                return;
        }
    }
}
