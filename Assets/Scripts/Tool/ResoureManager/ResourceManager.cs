using UnityEngine;
using System.Collections.Generic;

public class ResourceManager
{
    private static readonly Dictionary<string, Dictionary<string, string>> _map;

    static ResourceManager()
    {        
        _map = ConfigurationReader.ReadConfigDic(FileHelper.LoadStreamingAssetsFile("ResourcePathConfig.txt").text);
    }

    public static T Load<T>(string resourceName) where T : Object
    {
        string typeName = FileHelper.RemoveNameSpace(typeof(T).ToString());
        if (!_map.ContainsKey(typeName))
        {
            Debug.Log(string.Format("Type {0} donest has path data.", typeName));
            return null;
        }

        if (!_map[typeName].ContainsKey(resourceName))
        {
            Debug.Log(string.Format("Resource {0} donest has path data.", resourceName));
            return null;
        }

        string path = _map[typeName][resourceName];
        return Resources.Load<T>(path);
    }

}
