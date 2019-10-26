#pragma strict

// Main Menu + Tutorial

var Scripts: ScriptReference3;
var TutorialUse: TutorialStuff;
var LevelUse: LevelStuff;
var StartingCards: StartingCardDeal;
var MainMenuGameObjectsToDisable = new GameObject[4];
var Layers = new GameObject[3];
var LevelSelected: int;
var CardsPlayedParentGameObject: GameObject;
var ContinueButtonDisplay: UnityEngine.UI.Button;
var InstantiatedCard: GameObject;
var HandCardRotationTransform: Transform;
var PreviousNumberOfCards: int;
var StartingImages: GameObject;
var MainMenuCanvas = new GameObject[3];
var MainMenuButtons = new UnityEngine.UI.Button[10];

private var CurrentPage: int;
private var PreviousPage: int;

function Start () {
    Camera.main.transform.position.z = -23.8;
    SetContinueButtonDisplay();
    CurrentPage = 1;
}

function SetContinueButtonDisplay() {
    if (File.Exists(Application.persistentDataPath + "/playerinfo.dat")){
        ContinueButtonDisplay.interactable = true;
    }

    else{
        ContinueButtonDisplay.interactable = false;
    }
}

function ClickSettings() {

    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    PreviousPage = CurrentPage;

    if (CurrentPage == 1){  //Starting Main Menu
        Layers[0].SetActive(false);
        StartingImages.SetActive(false);
        MainMenuCanvas[1].SetActive(false);

        Layers[1].SetActive(true);
        MainMenuCanvas[2].SetActive(true);
        CurrentPage = 4;
    }

    else if (CurrentPage == 2){   //Mode Selection
        MainMenuCanvas[2].SetActive(true);
        MainMenuCanvas[0].SetActive(false);
        CurrentPage = 4;
    }

    else if (CurrentPage == 3){     //CampaignMenu
        MainMenuCanvas[2].SetActive(true);
        MainMenuCanvas[1].SetActive(false);
        CurrentPage = 4;
    }
}

function ClickNewGame() {    // To ModeSelection
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    Layers[0].SetActive(false);
    Layers[1].SetActive(true);
    StartingImages.SetActive(false);
    MainMenuCanvas[0].SetActive(true);
    CurrentPage = 2;
}

function ClickBack1() {  //To StartingMainMenu
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    Layers[0].SetActive(true);
    Layers[1].SetActive(false);
    MainMenuCanvas[0].SetActive(false);
    StartingImages.SetActive(true);
    CurrentPage = 1;
}

function ClickBack2() {  //To ModeSelection
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    MainMenuCanvas[0].SetActive(true);
    MainMenuCanvas[1].SetActive(false);
    CurrentPage = 2;
}

function ClickBack3() {  //From SettingsPage to previous page
    
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    if (PreviousPage == 1){ //Starting Main Menu
        MainMenuCanvas[2].SetActive(false);
        Layers[0].SetActive(true);
        Layers[1].SetActive(false);
        StartingImages.SetActive(true);
        CurrentPage = 1;
    }

    if (PreviousPage == 2){ // Mode Selection
        MainMenuCanvas[2].SetActive(false);
        MainMenuCanvas[0].SetActive(true);
        CurrentPage = 2;
    }

    if (PreviousPage == 3){   //Campaign Menu
        MainMenuCanvas[2].SetActive(false);
        MainMenuCanvas[1].SetActive(true);
        CurrentPage = 3;
    }
}

function ClickCampaign() {   //To CampaignMenu
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    MainMenuCanvas[1].SetActive(true);
    MainMenuCanvas[0].SetActive(false);
    CurrentPage = 3;
}

function ClickLevel2() {
    LevelSelected = 2;
    LevelUse.PositionInTalking = 0;
    LevelUse.PositionInEnding = 0;
    MoveCameraClickPlay();
    LevelUse.SpeechLoaded = false;
    LevelUse.FinishTalk = false;
    LevelUse.FinishTalkEnding = true;
    Scripts.UIControllerScript.YearDetails(LevelUse.SpeechBubble[LevelSelected-2].CurrentYear, LevelUse.SpeechBubble[LevelSelected-2].BcOrAd);
    
    Scripts.ParticleEffectControllerScript.SetWeather(1);
    Scripts.HandCardRotationScript.Components.ResourceCardsReadyCheck = false;
    ResetGame(LevelSelected);
    StandardLevelSetUp();
}

function AngieTalkEnding() {
    var ReferencePath = LevelUse.SpeechBubble[LevelSelected-2].SpeechEnding[LevelUse.PositionInEnding];
    if (ReferencePath.Text == ""){
        LevelUse.FinishTalkEnding = true;
        LevelUse.SpeechObjects[0].SetActive(false);
        if (LevelUse.PositionInEnding <= 2) Scripts.CameraControlScript.WinGame("Level 2 Completed!", "From the deep wilderness,\na new kingdom emerges.");
        if (LevelUse.PositionInEnding > 2) Scripts.CameraControlScript.LoseGame("Objective not met", "Tip: Faith and Humans, although\nvaluable, are not essential here.");
        return;
    }

    LevelUse.SpeechObjects[1].transform.localScale.x = ReferencePath.Size[0];
    LevelUse.SpeechObjects[1].transform.localScale.y = ReferencePath.Size[1];
    ReferencePath.Text = ReferencePath.Text.Replace("NWL", "\n");
    LevelUse.SpeechObjects[1].GetComponent.<UnityEngine.UI.Text>().text = ReferencePath.Text;
    LevelUse.SpeechAnimations[0].Play("CallOutWobble");

    yield WaitForSeconds(0.5f);
    LevelUse.PositionInEnding += 1;
    LevelUse.SpeechLoaded = true;
    LevelUse.SpeechAnimations[1].Play("ShowButtons");
}

