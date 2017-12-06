using UnityEngine.UI;

public class UIText : Text
{
    private string _languageKey;

    protected override void Awake()
    {
        base.Awake();
        
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
#endif

            _languageKey = GetLanguageKey();
            LocalizationManager.Instance.OnLanaguageChanged += ln => { UpdateText(); };
            UpdateText();

#if UNITY_EDITOR
        }
#endif
    }

    public string GetLanguageKey()
    {
        return gameObject.name.Split('_')[1];
    }

    private void UpdateText()
    {
        text = LocalizationManager.Instance.GetContent(_languageKey);
    }
}
