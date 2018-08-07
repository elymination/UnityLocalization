using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVEditor
{
    [MenuItem("Assets/Edit CSV file")]
    private static void EditCSV()
    {
        CSVEditorWindow.LoadFile(Selection.activeObject as TextAsset);
        CSVEditorWindow.ShowWindow();
    }

    [MenuItem("Assets/Edit CSV file", true)]
    private static bool ValidateEditCSV()
    {
        Object selected = Selection.activeObject; 
        if (selected == null)
        {
            return false;
        }
        string extension = string.Empty;
        TextAsset asset = selected as TextAsset;
        if (asset != null)
        {
            extension = Path.GetExtension(AssetDatabase.GetAssetPath(selected));
        }
        return selected.GetType() == typeof(TextAsset) && extension.Equals(".csv");
    }
}
