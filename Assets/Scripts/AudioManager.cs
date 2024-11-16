using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public bool IsMusicEnabled { get; private set; }
    public float MusicVolume { get; private set; }
    public bool IsSFXEnabled { get; private set; }
    public float SFXVolume { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void LoadSettings()
    {
        IsMusicEnabled = PlayerPrefs.GetInt("IsMusicEnabled", 1) == 1;
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        IsSFXEnabled = PlayerPrefs.GetInt("IsSFXEnabled", 1) == 1;
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void SetMusicEnabled(bool enabled)
    {
        IsMusicEnabled = enabled;
        PlayerPrefs.SetInt("IsMusicEnabled", enabled ? 1 : 0);
        ApplySettings();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        ApplySettings();
    }

    public void SetSFXEnabled(bool enabled)
    {
        IsSFXEnabled = enabled;
        PlayerPrefs.SetInt("IsSFXEnabled", enabled ? 1 : 0);
        ApplySettings();
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        ApplySettings();
    }

    private void ApplySettings()
    {
   
        foreach (var source in GameObject.FindGameObjectsWithTag("Music"))
        {
            var audioSource = source.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = IsMusicEnabled ? MusicVolume : 0f;
                audioSource.mute = !IsMusicEnabled;
            }
        }

        foreach (var source in GameObject.FindGameObjectsWithTag("SFX"))
        {
            var audioSource = source.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = IsSFXEnabled ? SFXVolume : 0f;
                audioSource.mute = !IsSFXEnabled;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        ApplySettings();
    }
}