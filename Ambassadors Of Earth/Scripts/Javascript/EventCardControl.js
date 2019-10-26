﻿#pragma strict

var Scripts: ScriptReference;
var Components: ComponentReference;
var ShuffleCount: CardCounting;
var Important: ImportantVars;
var AllEventCardsInGame = new AllEventCards[1];
var EventDisplay: AllEventCardSprites;
var ImportantReset: ImportantVars;
var AllEventCardsReset: AllEventCards;
var Victory: VictoryCondition;
var ShuffleCheck: boolean = false;

private var FinalYearShownCheck: boolean;
private var MovePoint: Vector3;

function LaunchTutorial(){
    
    Important.NumberOfShuffles = 0;
    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[8].SetActive(false);
    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[9].SetActive(false);
    Scripts.UIControllerScript.UpdateRoundNumber(Important.NumberOfShuffles+1);
    Scripts.MainMenuControllerScript.TutorialUse.ResourceCards.position.z = 15;

    for (var ClearCard = 0; ClearCard < Important.CardToUse.length; ClearCard++){
        Important.CardToUse[ClearCard].CardIdentifier[0] = 0;
        Important.CardToUse[ClearCard].CardIdentifier[1] = 0;
        Important.CardToUse[ClearCard].SpriteDisplayIdentifier[0] = 0;
        Important.CardToUse[ClearCard].SpriteDisplayIdentifier[1] = 0;
    }

    for (ClearCard = 0; ClearCard < 10; ClearCard++){
        Important.EventCardOrder[ClearCard] = ClearCard + 1;
    }

    Important.CardToUse[0].CardIdentifier[0] = 7;
    Important.CardToUse[0].CardIdentifier[1] = 1;
    Important.CardToUse[0].SpriteDisplayIdentifier[0] = 7;
    Important.CardToUse[0].SpriteDisplayIdentifier[1] = 1;

    Important.CardToUse[1].CardIdentifier[0] = 7;
    Important.CardToUse[1].CardIdentifier[1] = 2;
    Important.CardToUse[1].SpriteDisplayIdentifier[0] = 7;
    Important.CardToUse[1].SpriteDisplayIdentifier[1] = 2;

    Important.CardToUse[2].CardIdentifier[0] = 1;
    Important.CardToUse[2].CardIdentifier[1] = 1;
    Important.CardToUse[2].SpriteDisplayIdentifier[0] = 1;
    Important.CardToUse[2].SpriteDisplayIdentifier[1] = 8;

    Important.CardToUse[3].CardIdentifier[0] = 5;
    Important.CardToUse[3].CardIdentifier[1] = 2;
    Important.CardToUse[3].SpriteDisplayIdentifier[0] = 5;
    Important.CardToUse[3].SpriteDisplayIdentifier[1] = 2;

    Important.CardToUse[4].CardIdentifier[0] = 6;
    Important.CardToUse[4].CardIdentifier[1] = 1;
    Important.CardToUse[4].SpriteDisplayIdentifier[0] = 6;
    Important.CardToUse[4].SpriteDisplayIdentifier[1] = 1;
}

function InitialiseNewGame(){
    
    Important.NumberOfShuffles = 0;
    Components.EventCard.transform.position = Vector3(22.25,0.05,1.51);
    
    Important.PositiveHumanEvent[0] = 0;
    Important.NegativeHumanEvent[0] = 0;
    Important.LowMoraleEvent[0] = 0;
    Important.LowFoodEvent[0] = 0;
    Important.PositiveHumanEvent[1] = 0;
    Important.NegativeHumanEvent[1] = 0;
    Important.LowMoraleEvent[1] = 0;
    Important.LowFoodEvent[1] = 0;
    Important.HumanRelationship = 0;
    Important.FoodProduction = 0;
    Important.FoodConsumption = 0;
    Important.Morale = 0;
    Important.HumanTies = 0;
    UpdateHumanRelationship(0);
    FinalYearShownCheck = false;

    if (Scripts.MainMenuControllerScript.LevelSelected == 1){ 
        ShuffleCheck = false;
        ShuffleCard();  
        yield WaitForSeconds(0.1);
        while (ShuffleCheck == false){
            yield WaitForSeconds(0.1);
        }

        Scripts.HandCardRotationScript.Important.NoClicking = false;
    }
}

private var StartMovingCheck: boolean = false;
private var LerpProgress: float = 0;

function Update () {
        
    if (StartMovingCheck == true){
        LerpProgress += Time.deltaTime/1.5;
        Components.EventCard.transform.localPosition = Vector3.Lerp(Components.EventCard.transform.localPosition,MovePoint,LerpProgress);

        if (LerpProgress >= 1){
            StartMovingCheck = false;
        }
    }
}

function EventCardClicked(){ // Clicking is taken from HandCardRotation
    Important.EventCardSelected = true; 
    UpdateOptionCardInfo();
    LerpProgress = 0;
    StartMovingCheck = true;
    //Debug.DrawLine(ray.origin, hit.point);
    MovePoint= Vector3(-0.61,0,-10.2); 
    Scripts.OptionCardScripts[0].MoveOnClick();
    Scripts.OptionCardScripts[1].MoveOnClick();
    Scripts.OptionCardScripts[2].MoveOnClick();
}

function EventCardDeselected(){ // Clicking is taken from HandCardRotation
    LerpProgress = 0;
    Important.EventCardSelected = false; 
    StartMovingCheck = true;
    //Debug.DrawLine(ray.origin, hit.point);
    MovePoint= Vector3(-0.61,0.092,-3.9); 
    Scripts.OptionCardScripts[0].MoveBackOnClick();
    Scripts.OptionCardScripts[1].MoveBackOnClick();
    Scripts.OptionCardScripts[2].MoveBackOnClick();
}

function BattleCardClicked(){
    LerpProgress = 0;
    Scripts.CharacterBattleControllerScript[0].MoveToActivePosition();
    Scripts.CharacterBattleControllerScript[1].MoveToActivePosition();
    Important.EventCardSelected = true; 
    StartMovingCheck = true;
    //Debug.DrawLine(ray.origin, hit.point);
    MovePoint= Vector3(-0.61,0,-10.2); 
}

