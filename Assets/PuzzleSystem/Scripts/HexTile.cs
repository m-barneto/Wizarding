using UnityEngine;

public class HexTile : MonoBehaviour {
    public Vector2Int axial;
    public Aspect aspect;
    public bool locked;
    public AspectPuzzleGraph graph;

    SpriteRenderer spriteRenderer;
    Renderer tileRenderer;

    [SerializeField]
    Material lockedMaterial;
    [SerializeField]
    Material unlockedMaterial;


    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        tileRenderer = GetComponentInChildren<Renderer>();
    }

    public void SetAspect(Aspect aspect) {
        this.aspect = aspect;
        this.spriteRenderer.sprite = aspect == null ? null : aspect.icon;
        this.SetLocked(aspect == null ? false : true);
    }

    public void SetLocked(bool locked) {
        this.locked = locked;
        tileRenderer.material = locked ? lockedMaterial : unlockedMaterial;        
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
