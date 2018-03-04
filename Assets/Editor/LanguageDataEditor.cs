using System.IO;
using ExcelDataReader;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class LanguageDataEditor
{
    [MenuItem("Tools/LanguageHelper/LoadLanguageIDs")]
    public static void LoadLanguageIDs()
    {
        string path = LocalizationManager.dataPath;
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var result = reader.AsDataSet();

                string content = "public enum EMultiLanguageContent\n";
                content += "{\n";

                for (int i = 1; i < result.Tables[0].Rows.Count; ++i)
                {
                    content += "\t" + result.Tables[0].Rows[i][0];
                    if (i == result.Tables[0].Rows.Count - 1)
                        content += "\n";
                    else
                        content += ",\n";
                }
                content += "}\n\n";

                content += "public enum EMultiLanguageName\n";
                content += "{\n";
                var firstRow = result.Tables[0].Rows[0].ItemArray;
                for (int i = 1; i < firstRow.Length; ++i)
                {
                    content += "\t" + firstRow[i] + ",\n";
                }
                content += "}\n";
                string csharpPath = LocalizationManager.scirptPath;

                File.WriteAllText(csharpPath, content);
                AssetDatabase.Refresh();
            }
        }
    }

    [MenuItem("GameObject/UI/MultiLanguageText")]
    public static void CreateMultiLanguageText()
    {
        var go = new GameObject("MultiLanguageText_");
        go.AddComponent<Text>();
        go.AddComponent<MultiLanguageText>();
        go.transform.SetParent(Selection.activeTransform);
    }
}
