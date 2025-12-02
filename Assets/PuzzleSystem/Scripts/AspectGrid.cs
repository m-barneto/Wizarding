using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AspectGrid {
    static AspectGrid _instance;
    public static AspectGrid Instance {
        get {
            if (_instance == null) {
                _instance = new AspectGrid();
            }
            return _instance;
        }
    }

    GUIStyle iconStyle;
    Texture2D tooltipBackground;
    GUIStyle tipStyle;
    GUIContent[] aspectIcons;
    Material spriteMaterial;
    int aspectSelection = 0;

    AspectGrid() {
        spriteMaterial = Resources.Load<Material>("Materials/Aspect Grid Icon Material");

        tooltipBackground = new Texture2D(2, 2);
        Color[] pixels = new Color[2 * 2];
        for (int i = 0; i < 2 * 2; i++) {
            pixels[i] = new Color(.2f, .2f, .2f);
        }
        tooltipBackground.SetPixels(pixels);
        tooltipBackground.Apply();
        tooltipBackground.hideFlags = HideFlags.DontUnloadUnusedAsset;

        tipStyle = new GUIStyle(GUI.skin.box);
        tipStyle.normal.textColor = Color.white;
        tipStyle.fontStyle = FontStyle.Bold;
        tipStyle.alignment = TextAnchor.MiddleLeft;
        tipStyle.fontSize = 24;
        tipStyle.padding = new RectOffset(6, 6, 4, 4);
        tipStyle.normal.background = tooltipBackground;
    }
    public void ShowAspectGrid() {
        if (iconStyle == null) {
            iconStyle = new GUIStyle(GUI.skin.button);
            iconStyle.fixedWidth = 64;
            iconStyle.fixedHeight = 64;
            iconStyle.imagePosition = ImagePosition.ImageOnly;
            iconStyle.contentOffset = Vector2.zero;
        }
        if (aspectIcons == null) {
            aspectIcons = AspectDatabase.Instance.aspects.Select(x => new GUIContent(
                x.aspectName,
                HexUtils.RenderSpritePreserveAspect(x.icon, 64, spriteMaterial),
                x.aspectName)
            ).ToArray();
        }
        aspectSelection = GUI.SelectionGrid(new Rect(10, 175, 280, 300), aspectSelection, aspectIcons, 6, iconStyle);
    }

    public void ShowAspectTooltip() {
        string tip = GUI.tooltip.FirstCharacterToUpper();
        if (!string.IsNullOrEmpty(tip)) {
            Vector2 mouse = Event.current.mousePosition;
            Vector2 offset = new Vector2(18, 18);

            // Calculate size of box
            Vector2 size = tipStyle.CalcSize(new GUIContent(tip));

            GUI.Label(new Rect(mouse.x + offset.x, mouse.y + offset.y, size.x, size.y), tip, tipStyle);
        }
    }

    public Aspect GetSelectedAspect() {
        return AspectDatabase.Instance.aspects[aspectSelection];
    }
}
