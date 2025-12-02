using UnityEngine;

public class PuzzleEditor : MonoBehaviour {
    [SerializeField]
    PuzzleUI puzzleUI;

    ResearchPuzzle puzzle = null;
    GridController grid;
    int gridSize = 4;
    Camera cam;
    PuzzlePlayer puzzlePlayer;

    private void Awake() {
        grid = GetComponent<GridController>();
        grid.GenerateGrid(gridSize);
        cam = Camera.main;
        puzzlePlayer = GetComponent<PuzzlePlayer>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HexTile tile = grid.GetTileUnderMouse();
            if (tile) {
                if (tile.aspect) {
                    grid.hexTiles[tile.axial].SetAspect(null);
                    grid.hexTiles[tile.axial].SetLocked(false);
                } else {
                    grid.hexTiles[tile.axial].SetAspect(puzzleUI.GetSelectedAspect());
                    grid.hexTiles[tile.axial].SetLocked(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            HexTile tile = grid.GetTileUnderMouse();
            if (tile) {
                grid.hexTiles[tile.axial].SetLocked(!tile.locked);
            }
        }
    }

    private void OnGUI() {
        return;
        GUIStyle myStyle = new GUIStyle(GUI.skin.button);
        myStyle.contentOffset = new Vector2(15, 15);
        GUILayout.BeginArea(new Rect(0, 0, 425, 800), myStyle);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;

        if (GUILayout.Button("Open Player", buttonStyle)) {
            this.enabled = false;
            puzzlePlayer.enabled = true;
        }

        if (GUILayout.Button("Load Puzzle", buttonStyle)) {
            puzzle = PuzzleIO.LoadPuzzle();
            // Check if puzzle is null before actually loading into editor
            grid.LoadPuzzle(puzzle);
        }
        if (GUILayout.Button("Save Puzzle As", buttonStyle)) {
            // Go through the hextiles and add the tiles that have changed to the puzzle
            puzzle.tiles.Clear();
            puzzle.gridRadius = gridSize;
            foreach (var kvpTile in grid.hexTiles) {
                HexTile tile = kvpTile.Value;
                if (tile.aspect != null || tile.locked == true) {
                    puzzle.tiles.Add(tile.TileData());
                }
            }
            PuzzleIO.SavePuzzle(puzzle);
        }

        GUILayout.Label("Grid Radius", buttonStyle);
        int newGridSize = (int)Mathf.Round(GUILayout.HorizontalSlider(gridSize, 1f, 10f));
        if (gridSize != newGridSize) {
            gridSize = newGridSize;
            // Regenerate the grid
            grid.GenerateGrid(gridSize);
        }

        AspectGrid.Instance.ShowAspectGrid();

        GUILayout.EndArea();

        AspectGrid.Instance.ShowAspectTooltip();
    }

    public void OpenPlayer() {
        this.enabled = false;
        puzzlePlayer.enabled = true;
    }

    public void LoadPuzzle() {
        puzzle = PuzzleIO.LoadPuzzle();
        // Check if puzzle is null before actually loading into editor
        grid.LoadPuzzle(puzzle);
    }

    public void SavePuzzle() {
        puzzle.tiles.Clear();
        puzzle.gridRadius = gridSize;
        foreach (var kvpTile in grid.hexTiles) {
            HexTile tile = kvpTile.Value;
            if (tile.aspect != null || tile.locked == true) {
                puzzle.tiles.Add(tile.TileData());
            }
        }
        PuzzleIO.SavePuzzle(puzzle);
    }

    public void SetGridRadius(int gridRadius) {
        gridSize = gridRadius;
        grid.GenerateGrid(gridSize);
    }

    public int GetGridRadius() {
        return gridSize;
    }
}
