using System;
using System.Collections.Generic;
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

        private bool CanPlay(Vector3Int gridPos)
        {
            if (activeTilemap.HasTile(gridPos))
            {
                return false;
            }
            
            if (TilemapManager._instance.multiTilePieceMap.ContainsKey(gridPos))
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
                    if (TilemapManager._instance.multiTilePieceMap.ContainsKey(pos))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void ClickEvent(Vector3Int gridPos)
        {
            SetActiveTilemap();
            
            if (TilemapManager._instance.multiTilePieceMap.ContainsKey(gridPos)) {
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
                  if (TilemapManager._instance.multiTilePieceMap.ContainsKey(pos))
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
                  // Get active tilemap based on piece.elevation
                  int elevation = BuildSettingsScriptableObject.elevation + piece.elevation;
                  Tilemap tilemap = TilemapManager
                      ._instance
                      .GetTilemapFromElevation(elevation);
                  tilemap.SetTile(pos, piece.tile);
                  TilemapManager._instance.multiTilePieceMap[pos] = new TilemapManager.MultiTilePieceMapElement
                  {
                      rootPos = gridPos,
                      piece = BuildSettingsScriptableObject.multiTilePiece
                  };
              }
            } else {
              // Single piece
              activeTilemap.SetTile(gridPos, BuildSettingsScriptableObject.selectedPiece);
            }
        }
    
        private void RightClickEvent(Vector3Int gridPos)
        {
            SetActiveTilemap();
            
            // Delete all tiles in multi-tile piece
            if (TilemapManager._instance.multiTilePieceMap
                .TryGetValue(gridPos, out TilemapManager.MultiTilePieceMapElement element))
            {
                MultiTilePiece multi = TilemapManager._instance.multiTilePieceMap[element.rootPos].piece;
                foreach (TilePiece tilePiece in multi.tilePieces)
                {
                    var pos = new Vector3Int(
                        x: element.rootPos.x + tilePiece.x,
                        y: element.rootPos.y + tilePiece.y,
                        z: element.rootPos.z + tilePiece.z
                    );
                    int elevation = BuildSettingsScriptableObject.elevation + tilePiece.elevation;
                    Tilemap tilemap = TilemapManager
                        ._instance
                        .GetTilemapFromElevation(elevation);
                    tilemap.SetTile(pos, null);
                    TilemapManager._instance.multiTilePieceMap.Remove(pos);
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
                // tilemapExtrasRenderer.material.SetColor ("_TintColor", 
                //     new Color(
                //         r: 255,
                //         g: 255,
                //         b: 255,
                //         a: 255
                //     )
                // );
                tilemapExtras.color = new Color(
                    r: 255,
                    g: 25,
                    b: 255,
                    a: tilemapExtras.color.a
                );
            } else
            {
                Debug.Log("Cannot play");
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
                    // foreach (TilePiece piece in BuildSettingsScriptableObject.multiTilePiece.tilePieces) {
                    //     var pos = new Vector3Int(
                    //         x: gridPos.x + piece.x,
                    //         y: gridPos.y + piece.y,
                    //         z: gridPos.z + piece.z
                    //     );
                    //     // Get active tilemap based on piece.elevation
                    //     int elevation = BuildSettingsScriptableObject.elevation + piece.elevation;
                    //     Tilemap tilemap = TilemapManager
                    //         ._instance
                    //         .GetTilemapFromElevation(elevation);
                    //     tilemap.SetTile(pos, piece.tile);
                    //     TilemapManager._instance.multiTilePieceMap[pos] = new TilemapManager.MultiTilePieceMapElement
                    //     {
                    //         rootPos = gridPos,
                    //         piece = BuildSettingsScriptableObject.multiTilePiece
                    //     };
                    // }
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
