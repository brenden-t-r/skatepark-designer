using System.Collections.Generic;
using UnityEngine;

namespace EditorScene
{
    public class BuildSettingsScriptableObject : ScriptableObject
    {
        public static GameTile selectedPiece;
        public static int elevation = 0;

        // For pieces that span multiple tiles
        public static bool isMultiTilePiece = false;
        public static MultiTilePiece multiTilePiece;
    }

    public class MultiTilePiece
    {
        public List<TilePiece> tilePieces;
    }
}
