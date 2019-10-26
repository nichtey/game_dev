#pragma strict
import UnityEngine.SceneManagement;

var async: AsyncOperation; 
var LoadingScreenAnim: Animation;
var LoadingBar: UnityEngine.UI.Slider;
var LoadingText: UnityEngine.UI.Text;
var StartLoadingCheck: Boolean;

function Start () {
    LoadingScreenAnim.Play("StartGameScene");
    InvokeRepeating("AnimateText1", 0.5f, 1.5f);
    InvokeRepeating("AnimateText2", 1.0f, 1.5f);
    InvokeRepeating("AnimateText3", 1.5f, 1.5f);
    yield WaitForSeconds(1.5f);
    StartLoadingGame();

}

function AnimateText1(){
    LoadingText.text = "Loading.";
}
function AnimateText2(){
    LoadingText.text = "Loading..";
}
function AnimateText3(){
    LoadingText.text = "Loading...";
}

function StartLoadingGame(){
    //async = Application.LoadLevelAsync("Main Game");
    async = SceneManager.LoadSceneAsync("Main Game");
    StartLoadingCheck = true;
    async.allowSceneActivation = false;
}

function Update () {

    if (StartLoadingCheck == true){
        LoadingBar.value = async.progress + 0.1;
        
        if (async.progress == 0.9){
            StartLoadingCheck = false;
            LoadingScreenAnim.Play("RemoveGameScene");
            FinishedLoading();
        }
    }
}

function FinishedLoading(){
    yield WaitForSeconds(1.0f);
    async.allowSceneActivation = true;
}