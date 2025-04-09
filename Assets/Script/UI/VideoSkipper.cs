using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSkipper : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "NombreDeTuSiguienteEscena";
    // Start is called before the first frame update
    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) // Puedes usar cualquier tecla
        {
            SkipVideo();
        }
    }
    void SkipVideo()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(nextSceneName);
    }

}