function NextSpeechEnding(){
    LevelUse.SpeechLoaded = false;
    yield WaitForSeconds(0.5f);
    AngieTalkEnding();
}

function PreviousSpeechEnding(){
    LevelUse.SpeechLoaded = false;
    AngieTalkEnding();
}

function AngieTalk(){

    var ReferencePath = LevelUse.SpeechBubble[LevelSelected-2].Speech[LevelUse.PositionInTalking];
    if (ReferencePath.Text == ""){
        LevelUse.FinishTalk = true;
        LevelUse.SpeechObjects[0].SetActive(false);
        Scripts.EventCardControlScript.EventCardManager();
        return;
    }

        LevelUse.SpeechObjects[1].transform.localScale.x = ReferencePath.Size[0];
        LevelUse.SpeechObjects[1].transform.localScale.y = ReferencePath.Size[1];
        ReferencePath.Text = ReferencePath.Text.Replace("NWL", "\n");
        LevelUse.SpeechObjects[1].GetComponent.<UnityEngine.UI.Text>().text = ReferencePath.Text;
        LevelUse.SpeechAnimations[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        LevelUse.PositionInTalking += 1;
        LevelUse.SpeechLoaded = true;
        LevelUse.SpeechAnimations[1].Play("ShowButtons");
}

function NextSpeech(){
    LevelUse.SpeechLoaded = false;
    yield WaitForSeconds(0.5f);
    AngieTalk();
}

function PreviousSpeech(){
    LevelUse.SpeechLoaded = false;
    AngieTalk();
}

private var Sign: String;

function DetermineSign(IntToCheck: int){
    if (IntToCheck > 0) Sign = "+";
    if (IntToCheck <= 0) Sign = "";
    return Sign;
}

function DetermineHumanTies(HumanTieToCheck: int){

    var DisplaySpeech: String;

    if (HumanTieToCheck <= -30){DisplaySpeech = "Humans view us with contempt (Human Ties: " + HumanTieToCheck + ").\nThey will attempt to oppose our every move.";}
    else if (HumanTieToCheck <=-10 && HumanTieToCheck > -30){DisplaySpeech = "Humans remain suspicious of us (Human Ties: " + HumanTieToCheck + ").\nThey may occasionally disrupt our community.";}
    else if (HumanTieToCheck > - 10 && HumanTieToCheck <10){DisplaySpeech = "Humans view us in a neutral light (Human Ties: " + HumanTieToCheck + ").\nThey will neither help nor hinder us intentionally.";}
    else if (HumanTieToCheck >= 10 && HumanTieToCheck < 30){DisplaySpeech = "Humans are happy to work with us (Human Ties: " + HumanTieToCheck + ").\nThey will occasionally offer to help our community.";}
    else {DisplaySpeech = "Close relations have been established (Human Ties: " + HumanTieToCheck + ").\nHumans will work with us for the betterment of Earth.";}
    return DisplaySpeech;
}

function DetermineMorale(MoraleToCheck: int){

    var DisplaySpeech: String;

    if (MoraleToCheck >= 8){DisplaySpeech = "Morale: " + MoraleToCheck*5 + "% (Satisfactory)";}
    else if (MoraleToCheck <8 && MoraleToCheck >= 2){DisplaySpeech = "Morale: " + MoraleToCheck*5 + "% (Low)";}
    else if (MoraleToCheck <2){DisplaySpeech = "Morale: " + MoraleToCheck*5 + "% (Abyssal)";}
    else {DisplaySpeech = "Morale: " + MoraleToCheck*5 + "% (Non-existent)";}

    return DisplaySpeech;
}

function DetermineFood(FoodToCheck: int){

    var DisplaySpeech: String;

    if (FoodToCheck >= 40){DisplaySpeech = "Food: " + FoodToCheck + " (Adequate)";}
    else if (FoodToCheck < 40 && FoodToCheck >= 10){DisplaySpeech = "Food: " + FoodToCheck + " (Low)";}
    else if (FoodToCheck < 10){DisplaySpeech = "Food: " + FoodToCheck + " (Scarce)";}
    else {DisplaySpeech = "Food: " + FoodToCheck + " (None)";}

    return DisplaySpeech;
}

function StandardLevelSetUp(){
    LevelUse.SpeechObjects[0].SetActive(false);
    TutorialUse.Tutorial[0].SetActive(false);
    TutorialUse.Tutorial[8].SetActive(false);
    TutorialUse.Tutorial[9].SetActive(false);
    Scripts.UIControllerScript.Information.SetActive(false);

    Scripts.UIControllerScript.InformationOverviewText[0].text = LevelUse.SpeechBubble[LevelSelected-2].LevelObjective;
    Scripts.UIControllerScript.InformationOverviewText[0].text = Scripts.UIControllerScript.InformationOverviewText[0].text.Replace("NWL", "\n");

    var NetFood = LevelUse.SpeechBubble[LevelSelected-2].FoodProduction - LevelUse.SpeechBubble[LevelSelected-2].FoodConsumption;
    Scripts.UIControllerScript.InformationOverviewText[1].text = "Food: " + DetermineSign(NetFood) + NetFood;
    Scripts.UIControllerScript.InformationOverviewText[2].text = "Morale: " + DetermineSign(LevelUse.SpeechBubble[LevelSelected-2].Morale) + LevelUse.SpeechBubble[LevelSelected-2].Morale;
    Scripts.UIControllerScript.InformationOverviewText[3].text = "Human Ties: " + DetermineSign(LevelUse.SpeechBubble[LevelSelected-2].HumanTies) + LevelUse.SpeechBubble[LevelSelected-2].HumanTies;
    Scripts.UIControllerScript.InformationOverviewText[4].text = "Food Consumption: " + LevelUse.SpeechBubble[LevelSelected-2].FoodConsumption;
    Scripts.UIControllerScript.InformationOverviewText[5].text = "Food Production: " + LevelUse.SpeechBubble[LevelSelected-2].FoodProduction;
    Scripts.UIControllerScript.InformationOverviewText[6].text = "Year: "+ LevelUse.SpeechBubble[LevelSelected-2].CurrentYear + " " + LevelUse.SpeechBubble[LevelSelected-2].BcOrAd;
    Scripts.UIControllerScript.InformationOverviewText[7].text = "Events This Year: 1/10";
    Scripts.UIControllerScript.InformationOverviewText[8].text = "You have not added any events for next year (yet).";
    Scripts.UIControllerScript.InformationOverviewText[9].text = "";

    Scripts.UIControllerScript.InformationCharacterText[2].text = DetermineMorale(LevelUse.SpeechBubble[LevelSelected-2].StartingMorale);
    Scripts.UIControllerScript.InformationCharacterText[3].text = DetermineFood(LevelUse.SpeechBubble[LevelSelected-2].StartingFood);
    Scripts.UIControllerScript.InformationCharacterText[6].text = DetermineMorale(LevelUse.SpeechBubble[LevelSelected-2].StartingMoraleHuman);
    Scripts.UIControllerScript.InformationCharacterText[7].text = DetermineFood(LevelUse.SpeechBubble[LevelSelected-2].StartingFoodHuman);
    Scripts.UIControllerScript.InformationCharacterText[8].text = DetermineHumanTies(LevelUse.SpeechBubble[LevelSelected-2].StartingHumanTies);

    Scripts.NatureControllerScript.CurrentScienceProgress = 0.25;
    var FoodStartingFrac: float = parseFloat(LevelUse.SpeechBubble[LevelSelected-2].StartingFood)/100;
    Scripts.NatureControllerScript.IncreaseNatureFood(FoodStartingFrac);
    Scripts.NatureControllerScript.Health = 0;
    Scripts.NatureControllerScript.UpdateMorale(LevelUse.SpeechBubble[LevelSelected-2].StartingMorale);

    Scripts.UIControllerScript.ClearMedals();

    yield WaitForSeconds(1.5f);

    while (Scripts.HandCardRotationScript.Components.ResourceCardsReadyCheck == false){
        yield WaitForSeconds(0.1f);
    }

    TutorialUse.ResourceCards.position.z = 0; 
    for(var i = 0; i< StartingCards.CardDistribution[0]+StartingCards.CardDistribution[1]+StartingCards.CardDistribution[2]+StartingCards.CardDistribution[3]+StartingCards.CardDistribution[4]; i++){        
        TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.position.x = -0.84;
        TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.position.y = 3.44;
        TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.position.z = 17.5;
        TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.rotation = Quaternion(0, -0.7071, 0.7071, 0);
        
    }
  
    Scripts.HandCardRotationScript.AdjustmentCheck = true;

    TutorialUse.Tutorial[8].SetActive(true);
    for (i=0; i<3; i++){
        TutorialUse.CounterSprite[i].color.a = 0;
        TutorialUse.CounterText[i].color.a = 0;
    }
    TutorialUse.TutorialAnim[8].Play("PopOutCardCounter");
    
    TutorialUse.Tutorial[9].SetActive(true);
    TutorialUse.YearSprite.color.a = 0;
    TutorialUse.YearText.color.a = 0;
    yield WaitForSeconds(0.25f);
    TutorialUse.TutorialAnim[9].Play("PopOutYear");

    yield WaitForSeconds(0.5f);
    LevelUse.SpeechObjects[0].SetActive(true);
    LevelUse.SpeechObjects[2].transform.position.x = 20;
    AngieTalk();

    for (i = 0; i < Scripts.EventCardControlScript.Important.CardsInDeck.length; i++){
        Scripts.EventCardControlScript.Important.CardsInDeck[i].Name = "";
        Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[0] = 0;
        Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[1] = 0;
        Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[0] = 0;
        Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[1] = 0;
        Scripts.EventCardControlScript.Important.CardsInDeck[i].CardPriority = 0;
    }

    var ReferencePath = Scripts.EventCardControlScript.AllEventCardsInGame[0].LevelChosen[LevelSelected-2].CardsInLevel;
    for (i = 0; i < ReferencePath.length; i++ ){
        if (ReferencePath[i].CardIdentifier[0] == 0) break;
        Scripts.EventCardControlScript.Important.CardsInDeck[i].Name = ReferencePath[i].Name;
        Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[0] = ReferencePath[i].CardIdentifier[0];
        Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[1] = ReferencePath[i].CardIdentifier[1];
        Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[0] = ReferencePath[i].SpriteDisplayIdentifier[0];
        Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[1] = ReferencePath[i].SpriteDisplayIdentifier[1];
        Scripts.EventCardControlScript.Important.CardsInDeck[i].CardPriority = ReferencePath[i].CardPriority;
    }

    Scripts.EventCardControlScript.ShuffleCheck = false;
    Scripts.EventCardControlScript.ShuffleCard();  
    yield WaitForSeconds(0.1);
    while (Scripts.EventCardControlScript.ShuffleCheck == false){
        yield WaitForSeconds(0.1);
    }
    Scripts.HandCardRotationScript.Important.NoClicking = false;
    
}

function ClickTutorial(){
    LevelSelected = 1;

    TutorialUse.TopCardPosition.position = Vector3(22.25,0.05,1.51);              // Out of Scene
    TutorialUse.OptionCardTransforms[0].position = Vector3(16,5.41,-2.64);            // OutOfScene
    TutorialUse.OptionCardTransforms[1].position = Vector3(21,7.05,-2.64);            // OutOfScene
    TutorialUse.OptionCardTransforms[2].position = Vector3(26.31,9.35,-2.64);         // OutOfScene
    LevelUse.SpeechObjects[0].SetActive(false);
    TutorialUse.Tutorial[0].SetActive(false);
    TutorialUse.Tutorial[8].SetActive(false);
    TutorialUse.Tutorial[9].SetActive(false);
    Scripts.UIControllerScript.RoundNumberDisplayText.text = "10000 BC";
    MoveCameraClickPlay();
    TutorialUse.Tutorial[1].transform.localScale.x = 0.1;
    TutorialUse.Tutorial[1].transform.localScale.y = 0.1;
    TutorialUse.TutorialText.text = "Hey!";
    TutorialUse.TutorialButtons.transform.position.x = 20;
    FirstTimeBlink();
    ResetGame(LevelSelected);
    Scripts.HandCardRotationScript.Important.NoClicking = false;
    Scripts.NatureControllerScript.CurrentScienceProgress = 0.25;
    Scripts.NatureControllerScript.IncreaseNatureFood(1);
    Scripts.NatureControllerScript.UpdateMorale(20);
}

function FirstTimeBlink(){
    yield WaitForSeconds(0.8f);
    Scripts.CameraControlScript.Blink();
    yield WaitForSeconds(7.5f);
    TutorialUse.Tutorial[0].SetActive(true);
    TutorialUse.ButtonDimScreen.SetActive(false);
    TutorialUse.PositionInTutorial = 0;
    TutorialClickNext();
}

function TutorialResetHand(){
    
    var PreviousNumberOfCards = HandCardRotationTransform.childCount -1;
    for (var Delete = PreviousNumberOfCards - 1; Delete > -1; Delete--){
        Destroy(HandCardRotationTransform.GetChild(Delete).gameObject);
    }

    yield WaitForSeconds(0.1);

    for (var life = 0; life < StartingCards.CardDistribution[0]; life++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[0]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (life);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[life];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[life];
    }

    for (var Resource = life; Resource < StartingCards.CardDistribution[1]+life; Resource++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[1]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Resource);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Resource];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Resource];
    }

    for (var Faith = Resource; Faith < StartingCards.CardDistribution[2]+Resource; Faith++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[2]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Faith);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Faith];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Faith];
    }

    for (var Human = Faith; Human < StartingCards.CardDistribution[3]+Faith; Human++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[3]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Human);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Human];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Human];
    }

    for (var Chaos = Human; Chaos < StartingCards.CardDistribution[4]+Human; Chaos++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[4]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Chaos);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Chaos];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Chaos];
    } 

    if (TutorialUse.PositionInTutorial == 17){
        var InstantiatedCards = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[4]);
        InstantiatedCards.transform.position = Vector3(-0.63,0,4.3);
        InstantiatedCards.transform.parent = Scripts.HandCardRotationScript.Components.CardsPlayedParent.transform;
        Scripts.HandCardRotationScript.Components.CardsPlayed[0] = InstantiatedCards;
        Scripts.HandCardRotationScript.Components.CardsPlayed[0].tag = "PlayedCards";
    }

    TutorialUse.PositionInTutorial -= 2;
    TutorialClickNext();
}

