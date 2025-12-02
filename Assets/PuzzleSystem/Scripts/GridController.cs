using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class GridController : MonoBehaviour {
    public GameObject prefab;
    public Dictionary<Vector2Int, HexTile> hexTiles = new Dictionary<Vector2Int, HexTile>();
    AspectPuzzleGraph puzzleGraph;
    Dictionary<(Vector2Int, Vector2Int), GameObject> connections;
    Camera cam;

    private void Awake() {
        cam = Camera.main;
        puzzleGraph = new();
    }

    public void GenerateGrid(int gridSize, bool generateGraph = true) {
        puzzleGraph = new();

        List<Vector2Int> tilePositions = HexUtils.GenerateHexRing(gridSize);
        GenerateTiles(tilePositions);

        if (generateGraph) {
            puzzleGraph.BuildNeighbors();
            puzzleGraph.UpdateConnections();
        }
    }

    public void GenerateTiles(List<Vector2Int> tiles) {
        // Remove all the hexTileGameObjects
        foreach (var key in hexTiles.Keys) {
            Destroy(hexTiles[key].gameObject);
        }
        hexTiles.Clear();

        // Find tiles that arent in hexTiles
        for (int i = 0; i < tiles.Count; i++) {
            Vector2Int tilePos = tiles[i];

            if (!hexTiles.ContainsKey(tilePos)) {
                // Create the tile and add it to hexTiles
                Vector3 world = HexUtils.CubeToWorld(tilePos);

                var tile = Instantiate(prefab, world, Quaternion.identity, transform);
                HexTile hexTile = tile.GetComponent<HexTile>();
                hexTile.graph = puzzleGraph;
                hexTile.axial = tilePos;
                tile.transform.name = $"Tile {tilePos.x}, {tilePos.y}";
                hexTiles[tilePos] = hexTile;

                puzzleGraph.AddTile(hexTile);
            }
        }
    }

    public void LoadPuzzle(ResearchPuzzle puzzle) {
        GenerateGrid(puzzle.gridRadius, false);
        Vector2Int tilePos;
        foreach (var tile in puzzle.tiles) {
            tilePos = new Vector2Int(tile.q, tile.r);
            if (hexTiles.ContainsKey(tilePos)) {
                hexTiles[tilePos].SetAspect(tile.aspectId.Length == 0 ? null : AspectDatabase.Instance.GetByName(tile.aspectId));
                hexTiles[tilePos].SetLocked(tile.locked);

            } else {
                Debug.Log($"Key not found in grid {tile.q}, {tile.r}");
            }
        }
        puzzleGraph.BuildNeighbors();
        UpdateConnections();
    }

    void UpdateConnections() {
        puzzleGraph.UpdateConnections();
        // Go through the connections and build our dictionary

    }


    public HexTile GetTileUnderMouse() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f)) {
            GameObject go = hit.collider.gameObject;
            if (go.TryGetComponent(out HexTile tile)) {
                return tile;
            }
        }
        return null;
    }
}
