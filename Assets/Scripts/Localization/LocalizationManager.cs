using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;
using Sirenix.OdinInspector;
using UnityEngine;

[MonoSingletonUsage]
public class LocalizationManager : MonoSingleton<LocalizationManager>
{
    [ShowInInspector]
    private Dictionary<string, LanguageData> _langeDatas;

    private LanguageData _currLanguageData;

    private const string _dataPath = "LanguageConfig.xlsx";

    private const string _scirptPath = "Scripts/Localization/EMultiLanguageID.cs";

    public event Action<string> OnLanaguageChanged; 

    public static string dataPath { get { return Application.streamingAssetsPath + "/" + _dataPath; } }

    public static string scirptPath { get { return Application.dataPath + "/" + _scirptPath; } }

    protected override void OnCreate()
    {
        base.OnCreate();
  
        LoadData();

        _currLanguageData = _langeDatas[GameDataManager.Instance.GetLanguageSetting().languageName];
    }

    public void LoadData()
    {
        _langeDatas = new Dictionary<string, LanguageData>();

        string path = Application.streamingAssetsPath + "/LanguageConfig.xlsx";
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var result = reader.AsDataSet();

                var table = result.Tables[0];

                var row0 = table.Rows[0].ItemArray;
                for (int i = 1; i < row0.Length; ++i)
                {
                    var languageName = row0[i].ToString();
                    var languageData = new LanguageData {LanguageName = languageName};
                    _langeDatas.Add(languageName, languageData);                     
                }

                for (int i = 1; i < table.Rows.Count; ++i)
                {
                    var row = table.Rows[i];
                    int col = 1;
                    foreach (var languageName in _langeDatas.Keys)
                    {
                        _langeDatas[languageName].LanguageTexts.Add(row[col].ToString());
                        col++;
                    }
                }
            }
        }
    }

    public string[] GetAllLanguages()
    {
        return _langeDatas.Keys.ToArray();
    }

    public string GetCurrLanguageName()
    {
        return _currLanguageData.LanguageName;
    }

    public void SetLanguage(string languageName)
    {
        if (!_langeDatas.ContainsKey(languageName))
        {
            Debug.LogWarning(string.Format("Don't contain lanaguage {0} data.", languageName));
            return;
        }

        _currLanguageData = _langeDatas[languageName];
        GameDataManager.Instance.SetLanaguageSetting(new LanguageSetting {languageName = languageName});
        UpdateTexts();
    }

    public string GetContent(EMultiLanguageContent id)
    {
        if (_currLanguageData != null)
            return _currLanguageData[id];

        return null;
    }

    private void UpdateTexts()
    {
        if (OnLanaguageChanged != null)
            OnLanaguageChanged(_currLanguageData.LanguageName);
    }

    private void ClearEvent()
    {
        OnLanaguageChanged = null;
    }
}
