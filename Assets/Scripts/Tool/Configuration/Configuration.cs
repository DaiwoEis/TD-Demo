using System.Collections.Generic;

public class Configuration
{
    protected static Dictionary<string, Dictionary<string, string>> mDictionary = new Dictionary<string, Dictionary<string, string>>();

    public static void LoadConfig(string configPath)
    {
        mDictionary =
            ConfigurationReader.ReadConfigDic(FileHelper.LoadStreamingAssetsFile(configPath).text, splitor: '=');
    }

    public static string Get(string mainKey, string subKey)
    {
        if (mDictionary.ContainsKey(mainKey) && mDictionary[mainKey].ContainsKey(subKey))
            return mDictionary[mainKey][subKey];

        return mainKey + "." + subKey;
    }

    public static Dictionary<string, string> Get(string mainKey)
    {
        if (mDictionary.ContainsKey(mainKey))
            return mDictionary[mainKey];

        return null;
    }

    public static int GetInt(string mainKey, string subKey)
    {
        int ret;
        int.TryParse(Get(mainKey, subKey), out ret);
        return ret;
    }

    public static float GetFloat(string mainKey, string subKey)
    {
        float ret;
        float.TryParse(Get(mainKey, subKey), out ret);
        return ret;
    }

    public static int GetChildCount(string mainKey)
    {
        if (mDictionary.ContainsKey(mainKey))
        {
            int ChildCount = mDictionary[mainKey].Count;
            return ChildCount;
        }
        return 0;
    }

    public static string GetContent(string mainKey, string subKey)
    {
        string ret = Get(mainKey, subKey);
        if (ret.StartsWith("\"")) ret = ret.Substring(1, ret.Length - 1);
        if (ret.EndsWith(";")) ret = ret.Substring(0, ret.Length - 2);
        if (ret.EndsWith("\"")) ret = ret.Substring(0, ret.Length - 2);
        return ret;
    }
}