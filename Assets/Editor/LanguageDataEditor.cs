using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;

public class LanguageDataEditor : ScriptableWizard
{
    [MenuItem("Tools/LanguageHelper/UpdateCurrentSceneLanguageKey")]
    public static void UpdateCurrentSceneLanguageKey()
    {
        var ofn = new OpenFileName("选择要更新的json文件");
        if (LocalDialog.GetSaveFileName(ofn))
        {
            LanguageData languageData = JsonConvert.DeserializeObject<LanguageData>(File.ReadAllText(ofn.file));
            if(languageData.LanguageTextDic == null)
                languageData.LanguageTextDic = new Dictionary<string, string>();
            foreach (UIText uiText in FindObjectsOfType<UIText>())
            {
                string key = uiText.GetLanguageKey();
                if (!languageData.LanguageTextDic.ContainsKey(key))
                    languageData.LanguageTextDic.Add(key, "");
            }
            File.WriteAllText(ofn.file, JsonConvert.SerializeObject(languageData, Formatting.Indented), Encoding.UTF8);
        }
    }
}