function ClearPrevious(){

    TutorialUse.FinishLoadCheck = false;

    if (TutorialUse.RemoveTextCheck == true){
        TutorialUse.RemoveTextCheck = false;
        TutorialUse.TutorialAnim[0].Play("FadeCallOut");
        yield WaitForSeconds(0.5f);
    }

    if (TutorialUse.RemoveTopCardCheck == true){
        TutorialUse.RemoveTopCardCheck = false;
        Scripts.EventCardControlScript.Components.anim.Play("RemoveCard");
        yield WaitForSeconds(1.25f);
    }

    if (TutorialUse.RemoveResourceCardCheck == true){
        TutorialUse.RemoveResourceCardCheck = false;
        TutorialUse.TutorialAnim[TutorialUse.ResourceCardFadeOutNumber - 1].Play("RemoveCard");
        yield WaitForSeconds(1.25f);
    }

    TutorialClickNext();
}

function ClickPrevious(){

    if (TutorialUse.PositionInTutorial == 1) return;
    TutorialUse.RemoveTextCheck = false;
    TutorialUse.RemoveTopCardCheck = false;
    TutorialUse.RemoveResourceCardCheck = false;
    TutorialUse.EventCardClickCheck = false;
    TutorialUse.FinishLoadCheck = false;
    TutorialUse.TutorialButtons.transform.position.x = 20;

    if (TutorialUse.PositionInTutorial == 16){
        Scripts.HandCardRotationScript.Components.CardsPlayed[0].GetComponent.<ResourceCardControl>().CardNumber = 5;
        Scripts.HandCardRotationScript.Components.CardsPlayed[0].GetComponent.<ResourceCardControl>().CardPlayPosition = 1;
        Scripts.HandCardRotationScript.CardSelected = Scripts.HandCardRotationScript.Components.CardsPlayed[0];
        Scripts.HandCardRotationScript.DeselectCard();
    }
    
    if (TutorialUse.PositionInTutorial == 17){
        StartingCards.CardDistribution[0] = 1;
        StartingCards.CardDistribution[1] = 1;
        StartingCards.CardDistribution[2] = 1;
        StartingCards.CardDistribution[3] = 1;
        StartingCards.CardDistribution[4] = 0;
        TutorialResetHand();
        return;
    }
    
    TutorialUse.PositionInTutorial -= 2;
    TutorialClickNext();
}

