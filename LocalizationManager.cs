using System.Collections;
using System.IO;

using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    string _Language;
    string _TmpLanguage;

    string _File;

    public string File
    {
        get
        {
            return _File;
        }
        set
        {
            _File = value;
        }
    }

    void Awake()
    {
        TextAsset FileData = Resources.Load<TextAsset>("Localization");
        LocalizationService.LoadData(FileData.text);
        LocalizationService.SetLanguage(_Language);
        _TmpLanguage = _Language;
    }

    public void SetLanguage(string pLanguage)
    {
        _Language = pLanguage;
    }

    void Update()
    {
        if (_TmpLanguage != _Language)
        {
            LocalizationService.SetLanguage(_Language);
            _TmpLanguage = _Language;
            LocalizedUI[] lLocalized = FindObjectsOfType<LocalizedUI>();
            foreach (LocalizedUI lUI in lLocalized)
            {
                lUI.SetText();
            }
        }
    }
}
