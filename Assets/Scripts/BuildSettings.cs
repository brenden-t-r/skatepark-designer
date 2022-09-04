using UnityEngine;

public class BuildSettings : ScriptableObject
{
    public static BuildPiece selectedPiece = BuildPiece.BLOCK;
}

public enum BuildPiece
{
    BLOCK, BLOCK_HALF, BLOCK_CORNER, 
    RAMP
}