function BattleCardDeselected(){ // Clicking is taken from HandCardRotation
    
    if (Scripts.NatureControllerScript.BattleOptionChooseCheck == true) return;
    LerpProgress = 0;
    Scripts.CharacterBattleControllerScript[0].MoveToInactivePosition();
    Scripts.CharacterBattleControllerScript[1].MoveToInactivePosition();
    Important.EventCardSelected = false; 
    yield WaitForSeconds(0.5f);
    StartMovingCheck = true;
    //Debug.DrawLine(ray.origin, hit.point);
    MovePoint= Vector3(-0.61,0.092,-3.9); 
}

function BattleCardRemoved(){
    Scripts.HandCardRotationScript.SortHandCards();
    LerpProgress = 0;
    this.gameObject.tag = "EventCards";
    Important.SpecialCardActivated = 0;
    Important.EventCardSelected = false; 
    Scripts.CharacterBattleControllerScript[0].MoveToInactivePosition();
    Scripts.CharacterBattleControllerScript[1].MoveToInactivePosition();
    yield WaitForSeconds(0.5f);
    StartMovingCheck = true;
    MovePoint= Vector3(-0.61,0.092,-3.9); 
    yield WaitForSeconds (1);
    Components.anim.Play("RemoveCard");
    yield WaitForSeconds (1.183f);
    EventCardManager();
}

function EventCardRemoved(){

    Scripts.HandCardRotationScript.SortHandCards();
    LerpProgress = 0;
    Important.EventCardSelected = false; 
    StartMovingCheck = true;
    MovePoint= Vector3(-0.61,0.092,-3.9); 
    Scripts.OptionCardScripts[0].MoveBackOnClick();
    Scripts.OptionCardScripts[1].MoveBackOnClick();
    Scripts.OptionCardScripts[2].MoveBackOnClick();
    yield WaitForSeconds (1);

    if (Important.ImmidiateDrawCardNumber == 0){
        ShuffleCount.PositionInShuffle +=1;
        Scripts.UIControllerScript.InformationOverviewText[7].text = "Events This Year: " + (ShuffleCount.PositionInShuffle+1) + "/10";
    }
    
    if (Important.ImmidiateDrawCardNumber!= 0 && Scripts.HandCardRotationScript.Important.SelectedOptionCardNumber ==2){
        Scripts.CameraControlScript.LoseGame("Defeated", "Always keep track of your life and\nchaos cards to avoid this fate!");
        Components.anim.Play("RemoveCard");
        return;
    }
    
    if (Important.DestroyEventCheck == false){
        Components.anim.Play("RemoveCard");
    }

    if (Important.DestroyEventCheck == true){
        Important.DestroyEventCheck = false;
        Components.anim.Play("DestroyCard");
        yield WaitForSeconds(0.5f);
    }

    yield WaitForSeconds (1.183f);
    if (Scripts.MainMenuControllerScript.LevelSelected == 1){
        Scripts.MainMenuControllerScript.ClearPrevious();
        return;
    }
    EventCardManager();
}

function AnnualReportRemoved(){

    var TempFloat: float =  Important.FoodProduction - Important.FoodConsumption;

    Scripts.NatureControllerScript.IncreaseNatureFood(TempFloat/100);
    UpdateHumanRelationship(Important.HumanTies);
    Scripts.NatureControllerScript.UpdateMorale(Important.Morale);

    Components.AnnualReportAnim.Play("RemoveAnnualReport");
    ShuffleCount.PositionInShuffle ++;

    yield WaitForSeconds(0.50f);
    Components.AnnualReportAnim.Play("RemoveCard");
    yield WaitForSeconds(1.183f);
    EventCardManager();
}

private var RandomlyDestroyedCardNumbers = new int[3];

