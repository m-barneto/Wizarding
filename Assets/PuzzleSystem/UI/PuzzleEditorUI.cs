using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.VisualElement;

[RequireComponent(typeof(UIDocument))]
public class PuzzleEditorUI : MonoBehaviour {
    [SerializeField]
    PuzzleEditor puzzleEditor;

    public Vector2 tooltipOffset = new Vector2(12, 18);
    Label tooltip;
    bool tooltipVisible = false;

    Texture2D[] icons;
    Material spriteMaterial;

    UIDocument doc;
    VisualElement iconGrid;
    Label radiusValue;
    Slider radiusSlider;

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
        radiusValue = root.Q<Label>("lblGridRadius");
        radiusSlider = root.Q<Slider>("radiusSlider");
        radiusSlider.value = puzzleEditor.GetGridRadius();

        tooltip = new Label();
        tooltip.AddToClassList("tooltip");
        tooltip.style.position = Position.Absolute;
        tooltip.style.display = DisplayStyle.None;
        tooltip.pickingMode = PickingMode.Ignore;
        tooltip.style.unityTextAlign = TextAnchor.MiddleLeft;

        root.Add(tooltip);

        if (radiusSlider != null && radiusValue != null) {
            radiusSlider.RegisterValueChangedCallback(evt => {
                int val = Mathf.RoundToInt(evt.newValue);
                radiusValue.text = $"Grid Radius: {val.ToString()}";
                radiusSlider.value = val;
                puzzleEditor.SetGridRadius(val);
            });
            radiusValue.text = $"Grid Radius: {Mathf.RoundToInt(radiusSlider.value).ToString()}";
        }

        // wire top buttons
        var btnOpen = root.Q<Button>("btnOpen");
        var btnLoad = root.Q<Button>("btnLoad");
        var btnSave = root.Q<Button>("btnSave");
        if (btnOpen != null) btnOpen.clicked += () => puzzleEditor.OpenPlayer();
        if (btnLoad != null) btnLoad.clicked += () => {
            puzzleEditor.LoadPuzzle();
            radiusSlider.value = puzzleEditor.GetGridRadius();
        };
        if (btnSave != null) btnSave.clicked += () => puzzleEditor.SavePuzzle();

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
            tile.RegisterCallback<PointerEnterEvent>(evt => ShowTooltip(AspectDatabase.Instance.aspects[idx].aspectName.FirstCharacterToUpper(), evt));
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

    void ShowTooltip(string text, PointerEnterEvent evt) {
        if (tooltip == null) return;
        tooltip.text = text;
        tooltip.style.display = DisplayStyle.Flex;
        tooltipVisible = true;

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
            tooltipWidth = tooltip.MeasureTextSize(tooltip.text, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined).x + 12;
        if (tooltipHeight <= 0)
            tooltipHeight = tooltip.MeasureTextSize(tooltip.text, 0, MeasureMode.Undefined, 0, MeasureMode.Undefined).y + 8;

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