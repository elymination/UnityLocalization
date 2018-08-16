using UnityEngine;

public class LocalizedResource<T> : MonoBehaviour where T : Object
{
    [SerializeField]
    protected string _resourceSubfolder;
    [SerializeField]
    protected string _Key;

    protected T GetResource()
    {
        string lLocalizedFileName = LocalizationService.GetLocalizedString(_Key);
        string lCleanedString = _resourceSubfolder.Replace("/", "\\");
        if (lCleanedString.EndsWith("\\") == false)
        {
            lCleanedString += "\\";
        }
        return Resources.Load<T>(lCleanedString + lLocalizedFileName);
    }
}