function EventCardManager(){

    if (Important.ImmidiateDrawCardNumber != 0) Important.ImmidiateDrawCardNumber = 0;

    Scripts.OptionClickControlScript.Components.AnimationStatsChange[0].SetActive(false);
    Scripts.OptionClickControlScript.Components.AnimationStatsChange[1].SetActive(false);
    Scripts.OptionClickControlScript.Components.AnimationStatsChange[2].SetActive(false);
    Scripts.OptionClickControlScript.Components.AnimationStatsChange[3].SetActive(false);
    Components.MissionSpriteHolder.SetActive(false);

    Components.OptionCardEffect[0].text ="";
    Components.OptionCardEffect[1].text ="";
    Components.OptionCardEffect[2].text ="";

    if (Scripts.HandCardRotationScript.Important.NumberOfCards > 12){
        RandomlyDestroyedCardNumbers[0] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards); //random.range not inclusive of max integer
        RandomlyDestroyedCardNumbers[1] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);
        RandomlyDestroyedCardNumbers[2] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);

        while ( RandomlyDestroyedCardNumbers[1] == RandomlyDestroyedCardNumbers[0])RandomlyDestroyedCardNumbers[1] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);
        while ( RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[0] || RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[1])RandomlyDestroyedCardNumbers[2] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);
        Components.EventCardRenderer.sprite = EventDisplay.Miscellaneous.Position[0].SpriteGroup[3];
        Components.EventCardTitle.text = "Overload";
        Components.EventCardDescription.text = "Discard 3 random resource cards\nfrom your hand";
        Components.EventCardType.text = "Special Card";
        this.gameObject.tag = "NoOptionEventCard";
        Components.anim.Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.183f);
        Scripts.HandCardRotationScript.DestroyExtra3Cards(RandomlyDestroyedCardNumbers[0], RandomlyDestroyedCardNumbers[1], RandomlyDestroyedCardNumbers[2]);
        return;
    }

    if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] == 0){
        Important.ImmidiateDrawCardNumber = 3;
        LoadCardInfo(3);
        return;
    }

    if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] == 3){
        Important.ImmidiateDrawCardNumber = 4;
        LoadCardInfo(4);
        return;
    }

    if (Important.SpecialCardActivated == 1){
        Components.EventCardRenderer.sprite = EventDisplay.Miscellaneous.Position[1].SpriteGroup[3];
        Components.EventCardTitle.text = "The Struggle";
        Components.EventCardDescription.text = "Humans and beasts face off in\nan intense skirmish.";
        Components.EventCardType.text = "Special Card";
        this.gameObject.tag = "BattleCard";
        Components.anim.Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield WaitForSeconds(1.183f);
        Scripts.HandCardRotationScript.Important.NoClicking = false;
        return;
    }

    if(ShuffleCount.PositionInShuffle > ShuffleCount.NumberOfCardsBeforeShuffle){
        
        Important.NumberOfShuffles++ ;
        ShuffleCheck = false;   //WorkingOnNow      
        ShuffleCard();
        yield WaitForSeconds(3);
        Components.HideWhileShuffling[0].SetActive(true);
        Components.HideWhileShuffling[1].SetActive(true);
        Components.HideWhileShuffling[2].SetActive(true);
        Components.ShufflingCardsAnimationObject.SetActive(false);
        while (ShuffleCheck == false){
            yield WaitForSeconds(0.1);
        }
    }

    if(ShuffleCount.PositionInShuffle > ShuffleCount.NumberOfCardsBeforeShuffle -1){
        
        var Sign: String;

        Components.AnnualReportText[0].text = "Food Production: " + Important.FoodProduction;
        Important.FoodConsumption = Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] * 3;
        Components.AnnualReportText[1].text = "Food Consumption: " + Important.FoodConsumption;

        if (Important.FoodProduction - Important.FoodConsumption > 0) Sign = "+";
        else Sign = "";
        Components.AnnualReportText[2].text = "Food: " +  Sign + (Important.FoodProduction - Important.FoodConsumption);

        if (Important.Morale > 0) Sign = "+";
        else Sign = "";
        Components.AnnualReportText[3].text = "Morale: " + Sign + Important.Morale;

        if (Important.HumanTies > 0) Sign = "+";
        else Sign = "";
        Components.AnnualReportText[4].text = "Human Ties: " + Sign + Important.HumanTies;

        Components.AnnualReportAnim.Play("DrawCard");
        yield WaitForSeconds(1.183f);
        Components.AnnualReportAnim.Play("ShowAnnualReport");
        yield WaitForSeconds(0.3f);
        Scripts.HandCardRotationScript.Important.NoClicking = false;
        return;
    }

    if (Scripts.MainMenuControllerScript.LevelSelected != 1){
        if (Important.NumberOfShuffles != 0 && Important.NumberOfShuffles +1 == Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected-2].RoundsNumber && FinalYearShownCheck == false){ 
            Scripts.MiscellaneousGameManagementScript.NewNotice(1, "Final Year", "Be sure to meet the objectives\nby the end of this year!", "Notice");
            FinalYearShownCheck = true;
            return; 
        }
    }

    EventCardPlayed = Important.EventCardOrder[ShuffleCount.PositionInShuffle];
    
    while (Important.CardToUse[EventCardPlayed-1].CardIdentifier[0] == 0){ //Check if it is a blank card
        ShuffleCount.PositionInShuffle += 1;
        ShuffleCount.NumberOfCardsBeforeShuffle += 1;
        EventCardPlayed = Important.EventCardOrder[ShuffleCount.PositionInShuffle];
    }

    if (Important.HumanRelationship <= - 10 && Important.CardToUse[EventCardPlayed-1].CardPriority != -1 && Important.NegativeHumanEvent[1] < Important.NegativeHumanEvent[0]){ //Negative Human Relation
        var RollLuck = Random.Range( 0.0f, 1.0f );
        if (RollLuck >= 0.9 + 0.02*(Important.HumanRelationship+10)){
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[0] = 3;
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[1] = 1;
            Important.CardToUse[EventCardPlayed-1].CardPriority = -1;
            Important.NegativeHumanEvent[1] ++;
        }
    }

    if (Important.HumanRelationship >= 10 && Important.CardToUse[EventCardPlayed-1].CardPriority != -1 && Important.PositiveHumanEvent[1] < Important.PositiveHumanEvent[0]){  //Positive Relation
        
        RollLuck = Random.Range( 0.0f, 1.0f );

        if (RollLuck >= 0.9 - 0.02*(Important.HumanRelationship -10)){
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[0] = 4;
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[1] = 1;
            Important.CardToUse[EventCardPlayed-1].CardPriority = -1;
            Important.PositiveHumanEvent[1] ++;
        }
    }

    if (Scripts.NatureControllerScript.Health < 8 && Important.CardToUse[EventCardPlayed-1].CardPriority != -1 && Important.LowMoraleEvent[1] < Important.LowMoraleEvent[0]){ //Low Morale

        RollLuck = Random.Range( 0.0f, 1.0f );
        if (Scripts.NatureControllerScript.Health == 0) RollLuck = 1;   //100% chance to draw low morale card if morale = 0.

        if (RollLuck >= 0.9 - 0.1*(7.5 - Scripts.NatureControllerScript.Health)){

            var RollNumberMorale: int;
            RollNumberMorale = Random.Range (1, 4);
            //Debug.Log(RollNumberMorale);
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[0] = 2;
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[1] = RollNumberMorale;
            Important.CardToUse[EventCardPlayed-1].CardPriority = -1;
            Important.LowMoraleEvent[1] ++;
        }
    }

     if (Scripts.NatureControllerScript.CurrentFood < 40 && Important.CardToUse[EventCardPlayed-1].CardPriority != -1 && Important.LowFoodEvent[1] < Important.LowFoodEvent[0]){ 

        RollLuck = Random.Range( 0.0f, 1.0f );
        if (Scripts.NatureControllerScript.CurrentFood == 0) RollLuck = 1;   //100% chance to draw low food card if morale = 0.

        if (RollLuck >= 0.9 - 0.02*(35 - Scripts.NatureControllerScript.CurrentFood)){

            var RollNumberFood: int;
            RollNumberFood = Random.Range (16, 19);
            //Debug.Log(RollNumberFood);
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[0] = 2;
            Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[1] = RollNumberFood;
            Important.CardToUse[EventCardPlayed-1].CardPriority = -1;
            Important.LowFoodEvent[1] ++;
        }
    }

    var TechLevel: int;
    var SpritePosition: int;
    TechLevel = Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[0];
    SpritePosition = Important.CardToUse[EventCardPlayed-1].SpriteDisplayIdentifier[1];

    Components.EventCardRenderer.sprite = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].SpriteGroup[3];
    Components.EventCardTitle.text = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].EventCardText[0];
    Components.EventCardDescription.text = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].EventCardText[1];
    Components.EventCardDescription.text = Components.EventCardDescription.text.Replace("NWL", "\n");
    Components.EventCardType.text = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].EventCardText[2];
    Scripts.OptionClickControlScript.EventDetails.EnableAnimation = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].CalledCard.EnableAnimation; //Shuffle in animation
    Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[0] = TechLevel;
    Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[1] = SpritePosition;

    if (EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].MissionProgress != 0){
        Components.MissionSpriteHolder.SetActive(true);
        Components.MissionSpriteRenderers[0].sprite = Components.MissionSprites[1];

        if (EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].MissionProgress == 1){
            Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[0];
            Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
        }

        if (EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].MissionProgress == 2){
            Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
            Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
        }

        if (EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].MissionProgress == 3){
            Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
            Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[1];
        }
    }

    for (var i = 0; i < 3; i++){    //WORKINGONNOW
            
            Scripts.OptionClickControlScript.EventDetails.TechnologyRequired[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].SkillsRequirements[i];
            Scripts.OptionClickControlScript.EventDetails.ResourceRequirements[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].CardRequirements[i];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].ResourcesUsed[i];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i+3] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].ResourcesUsed[i+3];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i+6] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].ResourcesUsed[i+6];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].ResourcesGiven[i];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i+3] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].ResourcesGiven[i+3];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i+6] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].ResourcesGiven[i+6];
            Scripts.OptionClickControlScript.EventDetails.StatsChange[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].StatsChange[i];
            Scripts.OptionClickControlScript.EventDetails.StatsChange[i+3] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].StatsChange[i+3];
            Scripts.OptionClickControlScript.EventDetails.UnlockOptionNumber[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].UnlockOptionNumber[i];
            Scripts.OptionClickControlScript.EventDetails.RemoveEventCheck[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].RemoveEventCheck[i];
            Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].CalledCard.TechLevel[i];
            Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].CalledCard.Position[i]; 
            Scripts.OptionClickControlScript.EventDetails.ChanceToAddCard[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].CalledCard.ChanceToAddCard[i];
            Scripts.OptionClickControlScript.EventDetails.DiscardCards[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].DiscardCards[i]; 
            Scripts.OptionClickControlScript.EventDetails.LoseCheck[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].LoseCheck[i]; 
            Scripts.OptionClickControlScript.EventDetails.PromotionSprite[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].PromotionSprite[i];    
            Scripts.OptionClickControlScript.EventDetails.PromotionText[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].PromotionText[i];    
            Scripts.OptionClickControlScript.EventDetails.PromotionText[i+3] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].PromotionText[i+3];   
            Scripts.OptionClickControlScript.EventDetails.HumanRelationshipChange[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].HumanRelationshipChange[i];
            Scripts.OptionClickControlScript.EventDetails.FoodChange[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].FoodChange[i];
           
            Scripts.OptionClickControlScript.EventDetails.FoodProductionChange[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].FoodProductionChange[i];   
            Scripts.OptionClickControlScript.EventDetails.MoraleProductionChange[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].MoraleProductionChange[i];
            Scripts.OptionClickControlScript.EventDetails.HumanTiesProductionChange[i] = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].HumanTiesProductionChange[i];

            Components.OptionCardRenderers[i].sprite = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].SpriteGroup[i];
            Components.LockedOptionBlacken[i].SetActive(false);
            Components.OptionCardTitleRenderers[i].text = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].OptionCardTitle[i];
            Components.OptionCardTitleRenderers[i].text = Components.OptionCardTitleRenderers[i].text.Replace("NWL", "\n");
            Components.OptionCardHolder[i].tag = "OptionCards";
            Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
            Components.OptionCardQuoteSize[i].localScale.x = 0.07;
            Components.OptionCardQuoteSize[i].localScale.y = 0.07;
            Components.OptionCardQuoteRenderers[i].text = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].OptionCardQuote[i];
            Components.OptionCardQuoteRenderers[i].text = Components.OptionCardQuoteRenderers[i].text.Replace("NWL", "\n");
            Components.OptionCardEffect[i].text = EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].OptionCardEffect[i];
            Components.OptionCardEffect[i].text = Components.OptionCardEffect[i].text.Replace("NWL", "\n");
            
            if (EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].UnlockOptionNumber[i] == -1){   //Check if any option is locked
                Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
                Components.OptionCardHolder[i].tag = "LockedOption";
            }

            if (EventDisplay.Normal[TechLevel-1].Position[SpritePosition-1].SpriteGroup[i] == null){       //For Standard Cards to check if any option is not present.
                Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
                //Components.LockedOptionBlacken[i].SetActive(true);
                Components.OptionCardTitleRenderers[i].text = "Option Unavailable";
                Components.OptionCardHolder[i].tag = "LockedOption";
                Components.OptionCardQuoteSize[i].localScale.x = 0.07;
                Components.OptionCardQuoteSize[i].localScale.y = 0.07;
                Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
                Components.OptionCardQuoteRenderers[i].text = '<i>"Make an empty space and\ncreativity will instantly fill it."</i>';
            }
    }

    if (Scripts.MainMenuControllerScript.TutorialUse.UpdateEventClickPrevious == true){
        UpdateOptionCardInfo();
        return;
    }

    Components.anim.Play("DrawCard");
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);   
    if (Scripts.MainMenuControllerScript.LevelSelected != 1) Important.CardsInDeck[SpritePosition-1].CardPriority = 0;

    yield WaitForSeconds(1.183f);
    Scripts.HandCardRotationScript.Important.NoClicking = false;
}

