using SFB;
using System.IO;
using UnityEngine;
using static ResearchPuzzle;

public class PuzzleIO {
    public static string GameDataPath() {
        Directory.CreateDirectory(Path.Combine(Application.dataPath, "..", "GameData"));
        return Path.Combine(Application.dataPath, "..", "GameData");
    }

    public static void SavePuzzle(ResearchPuzzle data) {
        string path = StandaloneFileBrowser.SaveFilePanel(
            "Save Puzzle",
            GameDataPath(),
            "new_puzzle",
            "json"
        );

        if (!string.IsNullOrEmpty(path)) {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
    }

    public static ResearchPuzzle LoadPuzzle() {
        var extensions = new[] {
            new ExtensionFilter("Puzzle Files", "json")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Load Puzzle File",
            GameDataPath(),
            "json",
            false
        );

        if (paths.Length > 0) {
            string json = File.ReadAllText(paths[0]);
            ResearchPuzzle data = JsonUtility.FromJson<ResearchPuzzle>(json);

            return data;
        }
        return null;
    }
}
