using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Regular localized UI text. Add this component of a GameObject that has a Text component to make it localized.
/// </summary>
[RequireComponent(typeof(UnityEngine.UI.Text))]
[DisallowMultipleComponent]
public class LocalizedUI : MonoBehaviour
{
	[SerializeField]
	string _Key;

    Text _Text;

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
        _Text = gameObject.GetComponent<Text>();
	    _Text.text = LocalizationService.GetLocalizedString(_Key);
	}

	public void SetText()
	{
		_Text.text = LocalizationService.GetLocalizedString(_Key);
	}
}
