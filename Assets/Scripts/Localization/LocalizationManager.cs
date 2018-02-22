using System.Collections;
using System.IO;

using UnityEngine;

[System.Serializable]
public class LocalizationManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
	private string mLanguage;
	private string mTmpLanguage;

    [HideInInspector]
    [SerializeField]
    private string mFile;

	public string File
	{
		get
		{
			return mFile;
		}
		set
		{
            mFile = value;
		}
	}

	void Awake()
	{
		TextAsset lFileData = Resources.Load<TextAsset>(File);
        LocalizationService.LoadData(lFileData.text);
		LocalizationService.SetLanguage(mLanguage);
        mTmpLanguage = mLanguage;
	}
    
    public void SetLanguage(string pLanguage)
    {
        mLanguage = pLanguage;
    }

	void Update()
	{
		if (mTmpLanguage != mLanguage)
		{
			LocalizationService.SetLanguage(mLanguage);
            mTmpLanguage = mLanguage;
			LocalizedUI[] lLocalized = FindObjectsOfType<LocalizedUI>();
			foreach (LocalizedUI lUI in lLocalized)
			{
				lUI.SetText();
			}
		}
	}
}
