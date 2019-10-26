#pragma strict

var Scripts: MainScripts;
var ResourceCardCounterText: UI.Text;
var LifeResourceCardCounterText: UI.Text;
var ChaosResourceCardCounterText: UI.Text;
var ResourceCardExplanation: GameObject;
var ResourceCardExplanationText: UI.Text;
var OptionClickControlScript:OptionClickControl;
var PauseButton: GameObject;
var PauseMenu: GameObject;
var DimScreen: GameObject;
var anim: Animation;
var RoundNumberDisplay: GameObject;
var RoundNumberDisplayText: UnityEngine.UI.Text;
var BcOrAd: String;
var CurrentYear: int;
var Information: GameObject;
var InformationText = new GameObject[2];
var InformationOverviewText = new UnityEngine.UI.Text[10];
var InformationCharacterText = new UnityEngine.UI.Text[9];
var InformationSkillSpriteHolders = new UnityEngine.UI.Image[6];
var InformationSkillDesign = new SpriteRenderer[6];
var TabAnim: Animation;

function ClearMedals(){
    for (var i = 0; i<6; i++){
        InformationSkillSpriteHolders[i].enabled = false;
        InformationSkillDesign[i].color = Color.black;
    }
}

function ClickOverview(){
    if (InformationText[0].activeSelf == true) return;
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    TabAnim.Play("ClickOverviewTab");
    InformationText[0].SetActive(true);
    InformationText[1].SetActive(false);
}

function ClickCharacter(){
    if (InformationText[1].activeSelf == true) return;
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    TabAnim.Play("ClickCharacterTab");
    InformationText[0].SetActive(false);
    InformationText[1].SetActive(true);
}

function CloseInformation(){
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    Information.SetActive(false);
    Scripts.HandCardRotationScript.Important.GamePaused = false;
}

function YearDetails(YearNow: int, BCADCheck: String){
    BcOrAd = BCADCheck;
    CurrentYear = YearNow;
    RoundNumberDisplayText.text = CurrentYear + " " + BcOrAd;
}

function UpdateRoundNumber(RoundNumber: int){
    if (BcOrAd == "BC") RoundNumberDisplayText.text = (CurrentYear - RoundNumber) + " " + BcOrAd;
    if (BcOrAd == "AD") RoundNumberDisplayText.text = (CurrentYear + RoundNumber) + " " + BcOrAd;
    InformationOverviewText[6].text = "Year: " + RoundNumberDisplayText.text;
    InformationOverviewText[7].text = "Events This Year: 1/10";
}

function Start () {

}

function PauseClicked(){
    if (Scripts.HandCardRotationScript.Important.GamePaused == true) return;
    DimScreen.SetActive(true);
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    PauseMenu.SetActive(true);
    anim.Play("Pause Menu");
    Scripts.HandCardRotationScript.Important.GamePaused = true;
}

function ResumeClicked(){
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    anim.Play("RemovePauseMenu");
    Scripts.HandCardRotationScript.Important.GamePaused = false;
    RemovePauseMenu();
}

