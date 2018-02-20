using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVEditorWindow : EditorWindow
{
    private static TextAsset mEditedFile;
    private static Dictionary<string, Dictionary<string, string>> mCopiedValues;
    private static Vector2 mScrollPosition;

    public static void LoadFile(TextAsset pFile)
    {
        mEditedFile = pFile;
    }

    public static void ShowWindow()
    {
        LocalizationService.LoadCSV(AssetDatabase.GetAssetPath(mEditedFile));
        mCopiedValues = LocalizationService.GetValues();
        GetWindow<CSVEditorWindow>(false, "CSV Editor", true);
    }

    void OnGUI()
    {
        List<string> lLanguages = LocalizationService.GetLanguages();
        List<string> lKeys = LocalizationService.GetKeys();
        if (GUILayout.Button("Update"))
        {
            LocalizationService.Update(mCopiedValues);
        }
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