function LoadCardInfo (PositionNumber: int){

    var ReferencePosition = EventDisplay.Miscellaneous.Position[PositionNumber-1];
    Components.EventCardRenderer.sprite = ReferencePosition.SpriteGroup[3];
    Components.EventCardTitle.text = ReferencePosition.EventCardText[0];
    Components.EventCardDescription.text = ReferencePosition.EventCardText[1];
    Components.EventCardDescription.text = Components.EventCardDescription.text.Replace("NWL", "\n");
    Components.EventCardType.text = ReferencePosition.EventCardText[2];
    Scripts.OptionClickControlScript.EventDetails.EnableAnimation = ReferencePosition.CalledCard.EnableAnimation; //Shuffle in animation
    Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[0] = 0;
    Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[1] = PositionNumber;

    if (ReferencePosition.MissionProgress != 0){
        Components.MissionSpriteHolder.SetActive(true);
        Components.MissionSpriteRenderers[0].sprite = Components.MissionSprites[1];

        if (ReferencePosition.MissionProgress == 1){
            Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[0];
            Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
        }

        if (ReferencePosition.MissionProgress == 2){
            Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
            Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
        }

        if (ReferencePosition.MissionProgress == 3){
            Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
            Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[1];
        }

    }

    for (var i = 0; i < 3; i++){
            
        Scripts.OptionClickControlScript.EventDetails.TechnologyRequired[i] = ReferencePosition.SkillsRequirements[i];
        Scripts.OptionClickControlScript.EventDetails.ResourceRequirements[i] = ReferencePosition.CardRequirements[i];
        Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i] = ReferencePosition.ResourcesUsed[i];
        Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i+3] = ReferencePosition.ResourcesUsed[i+3];
        Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i+6] = ReferencePosition.ResourcesUsed[i+6];
        Scripts.OptionClickControlScript.EventDetails.ResourceGained[i] = ReferencePosition.ResourcesGiven[i];
        Scripts.OptionClickControlScript.EventDetails.ResourceGained[i+3] = ReferencePosition.ResourcesGiven[i+3];
        Scripts.OptionClickControlScript.EventDetails.ResourceGained[i+6] = ReferencePosition.ResourcesGiven[i+6];
        Scripts.OptionClickControlScript.EventDetails.StatsChange[i] = ReferencePosition.StatsChange[i];
        Scripts.OptionClickControlScript.EventDetails.StatsChange[i+3] = ReferencePosition.StatsChange[i+3];
        Scripts.OptionClickControlScript.EventDetails.UnlockOptionNumber[i] = ReferencePosition.UnlockOptionNumber[i];
        Scripts.OptionClickControlScript.EventDetails.RemoveEventCheck[i] = ReferencePosition.RemoveEventCheck[i];
        Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[i] = ReferencePosition.CalledCard.TechLevel[i];
        Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[i] = ReferencePosition.CalledCard.Position[i]; 
        Scripts.OptionClickControlScript.EventDetails.ChanceToAddCard[i] = ReferencePosition.CalledCard.ChanceToAddCard[i];
        Scripts.OptionClickControlScript.EventDetails.DiscardCards[i] = ReferencePosition.DiscardCards[i]; 
        Scripts.OptionClickControlScript.EventDetails.LoseCheck[i] = ReferencePosition.LoseCheck[i]; 
        Scripts.OptionClickControlScript.EventDetails.PromotionSprite[i] = ReferencePosition.PromotionSprite[i];    
        Scripts.OptionClickControlScript.EventDetails.PromotionText[i] = ReferencePosition.PromotionText[i];    
        Scripts.OptionClickControlScript.EventDetails.PromotionText[i+3] = ReferencePosition.PromotionText[i+3];   
        Scripts.OptionClickControlScript.EventDetails.HumanRelationshipChange[i] = ReferencePosition.HumanRelationshipChange[i];
        Scripts.OptionClickControlScript.EventDetails.FoodChange[i] = ReferencePosition.FoodChange[i];
           
        Scripts.OptionClickControlScript.EventDetails.FoodProductionChange[i] = ReferencePosition.FoodProductionChange[i];   
        Scripts.OptionClickControlScript.EventDetails.MoraleProductionChange[i] = ReferencePosition.MoraleProductionChange[i];
        Scripts.OptionClickControlScript.EventDetails.HumanTiesProductionChange[i] = ReferencePosition.HumanTiesProductionChange[i];

        Components.OptionCardRenderers[i].sprite = ReferencePosition.SpriteGroup[i];
        Components.LockedOptionBlacken[i].SetActive(false);
        Components.OptionCardTitleRenderers[i].text = ReferencePosition.OptionCardTitle[i];
        Components.OptionCardTitleRenderers[i].text = Components.OptionCardTitleRenderers[i].text.Replace("NWL", "\n");
        Components.OptionCardHolder[i].tag = "OptionCards";
        Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
        Components.OptionCardQuoteSize[i].localScale.x = 0.07;
        Components.OptionCardQuoteSize[i].localScale.y = 0.07;
        Components.OptionCardQuoteRenderers[i].text = ReferencePosition.OptionCardQuote[i];
        Components.OptionCardQuoteRenderers[i].text = Components.OptionCardQuoteRenderers[i].text.Replace("NWL", "\n");
        Components.OptionCardEffect[i].text = ReferencePosition.OptionCardEffect[i];
        Components.OptionCardEffect[i].text = Components.OptionCardEffect[i].text.Replace("NWL", "\n");
            
        if (ReferencePosition.UnlockOptionNumber[i] == -1){   //Check if any option is locked
            Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
            Components.OptionCardHolder[i].tag = "LockedOption";
        }

        if (ReferencePosition.SpriteGroup[i] == null){       //For Standard Cards to check if any option is not present.
            Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
            Components.OptionCardTitleRenderers[i].text = "Option Unavailable";
            Components.OptionCardHolder[i].tag = "LockedOption";
            Components.OptionCardQuoteSize[i].localScale.x = 0.07;
            Components.OptionCardQuoteSize[i].localScale.y = 0.07;
            Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
            Components.OptionCardQuoteRenderers[i].text = '<i>"Make an empty space and\ncreativity will instantly fill it."</i>';
        }
    }

    Components.anim.Play("DrawCard");
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);   
    yield WaitForSeconds(1.183f);
    Scripts.HandCardRotationScript.Important.NoClicking = false;
}

