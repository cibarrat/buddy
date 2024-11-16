using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public Toggle musicToggle;
    public Slider musicSlider;
    public Toggle sfxToggle;
    public Slider sfxSlider;

    private void Start()
    {
        musicToggle.isOn = AudioManager.Instance.IsMusicEnabled;
        musicSlider.value = AudioManager.Instance.MusicVolume;
        sfxToggle.isOn = AudioManager.Instance.IsSFXEnabled;
        sfxSlider.value = AudioManager.Instance.SFXVolume;

        musicSlider.interactable = musicToggle.isOn;
        sfxSlider.interactable = sfxToggle.isOn;

        ApplyInitialSettings();

        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void ApplyInitialSettings()
    {
        foreach (var source in GameObject.FindGameObjectsWithTag("Music"))
        {
            var audioSource = source.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = AudioManager.Instance.MusicVolume;
                audioSource.mute = !AudioManager.Instance.IsMusicEnabled;
            }
        }

        foreach (var source in GameObject.FindGameObjectsWithTag("SFX"))
        {
            var audioSource = source.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = AudioManager.Instance.SFXVolume;
                audioSource.mute = !AudioManager.Instance.IsSFXEnabled;
            }
        }
    }

    private void OnMusicToggleChanged(bool value)
    {
 
        AudioManager.Instance.SetMusicEnabled(value);
   
        musicSlider.interactable = value;
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXToggleChanged(bool value)
    {

        AudioManager.Instance.SetSFXEnabled(value);
 
        sfxSlider.interactable = value;
    }

    private void OnSFXSliderChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}
