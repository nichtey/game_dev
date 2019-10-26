using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneControllerCS : MonoBehaviour {

    AsyncOperation async;
    public Animation LoadingScreenAnim;
    public Slider LoadingBar;
    public Text LoadingText;
    public bool StartLoadingCheck;

	void Start () {
        LoadingScreenAnim.Play("StartGameScene");
        InvokeRepeating("AnimateText1", 0.5f, 1.5f);
        InvokeRepeating("AnimateText2", 1.0f, 1.5f);
        InvokeRepeating("AnimateText3", 1.5f, 1.5f);
        StartCoroutine(StartLoadingGame());
    }

    void AnimateText1 () {
        LoadingText.text = "Loading.";
    }

    void AnimateText2 () {
        LoadingText.text = "Loading.";
    }

    void AnimateText3 () {
        LoadingText.text = "Loading.";
    }

    IEnumerator StartLoadingGame () {
        yield return new WaitForSeconds(1.5f);
        //async = Application.LoadLevelAsync("Main Game");
        async = SceneManager.LoadSceneAsync("Main Game");
        StartLoadingCheck = true;
        async.allowSceneActivation = false;
    }

    void Update () {
        if (StartLoadingCheck == true)
        {
            LoadingBar.value = async.progress + 0.1f;

            if (async.progress == 0.9f)
            {
                StartLoadingCheck = false;
                LoadingScreenAnim.Play("RemoveGameScene");
                StartCoroutine(FinishedLoading());
            }
        }
    }

    IEnumerator FinishedLoading(){
        yield return new WaitForSeconds(1.0f);
        async.allowSceneActivation = true;
    }
}
