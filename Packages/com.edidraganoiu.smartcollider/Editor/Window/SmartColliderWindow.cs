using UnityEditor;
using UnityEngine;

namespace SmartCollider.Editor.Window
{
    public class SmartColliderWindow : EditorWindow
    {
        [MenuItem("Tools/SmartCollider")]
        public static void ShowWindow()
        {
            GetWindow<SmartColliderWindow>("SmartCollider");
        }

        private void OnGUI()
        {
            GUILayout.Label("SmartCollider Options");

            if (GUILayout.Button("Analyze & Generate Colliders"))
            {
                Debug.Log("Clicked");
            }
        }
    }
}
