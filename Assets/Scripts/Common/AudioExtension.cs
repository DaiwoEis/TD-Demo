using UnityEngine;

public static class AudioExtension
{
    public static void Play(this AudioSource audioSource, AudioClip clip)
    {
        var clipNamePrefix = clip.name.Split('_')[0];
        SoundSetting soundSetting = GameDataManager.Instance.GetSoundSetting();
        if (clipNamePrefix == "BG")
        {
            audioSource.volume = soundSetting.bgVolume;
        }
        else if (clipNamePrefix == "FX")
        {
            audioSource.volume = soundSetting.fxVolume;            
        }
        else if (clipNamePrefix == "Speack")
        {
            audioSource.volume = soundSetting.speakVolume;
        }
        audioSource.PlayOneShot(clip);
    }

}