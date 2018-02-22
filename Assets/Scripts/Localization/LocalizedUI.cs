using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalizedUI : MonoBehaviour
{
	[SerializeField]
	private string _Key;

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
