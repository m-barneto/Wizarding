using UnityEngine;

[CreateAssetMenu(fileName = "Aspect", menuName = "Research/Aspect", order = 0)]
public class Aspect : ScriptableObject {
    [Header("Identity")]
    public string aspectName;
    public Sprite icon;

    [Header("Parents (base aspects have 0)")]
    public Aspect parent1;
    public Aspect parent2;

    // Helper: Is this a primal aspect?
    public bool IsPrimal => parent1 == null && parent2 == null;

    // Returns true if this aspect is made from another
    public bool HasParent(Aspect a) {
        return parent1 == a || parent2 == a;
    }

    // Thaumcraft-style "distance" in the research minigame
    public int Depth {
        get {
            int d1 = parent1 ? parent1.Depth : 0;
            int d2 = parent2 ? parent2.Depth : 0;
            return 1 + Mathf.Max(d1, d2);
        }
    }
}