private var HighestPriorityCards = new int[20];
private var HighPriorityCards = new int[20];
private var MidPriorityCards = new int[20];
private var LowPriorityCards = new int[20];

function ShuffleCard(){

    ShuffleCount.NumberOfCardsBeforeShuffle = 10;
    Important.PositiveHumanEvent[1] = 0;
    Important.NegativeHumanEvent[1] = 0;
    Important.LowMoraleEvent[1] = 0;
    Important.LowFoodEvent[1] = 0;
    Scripts.UIControllerScript.InformationOverviewText[8].text = "You have not added any events for next year (yet).";
    Scripts.UIControllerScript.InformationOverviewText[9].text = "";

    for (var i = 0; i< 10; i++){
        Scripts.OptionClickControlScript.Components.CardAddedNames[i] = "";
    }

    if(Important.NumberOfShuffles != 0){
        Scripts.UIControllerScript.UpdateRoundNumber(Important.NumberOfShuffles);
        
        var VictoryPath = Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected-2];
        if (VictoryPath.RoundsNumber == Important.NumberOfShuffles){
            VictoryPath.VictoryCheck = true;
            Scripts.HandCardRotationScript.Important.NoClicking = false;

            for (i = 0; i<5; i++){
                if (VictoryPath.CardsHolding[i] != 0){                
                    if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[i] < VictoryPath.CardsHolding[i]){
                        VictoryPath.VictoryCheck = false;
                        break;
                    }
                }
            }
            if (VictoryPath.VictoryCheck == true){
                //Debug.Log("WinLevel");
                Scripts.MainMenuControllerScript.LevelUse.FinishTalkEnding = false;
                Scripts.MainMenuControllerScript.LevelUse.PositionInEnding = 0;
                Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[0].SetActive(true);
                Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position.x = 20;
                Scripts.MainMenuControllerScript.AngieTalkEnding();
                return;
            }
            if (VictoryPath.VictoryCheck == false){
                //Debug.Log("LoseLevel");
                Scripts.MainMenuControllerScript.LevelUse.FinishTalkEnding = false;
                Scripts.MainMenuControllerScript.LevelUse.PositionInEnding = 3;
                Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[0].SetActive(true);
                Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position.x = 20;
                Scripts.MainMenuControllerScript.AngieTalkEnding();
                return;
            }
        }

        Components.ShufflingCardsAnimationObject.SetActive(true);
        Scripts.ShufflingCardAnimationScript.PlayShuffle();
        Components.HideWhileShuffling[0].SetActive(false);
        Components.HideWhileShuffling[1].SetActive(false);
        Components.HideWhileShuffling[2].SetActive(false);
    }

    for (var ClearCard = 0; ClearCard < Important.CardToUse.length; ClearCard++){
        Important.CardToUse[ClearCard].CardIdentifier[0] = 0;
        Important.CardToUse[ClearCard].CardIdentifier[1] = 0;
        Important.CardToUse[ClearCard].SpriteDisplayIdentifier[0] = 0;
        Important.CardToUse[ClearCard].SpriteDisplayIdentifier[1] = 0;
        Important.CardToUse[ClearCard].Name = "";
        Important.CardToUse[ClearCard].CardPriority = 0;
    }

    for (ClearCard = 0; ClearCard < 20; ClearCard++){
        Important.EventCardOrder[ClearCard] = 0;
        LowPriorityCards[ClearCard] = 0;
        MidPriorityCards[ClearCard] = 0;
        HighPriorityCards[ClearCard] = 0;
        HighestPriorityCards[ClearCard] = 0;
    }

    ShuffleCount.PositionInShuffle = 0;

    // Notice: Cannot say something is null then check if it is = 0 because they are not the same thing!
    // To check if something is null: if (shootTarget == null || shootTarget.Equals(null)) https://answers.unity.com/questions/586144/destroyed-monobehaviour-not-comparing-to-null.html
    //if card Identifier [0] == 0, that card is voided. 

    var ReferencePath = Important.CardsInDeck;

    for(var DE = 0; DE < Important.CardsToDestroy.length; DE++){  //Delete all cards that are to be deleted during shuffling.
        if (Important.CardsToDestroy[DE].TechLevel != 0){
            ReferencePath[Important.CardsToDestroy[DE].Position -1].CardIdentifier[0] = 0;
            ReferencePath[Important.CardsToDestroy[DE].Position -1].CardIdentifier[1] = 0;
            ReferencePath[Important.CardsToDestroy[DE].Position -1].SpriteDisplayIdentifier[0] = 0;
            ReferencePath[Important.CardsToDestroy[DE].Position -1].SpriteDisplayIdentifier[1] = 0;
            ReferencePath[Important.CardsToDestroy[DE].Position -1].CardPriority = 0;
            ReferencePath[Important.CardsToDestroy[DE].Position -1].Name = "";
            Important.CardsToDestroy[DE].TechLevel = 0;
            Important.CardsToDestroy[DE].Position = 0;
        }
    }
    
    var FG: int;
    FG = 0;

    for(var AD = 0; AD<ReferencePath.length; AD++){  //Add all cards that are to be added during shuffling.
        
        if (ReferencePath[AD].CardIdentifier[0] == 0){   //If the slot is empty
            if (Important.CardsToAddIn[FG].TechLevel != 0){

                ReferencePath[AD].SpriteDisplayIdentifier[0] = Important.CardsToAddIn[FG].TechLevel;
                ReferencePath[AD].SpriteDisplayIdentifier[1] = Important.CardsToAddIn[FG].Position;
                ReferencePath[AD].CardIdentifier[0] = 1;
                ReferencePath[AD].CardIdentifier[1] = AD+1;
                ReferencePath[AD].Name = EventDisplay.Normal[Important.CardsToAddIn[FG].TechLevel-1].Position[Important.CardsToAddIn[FG].Position-1].Name;
                ReferencePath[AD].CardPriority = -1; //Change later to custom made
                Important.CardsToAddIn[FG].TechLevel = 0;
                Important.CardsToAddIn[FG].Position = 0;
                FG ++;

            }
        }
    }

    yield WaitForSeconds (0.5f);

    if (Scripts.MainMenuControllerScript.LevelSelected >= 2){

        for (var a = 0; a < ReferencePath.length; a++){
            if (ReferencePath[a].CardIdentifier[0] != 0){       //if card Identifier [0] == 0, that card is voided. 
                if (ReferencePath[a].CardPriority <3 && ReferencePath[a].CardPriority != -1) ReferencePath[a].CardPriority ++;
                Important.CardToUse[a].CardPriority =  ReferencePath[a].CardPriority;
                
                Important.CardToUse[a].Name = ReferencePath[a].Name;
                Important.CardToUse[a].CardIdentifier[0] = ReferencePath[a].CardIdentifier[0];    
                Important.CardToUse[a].CardIdentifier[1] = ReferencePath[a].CardIdentifier[1];
                Important.CardToUse[a].SpriteDisplayIdentifier[0] = ReferencePath[a].SpriteDisplayIdentifier[0];   
                Important.CardToUse[a].SpriteDisplayIdentifier[1] = ReferencePath[a].SpriteDisplayIdentifier[1];   
            }
        }
    }
    
    for (var x = 0; x<Important.CardToUse.length; x ++){
        
        if (Important.CardToUse[x].CardPriority == -1){
            for (i = 0; i < 20; i ++){
                if (HighestPriorityCards[i] == 0){
                    HighestPriorityCards[i] = x+1;
                    break;
                }
            }
        }

        if (Important.CardToUse[x].CardPriority == 3){
            for (i = 0; i < 20; i ++){
                if (HighPriorityCards[i] == 0){
                    HighPriorityCards[i] = x+1;
                    break;
                }
            }
        }

        if (Important.CardToUse[x].CardPriority == 2){
            for (i = 0; i < 20; i ++){
                if (MidPriorityCards[i] == 0){
                    MidPriorityCards[i] = x+1;
                    break;
                }
            }
        }

        if (Important.CardToUse[x].CardPriority == 1){
            for (i = 0; i < 20; i ++){
                if (LowPriorityCards[i] == 0){
                    LowPriorityCards[i] = x+1;
                    break;
                }
            }
        }

    }


    for (i = 0; i<20; i++){
        if (HighestPriorityCards[i] != 0){
            Important.EventCardOrder[i] = HighestPriorityCards[i];
        }

        else break;
    }

    for (i = 0; i<20; i++){
        if (HighPriorityCards[i] != 0){
            for (var CheckZero = 0; CheckZero < 20; CheckZero++){
                if (Important.EventCardOrder[CheckZero] == 0){
                    Important.EventCardOrder[CheckZero] = HighPriorityCards[i];
                    break;
                }
            }
        }

        else break;
    }

    for (i = 0; i<20; i++){
        if (MidPriorityCards[i] != 0){
            for (CheckZero = 0; CheckZero < 20; CheckZero++){
                if (Important.EventCardOrder[CheckZero] == 0){
                    Important.EventCardOrder[CheckZero] = MidPriorityCards[i];
                    break;
                }
            }
        }

        else break;
    }

    for (i = 0; i<20; i++){
        if (LowPriorityCards[i] != 0){
            for (CheckZero = 0; CheckZero < 20; CheckZero++){
                if (Important.EventCardOrder[CheckZero] == 0){
                    Important.EventCardOrder[CheckZero] = LowPriorityCards[i];
                    break;
                }
            }
        }

        else break;
    }

    for (x = 9; x>0; x--){             //Fisher-Yates shuffle http://en.wikipedia.org/wiki/Fisher–Yates_shuffle, code from https://forum.unity.com/threads/shuffling-an-array.46234/
        var Temp = Important.EventCardOrder[x];                                 //Last element in the unshuffled array is stored temporarily
        var randIndex: int;
        randIndex = Random.Range(0, x+1);                                       //A random number is generated, to be smaller than or equal to number of unshuffled elements in array
        Important.EventCardOrder[x] = Important.EventCardOrder[randIndex];      //The last element is made equal to the random element in the array
        Important.EventCardOrder[randIndex] = Temp;                             //Random Array stores the value of the last element
    }
    

    for (x=10; x<20; x++){
        Important.EventCardOrder[x] = 0;
    }

    ShuffleCheck = true;
}

