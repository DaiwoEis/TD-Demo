using UnityEngine;
using UnityEngine.UI;

public class UIGameSetting : MonoBehaviour 
{

    private void Awake()
    {
        GameObject prefab = ResourceManager.Load<GameObject>("Btn_Language");
        foreach (string languageName in LocalizationManager.Instance.GetAllLanguages())
        {
            string ln = languageName;
            var btn = Instantiate(prefab, transform);
            btn.gameObject.name = string.Format("Btn_{0}", languageName);
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                LocalizationManager.Instance.SetLanguage(ln);
            });
            btn.transform.FindChildComponentByName<Text>("Text_Language").text = languageName;
        }
    }
}