function MainMenuClicked(){

    if (Scripts.ParticleEffectControllerScript.WeatherInPlay != 0){
        Scripts.ParticleEffectControllerScript.Weather[Scripts.ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
        Scripts.ParticleEffectControllerScript.WeatherInPlay = 0;
    }
    Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    anim.Play("RemovePauseMenu");
    MainMenuLoad();
    RemovePauseMenu();
    Scripts.MiscellaneousGameManagementScript.Save();
    Scripts.MainMenuControllerScript.SetContinueButtonDisplay();
}

function RemovePauseMenu(){
    yield WaitForSeconds(0.3f);
    PauseMenu.SetActive(false);
}

function MainMenuLoad(){
    yield WaitForSeconds(0.3f);
    Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
    Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
    Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
    Scripts.CameraControlScript.FadeThroughBlack(0.25);
    Scripts.CameraControlScript.MainMenuVideo.clip = Scripts.CameraControlScript.VideoClips[1];
    Scripts.CameraControlScript.MainMenuVideo.frame = 0;

    yield WaitForSeconds(0.8);
    Scripts.HandCardRotationScript.Important.GamePaused = false;
    Camera.main.transform.position.z = -23.8;
    yield WaitForSeconds(0.2);
    Scripts.CameraControlScript.MainMenuVideo.Play();
}

function Update () {
    if (AnimateSpriteCheck == 1){
        
        if (LerpFadeProgress < 1){
            LerpFadeProgress += Time.deltaTime/0.2;
            Fade = Mathf.Lerp(0, 1, LerpFadeProgress);
            Announcer.GetComponent.<UnityEngine.UI.Image>().color = Color(1, 1, 1, Fade);
        }

        if (LerpFadeProgress>=1){

            if (LerpMotionProgress <= 0.8){
                LerpMotionProgress+= Time.deltaTime/0.06;
                Announcer.transform.position.x = Mathf.Lerp(-0.72, -0.52, LerpMotionProgress);
            }

            if (LerpMotionProgress > 0.8 && LerpMotionProgress <= 1.7){
                LerpMotionProgress+= Time.deltaTime/0.12;
                Announcer.transform.position.x = Mathf.Lerp(-0.52, -0.92, LerpMotionProgress-0.8);
            }

            if (LerpMotionProgress > 1.7 && LerpMotionProgress <= 2.7){
                LerpMotionProgress+= Time.deltaTime/0.06;
                Announcer.transform.position.x = Mathf.Lerp(-0.92, -0.72, LerpMotionProgress-1.7);
            }
            if (LerpMotionProgress > 2.7){              
                StartTimer += Time.deltaTime;
                if (StartTimer >= 2.0){
                    LerpMotionProgress = 0;
                    LerpFadeOutProgress = 0;
                    AnimateSpriteCheck = 0;
                    DoneShaking = true;
                }   
            }
        }
    }

    if (DoneShaking == true){

        LerpFadeOutProgress += Time.deltaTime/0.5;
        Fade = Mathf.Lerp(1, 0, LerpFadeOutProgress);
        Announcer.GetComponent.<UnityEngine.UI.Image>().color = Color(1, 1, 1, Fade);

        if (LerpFadeOutProgress >=1 ){
            DoneShaking = false;
            Announcer.SetActive (false);
        }
    }
}

var Announcer: GameObject;
var LerpFadeProgress: float;
var LerpMotionProgress: float;
var StartTimer: float;
var AnimateSpriteCheck: int = 0;
var DoneShaking: boolean;
private var Fade: float = 0;
private var LerpFadeOutProgress: float = 0;

function Announcement(TextDisplay: String){

    Announcer.SetActive(true);
    Announcer.transform.GetChild(0).gameObject.GetComponent.<UnityEngine.UI.Text>().text = TextDisplay;
    LerpFadeProgress = 0;
    LerpMotionProgress = 0;
    StartTimer = 0.0;
    if (AnimateSpriteCheck ==1 || DoneShaking == true){
        LerpFadeProgress = 1;
        DoneShaking = false;
        Announcer.GetComponent.<UnityEngine.UI.Image>().color = Color(1, 1, 1, 1);
    }
    AnimateSpriteCheck = 1;

}

function DisplayCardNumberExplanation(){
    ResourceCardExplanation.SetActive(true);
    ResourceCardExplanationText.text = "This is the number of resource cards you have." + '\n'+ "You cannot hold more than 12 cards.";
}

function UpdateResourceCardNumber(NumberOfResourceCards: int){
    ResourceCardCounterText.text = NumberOfResourceCards.ToString();
}

function DisplayLifeCardNumberExplanation(){
    ResourceCardExplanation.SetActive(true);
    ResourceCardExplanationText.text = "This is the number of life cards you have." + '\n' + "You must have at least 1 life card.";
}

function UpdateLifeResourceCardNumber(NumberOfResourceCards: int){
    LifeResourceCardCounterText.text = NumberOfResourceCards.ToString();
}

function DisplayDestructionCardNumberExplanation(){
    ResourceCardExplanation.SetActive(true);
    ResourceCardExplanationText.text = "This is the number of chaos cards you have." + '\n' + "You cannot hold 3 or more chaos cards.";
}

function UpdateChaosResourceCardNumber(NumberOfResourceCards: int){
    ChaosResourceCardCounterText.text = NumberOfResourceCards.ToString();
}

function DisplayHumanTiesExplanation(){
    ResourceCardExplanation.SetActive(true);
    ResourceCardExplanationText.text = "This is our relationship with the humans." + '\n' + "Events may occur based on our relationship.";
}

function DisplayNatureMoraleExplanation(){
    ResourceCardExplanation.SetActive(true);
    ResourceCardExplanationText.text = "Morale (Red) currently stands at: " + Scripts.NatureControllerScript.Health*5 + "%." + "\n" + "Food stocks (Blue) in possession: " + Scripts.NatureControllerScript.CurrentFood;
}

function DisplayHumanMoraleExplanation(){
    ResourceCardExplanation.SetActive(true);
    ResourceCardExplanationText.text = "Morale (Red) currently stands at: " + Scripts.NatureControllerScript.Health*5 + "%." + "\n" + "Food stocks (Blue) in possession: " + Scripts.NatureControllerScript.Health*5;    //TO CHANGE
}

class MainScripts{
    var EventCardControlScript:EventCardControl;
    var HandCardRotationScript:HandCardRotation;
    var OptionClickControlScript:OptionClickControl;
    var CameraControlScript: Camera_Control;
    var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
    var MainMenuControllerScript: MainMenuController;
    var ParticleEffectControllerScript: ParticleEffectController;
    var NatureControllerScript: NatureController;
}