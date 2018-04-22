using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CSVEditorWindow : EditorWindow
{
    private static TextAsset mEditedFile;
    private static Dictionary<string, Dictionary<string, string>> mCopiedValues;
    private static Vector2 mScrollPosition;
    private static Vector2 mSecondScrollPosition;
    private static FileSystemWatcher mFileSystemWatcher;
    private static string mResourcesPath;
    private static string mAssetPath;
    private static string mKeyName;
    private static List<string> mLanguages;
    private static List<string> mKeys;

    private static Dictionary<string, Dictionary<string, string>> mAddedKeys;

    public static void LoadFile(TextAsset pFile)
    {
        mEditedFile = pFile;
    }

    public static void ShowWindow()
    {
        mResourcesPath = Application.dataPath + "/Resources/";
        mAssetPath = AssetDatabase.GetAssetPath(mEditedFile);
        LocalizationService.LoadCSV(mAssetPath);
        mLanguages = LocalizationService.GetLanguages();
        mKeys = LocalizationService.GetKeys();
        mCopiedValues = LocalizationService.GetValues();
        GetWindow<CSVEditorWindow>(false, "CSV Editor", true);
        mFileSystemWatcher = new FileSystemWatcher();
        mFileSystemWatcher.Path = mResourcesPath;
        mFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        mFileSystemWatcher.Filter = mEditedFile.name + ".csv";
        mFileSystemWatcher.Changed += OnFileChanged;
        mFileSystemWatcher.EnableRaisingEvents = true;
        mKeyName = string.Empty;
        mAddedKeys = new Dictionary<string, Dictionary<string, string>>();
    }

    private static void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        bool lFileIsLocked = true;
        while (lFileIsLocked == true)
        {
            try
            {
                // Attempt to open the file exclusively.
                using (FileStream fs = new FileStream(mAssetPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                {
                    fs.ReadByte();
                    lFileIsLocked = false;
                }
            }
            finally
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
        LocalizationService.LoadCSV(mAssetPath);
        mLanguages = LocalizationService.GetLanguages();
        mKeys = LocalizationService.GetKeys();
        mCopiedValues = LocalizationService.GetValues();
    }

    void OnGUI()
    {
        mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Empty, GUILayout.Width(200));
        foreach (string lLanguage in mLanguages)
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(lLanguage);
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
        foreach (string lKey in mKeys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Button("-", GUILayout.Width(15));
            GUILayout.Label(lKey, GUILayout.Width(200));

            foreach (string lLanguage in mLanguages)
            {
                mCopiedValues[lLanguage][lKey] = GUILayout.TextField(mCopiedValues[lLanguage][lKey], GUILayout.Width(200));
            }
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        // If temporary keys are created
        if (mAddedKeys.Count > 0)
        {
            mSecondScrollPosition = EditorGUILayout.BeginScrollView(mSecondScrollPosition);
            mSecondScrollPosition.x = mScrollPosition.x;
            foreach (string lKey in mAddedKeys.Keys)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Button("-", GUILayout.Width(15));
                GUILayout.Label(lKey, GUILayout.Width(200));
                foreach (string lLanguage in mLanguages)
                {
                    mAddedKeys[lKey][lLanguage] = GUILayout.TextField(mAddedKeys[lKey][lLanguage], GUILayout.Width(200));
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.BeginHorizontal();
        mKeyName = GUILayout.TextField(mKeyName, GUILayout.Width(200));
        if (GUILayout.Button("Add", GUILayout.Width(50)) == true)
        {
            if (mKeys.Contains(mKeyName) || mAddedKeys.Keys.Contains(mKeyName))
            {
                return;
            }
            Dictionary<string, string> lDictionary = new Dictionary<string, string>();
            foreach (string lLanguage in mLanguages)
            {
                lDictionary.Add(lLanguage, string.Empty);
            }
            mAddedKeys.Add(mKeyName, lDictionary);
            mKeyName = string.Empty;
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Save CSV", GUILayout.Width(200)) == true)
        {
            // Write to file
            using (StreamWriter sw = File.AppendText(mAssetPath))
            {
                bool lHasEmptyKey = false;
                string lFullText = string.Empty;
                foreach (string lKey in mAddedKeys.Keys)
                {
                    string lText = string.Empty;
                    lText += lKey;
                    lText += ";";
                    foreach (string lLanguage in mLanguages)
                    {
                        if (mAddedKeys[lKey][lLanguage] != string.Empty)
                        {
                            lText += mAddedKeys[lKey][lLanguage];
                        }
                        else
                        {
                            lHasEmptyKey = true;
                            break;
                        }
                        lText += ";";
                    }
                    if (lHasEmptyKey == true)
                    {
                        break;
                    }
                    lText = lText.Substring(0, lText.Length - 1);
                    lFullText += lText + "\n";
                }
                if (lHasEmptyKey == false)
                {
                    sw.WriteLine(lFullText);
                    mAddedKeys = new Dictionary<string, Dictionary<string, string>>();
                }
            }
        }
    }
}