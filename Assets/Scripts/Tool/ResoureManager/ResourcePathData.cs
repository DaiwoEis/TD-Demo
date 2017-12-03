using System.Collections.Generic;

public class ResourcePathData
{
    public static Dictionary<string, PathData> ResourcePathDatas = new Dictionary<string, PathData>
    {
        {"GameObject", new PathData("prefab", "Assets/Resources/Prefabs")},
        {"Texture2D", new PathData("texture", "Assets/Resources/Textures")},
        {"AudioClip", new PathData("audioclip", "Assets/Resources/Sounds")},
        {"ScriptableObject", new PathData("scriptableobject", "Assets/Resources/ScriptableObjects")},
        {"TextAsset", new PathData("textasset", "Assets/Resources/TextAssets")}
    };
}