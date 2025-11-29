using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
    public Grid grid;
    public GameObject prefab;
    public int radius = 3;
    public float size = 1f;
    float prevSize = 1f;

    public Dictionary<Vector2Int, HexTile> tiles = new Dictionary<Vector2Int, HexTile>();

    void Start() {
        GenerateHexWithGrid();
    }

    private void Update() {
        if (size != prevSize) {
            // Destroy all children
            for (int i = 0; i < this.transform.childCount; i++) {
                Destroy(this.transform.GetChild(i).gameObject);
            }

            GenerateHexWithGrid();
            prevSize = size;
        }
    }

    void GenerateHexWithGrid() {
        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                int z = -x - y;
                if (Mathf.Abs(z) > radius)
                    continue;

                Vector3Int cell = new Vector3Int(x, 0, z);
                HexTile tile = tiles[new Vector2Int(x, y)];

                Vector3 world = CubeToWorld(cell.x, cell.y, cell.z);

                Instantiate(prefab, world, Quaternion.identity, transform);
            }
        }
    }

    Vector3 CubeToWorld(int x, int y, int z) {
        float width = 2f * size;
        float height = Mathf.Sqrt(3) * size;

        float worldX = width * (3f / 4f) * x;
        float worldY = 0f;
        float worldZ = height * (z + x / 2f);

        return new Vector3(worldX, worldY, worldZ);
    }
}
