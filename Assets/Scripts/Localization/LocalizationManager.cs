using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

[MonoSingletonUsage]
public class LocalizationManager : MonoSingleton<LocalizationManager>
{
    [ShowInInspector]
    private Dictionary<string, LanguageData> _langeDatas;

    private LanguageData _currLanguageData;

    private const string SettingName = "LocalizationSetting";

    public event Action<string> OnLanaguageChanged; 

    protected override void OnCreate()
    {
        base.OnCreate();

        _langeDatas = new Dictionary<string, LanguageData>();
        LocalizationSetting setting = JsonConvert.DeserializeObject<LocalizationSetting>(ResourceManager.Load<TextAsset>(SettingName).text);
        foreach (var resourcesName in setting.LanguageDataResourcesNames)
        {
            LanguageData data = JsonConvert.DeserializeObject<LanguageData>(ResourceManager.Load<TextAsset>(resourcesName).text);
            _langeDatas.Add(data.LanguageName, data);
        }

        _currLanguageData = _langeDatas[GameDataManager.Instance.GetLanguageSetting().languageName];
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
        UpdateContents();
    }

    public string GetContent(string key)
    {
        if (_currLanguageData != null)
            return _currLanguageData[key];

        return null;
    }

    private void UpdateContents()
    {
        if (OnLanaguageChanged != null)
            OnLanaguageChanged(_currLanguageData.LanguageName);
    }

    private void ClearEvent()
    {
        OnLanaguageChanged = null;
    }

    public class LocalizationSetting
    {
        public int LanguageCount;

        public string[] LanguageDataResourcesNames;
    }
}
