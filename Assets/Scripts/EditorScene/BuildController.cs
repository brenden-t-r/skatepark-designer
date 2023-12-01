using System;
using System.Collections.Generic;
using System.Linq;
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
            
            var multiTileMapKey = TilemapManager
                ._instance
                .GetMultiTileMapKey(gridPos, BuildSettingsScriptableObject.elevation);
            if (TilemapManager._instance.multiTilePieceMap.ContainsKey(multiTileMapKey.GetKey())) {
                 // maybe just don't allow for now, or remove whole piece?
                 Debug.Log("Can't place tile here because it's part of multi-tile piece");
                 return;
            }
            
            if (BuildSettingsScriptableObject.isMultiTilePiece) {
              // Multiple tiles, relative to gridPos Vector3Int

              foreach (TilePiece piece in BuildSettingsScriptableObject.multiTilePiece.tilePieces)
              {
                  var pos = new Vector3Int(
                      x: gridPos.x + piece.x,
                      y: gridPos.y + piece.y,
                      z: gridPos.z + piece.z
                  );
                  int elevation = BuildSettingsScriptableObject.elevation + piece.elevation;
                  var mapKey = TilemapManager._instance.GetMultiTileMapKey(pos, elevation);
                  if (TilemapManager._instance.multiTilePieceMap.ContainsKey(mapKey.GetKey()))
                  {
                      // maybe just don't allow for now, or remove whole piece?
                      Debug.Log("Can't place tile here because it's part of multi-tile piece");
                      return;
                  }
              }
              
              foreach (TilePiece piece in BuildSettingsScriptableObject.multiTilePiece.tilePieces) {
                  var pos = new Vector3Int(
                      x: gridPos.x + piece.x,
                      y: gridPos.y + piece.y,
                      z: gridPos.z + piece.z
                  );
                  int elevation = BuildSettingsScriptableObject.elevation + piece.elevation;
                  var mapKey = TilemapManager._instance.GetMultiTileMapKey(pos, elevation);
                  // Get active tilemap based on piece.elevation
                  Tilemap tilemap = TilemapManager
                      ._instance
                      .GetTilemapFromElevation(elevation);
                  tilemap.SetTile(pos, piece.tile);
                  TilemapManager._instance.multiTilePieceMap[mapKey.GetKey()] = new TilemapManager.MultiTilePieceMapElement
                  {
                      rootPos = multiTileMapKey,
                      piece = BuildSettingsScriptableObject.multiTilePiece
                  };
                  
              }

              // var res = TilemapManager._instance.multiTilePieceMap.Keys;
              // var str = String.Join(",", res);
              // Debug.Log(str);
            } else {
              // Single piece
              activeTilemap.SetTile(gridPos, BuildSettingsScriptableObject.selectedPiece);
            }
        }
    
        private void RightClickEvent(Vector3Int gridPos)
        {
            SetActiveTilemap();
            
            // Delete all tiles in multi-tile piece
            var multiTileMapKey = TilemapManager._instance
                .GetMultiTileMapKey(gridPos, BuildSettingsScriptableObject.elevation);
            if (TilemapManager._instance.multiTilePieceMap
                .TryGetValue(multiTileMapKey.GetKey(), out TilemapManager.MultiTilePieceMapElement element))
            {
                MultiTilePiece multi = TilemapManager._instance.multiTilePieceMap[element.rootPos.GetKey()].piece;
                foreach (TilePiece tilePiece in multi.tilePieces)
                {
                    var pos = new Vector3Int(
                        x: element.rootPos.x + tilePiece.x,
                        y: element.rootPos.y + tilePiece.y,
                        z: element.rootPos.z + tilePiece.z
                    );
                    int elevation = BuildSettingsScriptableObject.elevation + tilePiece.elevation;
                    var mapKey = new TilemapManager.MultiTilePieceMapKey()
                    {
                        x = pos.x,
                        y = pos.y,
                        z = pos.z,
                        elevation = elevation
                    };
                    Tilemap tilemap = TilemapManager
                        ._instance
                        .GetTilemapFromElevation(elevation);
                    tilemap.SetTile(pos, null);
                    TilemapManager._instance.multiTilePieceMap.Remove(mapKey.GetKey());
                }
                
            }
            else
            {
                // Single piece
                activeTilemap.SetTile(gridPos, null);
            }
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

        private List<Vector3Int> highlightedTiles = new();

        private void HoverHighlight(Vector3Int hoverPos)
        {
            if (hoverPos == highlightedTilePos) return;
            tilemapExtrasRenderer.sortingOrder = BuildSettingsScriptableObject.elevation;
            ResetHoverHighlight();
            highlightedTilePos = hoverPos;

            if (CanPlay(hoverPos))
            {
                tilemapExtras.color = new Color(
                    r: 255,
                    g: 25,
                    b: 255,
                    a: tilemapExtras.color.a
                );
            } else
            {
                tilemapExtras.color = new Color(
                    r: 255,
                    g: 0,
                    b: 0,
                    a: tilemapExtras.color.a
                );
            }
            
            if (BuildSettingsScriptableObject.isMultiTilePiece)
            {
                foreach (TilePiece piece in BuildSettingsScriptableObject.multiTilePiece.tilePieces)
                {
                    var pos = new Vector3Int(
                        x: hoverPos.x + piece.x,
                        y: hoverPos.y + piece.y,
                        z: hoverPos.z + piece.z
                    );
                    highlightedTiles.Add(pos);
                    tilemapExtras.SetTile(pos, piece.tile);
                }
            }
            else
            {
                highlightedTile = BuildSettingsScriptableObject.selectedPiece;
                tilemapExtras.SetTile(hoverPos, highlightedTile);
            }
        }

        private void ResetHoverHighlight()
        {
            tilemapExtras.SetTile(highlightedTilePos, null);
            highlightedTiles.ForEach(x => { tilemapExtras.SetTile(x, null); });
            highlightedTiles = new List<Vector3Int>();
        }
        
        private bool CanPlay(Vector3Int gridPos)
        {
            SetActiveTilemap();
            
            if (activeTilemap.HasTile(gridPos))
            {
                return false;
            }
            
            var multiTileMapKey = TilemapManager
                ._instance
                .GetMultiTileMapKey(gridPos, BuildSettingsScriptableObject.elevation);
            if (TilemapManager._instance.multiTilePieceMap.ContainsKey(multiTileMapKey.GetKey()))
            {
                return false;
            }

            if (BuildSettingsScriptableObject.isMultiTilePiece)
            {
                // Multiple tiles, relative to gridPos Vector3Int

                foreach (TilePiece piece in BuildSettingsScriptableObject.multiTilePiece.tilePieces)
                {
                    var pos = new Vector3Int(
                        x: gridPos.x + piece.x,
                        y: gridPos.y + piece.y,
                        z: gridPos.z + piece.z
                    );
                    var mapKey = TilemapManager
                        ._instance
                        .GetMultiTileMapKey(pos, piece.elevation + BuildSettingsScriptableObject.elevation);
                    if (TilemapManager._instance.multiTilePieceMap.ContainsKey(mapKey.GetKey()))
                    {
                        return false;
                    }
                }
            }

            return true;
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
