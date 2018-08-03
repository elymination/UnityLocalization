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
            SetText();
        }
    }

    void Start()
    {
        _Text = gameObject.GetComponent<TextMeshProUGUI>();
        if (_Text != null)
        {
            _Text.text = LocalizationService.GetLocalizedString(_Key);
        }
    }

    public void SetText()
    {
        if (_Text != null)
        {
            _Text.text = LocalizationService.GetLocalizedString(_Key);
        }
    }
}