function TutorialClickNext(){

    if (TutorialUse.EventCardClickCheck == false) TutorialUse.TopCardPosition.position = Vector3(22.25,0.05,1.51);              // Out of Scene
    if (TutorialUse.EventCardClickCheck == false) TutorialUse.OptionCardTransforms[0].position = Vector3(16,5.41,-2.64);            // OutOfScene
    if (TutorialUse.EventCardClickCheck == false) TutorialUse.OptionCardTransforms[1].position = Vector3(21,7.05,-2.64);            // OutOfScene
    if (TutorialUse.EventCardClickCheck == false) TutorialUse.OptionCardTransforms[2].position = Vector3(26.31,9.35,-2.64);         // OutOfScene
    if (TutorialUse.PositionInTutorial >= 7) TutorialUse.ResourceCards.position.z = 0;
    if (TutorialUse.PositionInTutorial <= 31) TutorialUse.Tutorial[8].SetActive(false);
    if (TutorialUse.PositionInTutorial <= 23) TutorialUse.Tutorial[9].SetActive(false);
    TutorialUse.ButtonDimScreen.SetActive(false);

    TutorialUse.Tutorial[2].transform.position = Vector3(22.25,0.05,1.51);    // Out of Scene
    TutorialUse.Tutorial[3].transform.position = Vector3(22.25,0.05,1.51);    // Out of Scene
    TutorialUse.Tutorial[4].transform.position = Vector3(22.25,0.05,1.51);    // Out of Scene
    TutorialUse.Tutorial[5].transform.position = Vector3(22.25,0.05,1.51);    // Out of Scene
    TutorialUse.Tutorial[6].transform.position = Vector3(22.25,0.05,1.51);    // Out of Scene
    TutorialUse.Tutorial[7].transform.position = Vector3(22.25,0.05,1.51);    // Out of Scene

    TutorialUse.RemoveTextCheck = false;
    TutorialUse.RemoveTopCardCheck = false;
    TutorialUse.RemoveResourceCardCheck = false;
    TutorialUse.UpdateEventClickPrevious = false;

    if (TutorialUse.PositionInTutorial == 33){
        Scripts.CameraControlScript.WinGame("Tutorial Completed!", "Can you build a kingdom that can\nwithstand the test of time?");
    }

    if (TutorialUse.PositionInTutorial == 32){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Right, we are all set! Best\nof luck and prepare for your\nfirst challenge!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 33;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 31){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "These numbers are tracked\nand displayed right in the\nmiddle. You can't miss them!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 32;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");

        TutorialUse.Tutorial[8].SetActive(true);
        for (var i=0; i<4; i++){
            TutorialUse.CounterSprite[i].color.a = 0;
            TutorialUse.CounterText[i].color.a = 0;
        }
        TutorialUse.CounterSprite[4].color.a = 0;
        yield WaitForSeconds(0.25f);
        TutorialUse.TutorialAnim[8].Play("PopOutCardCounter");
    }

    if (TutorialUse.PositionInTutorial == 30){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.033;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Hold no more than 12 resources.\nKeep chaos level below 3.\nAnd NEVER let life go extinct!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 31;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 29){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "What are missions, you ask?\nLets not confuse you first!\nFor now, just remember...";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 30;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 28){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveTopCardCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Likewise, 'Mission Cards' will\nnever be repeated unless options\nin the mission say otherwise.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 4;
        Scripts.EventCardControlScript.EventCardManager();

        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 29;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 27){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveTopCardCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Events labeled 'Special cards'\nwill never be repeated unless\nthey are added by an option.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 3;
        Scripts.EventCardControlScript.EventCardManager();

        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 28;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 26){

        TutorialUse.RemoveResourceCardCheck = true;
        TutorialUse.ResourceCardFadeOutNumber = 8;
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Selecting certain options\ncan remove certain event\ncards permanently.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        TutorialUse.TutorialAnim[7].Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 27;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 25){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "All events can be repeated\nthe following year. However,\nthere are a few exceptions.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 26;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 24){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "A year passes after every 10\nevents, and you may or may not\nget the same events next year.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 25;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 23){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
        TutorialUse.TutorialText.text = "Well, the current year is\ndisplayed on your top left.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 24;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");

        TutorialUse.Tutorial[9].SetActive(true);
        TutorialUse.YearSprite.color.a = 0;
        TutorialUse.YearText.color.a = 0;
        yield WaitForSeconds(0.25f);
        TutorialUse.TutorialAnim[9].Play("PopOutYear");
    }

    if (TutorialUse.PositionInTutorial == 22){

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
        TutorialUse.TutorialText.text = "... Oh yes that's right!\nWhat does a year mean?";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 23;   
        Scripts.HandCardRotationScript.Important.NoClicking = false;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 21){
        TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
        TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
        TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
        TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
        TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
        TutorialUse.TutorialText.text = "Try it for yourself!\n(Tap and hold option 1)";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 22;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 20){
        TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
        TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
        TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
        TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
        TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "You can learn more about\nan option's effect by tapping\nand holding an option card.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 21;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 19){
        TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
        TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
        TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
        TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
        TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Side effects are displayed in\nlarge, bold characters, as\ncan be seen in option 1.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 20;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 18){

        if (TutorialUse.EventCardClickCheck == false){
            TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
            TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
            TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
            TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
            TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        }

        if (TutorialUse.EventCardClickCheck == true){
            yield WaitForSeconds(0.5f);
        }

        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Responding to many real\nworld events frequently\nresults in side effects.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");

        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 19;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 17){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.EventCardClickCheck = true;
        Scripts.EventCardControlScript.Important.EventCardSelected = false;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
        TutorialUse.TutorialText.text = "Let's take a look at a real\nevent this time, shall we?";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
        Scripts.EventCardControlScript.EventCardManager();

        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 18;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 16){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] == 0) TutorialUse.TutorialText.text = "Well done. 1 faith is roughly\nequivalent to 2 life resources.\nYour strategies matter!";
        if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] == 2) TutorialUse.TutorialText.text = "Oh dear! We don't need more\nchaos in the forest! Be more\ncareful next time.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 17;   
        Scripts.HandCardRotationScript.Important.NoClicking = false;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 15){
        TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
        TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
        TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
        TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
        TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        TutorialUse.Tutorial[1].transform.localScale.x = 0.033;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Great job! Well, I trust you can\nnow choose an option that is\nbeneficial for us all?";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.25f);
        TutorialUse.ButtonDimScreen.SetActive(true);
        TutorialUse.TutorialAnim[11].Play("DelayedDimScreenFadeIn");
        yield WaitForSeconds(0.25f);
        TutorialUse.PositionInTutorial = 16;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 14){
        TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
        TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
        TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
        TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
        TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        Scripts.HandCardRotationScript.NeedToCloseCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Right, now since all 3 options\nconsume a Chaos resource,\nswipe up that card!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 15;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 13){
        
        if (TutorialUse.EventCardClickCheck == false){
            TutorialUse.TopCardPosition.position = Vector3(-0.61, 0, -10.2);
            TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
            TutorialUse.OptionCardTransforms[0].position = Vector3(3.09,0.1,-3.6);
            TutorialUse.OptionCardTransforms[1].position = Vector3(-0.61,0.1,-3.6);
            TutorialUse.OptionCardTransforms[2].position = Vector3(-4.31,0.1,-3.6);
        }

        if (TutorialUse.EventCardClickCheck == true){
            yield WaitForSeconds(0.5f);
        }

        TutorialUse.UpdateEventClickPrevious = true;
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
        Scripts.EventCardControlScript.EventCardManager();
        TutorialUse.UpdateEventClickPrevious = false;

        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.033;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Events can consume or provide\nresources. This is indicated by red\nand green arrows respectively.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 14;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.EventCardClickCheck = false;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 12){
        TutorialUse.RemoveTextCheck = true;
        Scripts.EventCardControlScript.Important.EventCardSelected = false;
        TutorialUse.EventCardClickCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Every year, around 10 events\nhappen in the forest. Click the\nevent that I made for you!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
        Scripts.EventCardControlScript.EventCardManager();

        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 13;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 11){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveResourceCardCheck = true;
        TutorialUse.ResourceCardFadeOutNumber = 7;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.034;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Lastly, Chaos is something we\nabsolutely do not want in the\nforest! Keep as little as possible.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        TutorialUse.TutorialAnim[6].Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 12;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 10){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveResourceCardCheck = true;
        TutorialUse.ResourceCardFadeOutNumber = 6;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.034;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Humans are troublemakers!\nBut lets not forget the good ones.\nThese will fight for our interests.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        TutorialUse.TutorialAnim[5].Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 11;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 9){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveResourceCardCheck = true;
        TutorialUse.ResourceCardFadeOutNumber = 5;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Faith represents messengers\nthat spread your wisdom.\nAND ... I'm one of them!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        TutorialUse.TutorialAnim[4].Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 10;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }
    
    if (TutorialUse.PositionInTutorial == 8){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveResourceCardCheck = true;
        TutorialUse.ResourceCardFadeOutNumber = 4;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Minerals are rare, valuable\nresources. They are used to\ntrade and build facilities.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        TutorialUse.TutorialAnim[3].Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 9;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }


    if (TutorialUse.PositionInTutorial == 7){
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveResourceCardCheck = true;
        TutorialUse.ResourceCardFadeOutNumber = 3;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Life is the backbone of our\ncommunity. A highly versatile\nand replenishable resource.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        TutorialUse.TutorialAnim[2].Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 8;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 6){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        for(i = 0; i<5; i++){
            TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localPosition.x = 22.4;
            TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localPosition.y = -9.4;
            TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localRotation.eulerAngles.z = 72;
        }
        TutorialUse.ResourceCards.position.z = 0;          //Hide Resource Cards

        Scripts.EventCardControlScript.Components.EventCardRenderer.sprite = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].SpriteGroup[3];
        Scripts.EventCardControlScript.Components.EventCardTitle.text = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].EventCardText[0];
        Scripts.EventCardControlScript.Components.EventCardDescription.text = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].EventCardText[1];
        Scripts.EventCardControlScript.Components.EventCardDescription.text = Scripts.EventCardControlScript.Components.EventCardDescription.text.Replace("NWL", "\n");
        Scripts.EventCardControlScript.Components.EventCardType.text = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].EventCardText[2];
        TutorialUse.TopCardPosition.position = Vector3(-0.62,0.05,-3.9);    // In Scene
        TutorialUse.TopCardPosition.transform.rotation.eulerAngles = Vector3(180, 180, 0);    // In Scene
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.RemoveTopCardCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.037;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
        TutorialUse.TutorialText.text = "Resources that you currently\nhave are displayed below.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.9f);
        Scripts.HandCardRotationScript.AdjustmentCheck = true;
        TutorialUse.PositionInTutorial = 7;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 5){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "Maintaining peace in your\nforest is a matter of balancing\nfive types of resources.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 0;
        Scripts.EventCardControlScript.EventCardManager();
        yield WaitForSeconds(1.25f);
        TutorialUse.PositionInTutorial = 6;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 4){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "But since you may have\nforgotten what that means...\nLet me refresh your memory!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 5;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 3){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.038;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.038;
        TutorialUse.TutorialText.text = "As a tree elder, you are the\nnatural ruler of this forest.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 4;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 2){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.05;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.05;
        TutorialUse.TutorialText.text = "... Well, that's ok.";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 3;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }
    
    if (TutorialUse.PositionInTutorial == 1){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
        TutorialUse.TutorialText.text = "You do know that I am\nyour faithful and loyal\nservant, Angie, right?";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 2;   
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }

    if (TutorialUse.PositionInTutorial == 0){
        TutorialUse.ResourceCards.position.z = 15;          //Hide Resource Cards
        TutorialUse.RemoveTextCheck = true;
        TutorialUse.Tutorial[1].transform.localScale.x = 0.1;
        TutorialUse.Tutorial[1].transform.localScale.y = 0.1;
        TutorialUse.TutorialText.text = "Hey!";
        TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(0.5f);
        TutorialUse.PositionInTutorial = 1;
        TutorialUse.FinishLoadCheck = true;
        TutorialUse.TutorialAnim[10].Play("ShowButtons");
    }
}

