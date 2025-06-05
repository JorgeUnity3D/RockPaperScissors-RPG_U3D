using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Kapibara.Util.Editor 
{
    public class DataEditorWindow : OdinMenuEditorWindow {
        [MenuItem("RPS RPG Tools/Data Editor")]
        private static void OpenDataWindow() {
            GetWindow<DataEditorWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree() {
            OdinMenuTree tree = new OdinMenuTree();
            tree.AddAllAssetsAtPath("Player", "Assets/_RPS/ScriptableObjects/Player",
                typeof(SerializedScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Town", "Assets/_RPS/ScriptableObjects/Town",
                typeof(SerializedScriptableObject), true, true);
            return tree;
        }
    }
}