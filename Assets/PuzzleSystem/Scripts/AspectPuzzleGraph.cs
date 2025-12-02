using System.Collections.Generic;
using UnityEngine;

public class AspectPuzzleGraph {
    public class HexTileNode {
        public HexTile tile;
        public HashSet<HexTileNode> neighbors = new();
    }
    public class EdgeData {
        public HexTileNode A;
        public HexTileNode B;
        public GameObject EdgeObject;
    }

    public Dictionary<Vector2Int, HexTileNode> nodes = new();
    public Dictionary<(Vector2Int, Vector2Int), EdgeData> edges = new();

    private static readonly (int dq, int dr)[] neighborDirs = {
        (1, 0), (1, -1), (0, -1),
        (-1, 0), (-1, 1), (0, 1)
    };

    private static (Vector2Int, Vector2Int) GetEdgeKey(Vector2Int a, Vector2Int b) {
        return (a.x < b.x || (a.x == b.x && a.y < b.y)) ? (a, b) : (b, a);
    }

    private void CreateEdge(HexTileNode a, HexTileNode b) {
        var key = GetEdgeKey(a.tile.axial, b.tile.axial);

        if (edges.ContainsKey(key))
            return; // already exists

        var edge = new EdgeData {
            A = a,
            B = b,
            EdgeObject = SpawnEdgeObject(a, b)
        };

        edges[key] = edge;
    }

    private void RemoveEdge(HexTileNode a, HexTileNode b) {
        var key = GetEdgeKey(a.tile.axial, b.tile.axial);

        if (!edges.TryGetValue(key, out var edge))
            return;

        if (edge.EdgeObject != null)
            GameObject.Destroy(edge.EdgeObject);

        edges.Remove(key);
    }

    private GameObject SpawnEdgeObject(HexTileNode a, HexTileNode b) {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = $"Edge {a.tile.axial} <-> {b.tile.axial}";

        obj.transform.position = (a.tile.transform.position + b.tile.transform.position) / 2f;

        return obj;
    }

    public void AddTile(HexTile tile) {
        var node = new HexTileNode() {
            tile = tile
        };

        nodes[tile.axial] = node;
        // Build the tiles neighbors
        foreach (var (dq, dr) in neighborDirs) {
            var ncoord = new Vector2Int(tile.axial.x + dq, tile.axial.y + dr);
            if (nodes.TryGetValue(ncoord, out var neighbor)) {
                if (AspectDatabase.Instance.CanLink(tile.aspect, neighbor.tile.aspect)) {
                    node.neighbors.Add(neighbor);
                    neighbor.neighbors.Add(node);
                    CreateEdge(node, neighbor);
                }
            }
        }
    }

    public void UpdateTileAspect(HexTile hexTile) {
        // Update the tile's neighbors based on its new aspect
        if (nodes.TryGetValue(hexTile.axial, out var node)) {
            foreach (var (dq, dr) in neighborDirs) {
                var ncoord = new Vector2Int(hexTile.axial.x + dq, hexTile.axial.y + dr);
                if (nodes.TryGetValue(ncoord, out var neighbor)) {
                    bool canLink = AspectDatabase.Instance.CanLink(hexTile.aspect, neighbor.tile.aspect);


                    if (AspectDatabase.Instance.CanLink(hexTile.aspect, neighbor.tile.aspect)) {
                        if (!node.neighbors.Contains(neighbor)) {
                            node.neighbors.Add(neighbor);
                        }
                        if (!neighbor.neighbors.Contains(node)) {
                            neighbor.neighbors.Add(node);
                        }
                        CreateEdge(node, neighbor);
                    } else {
                        node.neighbors.Remove(neighbor);
                        neighbor.neighbors.Remove(node);
                        RemoveEdge(node, neighbor);
                    }
                }
            }
        }
    }

    public void Reset() {
        foreach (var edge in edges.Values) {
            if (edge.EdgeObject != null)
                GameObject.Destroy(edge.EdgeObject);
        }
        nodes.Clear();
        edges.Clear();
    }

    public bool AreAllLockedAspectsConnected() {
        // 1. Collect all locked nodes
        List<HexTileNode> lockedNodes = new List<HexTileNode>();

        foreach (var entry in nodes) {
            if (entry.Value.tile.locked) {
                lockedNodes.Add(entry.Value);
            }
        }

        // If 0 or 1 locked tile, trivially connected
        if (lockedNodes.Count <= 1)
            return true;

        // 2. BFS/DFS from the first locked node
        HashSet<HexTileNode> visited = new HashSet<HexTileNode>();
        Stack<HexTileNode> stack = new Stack<HexTileNode>();

        var start = lockedNodes[0];
        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0) {
            var node = stack.Pop();

            foreach (var n in node.neighbors) {
                if (!visited.Contains(n)) {
                    visited.Add(n);
                    stack.Push(n);
                }
            }
        }

        foreach (var ln in lockedNodes) {
            if (!visited.Contains(ln))
                return false;
        }
        return true;
    }
}
