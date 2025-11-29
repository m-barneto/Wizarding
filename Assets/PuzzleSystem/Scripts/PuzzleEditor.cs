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

    Dictionary<Vector2Int, HexTile> puzzleTiles;

    private void Awake() {
        aspects = aspectDatabase.aspects.Select(x => x.aspectName).ToArray();
        grid = GetComponent<GridController>();
        GenerateGrid();
        cam = Camera.main;
    }

    void GenerateGrid() {
        List<Vector2Int> tilePositions = HexUtils.GenerateHexRing(gridSize);
        grid.GenerateTiles(tilePositions);
        puzzleTiles = grid.hexTileGameObjects;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HexTile tile = GetTileUnderMouse();
            if (tile) {
                puzzleTiles[tile.axial].SetAspect(aspectDatabase.aspects[aspectSelection]);
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            HexTile tile = GetTileUnderMouse();
            if (tile) {
                puzzleTiles[tile.axial].SetAspect(null);
            }
        }
        if (Input.GetMouseButtonDown(2)) {
            HexTile tile = GetTileUnderMouse();
            if (tile) {
                puzzleTiles[tile.axial].SetLocked(!tile.locked);
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

        if (GUILayout.Button("Load Puzzle")) {
            puzzle = PuzzleIO.LoadPuzzle();
            // Check if puzzle is null before actually loading into editor
        }
        if (GUILayout.Button("Save Puzzle As")) {
            PuzzleIO.SavePuzzle(puzzle);
        }

        GUILayout.Label("Grid Radius");
        int newGridSize = (int)Mathf.Round(GUILayout.HorizontalSlider(gridSize, 1f, 10f));
        if (gridSize != newGridSize) {
            gridSize = newGridSize;
            // Regenerate the grid
            GenerateGrid();
        }

        aspectSelection = GUI.SelectionGrid(new Rect(5, 100, 150, 150), aspectSelection, aspects, 2);

        GUILayout.EndArea();
    }
}