private var EventCardPlayed: int;

// Card Requirements 0, 1, 2 for resource requirements and 3, 4, 5 for technology requirements of the option cards.
// Card Sprite 0, 1, 2 for option cards, 3 for event card
// CardsGiven 0,1,2 for resource given from first option card, 3,4,5 from second and 6,7,8 from third.
// Change stats [0, 1 and 2] will affect nature's health, attack and defence. [3, 4 and 5] will affect that of humans.

// A card from Cards To Shuffle in will be transfered to the next empty slot in the HumanTech0 array when they are required to be shuffled into deck.
// Card To Call is for a card to tell another card to be shuffled into deck -- If CardToCall[0, 1 or 2] = -1, that card is taken out of the deck. 0, 1, 2 corresponds to option card chosen.
// CardIdentifier[0] is to check human tech level, CardIdentifier[1] is to check position in the tech level array.

function UpdateOptionCardInfo(){ //Send info of the options of Event card to Option Card Controller
    
    if (Important.ImmidiateDrawCardNumber != 0){      //To Override the queue and immidiately draw certain cards (eg. Desolation)
        Scripts.OptionClickControlScript.UpdateCards(
            AllEventCardsInGame[0].Miscellaneous[Important.ImmidiateDrawCardNumber-1].CardIdentifier[0],
            AllEventCardsInGame[0].Miscellaneous[Important.ImmidiateDrawCardNumber-1].CardIdentifier[1]);

        return;
    }

    Scripts.OptionClickControlScript.UpdateCards(
        Important.CardToUse[EventCardPlayed-1].CardIdentifier[0],
        Important.CardToUse[EventCardPlayed-1].CardIdentifier[1]);
}

