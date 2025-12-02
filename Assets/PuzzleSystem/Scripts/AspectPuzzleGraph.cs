using System.Collections.Generic;
using UnityEngine;

public class AspectPuzzleGraph {
    public class HexTileNode {
        public HexTile tile;
        public List<HexTileNode> neighbors = new();
    }

    public Dictionary<Vector2Int, HexTileNode> nodes = new();
    private static readonly (int dq, int dr)[] neighborDirs = {
        (1, 0), (1, -1), (0, -1),
        (-1, 0), (-1, 1), (0, 1)
    };

    public void AddTile(HexTile tile) {
        var node = new HexTileNode() {
            tile = tile
        };

        nodes[tile.axial] = node;
    }

    public void BuildNeighbors() {
        foreach (var kvp in nodes) {
            var node = kvp.Value;
            node.neighbors.Clear();

            foreach (var (dq, dr) in neighborDirs) {
                var ncoord = new Vector2Int(node.tile.axial.x + dq, node.tile.axial.x + dr);

                if (nodes.TryGetValue(ncoord, out var neighbor)) {
                    node.neighbors.Add(neighbor);
                }
            }
        }
    }

    public void UpdateConnections() {
        foreach (var node in nodes.Values) {
            node.neighbors.RemoveAll(n => !AspectDatabase.Instance.CanLink(node.tile.aspect, n.tile.aspect));
        }
    }
}