function ClickContinue(){
    Scripts.CameraControlScript.FadeThroughBlack(0.1);
    MoveCameraClickPlay();
    Scripts.MiscellaneousGameManagementScript.Load();
    Scripts.OptionCardScripts[0].MoveBackOnClick();
    Scripts.OptionCardScripts[1].MoveBackOnClick();
    Scripts.OptionCardScripts[2].MoveBackOnClick();
    Scripts.EventCardControlScript.Important.EventCardSelected = false;
}

function MoveCameraClickPlay(){
    Scripts.MiscellaneousGameManagementScript.FinalYearParticleEffect.SetActive(false);
    //var ColorSet = Color(0.3754, 0.3937, 0.4118, 1);
    Scripts.MiscellaneousGameManagementScript.BackgroundDim.Play("RestoreLightingBackground");

    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    Scripts.CameraControlScript.FadeThroughBlack(0.1);
    yield WaitForSeconds(0.8);
    MainMenuGameObjectsToDisable[3].GetComponent.<UnityEngine.Video.VideoPlayer>().Stop();
    MainMenuGameObjectsToDisable[0].SetActive(false);
    MainMenuGameObjectsToDisable[1].SetActive(false);
    MainMenuGameObjectsToDisable[2].SetActive(false);
    Camera.main.transform.position.z = 1.84;
    Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[1]);

    StartingImages.SetActive(true);
    MainMenuCanvas[1].SetActive(false);
    MainMenuCanvas[0].SetActive(true);
    Layers[0].SetActive(true);
    Layers[1].SetActive(false);
    CurrentPage = 1;
    for (var i = 0; i<MainMenuButtons.length; i++){       // Make the level selection buttons interactable again
        if (MainMenuButtons[i] == null) break;
        MainMenuButtons[i].interactable = true;
    }
}

