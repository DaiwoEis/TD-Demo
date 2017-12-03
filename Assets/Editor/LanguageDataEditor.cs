using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class LanguageDataEditor : ScriptableWizard
{   
    [ShowInInspector]
    public LanguageData LanguageData;

    [MenuItem("Tools/LanguageDataEditor")]
    public static void LanguageDataWizard()
    {
        DisplayWizard<LanguageDataEditor>("LanguageDataEditor", "Create", "Open");
    }

    private void OnWizardCreate()
    {

    }
}
