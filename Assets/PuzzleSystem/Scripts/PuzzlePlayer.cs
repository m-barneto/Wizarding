using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;

public class PuzzlePlayer : MonoBehaviour {
    PuzzleEditor puzzleEditor;
    ResearchPuzzle puzzle;
    GridController grid;


    void Awake() {
        grid = GetComponent<GridController>();
        puzzleEditor = GetComponent<PuzzleEditor>();
    }

    private void OnGUI() {
        GUIStyle myStyle = new GUIStyle(GUI.skin.button);
        myStyle.contentOffset = new Vector2(15, 15);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;

        GUILayout.BeginArea(new Rect(0, 0, 425, 800), myStyle);

        if (GUILayout.Button("Open Editor", buttonStyle)) {
            this.enabled = false;
            puzzleEditor.enabled = true;
        }

        if (GUILayout.Button("Load Puzzle", buttonStyle)) {
            puzzle = PuzzleIO.LoadPuzzle();
            // Check if puzzle is null before actually loading into editor
            grid.LoadPuzzle(puzzle);
        }



        AspectGrid.Instance.ShowAspectGrid();

        GUILayout.EndArea();

        AspectGrid.Instance.ShowAspectTooltip();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Attempt to place aspect
            HexTile tile = grid.GetTileUnderMouse();
        }
    }
}
