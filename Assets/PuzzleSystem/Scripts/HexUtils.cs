using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class HexUtils {
    public static List<Vector2Int> GenerateHexRing(int radius) {
        var list = new List<Vector2Int>();

        for (int q = -radius; q <= radius; q++) {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++) {
                list.Add(new Vector2Int(q, r));
            }
        }

        return list;
    }

    public static Vector3 CubeToWorld(Vector2Int pos, float size = 1.2f) {
        int x = pos.x;
        int z = pos.y;
        int y = -x - z;
        float width = 2f * size;
        float height = Mathf.Sqrt(3) * size;

        float worldX = width * (3f / 4f) * x;
        float worldY = 0f;
        float worldZ = height * (z + x / 2f);

        return new Vector3(worldX, worldY, worldZ);
    }
}