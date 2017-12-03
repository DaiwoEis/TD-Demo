using System.IO;
using System.Text;
using UnityEditor;

public class GenerateResourcePathConfig
{
    [MenuItem("Tools/GenerateResourcePathConfigs")]
    public static void GeneratePathConfigs()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var rpd in ResourcePathData.ResourcePathDatas)
        {
            var mainKey = rpd.Key;

            sb.AppendLine("[" + mainKey + "]");

            var pathData = rpd.Value;
            if (!Directory.Exists(Path.GetFullPath(pathData.resourceStoragePath)))
            {                
                Directory.CreateDirectory(Path.GetFullPath(pathData.resourceStoragePath));
                sb.AppendLine();
                continue;
            }

            string[] guids = AssetDatabase.FindAssets("t:" + pathData.typeName, new[] {pathData.resourceStoragePath});

            for (var i = 0; i < guids.Length; i++)
            {
                string completePath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var prefabName = Path.GetFileNameWithoutExtension(completePath);
                string path = completePath.Replace(Path.GetExtension(completePath), "").Replace("Assets/Resources/", "");
                sb.AppendLine(prefabName + ":" + path);
            }

            sb.AppendLine();
        }

        if (!Directory.Exists(Path.GetFullPath("Assets/StreamingAssets")))
            Directory.CreateDirectory(Path.GetFullPath("Assets/StreamingAssets"));

        File.WriteAllText("Assets/StreamingAssets/ResourcePathConfig.txt", sb.ToString());
        AssetDatabase.Refresh();
    }
}