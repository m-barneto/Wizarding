using UnityEngine;

public class PuzzlePlayer : MonoBehaviour {
    PuzzleEditor puzzleEditor;
    ResearchPuzzle puzzle = null;


    void Awake() {
        puzzleEditor = GetComponent<PuzzleEditor>();
    }

    private void OnGUI() {
        GUIStyle myStyle = new GUIStyle(GUI.skin.button);
        myStyle.contentOffset = new Vector2(15, 15);
        GUILayout.BeginArea(new Rect(0, 0, 300, 600), myStyle);

        if (GUILayout.Button("Open Editor")) {
            this.enabled = false;
            puzzleEditor.enabled = true;
        }

        if (GUILayout.Button("Load Puzzle")) {
            puzzle = PuzzleIO.LoadPuzzle();
            // Check if puzzle is null before actually loading into editor
        }



        GUILayout.EndArea();
    }
}
