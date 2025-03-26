using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTexture : MonoBehaviour
{
    [SerializeField] Renderer vallaTutorial;
    [SerializeField] Texture[] tutorialTextures;
    [SerializeField] float timeBetweenTextures = 2f;

    int currentTextureIndex = 0;
    Material vallaMaterial;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        if (vallaTutorial == null) vallaTutorial = GetComponent<Renderer>();
        vallaMaterial = vallaTutorial.material;
        timer = timeBetweenTextures;
        //vallaMaterial.mainTextureScale = new Vector2(3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeTexture();
            timer = timeBetweenTextures;
        }
    }
    void ChangeTexture()
    {
        if (tutorialTextures.Length == 0) return;
        currentTextureIndex = (currentTextureIndex + 1) % tutorialTextures.Length;
        vallaMaterial.mainTexture = tutorialTextures[currentTextureIndex];
    }
}
