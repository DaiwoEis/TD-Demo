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

    private const string SettingName = "LocalizationSetting";

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
    }

    public string[] GetAllLanguages()
    {
        return _langeDatas.Keys.ToArray();
    }

    public class LocalizationSetting
    {
        public int LanguageCount;

        public string[] LanguageDataResourcesNames;
    }
}
