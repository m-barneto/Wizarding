using System.Collections.Generic;
using System.Drawing;
using Unity.VectorGraphics;
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

    public static Texture2D RenderSpritePreserveAspect(Sprite sprite, int targetSize, Material mat) {
        // Calculate aspect-correct render size
        float w = sprite.rect.width;
        float h = sprite.rect.height;
        float aspect = w / h;

        int renderW, renderH;

        if (aspect > 1f) {
            renderW = targetSize;
            renderH = Mathf.RoundToInt(targetSize / aspect);
        } else {
            renderH = targetSize;
            renderW = Mathf.RoundToInt(targetSize * aspect);
        }

        Texture2D rendered = VectorUtils.RenderSpriteToTexture2D(
            sprite,
            renderW,
            renderH,
            mat
        );

        Texture2D final = new Texture2D(targetSize, targetSize, TextureFormat.RGBA32, false);

        UnityEngine.Color clear = new UnityEngine.Color(0, 0, 0, 0);
        for (int i = 0; i < final.width * final.height; i++)
            final.SetPixel(i % targetSize, i / targetSize, clear);

        int offsetX = (targetSize - renderW) / 2;
        int offsetY = (targetSize - renderH) / 2;

        for (int y = 0; y < renderH; y++)
            for (int x = 0; x < renderW; x++)
                final.SetPixel(x + offsetX, y + offsetY, rendered.GetPixel(x, y));

        final.Apply();
        return final;
    }
}