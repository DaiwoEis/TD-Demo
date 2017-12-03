using System.Collections.Generic;
using ConfigDic = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>;
using System.IO;

public class ConfigurationReader
{
    public static ConfigDic ReadConfigDic(
        string configString,
        string mainKeyLeft = "[",
        string mainKeyRight = "]",
        char splitor = ':')
    {
        ConfigDic dic = new ConfigDic();
        StringReader reader = new StringReader(configString);
        string line;
        string mainKey = "";

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line == string.Empty) continue;

            if (line.StartsWith(mainKeyLeft))
            {
                mainKey = line.Replace(mainKeyLeft, "").Replace(mainKeyRight, "").Trim();
                dic.Add(mainKey, new Dictionary<string, string>());
            }
            else
            {
                string[] subKeyValue = line.Split(splitor);
                dic[mainKey].Add(subKeyValue[0].Trim(), subKeyValue[1].Trim());
            }
        }
        return dic;
    }
}