using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    [SerializeField] private List<Button> buildButtons;
    [SerializeField] private List<GameTile> tiles;
    [SerializeField] private Image selectedTileType;
    [SerializeField] private Button btnElevationUp;
    [SerializeField] private Button btnElevationDown;
    private Dictionary<GameTile.TileTypes, GameTile> tileTypes;
    private static int MAX_ELEVATION = 4;
    private void Start()
    {
        tileTypes = new Dictionary<GameTile.TileTypes, GameTile>();
        foreach (var tile in tiles)
        {
           tileTypes.Add(tile.type, tile); 
        }
        foreach (var btn in buildButtons)
        {
            btn.onClick.AddListener(() => BtnOnClick(btn));
        }
        btnElevationUp.onClick.AddListener(ElevationUp);
        btnElevationDown.onClick.AddListener(ElevationDown);
    }

    private void BtnOnClick(Button btn)
    {
        BuildSettings.selectedPiece = tileTypes[BtnToPiece(btn)];
        selectedTileType.sprite = BuildSettings.selectedPiece.sprite;
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
            default:
                Debug.LogError("Invalid build button name");
                return GameTile.TileTypes.BLOCK;
        }
    }
    
    private void ElevationUp()
    {
        BuildSettings.elevation += 1;
        btnElevationDown.interactable = true;
        if (BuildSettings.elevation == MAX_ELEVATION)
        {
            btnElevationUp.interactable = false;
        }
    }
    
    private void ElevationDown()
    {
        BuildSettings.elevation -= 1;
        btnElevationUp.interactable = true;
        if (BuildSettings.elevation == 0)
        {
            btnElevationDown.interactable = false;
        }
    }
}