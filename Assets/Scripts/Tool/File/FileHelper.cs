using System.Linq;
using UnityEngine;

public static class FileHelper 
{

    public static string RemoveNameSpace(string typeName)
    {
        var splitsStr = typeName.Split('.').ToList();
        return splitsStr[splitsStr.Count - 1];
    }

    public static string StreamingAssetsPath()
    {
#if UNITY_EDITOR
        return "file://" + Application.dataPath + "/StreamingAssets";

#elif UNITY_STANDALONE_WIN
		return "file://" + Application.dataPath + "/StreamingAssets";

#elif UNITY_IPHONE
		return "file://" + Application.dataPath + "/Raw";

#elif UNITY_ANDROID
		return "jar:file://" + Application.dataPath + "!/assets";
#endif
    }

    public static WWW LoadStreamingAssetsFile(string subPath)
    {
        WWW www = new WWW(string.Format("{0}/{1}", StreamingAssetsPath(), subPath));
        while (true)
        {
            if (www.isDone)
                return www;
        }
    }
}
