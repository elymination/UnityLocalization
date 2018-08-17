using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    TextAsset _LocalizationFile;
    [SerializeField]
    string _Language;

    public string Language
    {
        set
        {
            _Language = value;
            LocalizationService.SetLanguage(value);
            UpdateLocalizedUI();
        }
    }

    void Awake()
    {
        if (_LocalizationFile == null)
        {
            return;
        }
        LocalizationService.LoadData(_LocalizationFile.text);
        LocalizationService.SetLanguage(_Language);
    }

    void UpdateLocalizedUI()
    {
        LocalizedUI[] lLocalized = FindObjectsOfType<LocalizedUI>();
        foreach (LocalizedUI lUI in lLocalized)
        {
            lUI.SetText();
        }
    }
}
