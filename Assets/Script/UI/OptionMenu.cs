using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    [Header("Sliders de Opciones")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider brightnessSlider;
    public Toggle muteToggle;
    public Image brightnessOverlay;

    private float defaultMusicVolume = 1f;
    private float defaultSfxVolume = 1f;
    private float defaultBrightness = 1f;
    private bool isMute = false;
    void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume", defaultSfxVolume);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
        isMute = PlayerPrefs.GetInt("IsMute", 0) == 1;
        muteToggle.isOn = isMute;
        ApplyBrightness(brightnessSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        PlayerPrefs.SetInt("Muted", isMute ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Juego muteado: " + isMute);
    }
    private void ApplyBrightness(float value)
    {
        Color overLayColor = brightnessOverlay.color;
        overLayColor.a = 1 - value;
        brightnessOverlay.color = overLayColor;
    }
}
