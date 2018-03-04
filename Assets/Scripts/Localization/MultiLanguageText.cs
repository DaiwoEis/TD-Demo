using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageText : MonoBehaviour
{
    [ShowInInspector]
    public EMultiLanguageContent textID;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
#endif        

            LocalizationManager.Instance.OnLanaguageChanged += ln => { UpdateText(); };
            UpdateText();

#if UNITY_EDITOR
        }
#endif
    }

    private void UpdateText()
    {
        _text.text = LocalizationManager.Instance.GetContent(textID);
    }
}
