using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridController : MonoBehaviour {
    public GameObject prefab;
    public Dictionary<Vector2Int, HexTile> hexTileGameObjects = new Dictionary<Vector2Int, HexTile>();


    public void GenerateTiles(List<Vector2Int> tiles) {
        // Remove all the hexTileGameObjects
        foreach (var key in hexTileGameObjects.Keys) {
            Destroy(hexTileGameObjects[key].gameObject);
        }
        hexTileGameObjects.Clear();

        // Find tiles that arent in hexTileGameObjects
        for (int i = 0; i < tiles.Count; i++) {
            Vector2Int tilePos = tiles[i];

            if (!hexTileGameObjects.ContainsKey(tilePos)) {
                // Create the tile and add it to hexTileGameObjects
                Vector3 world = HexUtils.CubeToWorld(tilePos);

                var tile = Instantiate(prefab, world, Quaternion.identity, transform);
                HexTile hexTile = tile.GetComponent<HexTile>();
                hexTile.axial = tilePos;
                tile.transform.name = $"Tile {tilePos.x}, {tilePos.y}";
                hexTileGameObjects[tilePos] = hexTile;

            }
        }
    }
}
