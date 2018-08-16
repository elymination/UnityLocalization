using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
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
        TextAsset FileData = Resources.Load<TextAsset>("Localization");
        LocalizationService.LoadData(FileData.text);
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
