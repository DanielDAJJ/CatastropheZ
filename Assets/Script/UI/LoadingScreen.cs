using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public string sceneToLoad;
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    void Update()
    {
        
    }
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(2f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
