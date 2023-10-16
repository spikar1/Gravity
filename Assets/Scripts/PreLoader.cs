using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoader : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    [SerializeField]
    private bool ignoreAllButLoading = false;

    private void Start()
    {
        if (ignoreAllButLoading)
            SceneManager.LoadScene(sceneToLoad);
        else
            return;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }
}
