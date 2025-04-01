using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    [Header("Sliders de Opciones")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider brightnessSlider;
    public Toggle muteToggle;
    public Image brightnessOverlay;

    private static OptionMenu instance;

    private float defaultMusicVolume = 1f;
    private float defaultSfxVolume = 1f;
    private float defaultBrightness = 1f;
    private bool isMute = false;
    private void Awake()
    {
        GameObject uiParent = GameObject.Find("UI");
        if (uiParent != null)
        {
            DontDestroyOnLoad(uiParent); 
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        LoadSettings();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        brightnessOverlay = GameObject.Find("BrightnessOverlay")?.GetComponent<Image>();
        if (brightnessOverlay != null)
        {
            float savedBrightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
            ApplyBrightness(savedBrightness);
        }
        else
        {
            Debug.LogWarning("No se encontró BrightnessOverlay en la escena: " + scene.name);
        }
    }
    void LoadSettings()
    {
        if (brightnessSlider == null || brightnessOverlay == null ||
            musicVolumeSlider == null || sfxVolumeSlider == null || muteToggle == null)
        {
            Debug.LogError("Uno o más componentes del menú de opciones no están asignados en el Inspector.");
            return;
        }
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume", defaultSfxVolume);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
        isMute = PlayerPrefs.GetInt("IsMute", 0) == 1;
        muteToggle.isOn = isMute;
        ApplyBrightness(brightnessSlider.value);
    }
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        Debug.Log("Volumen de música: " + volume);
    }
    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
        PlayerPrefs.Save();
        Debug.Log("Volumen de efectos de sonido: " + volume);
    }
    public void SetBrightness(float brightness)
    {
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.Save();
        ApplyBrightness(brightness);
        Debug.Log("Brillo: " + brightness);
    }
    public void ToggleMute(bool mute)
    {
        isMute = mute;
        PlayerPrefs.SetInt("IsMute", isMute ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Juego muteado: " + isMute);
    }
    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color overLayColor = brightnessOverlay.color;
            overLayColor.a = 1 - value;
            brightnessOverlay.color = overLayColor;
        }
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
