using UnityEngine;

public class PuzzleController : MonoBehaviour {
    [SerializeField]
    PuzzleUI puzzleUI;
    ResearchPuzzle puzzle = null;
    GridController grid;
    int gridRadius = 2;

    private void Awake() {
        grid = GetComponent<GridController>();
        grid.GenerateGrid(gridRadius);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HexTile tile = grid.GetTileUnderMouse();
            if (tile) {
                if (tile.aspect) {
                    grid.hexTiles[tile.axial].SetAspect(null);
                    grid.hexTiles[tile.axial].SetLocked(false);
                    grid.hexTiles[tile.axial].ShowWisp(false);
                } else {
                    grid.hexTiles[tile.axial].SetAspect(puzzleUI.GetSelectedAspect());
                    grid.hexTiles[tile.axial].SetLocked(true);
                    grid.hexTiles[tile.axial].ShowWisp(true);
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
}
