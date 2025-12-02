using UnityEngine;

public class HexTile : MonoBehaviour {
    public Vector2Int axial;
    public Aspect aspect;
    public bool locked;
    public AspectPuzzleGraph graph;

    SpriteRenderer spriteRenderer;
    Renderer tileRenderer;
    Transform wisp;

    [SerializeField]
    Material lockedMaterial;
    [SerializeField]
    Material unlockedMaterial;


    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        tileRenderer = GetComponentInChildren<Renderer>();
        wisp = GetComponentInChildren<Transform>();
    }

    public void SetAspect(Aspect aspect) {
        this.aspect = aspect;
        this.spriteRenderer.sprite = aspect?.icon;
        this.graph.UpdateTileAspect(this);


        if (aspect == null) {
            this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        } else {
            this.transform.position = new Vector3(this.transform.position.x, -.19f, this.transform.position.z);
        }
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