function ClickQuit(){
    Confirmation("Are you sure you want to quit?", 1);
}

function ClickReset(){
    Confirmation("Delete game data permanently?", 2);
}

var ConfirmationText: UnityEngine.UI.Text;
var ConfirmationAnim: Animation;
var DimScreen: GameObject;
private var ConfirmationNumber: int;

function Confirmation(TextDisplay: String, ConfirmationNumberInput: int){
    ConfirmationNumber = ConfirmationNumberInput;
    ConfirmationText.text = TextDisplay;
    DimScreen.SetActive(true);
    ConfirmationAnim.Play("Confirmation");
}

function CloseConfirmation(){
    ConfirmationAnim.Play("RemoveConfirmation");
    WaitTime();
}

function ExecuteConfirmation(){
    if (ConfirmationNumber == 1){ Application.Quit();}
    if (ConfirmationNumber == 2){ Scripts.MiscellaneousGameManagementScript.DeleteData(); CloseConfirmation();}
}

function WaitTime(){
    yield WaitForSeconds(0.30f);
    DimScreen.SetActive(false);
}

function ResetGame(Level: int){

    for (var x = 0; x<5; x++){
        StartingCards.CardDistribution[x] = StartingCards.LevelNumber[Level-1].Distribution[x];
    }

    var CardsToDelete: int;
    CardsToDelete = CardsPlayedParentGameObject.transform.childCount;
    
    /*for (x = 0; x<20; x++){
        if(Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[x].CardIdentifier[0] != 0){
            Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[x].CardPriority = 1;
        }
    }*/

    if (CardsToDelete > 0){
        for (var del = CardsToDelete-1; del > -1; del--){
            Destroy(CardsPlayedParentGameObject.transform.GetChild(del).gameObject);
        }
    }
    
    Scripts.OptionCardScripts[0].MoveBackOnClick();
    Scripts.OptionCardScripts[1].MoveBackOnClick();
    Scripts.OptionCardScripts[2].MoveBackOnClick();
    

    PreviousNumberOfCards = HandCardRotationTransform.childCount -1;
    for (var Delete = PreviousNumberOfCards - 1; Delete > -1; Delete--){
        Destroy(HandCardRotationTransform.GetChild(Delete).gameObject);
    }

    yield WaitForSeconds(0.1);

    for (var life = 0; life < StartingCards.CardDistribution[0]; life++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[0]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (life);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[life];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[life];
    }

    for (var Resource = life; Resource < StartingCards.CardDistribution[1]+life; Resource++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[1]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Resource);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Resource];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Resource];
    }

    for (var Faith = Resource; Faith < StartingCards.CardDistribution[2]+Resource; Faith++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[2]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Faith);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Faith];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Faith];
    }

    for (var Human = Faith; Human < StartingCards.CardDistribution[3]+Faith; Human++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[3]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Human);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Human];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Human];
    }

    for (var Chaos = Human; Chaos < StartingCards.CardDistribution[4]+Human; Chaos++){
        InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[4]);
        InstantiatedCard.transform.parent = HandCardRotationTransform;
        InstantiatedCard.transform.SetSiblingIndex (Chaos);
        InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Chaos];
        InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Chaos];
    } 
    
    //Cannot reset by class because it will make a "shallow" copy e.g. cannot do: Scripts.EventCardControlScript.AllEventCardsInGame[0] = Scripts.EventCardControlScript.AllEventCardsReset;   

    //Things that need to be reset from EventCardControl (Important) 
    Scripts.EventCardControlScript.Important.EventCardSelected = false;
    Scripts.EventCardControlScript.Important.SpecialCardActivated = 0;
    
    /*for (var i = 0; i<20; i++){     // 13 is the number of tech 1 cards
        Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardIdentifier[0] = 0;
        Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].CardIdentifier[1] = 0;
        Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].SpriteDisplayIdentifier[0] = 0;
        Scripts.EventCardControlScript.AllEventCardsInGame[0].HumanTech0[i].SpriteDisplayIdentifier[1] = 0;
    }*/

    for (var i = 0; i<10; i++){
        Scripts.EventCardControlScript.Important.CardsToAddIn[i].TechLevel = 0;
        Scripts.EventCardControlScript.Important.CardsToAddIn[i].Position = 0;
        Scripts.EventCardControlScript.Important.CardsToDestroy[i].TechLevel = 0;
        Scripts.EventCardControlScript.Important.CardsToDestroy[i].Position = 0;
    }

    for (i = 0; i < HandCardRotationTransform.childCount-1; i++) {
            if (HandCardRotationTransform.GetChild(i).gameObject.transform.childCount >0){
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent.<Transform>().localPosition.z = -0.01;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(2).gameObject.GetComponent.<Transform>().localPosition.z = -0.03;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(3).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
        }
    }

    Scripts.HandCardRotationScript.InitialiseNewGame();
    if (LevelSelected != 1){
        Scripts.EventCardControlScript.InitialiseNewGame();
        TutorialUse.ResourceCards.position.z = 15;
    }

    if (LevelSelected == 1) Scripts.EventCardControlScript.LaunchTutorial();
}

