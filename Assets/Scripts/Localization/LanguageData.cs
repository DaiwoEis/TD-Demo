using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class LanguageData
{
    public string LanguageName;

    [ShowInInspector]
    public List<string> LanguageTexts;

    public LanguageData()
    {        
        LanguageTexts = new List<string>();
    }

    public string this[EMultiLanguageContent id]
    {
        get { return LanguageTexts[(int)id]; }
    }
}
