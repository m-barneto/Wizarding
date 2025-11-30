using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class GridController : MonoBehaviour {
    public GameObject prefab;
    public Dictionary<Vector2Int, HexTile> hexTiles = new Dictionary<Vector2Int, HexTile>();

    public void GenerateGrid(int gridSize) {
        List<Vector2Int> tilePositions = HexUtils.GenerateHexRing(gridSize);
        GenerateTiles(tilePositions);
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
                hexTile.axial = tilePos;
                tile.transform.name = $"Tile {tilePos.x}, {tilePos.y}";
                hexTiles[tilePos] = hexTile;

            }
        }
    }

    public void LoadPuzzle(ResearchPuzzle puzzle) {
        GenerateGrid(puzzle.gridRadius);
        foreach (var tile in puzzle.tiles) {

        }
    }
}
