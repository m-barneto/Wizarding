using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AspectDatabase", menuName = "Research/Aspect Database")]
public class AspectDatabase : ScriptableObject {
    static AspectDatabase _instance;
    public static AspectDatabase Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<AspectDatabase>("Scriptable Objects/AspectDatabase");
            }
            return _instance;
        }
    }

    public List<Aspect> aspects;

    public Aspect GetByName(string name) {
        return aspects.Find(a => a.aspectName == name);
    }

    public bool CanLink(Aspect a, Aspect b) {
        // basic rule: aspects connect if they share a parent or are parent-child
        if (a == b) return true;
        if (a.HasParent(b) || b.HasParent(a)) return true;
        if (a.parent1 == b || a.parent2 == b) return true;
        if (b.parent1 == a || b.parent2 == a) return true;

        return false;
    }
}
