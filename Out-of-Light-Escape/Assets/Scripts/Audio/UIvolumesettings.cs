using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void OnEnable()
    {
        if (MusicManager.instance == null)
            return;

        masterSlider.SetValueWithoutNotify(MusicManager.instance.GetMasterVolume());
        musicSlider.SetValueWithoutNotify(MusicManager.instance.GetMusicVolume());
        sfxSlider.SetValueWithoutNotify(MusicManager.instance.GetSFXVolume());

        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();

        masterSlider.onValueChanged.AddListener(MusicManager.instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(MusicManager.instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(MusicManager.instance.SetSFXVolume);
    }
}