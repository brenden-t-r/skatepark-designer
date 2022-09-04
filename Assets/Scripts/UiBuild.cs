using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    [SerializeField] private List<Button> buildButtons;

    private void Start()
    {
        foreach (var btn in buildButtons)
        {
            btn.onClick.AddListener(() => BuildSettings.selectedPiece = BtnToPiece(btn));
        }
    }

    private BuildPiece BtnToPiece(Button btn)
    {
        switch (btn.name)
        {
            case "BtnBuildBlock":
                return BuildPiece.BLOCK;
            case "BtnBuildBlockHalf":
                return BuildPiece.BLOCK_HALF;
            case "BtnBuildBlockCorner":
                return BuildPiece.BLOCK_CORNER;
            case "BtnBuildBlockRamp":
                return BuildPiece.RAMP;
            default:
                Debug.LogError("Invalid build button name");
                return BuildPiece.BLOCK;
        }
    }
}
