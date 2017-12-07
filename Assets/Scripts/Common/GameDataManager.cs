using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[MonoSingletonUsage]
public class GameDataManager : MonoSingleton<GameDataManager>
{
    private const string fileName = "TDGameData.json";

    private string path;

    private GameDataSetting _gameDataSetting;

    protected override void OnCreate()
    {
        base.OnCreate();

        path = Application.persistentDataPath + "/" + fileName;
        if (!File.Exists(path))
            File.WriteAllText(path, FileHelper.LoadStreamingAssetsFile(fileName).text);   
        
        ReadSetting();
    }

    private void ReadSetting()
    {
        using (StreamReader sr = File.OpenText(path))
        using (JsonTextReader jtr = new JsonTextReader(sr))
        {
            JsonSerializer serializer = new JsonSerializer();
            _gameDataSetting = serializer.Deserialize<GameDataSetting>(jtr);
        }
    }

    private void SaveSetting()
    {
        using (StreamWriter sw = File.CreateText(path))
        using (JsonTextWriter jtw = new JsonTextWriter(sw))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(jtw, _gameDataSetting);
        }
    }

    public SoundSetting GetSoundSetting()
    {
        return _gameDataSetting.soundSetting;
    }

    public void SaveSoundSetting(SoundSetting soundSetting)
    {
        _gameDataSetting.soundSetting = soundSetting;
        SaveSetting();
    }

    public LanguageSetting GetLanguageSetting()
    {
        return _gameDataSetting.languageSetting;
    }

    public void SetLanaguageSetting(LanguageSetting languageSetting)
    {
        _gameDataSetting.languageSetting = languageSetting;
        SaveSetting();
    }
}

public class GameDataSetting
{
    public SoundSetting soundSetting;

    public LanguageSetting languageSetting;
}

public struct SoundSetting
{
    public float bgVolume;

    public float fxVolume;

    public float speakVolume;
}

public struct LanguageSetting
{
    public string languageName;
}