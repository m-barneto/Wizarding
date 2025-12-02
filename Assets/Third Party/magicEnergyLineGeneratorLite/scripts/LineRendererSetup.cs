#if UNITY_EDITOR
using UnityEditor;
#endif 
using UnityEngine;

namespace soulercoasterLite.scripts {
    public static class LineRendererSetup {
        
        public static void setupLineRenderer(this MonoBehaviour monoBehaviour) {
            if (monoBehaviour.GetComponent<LineRenderer>().sharedMaterial != null) {
                return;
            }

            Material mat = Resources.GetBuiltinResource<Material>("defaultLineMaterial t:material");

            monoBehaviour.GetComponent<LineRenderer>().sharedMaterial = mat;
            monoBehaviour.GetComponent<LineRenderer>().useWorldSpace = false;
        }
    }
}