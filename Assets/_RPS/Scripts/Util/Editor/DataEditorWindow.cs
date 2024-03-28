using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Kapibara.RPS {
    public class DataEditorWindow : OdinMenuEditorWindow {
        [MenuItem("RPS RPG Tools/Data Editor")]
        private static void OpenDataWindow() {
            GetWindow<DataEditorWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree() {
            OdinMenuTree tree = new OdinMenuTree();
            tree.AddAllAssetsAtPath("Game States", "Assets/_Game/ScriptableObjects/GameStates",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Player Data", "Assets/_Game/ScriptableObjects/Player",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Enemies Data", "Assets/_Game/ScriptableObjects/Enemies",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Languages", "Assets/_Game/ScriptableObjects/Languages",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Town Views", "Assets/_Game/ScriptableObjects/TownViews",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Town Views", "Assets/_Game/ScriptableObjects/TrainingStats",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Town Views", "Assets/_Game/ScriptableObjects/Items",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Map Levels", "Assets/_Game/ScriptableObjects/MapLevels",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Stats", "Assets/_Game/ScriptableObjects/Stats",
                typeof(SerializedScriptableObject), true, true);
            
            return tree;
        }
    }
}