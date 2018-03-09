using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow_Setting : UIWindow
{
    private Dropdown _languageDropdown;

    private Slider _bgVolumeSlider;
    private Slider _fxVolumeSlider;
    private Slider _speackVolumeSlider;

    [SerializeField]
    private bool _dirty;

    private void Awake()
    {
        _languageDropdown = transform.FindChildComponentByName<Dropdown>("Dropdown");
        _bgVolumeSlider = transform.FindChildComponentByName<Slider>("Slider_BGVolume");
        _fxVolumeSlider = transform.FindChildComponentByName<Slider>("Slider_FXVolume");
        _speackVolumeSlider = transform.FindChildComponentByName<Slider>("Slider_SpeackVolume");

        _languageDropdown.onValueChanged.AddListener(i => { _dirty = true; });
        _bgVolumeSlider.onValueChanged.AddListener(i => { _dirty = true; });
        _fxVolumeSlider.onValueChanged.AddListener(i => { _dirty = true; });
        _speackVolumeSlider.onValueChanged.AddListener(i => { _dirty = true; });

        transform.FindChildComponentByName<Button>("Btn_Back").onClick.AddListener(() =>
        {
            if (_dirty)
            {
                ReadSetting();
                _dirty = false;
            }
            UIManager.Instance.BackToLastWindow();
        });
   
        transform.FindChildComponentByName<Button>("Btn_Apply").onClick.AddListener(SaveSetting);

        ReadSetting();

        _dirty = false;
    }

    private void SaveSetting()
    {
        _dirty = false;

        LocalizationManager.Instance.SetLanguage(_languageDropdown.options[_languageDropdown.value].text);
        SoundSetting soundSetting = new SoundSetting
        {
            bgVolume = _bgVolumeSlider.value,
            fxVolume = _fxVolumeSlider.value,
            speakVolume = _speackVolumeSlider.value
        };
        GameDataManager.Instance.SaveSoundSetting(soundSetting);
    }

    private void ReadSetting()
    {
        string[] allLanguageNames = LocalizationManager.Instance.GetAllLanguages();
        _languageDropdown.options = allLanguageNames.Select(l => new Dropdown.OptionData(l))
            .ToList();
        string currLanguageName = LocalizationManager.Instance.GetCurrLanguageName();
        int i = 0;
        while (i < allLanguageNames.Length)
        {
            if (allLanguageNames[i] == currLanguageName)
                break;
            ++i;
        }
        _languageDropdown.value = i;

        SoundSetting soundSetting = GameDataManager.Instance.GetSoundSetting();
        _bgVolumeSlider.value = soundSetting.bgVolume;
        _fxVolumeSlider.value = soundSetting.fxVolume;
        _speackVolumeSlider.value = soundSetting.speakVolume;
    }
}
