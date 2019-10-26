#pragma strict

var MainCamera: GameObject;
var FadeBlackGameObject: GameObject;
var FadeLerp: float = 1;
var BlackSpriteRenderer: SpriteRenderer;
var AlphaInitialValue: float;
var AlphaTargetValue: float;

var Scripts: ScriptReference4;
var LoadingSpriteRenderer: SpriteRenderer;
//var EventCardsToLoad: Sprite[];
var SpritesToLoad: Sprite[];
var GameObjectsToLoad: GameObject[];
var MainMenuVideo: UnityEngine.Video.VideoPlayer;
var VideoClips = new UnityEngine.Video.VideoClip[5];
var EndSceneLoseVideo: UnityEngine.Video.VideoPlayer;
var EndSceneWinVideo: UnityEngine.Video.VideoPlayer;

var BlinkLids = new GameObject[3];
var Anim: Animation;

var MainMenuRenderTexture: RenderTexture;

function Start () {

    MainMenuVideo.Prepare();

    for (var i = 0; i< SpritesToLoad.length; i++){
        LoadingSpriteRenderer.sprite =SpritesToLoad[i];
    }

    LoadingSpriteRenderer.sprite = null;

    var LoadingGameObject = Instantiate( GameObjectsToLoad[0]) ;
    Destroy(LoadingGameObject);

    Scripts.ParticleEffectControllerScript.Explosions[0].Play();
    Scripts.ParticleEffectControllerScript.Explosions[1].Play();
    Scripts.ParticleEffectControllerScript.Explosions[2].Play();
    Scripts.ParticleEffectControllerScript.BigExplosion.Play();

    Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);
    //yield WaitForSeconds(4.5);

    if (MainMenuVideo.isPrepared == false){
        yield;
    }

    MainMenuVideo.Play();
    MainMenuRenderTexture.DiscardContents();

    BlackSpriteRenderer.color.a = 1;
    AlphaTargetValue = 0;
    FadeLerp = 0;

}

function Update () {
    if (FadeLerp < 1){
        FadeLerp += Time.deltaTime/0.8;
        BlackSpriteRenderer.color.a = Mathf.Lerp(AlphaInitialValue,AlphaTargetValue,FadeLerp);
        if (FadeLerp > 1){
            AlphaInitialValue = AlphaTargetValue;
            if (AlphaTargetValue==0)BlackSpriteRenderer.enabled = false;
        }
    }
}

function FadeThroughBlack (TimeDelay: float){
    BlackSpriteRenderer.enabled = true;
    AlphaTargetValue = 1;
    FadeLerp = 0;
    yield WaitForSeconds(1+TimeDelay);
    AlphaTargetValue = 0;
    FadeLerp = 0;
}

function LoseGame(HeaderText: String, BodyText: String){

    Scripts.EndSceneControllerScript.LoseEndScene.SetActive(true);
    EndSceneLoseVideo.Prepare();
    FadeThroughBlack(0.25);
    yield WaitForSeconds(0.8);

    Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[2]);
    Camera.main.transform.position.z = 33.1;
    EndSceneLoseVideo.Play();
    Scripts.EndSceneControllerScript.EndSceneHeader.text = HeaderText;
    Scripts.EndSceneControllerScript.EndSceneDescription.text = BodyText;
}

function WinGame(HeaderText: String, BodyText: String){

    Scripts.EndSceneControllerScript.WinEndScene.SetActive(true);
    EndSceneWinVideo.Prepare();
    FadeThroughBlack(0.25);
    yield WaitForSeconds(0.8);
    
    Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[3]);
    Camera.main.transform.position.z = 33.1;
    EndSceneWinVideo.Play();
    Scripts.EndSceneControllerScript.EndSceneHeader.text = HeaderText;
    Scripts.EndSceneControllerScript.EndSceneDescription.text = BodyText;
}

class ScriptReference4{
    var EventCardControlScript: EventCardControl;
    var ParticleEffectControllerScript: ParticleEffectController;
    var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
    var EndSceneControllerScript: EndSceneController;
}

function Blink(){
    Anim.Play();
}