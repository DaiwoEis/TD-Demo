using System.Linq;
using UnityEngine.UI;

public class UIWindow_Setting : UIWindow 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Back").onClick.AddListener(() =>
        {
            CloseSelf();
        });

        var dropDown = transform.FindChildComponentByName<Dropdown>("Dropdown");
        string[] allLanguageNames = LocalizationManager.Instance.GetAllLanguages();
        dropDown.options = allLanguageNames.Select(l => new Dropdown.OptionData(l))
            .ToList();
        string currLanguageName = LocalizationManager.Instance.GetCurrLanguageName();
        int i = 0;
        while (i<allLanguageNames.Length)
        {
            if (allLanguageNames[i] == currLanguageName)
                break;
            ++i;
        }
        dropDown.value = i;

        dropDown.onValueChanged.AddListener(index =>
        {
            LocalizationManager.Instance.SetLanguage(dropDown.options[index].text);
        });
    }
}
