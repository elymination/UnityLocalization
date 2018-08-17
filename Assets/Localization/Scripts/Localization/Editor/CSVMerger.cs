using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVMerger
{
    [MenuItem("Assets/CSV Editor/Merge CSV Files")]
    private static void MergeCSV()
    {
        string[] lSelected = Selection.assetGUIDs;
        HashSet<string> lFiles = new HashSet<string>();
        string lLanguageLine = string.Empty;
        string lMergedData = string.Empty;

        foreach (string lItem in lSelected)
        {
            lFiles.Add(AssetDatabase.GUIDToAssetPath(lItem));
        }
        foreach (string lFile in lFiles)
        {
            TextAsset asset = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(lFile));
            if (asset == null)
            {
                Debug.LogError("Merge cannot be done. Make sure each file in the the Resources folder.");
                return;
            }
            MemoryStream myStream = new MemoryStream(Encoding.UTF8.GetBytes(asset.text));
            StreamReader lReader = new System.IO.StreamReader(myStream);
            string lFirstLine = lReader.ReadLine();
            if (lLanguageLine.Equals(string.Empty) == false)
            {
                if (lLanguageLine.Equals(lFirstLine) == false)
                {
                    Debug.LogError("Merge cannot be done. Make sure each file has the same languages (in the same order).");
                    return;
                }
            }
            else
            {
                lLanguageLine = lFirstLine;
            }
            lReader.Close();
        }
        lMergedData += lLanguageLine + "\n";
        foreach (string lFile in lFiles)
        {
            TextAsset asset = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(lFile));
            MemoryStream myStream = new MemoryStream(Encoding.UTF8.GetBytes(asset.text));
            StreamReader lReader = new System.IO.StreamReader(myStream);
            lReader.ReadLine();
            string lLine = string.Empty;
            while ((lLine = lReader.ReadLine()) != null)
            {
                lMergedData += lLine;
                lMergedData += "\n";
            }
            lReader.Close();
        }
        lMergedData = lMergedData.Substring(0, lMergedData.Length - 1);

        string lFilePath = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(lSelected[0])) + "/LocalizationFile.csv";
        ProjectWindowUtil.CreateAssetWithContent(lFilePath, lMergedData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/CSV Editor/Merge CSV Files", true)]
    private static bool ValidateMergeCSV()
    {
        string[] lSelected = Selection.assetGUIDs;
        if (lSelected.Length < 2)
        {
            return false;
        }
        foreach (string lItem in lSelected)
        {
            string lFileName = AssetDatabase.GUIDToAssetPath(lItem);
            string lExtension = Path.GetExtension(lFileName);
            if (lExtension != ".csv")
            {
                return false;
            }
        }
        return true;
    }
}
