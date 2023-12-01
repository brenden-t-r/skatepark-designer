using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiTile : ScriptableObject
{
    public static List<GameTile.TileTypes> multiTileTypes = new()
    {
        GameTile.TileTypes.Z_MULTI_TEST, GameTile.TileTypes.Z_MULTI_RAMP
        
    };
    
    public Sprite sprite;
    public GameTile.TileTypes type; // TODO
    public List<TilePiece> tilePieces;
    
#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a GameTile Asset
    [MenuItem("Assets/Create/MultiTile")]
    public static void CreateGameTile()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Multi Tile", "New Multi Tile", "Asset", "Save Multi Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<MultiTile>(), path);
    }
#endif
}

[Serializable]
public class TilePiece
{
    // Relative to tile clicked
    public int x; 
    public int y;
    public int z;
    public int elevation;
    public GameTile tile;
}