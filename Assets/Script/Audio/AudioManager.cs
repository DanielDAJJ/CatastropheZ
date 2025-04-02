using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
   public static AudioManager instance;
   public AudioClip gameover_dramatic;
   public AudioClip zombie_brains;
   public AudioClip zombie_gruñido;
   public AudioClip zombie_clothesrip;
   public AudioClip zoe_fighting_sound;
   public AudioClip constant_fire;
   public AudioClip fire;
   public AudioClip whispers_man;
   public AudioClip whispermom;
   public AudioClip church;
   public AudioClip heavy_rain;
   public AudioClip soft_rain;
   public AudioClip stepping;
   public AudioClip wind_atmosphere;
   public AudioClip walking;
   public AudioClip menu_melancholic;
   public AudioClip chew_eating_crunchy;
   public AudioClip chew_texture;
   public AudioClip keys;
   public AudioClip heavy_raincables_sparking_softly;
   public AudioClip explosion_cables_sparking;
   public AudioClip meow_cat_baby;
   public AudioClip meow_cat2;
   public AudioClip a_rayos;
   public AudioClip b_nono;
   public AudioClip c_solo;
   public AudioClip d;
   public AudioClip e;
   public AudioClip f;
   public AudioClip g;
   public AudioClip h;
   public AudioClip i;
   public AudioClip j;
   public AudioClip k;
   public AudioClip l;
   public AudioClip chancleta;

    private bool isMuted = false;
    private float sfxVolume = 1f;
    private AudioSource audioSource;//Para acceder el audiosource desde el código

   void Awake()
   {

      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(gameObject);
      }

      DontDestroyOnLoad(gameObject);
      audioSource = GetComponent<AudioSource>();
      if (audioSource == null)
      {
        audioSource = gameObject.AddComponent<AudioSource>();
      }
    }
   void Start()
   {
      PlayBackgroundMusic();
      audioSource = GetComponent<AudioSource>();
   }

   public void PlaySound(AudioClip clip)
   {
        if (clip == null)
        {
            Debug.LogWarning("Intentando reproducir un sonido nulo.");
            return;
        }
        audioSource.PlayOneShot(clip);
   }

   public void PlayBackgroundMusic()

   {
        if (gameover_dramatic == null)
        {
            Debug.LogWarning("No se ha asignado música de fondo.");
            return;
        }
        audioSource.clip = gameover_dramatic;
      audioSource.loop = true;
      audioSource.Play();
   }
    public void SetMusicVolume(float volume)
    {
        if (!isMuted)
        {
            audioSource.volume = volume;
        }

    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
    public void MuteAudio(bool mute)
    {
        isMuted = mute;
        audioSource.mute = mute;
    }
}
