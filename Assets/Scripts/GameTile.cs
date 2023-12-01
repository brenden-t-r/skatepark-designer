using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameTile : Tile
{
    public enum TileTypes
    {
        BLOCK, BLOCK_HALF, BLOCK_CORNER, 
        RAMP, RAMP_FRONT,
        
        // Multi
        Z_MULTI_TEST, Z_MULTI_RAMP,
        
        // Multi Ramp Back
        Z_P_RAMP_BACK_1,
        Z_P_RAMP_BACK_2,
        Z_P_RAMP_BACK_3,
        Z_P_RAMP_BACK_4,
        Z_P_RAMP_BACK_5
    }
    
    public TileTypes type;
    
    
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        base.RefreshTile(location, tilemap);
    }
    
    #if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a GameTile Asset
    [MenuItem("Assets/Create/GameTile")]
    public static void CreateGameTile()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Game Tile", "New Game Tile", "Asset", "Save Game Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<GameTile>(), path);
    }
    #endif
}
