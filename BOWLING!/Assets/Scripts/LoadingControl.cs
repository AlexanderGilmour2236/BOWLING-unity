using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
    public Image filledProgressBar;
    AsyncOperation _loadingOperation;
    private void Start()
    {
        _loadingOperation = SceneManager.LoadSceneAsync("MainScene");
    }

    // Update is called once per frame
    void Update()
    {
        filledProgressBar.fillAmount = _loadingOperation.progress;
    }
}
