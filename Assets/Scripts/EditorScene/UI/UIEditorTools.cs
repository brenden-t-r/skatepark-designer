using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EditorScene.UI
{
    public class UIEditorTools : MonoBehaviour
    {
        [SerializeField] private List<Button> buildButtons;
        [SerializeField] private List<GameTile> tiles;
        [SerializeField] private Image selectedTileType;
        [SerializeField] private Button btnElevationUp;
        [SerializeField] private Button btnElevationDown;
        [SerializeField] private List<MultiTile> multiTiles;
        private Dictionary<GameTile.TileTypes, GameTile> tileTypes;
        private Dictionary<GameTile.TileTypes, MultiTile> multiTileTypes;
        private static int MAX_ELEVATION = 4;
        private void Start()
        {
            tileTypes = new Dictionary<GameTile.TileTypes, GameTile>();
            multiTileTypes = new Dictionary<GameTile.TileTypes, MultiTile>();
            foreach (var tile in tiles)
            {
                tileTypes.Add(tile.type, tile); 
            }

            foreach (var multiTile in multiTiles)
            {
                multiTileTypes.Add(multiTile.type, multiTile);
            }
            foreach (var btn in buildButtons)
            {
                btn.onClick.AddListener(() => BtnOnClick(btn));
            }
            btnElevationUp.onClick.AddListener(ElevationUp);
            btnElevationDown.onClick.AddListener(ElevationDown);
            BuildSettingsScriptableObject.selectedPiece = tileTypes[GameTile.TileTypes.BLOCK];
            BuildSettingsScriptableObject.elevation = 0;
        }
        
        private void BtnOnClick(Button btn)
        {
            GameTile.TileTypes type = BtnToPiece(btn);
            if (MultiTile.multiTileTypes.Contains(type))
            {
                MultiTile tile = multiTileTypes[type];
                selectedTileType.sprite = tile.sprite;
                BuildSettingsScriptableObject.isMultiTilePiece = true;
                BuildSettingsScriptableObject.multiTilePiece = new MultiTilePiece()
                {
                    tilePieces = tile.tilePieces
                };
            }
            else
            {
                BuildSettingsScriptableObject.selectedPiece = tileTypes[BtnToPiece(btn)];
                BuildSettingsScriptableObject.isMultiTilePiece = false;
                BuildSettingsScriptableObject.multiTilePiece = null;
                selectedTileType.sprite = BuildSettingsScriptableObject.selectedPiece.sprite;
            }
        }

        private GameTile.TileTypes BtnToPiece(Button btn)
        {
            switch (btn.name)
            {
                case "BtnBuildBlock":
                    return GameTile.TileTypes.BLOCK;
                case "BtnBuildBlockHalf":
                    return GameTile.TileTypes.BLOCK_HALF;
                case "BtnBuildBlockCorner":
                    return GameTile.TileTypes.BLOCK_CORNER;
                case "BtnBuildBlockRamp":
                    return GameTile.TileTypes.RAMP;
                case "BtnBuildBlockRampFront":
                    return GameTile.TileTypes.RAMP_FRONT;
                case "BtnBuildBlockMultiTest":
                    return GameTile.TileTypes.Z_MULTI_TEST;
                default:
                    Debug.LogError("Invalid build button name");
                    return GameTile.TileTypes.BLOCK;
            }
        }
    
        private void ElevationUp()
        {
            BuildSettingsScriptableObject.elevation += 1;
            btnElevationDown.interactable = true;
            if (BuildSettingsScriptableObject.elevation == MAX_ELEVATION)
            {
                btnElevationUp.interactable = false;
            }
        }
    
        private void ElevationDown()
        {
            BuildSettingsScriptableObject.elevation -= 1;
            btnElevationUp.interactable = true;
            if (BuildSettingsScriptableObject.elevation == 0)
            {
                btnElevationDown.interactable = false;
            }
        }
    }
}
