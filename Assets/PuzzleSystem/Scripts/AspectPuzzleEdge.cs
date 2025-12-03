using Unity.VisualScripting;
using UnityEngine;

public class AspectPuzzleEdge : MonoBehaviour {
    public AspectPuzzleGraph.HexTileNode a;
    public AspectPuzzleGraph.HexTileNode b;
    LineRenderer lineRenderer;
    float placedTime;

    private void Awake() {
        lineRenderer = this.AddComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("Materials/Connection Material");
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.sortingOrder = -1;

        transform.rotation = Quaternion.LookRotation(Vector3.up);

        placedTime = Time.time;
    }

    public void SetLineRendererPositions() {
        lineRenderer.SetPosition(0, a.tile.transform.position + Vector3.up * 0.01f);
        lineRenderer.SetPosition(1, b.tile.transform.position + Vector3.up * 0.01f);
    }

    private void Update() {
        lineRenderer.material.mainTextureOffset = new Vector2(placedTime + Time.time * -0.5f, 0f);
    }
}