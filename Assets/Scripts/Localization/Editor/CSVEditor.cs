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
        Object lSelected = Selection.activeObject;
        string lExtension = string.Empty;

        TextAsset asset = lSelected as TextAsset;
        if (asset != null)
        {
            lExtension = Path.GetExtension(AssetDatabase.GetAssetPath(lSelected));
        }
        return lSelected.GetType() == typeof(TextAsset) && lExtension.Equals(".csv");
    }
}