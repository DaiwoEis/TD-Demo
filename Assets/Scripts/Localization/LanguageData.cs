using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class LanguageData
{
    public string LanguageName;

    [ShowInInspector]
    public Dictionary<string, string> LanguageTextDic;

    public string this[string key]
    {
        get { return LanguageTextDic[key]; }
    }
}
