using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVEditorWindow : EditorWindow
{
    private static TextAsset mEditedFile;
    private static Dictionary<string, Dictionary<string, string>> mCopiedValues;
    private static Vector2 mScrollPosition;
    private static FileSystemWatcher mFileSystemWatcher;
    private static string mResourcesPath;
    private static string mAssetPath;

    public static void LoadFile(TextAsset pFile)
    {
        mEditedFile = pFile;
    }

    public static void ShowWindow()
    {
        mResourcesPath = Application.dataPath + "/Resources/";
        mAssetPath = AssetDatabase.GetAssetPath(mEditedFile);
        LocalizationService.LoadCSV(mAssetPath);
        mCopiedValues = LocalizationService.GetValues();
        GetWindow<CSVEditorWindow>(false, "CSV Editor", true);
        mFileSystemWatcher = new FileSystemWatcher();
        mFileSystemWatcher.Path = mResourcesPath;
        mFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        mFileSystemWatcher.Filter = mEditedFile.name + ".csv";
        mFileSystemWatcher.Changed += OnFileChanged;
        mFileSystemWatcher.EnableRaisingEvents = true;
    }

    private static void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        LocalizationService.LoadCSV(mAssetPath);
        mCopiedValues = LocalizationService.GetValues();
    }

    void OnGUI()
    {
        List<string> lLanguages = LocalizationService.GetLanguages();
        List<string> lKeys = LocalizationService.GetKeys();
        mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Empty, GUILayout.Width(200));
        foreach (string lLanguage in lLanguages)
        {
            GUILayout.Label(lLanguage, GUILayout.Width(200));
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
        foreach (string lKey in lKeys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(lKey, GUILayout.Width(200));

            foreach (string lLanguage in lLanguages)
            {
                mCopiedValues[lLanguage][lKey] = GUILayout.TextField(mCopiedValues[lLanguage][lKey], GUILayout.Width(200));
            }
            GUILayout.EndHorizontal();

        }
        EditorGUILayout.EndScrollView();
    }
}
