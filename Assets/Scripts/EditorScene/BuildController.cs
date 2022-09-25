using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace EditorScene
{
    public class BuildController : MonoBehaviour, IPointerClickHandler
    {
        private Camera mainCamera;
        private Tilemap activeTilemap;
        private Tilemap tilemapExtras;
        private Renderer tilemapExtrasRenderer;
        private Tile highlightedTile;
        private Vector3Int highlightedTilePos;

        private void Start()
        {
            mainCamera = Camera.main;
            tilemapExtras = TilemapManager._instance.GetTilemapExtras();
            tilemapExtrasRenderer = tilemapExtras.GetComponent<Renderer>();
            activeTilemap = TilemapManager._instance.GetTilemapFromElevation(0);
            ClearMap();
            LoadMap();
        }

        private void Update()
        {
            Vector3Int mousePos = GetMousePosition();
            Vector3Int hoverPos = new Vector3Int(mousePos.x, mousePos.y, 0);
            HoverHighlight(hoverPos);
        }

        private Vector3Int GetMousePosition() {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return TilemapManager._instance.grid.WorldToCell(mouseWorldPos);
        }

        private void ClickEvent(Vector3Int gridPos)
        {
            SetActiveTilemap();
            activeTilemap.SetTile(gridPos, BuildSettingsScriptableObject.selectedPiece);
        }
    
        private void RightClickEvent(Vector3Int gridPos)
        {
            SetActiveTilemap();
            activeTilemap.SetTile(gridPos, null);
        }

        private void SetActiveTilemap()
        {
            activeTilemap = TilemapManager
                ._instance
                .GetTilemapFromElevation(BuildSettingsScriptableObject.elevation);
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
            tilemapExtrasRenderer.sortingOrder = BuildSettingsScriptableObject.elevation;
            if (highlightedTile != null)
            {
                ResetHoverHighlight();
            }
            highlightedTilePos = hoverPos;
            highlightedTile = BuildSettingsScriptableObject.selectedPiece;
            tilemapExtras.SetTile(hoverPos, highlightedTile);
        }

        private void ResetHoverHighlight()
        {
            tilemapExtras.SetTile(highlightedTilePos, null);
        }

        public void SaveMap()
        {
            TilemapManager._instance.SaveMap();
        }

        public void LoadMap()
        {
            TilemapManager._instance.LoadMap();
        }

        public void ClearMap()
        {
            TilemapManager._instance.ClearMap();
        }

    }
}
