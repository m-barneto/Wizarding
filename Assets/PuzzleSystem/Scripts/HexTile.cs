using UnityEngine;

public class HexTile : MonoBehaviour {
    public Vector2Int axial;
    public Aspect aspect;
    public bool locked;
    public AspectPuzzleGraph graph;

    [SerializeField]
    SpriteRenderer aspectSpriteRenderer;
    [SerializeField]
    Renderer tileMaterialRenderer;

    [SerializeField]
    SpriteRenderer wispSpriteRenderer;

    [SerializeField]
    Material lockedMaterial;
    [SerializeField]
    Material unlockedMaterial;

    public void SetAspect(Aspect aspect) {
        this.aspect = aspect;
        this.aspectSpriteRenderer.sprite = aspect?.icon;
        this.aspectSpriteRenderer.material.SetColor("_EmissionColor", new Color(1f, 1f, 1f) * 5f);
        this.graph.UpdateTileAspect(this);


        /*if (aspect == null) {
            this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        } else {
            this.transform.position = new Vector3(this.transform.position.x, -.19f, this.transform.position.z);
        }*/
    }

    public void SetLocked(bool locked) {
        this.locked = locked;
        tileMaterialRenderer.material = locked ? lockedMaterial : unlockedMaterial;        
    }

    public void ShowWisp(bool showWisp) {
        this.wispSpriteRenderer.enabled = showWisp;
    }

    public ResearchPuzzle.TileData TileData() {
        var tileData = new ResearchPuzzle.TileData() {
            q = axial.x,
            r = axial.y,
            locked = locked,
            aspectId = aspect ? aspect.aspectName : "empty"
        };
        return tileData;
    }

    public override string ToString() {
        return $"Aspect: {aspect?.aspectName}\nIsLocked: {locked}";
    }
}
