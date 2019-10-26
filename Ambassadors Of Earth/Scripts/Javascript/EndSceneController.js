#pragma strict

var CameraControlScript: Camera_Control;
var MainMenuControllerScript: MainMenuController;
var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
var ParticleEffectControllerScript: ParticleEffectController;

var EndSceneVideoPlayer: UnityEngine.Video.VideoPlayer;
var EndSceneWinVideo: UnityEngine.Video.VideoPlayer;
var EndSceneHeader: UnityEngine.UI.Text;
var EndSceneDescription: UnityEngine.UI.Text;
var WinEndScene: GameObject;
var LoseEndScene: GameObject;

function Start () {
	
}

function Update () {
	
}

function ReturnToMainMenuLose (){
    MiscellaneousGameManagementScript.PlayMusic(MiscellaneousGameManagementScript.MusicClips[0]);
    if (ParticleEffectControllerScript.WeatherInPlay != 0){
        ParticleEffectControllerScript.Weather[ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
        ParticleEffectControllerScript.WeatherInPlay = 0;
    }

    CameraControlScript.FadeThroughBlack(0.25);
    MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
    MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
    MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
    CameraControlScript.MainMenuVideo.clip = CameraControlScript.VideoClips[1];
    CameraControlScript.MainMenuVideo.frame = 0;
    MoveCameraClickMainMenu ();
    ResetEndScene();
}

function ReturnToMainMenuWon(){
    MiscellaneousGameManagementScript.PlayMusic(MiscellaneousGameManagementScript.MusicClips[0]);
    if (ParticleEffectControllerScript.WeatherInPlay != 0){
        ParticleEffectControllerScript.Weather[ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
        ParticleEffectControllerScript.WeatherInPlay = 0;
    }

    CameraControlScript.FadeThroughBlack(0.25);
    MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
    MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
    MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
    CameraControlScript.MainMenuVideo.clip = CameraControlScript.VideoClips[1];
    CameraControlScript.MainMenuVideo.frame = 0;
    MoveCameraClickMainMenu ();
    ResetEndScene();
}

function ResetEndScene(){
    yield WaitForSeconds(0.8f);
    EndSceneWinVideo.Stop();
    WinEndScene.SetActive(false);
    LoseEndScene.SetActive(false);
}

function MoveCameraClickMainMenu (){
    yield WaitForSeconds(0.8);
    Camera.main.transform.position.z = -23.8;
    yield WaitForSeconds(0.2);
    CameraControlScript.MainMenuVideo.Play();

}