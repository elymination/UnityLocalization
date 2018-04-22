using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;


public static class LocalizationService
{
    private static List<string> mLanguages;
    private static string mCurrentLanguage;
    private static Dictionary<string, Dictionary<string, string>> mValues;
    private static Dictionary<int, string> mLanguagesIndex;

    static LocalizationService()
    {
        mLanguages = new List<string>();
        mLanguagesIndex = new Dictionary<int, string>();
    }

    public static Dictionary<int, string> GetLanguageIndex()
    {
        return mLanguagesIndex;
    }

    public static List<string> GetLanguages()
    {
        return mLanguages;
    }

    public static string GetCurrentLanguage()
    {
        return mCurrentLanguage;
    }

    public static void SetLanguage(string pLanguage)
    {
        foreach (string lLanguage in mLanguages)
        {
            if (lLanguage.Equals(pLanguage))
            {
                mCurrentLanguage = lLanguage;
                break;
            }
        }
    }

    static void Load(StreamReader pStreamReader)
    {
        mValues = new Dictionary<string, Dictionary<string, string>>();
        mLanguagesIndex = new Dictionary<int, string>();
        mLanguages.Clear();

        string lLine;
        string lFirstLine = pStreamReader.ReadLine();
        List<string> lFirstLineCells = lFirstLine.Split(';').ToList();
        lFirstLineCells.RemoveAt(0);
        int lIndex = 0;
        foreach (string lCell in lFirstLineCells)
        {
            if (!mLanguages.Contains(lCell))
            {
                mValues.Add(lCell, new Dictionary<string, string>());
                mLanguagesIndex.Add(lIndex++, lCell);
                mLanguages.Add(lCell);
            }
        }

        while ((lLine = pStreamReader.ReadLine()) != null)
        {
            List<string> lCells = lLine.Split(';').ToList();
            string lKeyName = lCells[0];
            lCells.RemoveAt(0);
            int lCellIndex = 0;
            foreach (string lCell in lCells)
            {
                if (!lCell.Equals(string.Empty))
                {
                    string lLanguage = mLanguagesIndex[lCellIndex++];
                    mValues[lLanguage][lKeyName] = lCell;
                }
            }
        }
        pStreamReader.Close();
    }

    public static void LoadData(string pData)
    {
        MemoryStream myStream = new MemoryStream(Encoding.UTF8.GetBytes(pData));
        StreamReader lReader = new System.IO.StreamReader(myStream);
        Load(lReader);
    }

    public static void LoadCSV(string pFileName)
    {
        System.IO.StreamReader lReader = new System.IO.StreamReader(pFileName);
        Load(lReader);
    }

    public static string GetLocalizedString(string pKeyName)
    {
        if (mValues[mCurrentLanguage].ContainsKey(pKeyName))
        {
            return mValues[mCurrentLanguage][pKeyName];
        }

        return pKeyName;
    }

    public static string GetLocalizedString(string pKeyName, string pLanguage)
    {
        if (mValues[pLanguage].ContainsKey(pKeyName))
        {
            return mValues[pLanguage][pKeyName];
        }
        return pKeyName;
    }


    public static List<string> GetKeys()
    {
        return mValues.FirstOrDefault().Value.Keys.ToList<string>();
    }

    public static Dictionary<string, Dictionary<string, string>> GetValues()
    {
        return mValues;
    }

    public static void Update(Dictionary<string, Dictionary<string, string>> pValues)
    {
        mValues = pValues;
    }
}