class ScriptReference3{
    var CameraControlScript: Camera_Control;
    var EventCardControlScript: EventCardControl;
    var HandCardRotationScript: HandCardRotation;
    var OptionClickControlScript: OptionClickControl;
    var OptionCardScripts = new Option_Card_Movement[3];
    var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
    var ParticleEffectControllerScript: ParticleEffectController;
    var UIControllerScript: UIController;
    var NatureControllerScript: NatureController;
    }

class StartingCardDeal{
    var CardDistribution = new int[5]; //number of life, resource, faith, human and chaos cards
    var LevelNumber = new CardDistributionPerLevel[10];
    }

class CardDistributionPerLevel{
    var Distribution = new int[5];
}

class TutorialStuff{
    var PositionInTutorial: int;
    var ResourceCardFadeOutNumber: int;
    var RemoveResourceCardCheck: boolean = false;
    var RemoveTextCheck: boolean = false;
    var RemoveTopCardCheck: boolean = false;
    var EventCardClickCheck: boolean = false;
    var UpdateEventClickPrevious: boolean = false;
    var ClickMagnifierCheck: boolean = false;
    var FinishLoadCheck: Boolean;
    var OptionCardTransforms = new Transform[3];
    var CounterSprite = new UnityEngine.UI.Image[3];
    var CounterText = new UnityEngine.UI.Text[3];
    var Tutorial = new GameObject[10];
    var TutorialAnim = new Animation[12];
    var YearSprite: UnityEngine.UI.Image;
    var YearText: UnityEngine.UI.Text;
    var TutorialButtons: GameObject;
    var ButtonDimScreen: GameObject;
    var TutorialText: UnityEngine.UI.Text;
    var TopCardPosition: Transform;
    var ResourceCards: Transform; 
}

class LevelStuff{
    var PositionInTalking: int;
    var PositionInEnding: int;
    var FinishTalk: boolean;
    var FinishTalkEnding: boolean;
    var SpeechLoaded: boolean;
    var SpeechBubble = new OneLevelSpeech[10];
    var SpeechObjects = new GameObject[3];
    var SpeechAnimations = new Animation[3];
}

class OneLevelSpeech{
    var Level: String;
    var Speech = new OneSpeechInfo[10];
    var SpeechEnding = new OneSpeechInfo[3];
    var CurrentYear: int;
    var TargetYear: int;
    var BcOrAd: String;
    var LevelObjective: String;
    var Morale: int;
    var HumanTies: int;
    var FoodConsumption: int;
    var FoodProduction: int;
    var StartingMorale: int;
    var StartingFood: int;
    var StartingMoraleHuman: int;
    var StartingFoodHuman: int;
    var StartingHumanTies: int;
}

class OneSpeechInfo{
    var Text: String;
    var Size = new float[2];
}