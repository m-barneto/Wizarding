using UnityEngine;

public class PuzzlePlayer : MonoBehaviour {
    [SerializeField]
    PuzzleEditorUI puzzleEditorUI;
    [SerializeField]
    PuzzlePlayerUI puzzlePlayerUI;


    PuzzleEditor puzzleEditor;

    ResearchPuzzle puzzle;
    GridController grid;


    void Awake() {
        grid = GetComponent<GridController>();
        puzzleEditor = GetComponent<PuzzleEditor>();
    }

    private void OnGUI() {
        return;
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

        GUILayout.Label("Is Solved: " + grid.IsSolved());

        AspectGrid.Instance.ShowAspectGrid();

        GUILayout.EndArea();

        AspectGrid.Instance.ShowAspectTooltip();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HexTile tile = grid.GetTileUnderMouse();
            if (tile) {
                if (tile.aspect) {
                    grid.hexTiles[tile.axial].SetAspect(null);
                } else {
                    grid.hexTiles[tile.axial].SetAspect(puzzlePlayerUI.GetSelectedAspect());
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            HexTile tile = grid.GetTileUnderMouse();
            if (tile) {
                grid.hexTiles[tile.axial].SetAspect(null);
            }
        }
    }

    public void LoadPuzzle() {
        puzzle = PuzzleIO.LoadPuzzle();
        // Check if puzzle is null before actually loading into editor
        grid.LoadPuzzle(puzzle);
    }

    public void OpenEditor() {
        puzzlePlayerUI.gameObject.SetActive(false);
        this.enabled = false;

        puzzleEditorUI.gameObject.SetActive(true);
        puzzleEditorUI.enabled = true;
    }
}