function UpdateHumanRelationship (Change: int){
    
    var TypeofRelationship: String;
    Important.HumanRelationship += Change;
    
    if (Important.HumanRelationship >=10 && Important.HumanRelationship < 30){   //Max number of positive human events = 1
        TypeofRelationship = "  Friendly";
        Important.PositiveHumanEvent[0] = 1;
        Important.NegativeHumanEvent[0] = 0;
        Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[3];
        Components.HumanRelationSpritePosition.localPosition.x = -13.2;
    }

    else if (Important.HumanRelationship >=30){
        TypeofRelationship = " Allies";
        Important.PositiveHumanEvent[0] = 2;
        Important.NegativeHumanEvent[0] = 0;
        Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[4];
        Components.HumanRelationSpritePosition.localPosition.x = -12.5;
    }

    else if (Important.HumanRelationship <=-10 && Important.HumanRelationship > -30){
        TypeofRelationship = "  Guarded";
        Important.PositiveHumanEvent[0] = 0;
        Important.NegativeHumanEvent[0] = 1;
        Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[1];
        Components.HumanRelationSpritePosition.localPosition.x = -14.7;
    }

    else if (Important.HumanRelationship <= -30){
        TypeofRelationship = " Hostile";
        Important.PositiveHumanEvent[0] = 0;
        Important.NegativeHumanEvent[0] = 2;
        Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[0];
        Components.HumanRelationSpritePosition.localPosition.x = -13.98;
    }

    else{ 
        TypeofRelationship = "  Neutral";
        Important.PositiveHumanEvent[0] = 0;
        Important.NegativeHumanEvent[0] = 0;
        Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[2];
        Components.HumanRelationSpritePosition.localPosition.x = -12.5;
    }

    Components.HumanRelationDisplay.text = TypeofRelationship + " (" + Important.HumanRelationship + ")";
    Scripts.UIControllerScript.InformationCharacterText[8].text = Scripts.MainMenuControllerScript.DetermineHumanTies(Important.HumanRelationship);
}

