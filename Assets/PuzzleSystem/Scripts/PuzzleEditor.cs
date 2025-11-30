using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PuzzleEditor : MonoBehaviour {
    [SerializeField]
    AspectDatabase aspectDatabase;

    ResearchPuzzle puzzle = null;
    GridController grid;
    int gridSize = 4;
    int aspectSelection = 0;
    string[] aspects;
    Camera cam;
    PuzzlePlayer puzzlePlayer;

    private void Awake() {
        aspects = aspectDatabase.aspects.Select(x => x.aspectName).ToArray();
        grid = GetComponent<GridController>();
        grid.GenerateGrid(gridSize);
        cam = Camera.main;
        puzzlePlayer = GetComponent<PuzzlePlayer>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HexTile tile = GetTileUnderMouse();
            if (tile) {
                if (tile.aspect) {
                    grid.hexTiles[tile.axial].SetAspect(null);
                } else {
                    grid.hexTiles[tile.axial].SetAspect(aspectDatabase.aspects[aspectSelection]);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            HexTile tile = GetTileUnderMouse();
            if (tile) {
                grid.hexTiles[tile.axial].SetLocked(!tile.locked);
            }
        }
    }

    HexTile GetTileUnderMouse() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f)) {
            GameObject go = hit.collider.gameObject;
            if (go.TryGetComponent(out HexTile tile)) {
                return tile;
            }
        }
        return null;
    }

    private void OnGUI() {
        // create a style based on the default label style
        GUIStyle myStyle = new GUIStyle(GUI.skin.button);
        myStyle.contentOffset = new Vector2(15, 15);
        GUILayout.BeginArea(new Rect(0, 0, 300, 600), myStyle);

        if (GUILayout.Button("Open Player")) {
            this.enabled = false;
            puzzlePlayer.enabled = true;
        }

        if (GUILayout.Button("Load Puzzle")) {
            puzzle = PuzzleIO.LoadPuzzle();
            // Check if puzzle is null before actually loading into editor
            grid.LoadPuzzle(puzzle);
        }
        if (GUILayout.Button("Save Puzzle As")) {
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

        GUILayout.Label("Grid Radius");
        int newGridSize = (int)Mathf.Round(GUILayout.HorizontalSlider(gridSize, 1f, 10f));
        if (gridSize != newGridSize) {
            gridSize = newGridSize;
            // Regenerate the grid
            grid.GenerateGrid(gridSize);
        }

        aspectSelection = GUI.SelectionGrid(new Rect(10, 150, 280, 300), aspectSelection, aspects, 3);

        GUILayout.EndArea();
    }
}
