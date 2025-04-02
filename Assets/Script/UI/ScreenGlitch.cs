using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ScreenGlitch : MonoBehaviour
{
    private PostProcessVolume volume;
    private Grain grain;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    void Start()
    {
        volume = FindObjectOfType<PostProcessVolume>();
        if (volume != null)
        {
            volume.profile.GetSetting<Grain>();
            volume.profile.GetSetting<ChromaticAberration>();
            volume.profile.GetSetting<Vignette>();
        }
    }

    void Update()
    {
        
    }
}