class AllEventCards{
    var HumanTech0 = new CardInfo[20];  
    var HumanTech1 = new CardInfo[4];
    var HumanTech2 = new CardInfo[6];
    var HumanTech3 = new CardInfo[4];
    var Miscellaneous = new CardInfo[3];
    var CardsToShuffleIn = new CardInfo[5];
    var LevelChosen = new LevelSelected[5];
    }

class LevelSelected{
    var Name: String;
    var CardsInLevel = new CardInfo[20];
    }

class CardInfo{                             //Class Provides the Structure for the array "CardToUse"
    var Name: String; 
    var CardIdentifier = new int[2];
    var SpriteDisplayIdentifier = new int[2];
    var CardPriority: int;
    }

class CardsDestroyedAfterShuffle{
    var TechLevel: int;
    var Position: int;
    }

class CardsAddedAfterShuffle{
    var TechLevel: int;
    var Position: int;
    }

class ScriptReference{
    var CharacterBattleControllerScript = new CharacterBattleController[2];
    var OptionCardScripts = new Option_Card_Movement[3];
    var ShufflingCardAnimationScript: ShufflingCardAnimation;
    var NatureControllerScript:NatureController;
    var CameraControlScript: Camera_Control;
    var HandCardRotationScript: HandCardRotation;
    var OptionClickControlScript:OptionClickControl;
    var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
    var UIControllerScript: UIController;
    var MainMenuControllerScript: MainMenuController;
    }

class ComponentReference{
    var EndSceneVideoPlayer: UnityEngine.Video.VideoPlayer;
    var anim:Animation;
    var EventCard: GameObject;
    var ShufflingCardsAnimationObject: GameObject;
    var EventCardRenderer: SpriteRenderer;
    var EventCardTitle: UnityEngine.UI.Text;
    var EventCardDescription: UnityEngine.UI.Text;
    var EventCardType: UnityEngine.UI.Text;

    var OptionCardRenderers = new SpriteRenderer[3];
    var OptionCardTitleRenderers = new UnityEngine.UI.Text[3];
    var OptionCardQuoteRenderers = new UnityEngine.UI.Text[3];
    var OptionCardQuoteSize = new Transform[3];
    var OptionCardEffect = new UnityEngine.UI.Text[3];
    var Fonts = new Font[5];
    var LockedOptionSprite:Sprite;
    var LockedOptionBlacken= new GameObject[3];
    var OptionCardHolder = new GameObject[3];
    var MissionSpriteHolder: GameObject;
    var MissionSpriteRenderers = new SpriteRenderer[3];
    var MissionSprites = new Sprite[2];
    var HideWhileShuffling = new GameObject[3];
    var HumanRelationDisplay: UnityEngine.UI.Text;
    var HumanRelationSpriteRenderer: UnityEngine.UI.Image;
    var HumanRelationSprites = new Sprite[5];
    var HumanRelationSpritePosition: Transform;

    var AnnualReport:GameObject;
    var AnnualReportAnim: Animation;
    var AnnualReportText = new UnityEngine.UI.Text[5];
    }

class CardCounting{
    var PositionInShuffle: int;
    var NumberOfCardsBeforeShuffle: int = 10;
}

class ImportantVars{
    var PositiveHumanEvent = new int[2];
    var NegativeHumanEvent = new int[2];
    var LowMoraleEvent = new int[2];
    var LowFoodEvent = new int[2];

    var EventCardSelected: Boolean;
    var DestroyEventCheck: Boolean;
    var HumanRelationship: float;
    var NumberOfShuffles: int;
    var SpecialCardActivated: int = 0;    // SpecialCardActivated= 1 : Costly Struggle. 
    var ImmidiateDrawCardNumber: int;   // Draw this card immidiately inside Miscellaneous cards
    var EventCardOrder = new int[20];
    var CardToUse = new CardInfo[30];                //Array size = Number of Event Cards
    var CardsToDestroy = new CardsDestroyedAfterShuffle[10];
    var CardsToAddIn = new CardsAddedAfterShuffle[10];
    var CardsInDeck = new CardInfo[30];

    var FoodProduction: int;
    var FoodConsumption: int;
    var Morale: int;
    var HumanTies: int;
    }

class AllEventCardSprites{
    var Normal = new OneTech[5];    // Tech Level 0, 1, 2, 3 and cards to shuffle in
    var Miscellaneous: OneTech;
}

class OneTech{
    var Name: String;
    var Position = new OneEventCardSprite[10];
}

class OneEventCardSprite{
    var Name: String;
    var SpriteGroup = new Sprite[4];
    var EventCardText = new String[3];
    var OptionCardTitle = new String[3];
    var OptionCardQuote = new String[3];
    var OptionCardEffect = new String[3];
    var CardRequirements = new float[3];
    var SkillsRequirements = new int[3];
    var ResourcesUsed = new int[9];
    var ResourcesGiven = new int[9];
    var StatsChange = new int[6];
    var UnlockOptionNumber = new int[3];
    var CalledCard: CardCalled;
    var RemoveEventCheck = new Boolean[3];
    var DiscardCards = new int[3];
    var LoseCheck = new Boolean[3];
    var PromotionSprite = new Sprite[3];
    var PromotionText = new String[6];
    var HumanRelationshipChange = new int[3];
    var FoodChange = new float[3];
    var FoodProductionChange = new int[3];
    var MoraleProductionChange = new int[3];
    var HumanTiesProductionChange = new int[3];
    var MissionProgress: int;
}

class CardCalled{
    var TechLevel = new int[3];
    var Position = new int[3];
    var ChanceToAddCard= new float[3];
    var EnableAnimation: Boolean; //CardShuffleInAnimation
}

class VictoryCondition{
    var ResourceVictory = new ResourceGatheringVictory[10];
}

class ResourceGatheringVictory{
    var Name: String;
    var VictoryCheck: Boolean;
    var RoundsNumber: int;
    var CardsHolding = new int[5];
}