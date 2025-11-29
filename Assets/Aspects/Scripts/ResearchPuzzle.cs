using System.Collections.Generic;

[System.Serializable]
public class ResearchPuzzle {
    public int gridRadius;
    public List<TileData> tiles;

    [System.Serializable]
    public class TileData {
        public int q;
        public int r;
        public string aspectId;
        public bool locked;
    }
}
