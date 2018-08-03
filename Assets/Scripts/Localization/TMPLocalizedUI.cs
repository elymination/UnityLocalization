using UnityEngine;
using TMPro;

/// <summary>
/// TextMeshPro version of LocalizedUI.
/// </summary>
[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
[DisallowMultipleComponent]
public class TMPLocalizedUI : MonoBehaviour
{
    [SerializeField]
    string _Key;

    TextMeshProUGUI _Text;

    public string Key
    {
        get
        {
            return _Key;
        }
        set
        {
            _Key = value;
        }
    }

    void Start()
    {
        _Text = gameObject.GetComponent<TextMeshProUGUI>();
        _Text.text = LocalizationService.GetLocalizedString(_Key);
    }

    public void SetText()
    {
        _Text.text = LocalizationService.GetLocalizedString(_Key);
    }
}
