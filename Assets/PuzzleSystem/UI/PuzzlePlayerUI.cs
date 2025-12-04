using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.VisualElement;

[RequireComponent(typeof(UIDocument))]
public class PuzzlePlayerUI : MonoBehaviour {
    [SerializeField]
    PuzzlePlayer puzzlePlayer;

    public Vector2 tooltipOffset = new Vector2(12, 18);
    VisualElement tooltip;
    Label tooltipLabel;
    bool tooltipVisible = false;
    Image parentA, parentB, parentAdd;
    VisualElement parentContainer;

    Texture2D[] icons;
    Material spriteMaterial;

    UIDocument doc;
    VisualElement iconGrid;

    Aspect selectedAspect;
    VisualElement selectedTile;

    void Awake() {
        spriteMaterial = Resources.Load<Material>("Materials/Aspect Grid Icon Material");
        icons = AspectDatabase.Instance.aspects.Select(x => HexUtils.RenderSpritePreserveAspect(x.icon, 64, spriteMaterial)).ToArray();
        selectedAspect = AspectDatabase.Instance.aspects[0];

        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        // find elements
        iconGrid = root.Q<VisualElement>("iconGrid");

        tooltip = root.Q<VisualElement>("tooltip");
        tooltipLabel = root.Q<Label>("tooltipLabel");
        tooltip.pickingMode = PickingMode.Ignore;

        parentContainer = tooltip.Q<VisualElement>("parent-icons");
        parentA = tooltip.Q<Image>("parentA");
        parentB = tooltip.Q<Image>("parentB");
        parentAdd = tooltip.Q<Image>("parentAdd");
        parentAdd.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("UI/horizontal-flip"));

        // wire top buttons
        var btnOpen = root.Q<Button>("btnOpen");
        var btnLoad = root.Q<Button>("btnLoad");
        var btnSave = root.Q<Button>("btnSave");
        if (btnOpen != null) btnOpen.clicked += () => puzzlePlayer.OpenEditor();
        if (btnLoad != null) btnLoad.clicked += () => {
            puzzlePlayer.LoadPuzzle();
        };

        // populate icon grid
        PopulateGrid();
    }

    void PopulateGrid() {
        iconGrid.Clear();
        if (icons == null) return;

        for (int i = 0; i < icons.Length; i++) {
            var tile = new Button();
            tile.AddToClassList("iconTile");
            // create a background image element
            var img = new VisualElement();
            img.AddToClassList("iconImage");

            img.style.backgroundImage = new StyleBackground(icons[i]);

            tile.Add(img);

            // click handler
            int idx = i;
            tile.clicked += () => OnIconClicked(tile, idx);

            // tooltip on hover
            tile.RegisterCallback<PointerEnterEvent>(evt => ShowTooltip(AspectDatabase.Instance.aspects[idx], evt));
            tile.RegisterCallback<PointerLeaveEvent>(evt => HideTooltip());
            tile.RegisterCallback<PointerMoveEvent>(evt => {
                if (tooltipVisible) {
                    UpdateTooltip(evt.position);
                }
            });

            iconGrid.Add(tile);
        }
        iconGrid[0].AddToClassList("iconTile-selected");
        selectedTile = iconGrid[0];
    }

    void ShowTooltip(Aspect aspect, PointerEnterEvent evt) {
        if (tooltip == null) return;
        tooltipLabel.text = aspect.aspectName.FirstCharacterToUpper();
        tooltip.style.display = DisplayStyle.Flex;
        tooltipVisible = true;

        parentA.style.backgroundImage = new StyleBackground(aspect.parent1?.icon);
        parentB.style.backgroundImage = new StyleBackground(aspect.parent2?.icon);

        if (aspect.IsPrimal) {
            parentContainer.style.display = DisplayStyle.None;
            parentA.visible = false;
            parentB.visible = false;
            parentAdd.visible = false;
        } else {
            parentContainer.style.display = DisplayStyle.Flex;
            parentA.visible = true;
            parentB.visible = true;
            parentAdd.visible = true;
        }

        UpdateTooltip(evt.position);
    }

    void HideTooltip() {
        if (tooltip == null) return;
        tooltip.style.display = DisplayStyle.None;
        tooltipVisible = false;
    }

    void UpdateTooltip(Vector2 panelPos) {
        float x = panelPos.x + tooltipOffset.x;
        float y = panelPos.y + tooltipOffset.y;

        float tooltipWidth = tooltip.resolvedStyle.width;
        float tooltipHeight = tooltip.resolvedStyle.height;

        // Sometimes resolvedStyle width is 0 before layout; use approximate measured size
        if (tooltipWidth <= 0)
            tooltipWidth = tooltipLabel.MeasureTextSize(tooltipLabel.text, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined).x + 12;
        if (tooltipHeight <= 0)
            tooltipHeight = tooltipLabel.MeasureTextSize(tooltipLabel.text, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined).y + 8;

        // Apply absolute position (panel coordinates map to root's style.left/top)
        tooltip.style.left = x;
        tooltip.style.top = y;
    }

    void OnIconClicked(VisualElement tile, int idx) {
        if (selectedTile != null) {
            selectedTile.RemoveFromClassList("iconTile-selected");
        }
        tile.AddToClassList("iconTile-selected");

        selectedTile = tile;
        selectedAspect = AspectDatabase.Instance.aspects[idx];
    }

    public Aspect GetSelectedAspect() {
        return selectedAspect;
    }
}