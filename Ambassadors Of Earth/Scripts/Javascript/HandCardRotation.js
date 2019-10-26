#pragma strict

var Scripts: ScriptReference2;
var Components: ComponentReference2;
var Important: ImportantVars1;

private var CentrePoint: Vector3;
private var RotateLeft: boolean;
private var LeftMostCard: GameObject;
private var RotateRight: boolean;
private var RightMostCard: GameObject;
private var MidpointVector: Vector3;
private var RotateLerpSpeed: float = 0;


private var EventCardClickCheck: boolean;
private var OptionCardClickCheck: boolean;
private var NoOptionEventCardClickCheck: boolean;
private var DeselectResourceCardCheck: boolean; 
private var LockedOptionClickCheck: boolean;
private var NoticeClickCheck: boolean;
private var AnnualReportClickCheck: boolean;
private var NatureAvatarClickCheck: boolean;

private var NatureMoraleClickCheck: boolean;
private var HumanMoraleClickCheck: boolean;
private var NumberCardsClickCheck: boolean;
private var NumberLifeClickCheck: boolean;
private var NumberChaosClickCheck: boolean;
private var HumanTiesClickCheck: boolean;

var CardsPlayed = new GameObject[4];

function Start () {

    CentrePoint = Components.Pivot.GetComponent.<Renderer>().bounds.center;
    MidpointVector = Vector3(-0.36, 4.12, 9.4235) - CentrePoint;
    Components.CardPositions[0] = Vector3(0.5, -9.96, 0.56);
    Components.CardPositions[1] = Vector3(-0.09, -6.74, 0.52);
    Components.CardPositions[2] = Vector3(0.38, -3.31, 0.48);
    Components.CardPositions[3] = Vector3(1.82, -0.26, 0.44);
    Components.CardPositions[4] = Vector3(4.2, 2.14, 0.4);
    Components.CardPositions[5] = Vector3(7.23, 3.81, 0.36);
    Components.CardPositions[6] = Vector3(10.6, 4.39, 0.32);
    Components.CardPositions[7] = Vector3(14.15, 3.97, 0.28);
    Components.CardPositions[8] = Vector3(17.19, 2.36, 0.24);
    Components.CardPositions[9] = Vector3(19.66, -0.19, 0.20);
    Components.CardPositions[10] = Vector3(21.22, -3.27, 0.16);
    Components.CardPositions[11] = Vector3(21.69, -6.68, 0.12);
    Components.CardPositions[12] = Vector3(21.1, -10.16, 0.08);
    Components.CardPositions[13] = Vector3(19.5, -13.1, 0.04);
    Components.CardPositions[14] = Vector3(17.01, -15.47, 0.00);

    Components.CardRotations[0] = Quaternion(0.0, 0.0, 0.809, 0.5878);
    Components.CardRotations[1] = Quaternion(0.0, 0.0, 0.7071, 0.7071);
    Components.CardRotations[2] = Quaternion(0.0, 0.0, 0.5878, 0.809);
    Components.CardRotations[3] = Quaternion(0.0, 0.0, 0.454, 0.891);
    Components.CardRotations[4] = Quaternion(0.0, 0.0, 0.309, 0.951);
    Components.CardRotations[5] = Quaternion(0.0, 0.0, 0.1564345, 0.9876884);
    Components.CardRotations[6] = Quaternion(0.0, 0.0, 0.0, 1.0);
    Components.CardRotations[7] = Quaternion(0.0, 0.0, 0.1564, -0.9877);
    Components.CardRotations[8] = Quaternion(0.0, 0.0, 0.309, -0.951);
    Components.CardRotations[9] = Quaternion(0.0, 0.0, 0.454, -0.891);
    Components.CardRotations[10] = Quaternion(0.0, 0.0, 0.5878, -0.809);
    Components.CardRotations[11] = Quaternion(0.0, 0.0, 0.7071, -0.7071);
    Components.CardRotations[12] = Quaternion(0.0, 0.0, -0.809, 0.5878);
    Components.CardRotations[13] = Quaternion(0.0, 0.0, -0.891, 0.454);
    Components.CardRotations[14] = Quaternion(0.0, 0.0, -0.95106, 0.309);
}

function InitialiseNewGame(){
    SortHandCards();
    Important.NumberOfCards=transform.childCount -1;                          //Don't count pivot point into number of cards. 
    LeftMostCard = transform.GetChild(0).gameObject;
    RightMostCard = transform.GetChild(Important.NumberOfCards-1).gameObject; //Reference for right most card will be -1 in the [] brackets.
    RotateLeft = false;
    RotateRight = false;
    CardSelected = null;
    RotateToCentre();
}

function LoadSavedCards(){
    Scripts.UIControllerScript.UpdateRoundNumber(Scripts.EventCardControlScript.Important.NumberOfShuffles+1);
    LeftMostCard = transform.GetChild(0).gameObject;
    RightMostCard = transform.GetChild(Important.NumberOfCards-1).gameObject;
    RotateLeft = false;
    RotateRight = false;
    CardSelected = null;
    Scripts.UIControllerScript.UpdateLifeResourceCardNumber(Components.ResourceTypeCardCount[0]);
    Scripts.UIControllerScript.UpdateChaosResourceCardNumber(Components.ResourceTypeCardCount[4]);
    Scripts.UIControllerScript.UpdateResourceCardNumber(Important.NumberOfCards);
    yield WaitForSeconds(0.1);
    RotateToCentre();
}

private var startPos: Vector2;
private var endPosition: Vector2;
private var StartingAngle: float;
private var swipeDist: float;
private var swipeUpDist: float;
private var maxSwipeDist: float;
private var ResourceCardsSelected: boolean;
private var ElasticSpringBack: float;
var CardSelected: GameObject;
private var SwipeUpDetected: boolean;
private var CardGapCloseCheck:boolean;
private var UpdateChecker: boolean;
var TesterLerp: float = 0.0;
private var ElasticSpringLerp: float = 0;
private var SwipeUpLerp: float = 0;
private var ElasticSwipeCorrectionLerp: float = 0;

private var LerpProgress: float = 0.0;
private var StartedSwipingCheck: boolean;
private var NullCardInstance: GameObject;
private var NullCardCount:int;
private var RotatingNumber: int;

private var ClickedCard: GameObject;
var NeedToCloseCheck: Boolean = true;
private var TimePressed: float;


function Update () {

    if (RotateLeft == true && LeftMostCard != null){

        if (LeftMostCard.transform.rotation.eulerAngles.y <= 190 || LeftMostCard.transform.rotation.eulerAngles.y > 270){ 
            transform.RotateAround (CentrePoint, Vector3.up, (swipeDist/Screen.width)*800 * Time.deltaTime);
        }

        else{ RotateLeft = false; } //Debug.Log("Stop Rotating Left");

    }
    if (RotateRight == true && RightMostCard != null){

        if (RightMostCard.transform.rotation.eulerAngles.y >= 170 || RightMostCard.transform.rotation.eulerAngles.y < 90) { //RightMostCard.transform.rotation.eulerAngles.y >= 170
            transform.RotateAround (CentrePoint, Vector3.down, (swipeDist/Screen.width)*800 * Time.deltaTime);
        }

        else{ RotateRight = false; }  //Debug.Log("Stop Rotating Right");

    }

    if (ClickedCard!=null){
        TimePressed += Time.deltaTime;
        if (TimePressed >= 0.7 && TimePressed< 2){

            if (Scripts.MainMenuControllerScript.LevelSelected == 1 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22){
                if (CardSelected.GetComponent.<Option_Card_Movement>().OptionCardNumber != 1){
                    
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(3).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    
                    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
                    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Wrong option selected!\n(Tap and hold option 1)";
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                    return;
                }

                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("FadeCallOut");
            }

            CheckRayCast();
        }
    }

    if(Input.GetMouseButtonUp(0) && MagnifierClickedCheck == false){
        TimePressed = 0;
        ResourceCardsSelected = false;
        swipeDist = 0;

        if (Camera.main.transform.position.z == 62.3 && Scripts.OptionClickControlScript.Components.PromotionFinishCheck == true){
            Scripts.OptionClickControlScript.FinishedSpecialUpgrade();
        }

        else if (SwipeUpDetected == true){
            if (NullCardInstance!=null){ 
                DestroyImmediate(NullCardInstance);
                NullCardInstance = null;
            }
            SwipeUpReleased();
            if (CardSelected.transform.parent == Components.CardsPlayedParent.transform){
                CardSelected.transform.localScale.x = 0.35;
                CardSelected.transform.localScale.y = 0.35;
            }
            if (CardSelected.transform.parent == this.gameObject.transform){
                CardSelected.transform.localScale.x = 1;
                CardSelected.transform.localScale.y = 1;
            }
            SwipeUpDetected = false;
        }

        else if (ClickedCard != null){

            switch (ClickedCard.tag){
                case "PlayedCards":
                    ClickedCard.GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;
                
                case "EventCards":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;
                
                case "NoOptionEventCard":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "OptionCards":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(3).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "LockedOption":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(3).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "Notice":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "AnnualReport":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "NatureAvatar":
                    ClickedCard.GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "NatureStats":
                case "HumanStats":
                    ClickedCard.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color.white;
                    ClickedCard = null;
                    break;

                case "HumanTies":
                case "NumberOfChaosCards":
                case "NumberOfLifeCards":
                case "NumberOfCards":
                    ClickedCard.GetComponent.<UnityEngine.UI.Image>().color = Color.white;          
                    ClickedCard = null;
                    break;
            }
        }

        UpdateChecker = false;
        StartedSwipingCheck = false;

        var rayMouseUp: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hitMouseUp: RaycastHit;

        if (Physics.Raycast(rayMouseUp, hitMouseUp, 100)){
            CardSelected = hitMouseUp.collider.gameObject;

            switch (CardSelected.tag){
                
                case "EventCards":
                    if (Scripts.MainMenuControllerScript.LevelSelected != 1) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 13 || Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 18) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);

                    if (EventCardClickCheck == true){
                        EventCardClickCheck = false;
                        if (Scripts.EventCardControlScript.Important.EventCardSelected == false){
                            Scripts.EventCardControlScript.EventCardClicked();
                            return;
                        }
                        if (Scripts.EventCardControlScript.Important.EventCardSelected == true){
                            Scripts.EventCardControlScript.EventCardDeselected();
                            return;
                        }
                    }
                    break;
                
                case "OptionCards":
                    if (Scripts.MainMenuControllerScript.LevelSelected != 1) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if (Scripts.MainMenuControllerScript.LevelSelected == 1 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 16) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                
                    if (OptionCardClickCheck == true){
                        OptionCardClickCheck = false;
                        Important.SelectedOptionCardNumber = CardSelected.GetComponent.<Option_Card_Movement>().OptionCardNumber;
                        Scripts.OptionClickControlScript.CheckMeetRequirements(Components.CardsPlayed[0], Components.CardsPlayed[1], Components.CardsPlayed[2], Important.SelectedOptionCardNumber); //Debug.DrawLine(ray.origin, hit.point);
                    }
                    break;

                case "NoOptionEventCard":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if (NoOptionEventCardClickCheck == true){
                        NoOptionEventCardClickCheck = false;
                        Important.NoClicking = true;
                        Scripts.ParticleEffectControllerScript.DestroyExtraCards();
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[6]);
                        DestroyExtra3CardsConfirmed();
                    }
                    break;

                case "PlayedCards": 
                    if(DeselectResourceCardCheck == true){
                        DeselectResourceCardCheck = false;
                        DeselectCard();
                    }
                    break;

                case "LockedOption":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(LockedOptionClickCheck == true){
                        LockedOptionClickCheck = false;
                        Scripts.UIControllerScript.Announcement("This option cannot be selected currently");
                    }
                    break;

                case "Notice":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(NoticeClickCheck == true){
                        NoticeClickCheck = false;
                        Important.NoClicking = true;
                        Scripts.MiscellaneousGameManagementScript.NoticeRemoved();
                    }
                    break;

                case "AnnualReport":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(AnnualReportClickCheck == true){
                        AnnualReportClickCheck = false;
                        Important.NoClicking = true;
                        Scripts.EventCardControlScript.AnnualReportRemoved();
                    }
                    break;

                case "NatureAvatar":
                    if(NatureAvatarClickCheck == true){
                        NatureAvatarClickCheck = false;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.UIControllerScript.Information.SetActive(true);
                        Important.GamePaused = true;
                    }
                    break;

                case "NatureStats":
                    if(NatureMoraleClickCheck == true){
                        NatureMoraleClickCheck = false;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.UIControllerScript.DisplayNatureMoraleExplanation();
                    }
                    break;

                case "HumanStats":
                    if(HumanMoraleClickCheck == true){
                        HumanMoraleClickCheck = false;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.UIControllerScript.DisplayHumanMoraleExplanation();
                    }
                    break;

                case "NumberOfCards":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(NumberCardsClickCheck == true){
                        NumberCardsClickCheck= false;
                        Scripts.UIControllerScript.DisplayCardNumberExplanation();
                    }
                    break;

                case "NumberOfLifeCards":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(NumberLifeClickCheck == true){
                        NumberLifeClickCheck = false;
                        Scripts.UIControllerScript.DisplayLifeCardNumberExplanation();
                    }
                    break;

                case "NumberOfChaosCards":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(NumberChaosClickCheck == true){
                        NumberChaosClickCheck = false;
                        Scripts.UIControllerScript.DisplayDestructionCardNumberExplanation();
                    }
                    break;

                case "HumanTies":
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    if(HumanTiesClickCheck == true){
                        HumanTiesClickCheck = false;
                        Scripts.UIControllerScript.DisplayHumanTiesExplanation();
                    }
                    break;
            }
        }

        if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 15 && CardSelected.GetComponent.<ResourceCardControl>() != null && Components.CardsPlayed[0] != null){

            if (CardSelected.GetComponent.<ResourceCardControl>().ResourceType == 100){
                Scripts.MainMenuControllerScript.ClearPrevious();
                return;
            }

            else if (CardSelected.GetComponent.<ResourceCardControl>().ResourceType != 100){
                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "That's not the right card.\n(Swipe up the Chaos Resource)";
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                NeedToCloseCheck = true;
                DeselectCard();
                return;
            }
        }
    }
    
    if (StartMovingCheck == true){

        if (Components.CardsPlayed[0] == null){
            StartMovingCheck = false;
            return;
        }

        if (Components.CardsPlayed[0]!= null && Components.CardsPlayed[1] == null && Components.CardsPlayed[2] == null){ // if 1 card played
            LerpProgress += Time.deltaTime * (1.0f/0.5f); // Check Completeion of Lerp
            Components.CardsPlayed[0].transform.position = Vector3.Lerp(Components.CardsPlayed[0].transform.position, Vector3(-0.63,0,4.3), LerpProgress);
            Components.CardsPlayed[0].transform.rotation.eulerAngles = Vector3.Lerp(Components.CardsPlayed[0].transform.rotation.eulerAngles,Vector3(90,180,0), LerpProgress);
            
            if (LerpProgress>= 0.7){
                Components.CardsPlayed[0].GetComponent.<Renderer>().sortingOrder = 1;
                SortRendering(0, Components.CardsPlayed[0]);
            }
            
            if (LerpProgress >=1){
                StartMovingCheck = false;
            }
        }

        else if (Components.CardsPlayed[1] != null && Components.CardsPlayed[2] == null){ // if 2 cards played
            LerpProgress += Time.deltaTime * (1.0f/0.5f); // Check Completeion of Lerp
            Components.CardsPlayed[0].transform.position = Vector3.Lerp(Components.CardsPlayed[0].transform.position, Vector3(0.62,0,4.3), LerpProgress);
            Components.CardsPlayed[0].transform.rotation.eulerAngles = Vector3.Lerp(Components.CardsPlayed[0].transform.rotation.eulerAngles,Vector3(90,180,0), LerpProgress);
            
            Components.CardsPlayed[1].transform.position = Vector3.Lerp(Components.CardsPlayed[1].transform.position, Vector3(-1.88,0,4.3), LerpProgress);
            Components.CardsPlayed[1].transform.rotation.eulerAngles = Vector3.Lerp(Components.CardsPlayed[1].transform.rotation.eulerAngles,Vector3(90,180,0), LerpProgress);
            
            if (LerpProgress>= 0.7){
                Components.CardsPlayed[1].GetComponent.<Renderer>().sortingOrder = 1;
                SortRendering(0, Components.CardsPlayed[1]);
            }

            if (LerpProgress >=1){
                StartMovingCheck = false;
            }
        }

        else if (Components.CardsPlayed[1] != null && Components.CardsPlayed[2] != null){ //if 3 cards played
            LerpProgress += Time.deltaTime * (1.0f/0.5f); // Check Completeion of Lerp
            Components.CardsPlayed[0].transform.position = Vector3.Lerp(Components.CardsPlayed[0].transform.position, Vector3(1.87,0,4.3), LerpProgress);
            Components.CardsPlayed[0].transform.rotation.eulerAngles = Vector3.Lerp(Components.CardsPlayed[0].transform.rotation.eulerAngles,Vector3(90,180,0), LerpProgress);

            Components.CardsPlayed[1].transform.position = Vector3.Lerp(Components.CardsPlayed[1].transform.position, Vector3(-0.63,0,4.3), LerpProgress);
            Components.CardsPlayed[1].transform.rotation.eulerAngles = Vector3.Lerp(Components.CardsPlayed[1].transform.rotation.eulerAngles,Vector3(90,180,0), LerpProgress);

            Components.CardsPlayed[2].transform.position = Vector3.Lerp(Components.CardsPlayed[2].transform.position, Vector3(-3.13,0,4.3), LerpProgress);
            Components.CardsPlayed[2].transform.rotation.eulerAngles = Vector3.Lerp(Components.CardsPlayed[2].transform.rotation.eulerAngles,Vector3(90,180,0), LerpProgress);
            if (LerpProgress>= 0.7){
                Components.CardsPlayed[2].GetComponent.<Renderer>().sortingOrder = 1;
                SortRendering(0, Components.CardsPlayed[2]);
            }
            
            if (LerpProgress >=1){
                StartMovingCheck = false;
            }
        }
    }

    if(TesterLerp != 0 && TesterLerp <1 && AdjustmentCheck == false){
        TesterLerp += Time.deltaTime * (1.0f/0.5f); // Check Completeion of Lerp
        for (var a = 0; a < Important.NumberOfCards; a++) {
            if(transform.GetChild(a).gameObject.tag != "Pivot"){
                transform.GetChild(a).gameObject.transform.localPosition = Vector3.Lerp(transform.GetChild(a).gameObject.transform.localPosition,Components.CardPositions[a],TesterLerp);        //Squeeze in to fill space taken by removed card.
                transform.GetChild(a).gameObject.transform.localRotation = Quaternion.Lerp(transform.GetChild(a).gameObject.transform.localRotation,Components.CardRotations[a],TesterLerp);
                //transform.GetChild(a).gameObject.GetComponent.<Renderer>().sortingOrder = 4*a+2;
                if (transform.GetChild(a).gameObject.transform.childCount >0){
                    transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent.<Transform>().localPosition.z = -0.01;
                    transform.GetChild(a).gameObject.transform.GetChild(1).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
                    transform.GetChild(a).gameObject.transform.GetChild(2).gameObject.GetComponent.<Transform>().localPosition.z = -0.03;
                    transform.GetChild(a).gameObject.transform.GetChild(3).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
                }
            }
        }
    }

    if (AdjustmentCheck == true){       

        if(TesterLerp <1){

            TesterLerp += Time.deltaTime * (1.0f/0.5f); // Check Completeion of Lerp
            for (var i = 0; i < Important.NumberOfCards; i++) {
                if(transform.GetChild(i).gameObject.tag != "Pivot"){

                    transform.GetChild(i).gameObject.transform.localPosition.z = Components.CardPositions[i].z;
                    transform.GetChild(i).gameObject.transform.localPosition.x = Mathf.Lerp(transform.GetChild(i).gameObject.transform.localPosition.x,Components.CardPositions[i].x,TesterLerp);        //Squeeze in to fill space taken by removed card.
                    transform.GetChild(i).gameObject.transform.localPosition.y = Mathf.Lerp(transform.GetChild(i).gameObject.transform.localPosition.y,Components.CardPositions[i].y,TesterLerp);   
                    transform.GetChild(i).gameObject.transform.localRotation = Quaternion.Lerp(transform.GetChild(i).gameObject.transform.localRotation,Components.CardRotations[i],TesterLerp);
                    //transform.GetChild(i).gameObject.GetComponent.<Renderer>().sortingOrder = 4*i+2;
                    if (transform.GetChild(i).gameObject.transform.childCount >0){
                        transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent.<Transform>().localPosition.z = -0.01;
                        transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
                        transform.GetChild(i).gameObject.transform.GetChild(2).gameObject.GetComponent.<Transform>().localPosition.z = -0.03;
                        transform.GetChild(i).gameObject.transform.GetChild(3).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
                    }
                }
            }
        }
        
       else if (CardGapCloseCheck == false && TesterLerp >=1){
            ElasticSpringLerp = 0;
            TesterLerp = 0;
            AdjustmentCheck = false;
        }
    }

    if (Important.NumberOfCards > 1 && LeftMostCard!=null){
        if (swipeDist == 0 && SwipeUpDetected == false && AdjustmentCheck == false && RotatingInProgress == false){        //Check if  cards rotated too much to left  or too much to right and correct it
            
            var RightCardVector = RightMostCard.transform.position - CentrePoint;               // Vector of Centre of rightmost card and centre card
            var SignOfRotationRight = Mathf.Sign(RightMostCard.transform.position.x - -0.36);   // Determines which side of the middle point the right most card falls on
            var AngleToDeterminePositionRight = Vector3.Angle(RightCardVector, MidpointVector); // Angle between the middle point of centre card and rightmost card

            if (AngleToDeterminePositionRight > 0 && AngleToDeterminePositionRight<40 && SignOfRotationRight == 1){//RightMostCard.transform.rotation.eulerAngles.y < 180
                ElasticSwipeCorrectionLerp += Time.deltaTime/0.5;
                ElasticSpringBack =  Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                transform.RotateAround (CentrePoint, Vector3.up, ElasticSpringBack);
            }

            var LeftCardVector = LeftMostCard.transform.position - CentrePoint;
            var SignOfRotationLeft = Mathf.Sign(LeftMostCard.transform.position.x - -0.36);
            var AngleToDeterminePositionLeft = Vector3.Angle(LeftCardVector, MidpointVector); 

            if (AngleToDeterminePositionLeft > 0 && AngleToDeterminePositionLeft<40 && SignOfRotationLeft == -1){ //LeftMostCard.transform.rotation.eulerAngles.y > 180
                ElasticSwipeCorrectionLerp += Time.deltaTime/0.5;
                ElasticSpringBack =  Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                transform.RotateAround (CentrePoint, Vector3.down, ElasticSpringBack);
            }
        }
    }

    else if (Important.NumberOfCards == 1 && LeftMostCard!=null){
        if (swipeDist == 0 && SwipeUpDetected == false && AdjustmentCheck == false){

            if (Mathf.Abs(180 - LeftMostCard.transform.rotation.eulerAngles.y) < 40){      //Left Most Card should be in 160 to 200.

            
                if (LeftMostCard.transform.rotation.eulerAngles.y > 185){
                    ElasticSwipeCorrectionLerp += Time.deltaTime/0.5;
                    ElasticSpringBack =  Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                    transform.RotateAround (CentrePoint, Vector3.down, ElasticSpringBack);
                }

                else if (LeftMostCard.transform.rotation.eulerAngles.y < 175){
                    ElasticSwipeCorrectionLerp += Time.deltaTime/0.5;
                    ElasticSpringBack =  Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                    transform.RotateAround (CentrePoint, Vector3.up, ElasticSpringBack);
                }
            }

            if ( Mathf.Abs(-180 -LeftMostCard.transform.rotation.eulerAngles.y) <  40){      // Left Most Card should be in -160 to -200
                if (LeftMostCard.transform.rotation.eulerAngles.y > -175){
                    ElasticSwipeCorrectionLerp += Time.deltaTime/0.5;
                    ElasticSpringBack =  Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                    transform.RotateAround (CentrePoint, Vector3.down, ElasticSpringBack);
                }

                else if (LeftMostCard.transform.rotation.eulerAngles.y < -185){
                    ElasticSwipeCorrectionLerp += Time.deltaTime/0.5;
                    ElasticSpringBack =  Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                    transform.RotateAround (CentrePoint, Vector3.up, ElasticSpringBack);
                }
            }
        }
    }
    
    if(Input.GetMouseButtonDown(0) && MagnifierClickedCheck == false) {

        var ray: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit: RaycastHit;
        if (Physics.Raycast(ray, hit, 100)){
            CardSelected = hit.collider.gameObject;

            if (Important.GamePaused == true || Important.NoClicking == true) return;
           
            ResourceCardsSelected = false;
            NoOptionEventCardClickCheck = false;
            DeselectResourceCardCheck = false;
            OptionCardClickCheck = false;
            EventCardClickCheck = false;
            LockedOptionClickCheck = false;
            NoticeClickCheck = false;
            AnnualReportClickCheck = false;
            Scripts.UIControllerScript.ResourceCardExplanation.SetActive(false);

            if (Scripts.MainMenuControllerScript.LevelSelected == 1){

                if (Scripts.MainMenuControllerScript.TutorialUse.FinishLoadCheck == false) return;

                if (CardSelected.tag == "TutorialPrevious"){
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    Scripts.MainMenuControllerScript.ClickPrevious();
                    return;
                }  

                else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 16 || Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22){
                    if(CardSelected.tag != "OptionCards"){

                        if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 16){
                            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
                            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.035;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Tap on the option that you\nbelieve is the most beneficial\nto the well-being of the forest.";
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[11].Play("DelayedDimScreenFadeIn");
                        }
                        
                        else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22) {
                            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
                            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Let's give it a try.\n(Tap and hold option 1)";
                        }

                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        return;
                    }
                }              
                
                else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 15){
                    if(CardSelected.tag != "ResourceCards"){
                        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
                        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Locate your Chaos resource\ncard and swipe up.";
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                        NeedToCloseCheck = true;
                        return;
                    }

                    else if(CardSelected.tag == "ResourceCards"){
                        if (NeedToCloseCheck == true){
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("FadeCallOut");
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                        }
                        NeedToCloseCheck = false;
                    }
                }

                else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 13 || Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 18){
                    if (CardSelected.tag == "EventCards"){
                        EventCardClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                        CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                        ClickedCard = CardSelected;
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                        Scripts.MainMenuControllerScript.ClearPrevious();
                    }

                    else {
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.035;
                        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                        if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 13) Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Please select the event I made!\n(Tap on the card above)";
                        if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 18) Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Let me show you something!\n(Tap on the card above)";
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                    }
                    return;
                }

                else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial != 15 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial != 16 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial != 22){
                    //Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    
                    if(CardSelected.tag == "TutorialNext"){
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.ClearPrevious();
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                    }
                    
                    return;
                }
            }

            else if (Scripts.MainMenuControllerScript.LevelSelected != 1 && Scripts.MainMenuControllerScript.LevelUse.FinishTalk == false){
                
                if (Scripts.MainMenuControllerScript.LevelUse.SpeechLoaded == false) return;

                if(CardSelected.tag == "TutorialNext"){
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[0].Play("FadeCallOut");
                    Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[1].Play("FadeButtons");
                    Scripts.MainMenuControllerScript.NextSpeech();
                }

                else if(CardSelected.tag == "TutorialPrevious"){
                    if (Scripts.MainMenuControllerScript.LevelUse.PositionInTalking > 1){
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position.x = 20;
                        Scripts.MainMenuControllerScript.LevelUse.PositionInTalking -=2;
                        Scripts.MainMenuControllerScript.PreviousSpeech();
                    }
                }

                return;
            }

            else if (Scripts.MainMenuControllerScript.LevelSelected != 1 && Scripts.MainMenuControllerScript.LevelUse.FinishTalkEnding == false){
                
                if (Scripts.MainMenuControllerScript.LevelUse.SpeechLoaded == false) return;

                if(CardSelected.tag == "TutorialNext"){
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                    Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[0].Play("FadeCallOut");
                    Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[1].Play("FadeButtons");
                    Scripts.MainMenuControllerScript.NextSpeechEnding();
                }

                else if(CardSelected.tag == "TutorialPrevious"){
                    if (Scripts.MainMenuControllerScript.LevelUse.PositionInEnding > 1){
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position.x = 20;
                        Scripts.MainMenuControllerScript.LevelUse.PositionInEnding -=2;
                        Scripts.MainMenuControllerScript.PreviousSpeechEnding();
                    }
                }

                return;
            }

            switch (CardSelected.tag){
                case "AnnualReport":
                    AnnualReportClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "Notice":
                    NoticeClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "BattleCard":
                    if (Scripts.EventCardControlScript.Important.EventCardSelected == false){
                        Scripts.EventCardControlScript.BattleCardClicked();
                        return;
                    }
                    else if (Scripts.EventCardControlScript.Important.EventCardSelected == true){
                        Scripts.EventCardControlScript.BattleCardDeselected();
                        return;
                    }
                    break;

                case "ResourceCards":
                    ResourceCardsSelected = true; //Debug.DrawLine(ray.origin, hit.point);
                    ElasticSwipeCorrectionLerp = 0;
                    break;

                case "NoOptionEventCard":
                    NoOptionEventCardClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "PlayedCards":
                    DeselectResourceCardCheck = true;
                    CardSelected.GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "OptionCards":
                    OptionCardClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(3).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "EventCards":
                    EventCardClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "LockedOption":
                    LockedOptionClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(3).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "NatureAvatar":
                    NatureAvatarClickCheck = true;
                    CardSelected.GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "NatureStats":
                    NatureMoraleClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "HumanStats":
                    HumanMoraleClickCheck = true;
                    CardSelected.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    CardSelected.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "NumberOfCards":
                    NumberCardsClickCheck = true;
                    CardSelected.GetComponent.<UnityEngine.UI.Image>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "NumberOfLifeCards":
                    NumberLifeClickCheck = true;
                    CardSelected.GetComponent.<UnityEngine.UI.Image>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "NumberOfChaosCards":
                    NumberChaosClickCheck = true;
                    CardSelected.GetComponent.<UnityEngine.UI.Image>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;

                case "HumanTies":
                    HumanTiesClickCheck = true;
                    CardSelected.GetComponent.<UnityEngine.UI.Image>().color = Color (0.8, 0.8, 0.8 , 1);
                    ClickedCard = CardSelected;
                    break;
            }
        }

        var clickTime = Time.time;         
        startPos = Input.mousePosition;
        if (LeftMostCard != null) StartingAngle = LeftMostCard.transform.rotation.eulerAngles.y;
    }

    if (ResourceCardsSelected == false) return;

    if (SwipeUpDetected == true){
        var temp: Vector3 = Input.mousePosition;
        temp.z = 46.4f;
        CardSelected.transform.position = Camera.main.ScreenToWorldPoint(temp);
        SwipeUpLerp += Time.deltaTime/0.5;
        CardSelected.transform.rotation.eulerAngles.y = Mathf.Lerp(CardSelected.transform.rotation.eulerAngles.y, 180, SwipeUpLerp);
        CardSelected.transform.localScale.x = 0.55;
        CardSelected.transform.localScale.y = 0.55;
    }

    for (i=0; i<4; i++){
        if (CardsPlayed[i] != null){
        if (CardsPlayed[i].transform.parent == Components.CardsPlayedParent.transform){ 
            if (Camera.main.WorldToScreenPoint(CardsPlayed[i].transform.position).y > Screen.height/4){
                if (NullCardInstance!=null){
                    DestroyImmediate(NullCardInstance);
                    NullCardInstance = null;
                    TesterLerp = 0;
                    Important.NumberOfCards -= 1;
                }
                NullCardCount = 0;
            }

            if (Camera.main.WorldToScreenPoint(CardsPlayed[i].transform.position).y < Screen.height/4){

                if (NullCardCount==0){
                
                    var CurrentCardNumber = CardsPlayed[i].GetComponent.<ResourceCardControl>().CardNumber;
                    var SortAscending = new Array ();

                    if (Components.CardsPlayed[0] != null) SortAscending.push(Components.CardsPlayed[0].GetComponent.<ResourceCardControl>().CardNumber);
                    if (Components.CardsPlayed[1] != null) SortAscending.push(Components.CardsPlayed[1].GetComponent.<ResourceCardControl>().CardNumber);
                    if (Components.CardsPlayed[2] != null) SortAscending.push(Components.CardsPlayed[2].GetComponent.<ResourceCardControl>().CardNumber);
                    SortAscending.push(CurrentCardNumber);
                    SortAscending.Sort();

                    TesterLerp = 0;
                    NullCardCount+=1;
                    NullCardInstance = Instantiate(Components.NullCard);
                    NullCardInstance.transform.parent = this.gameObject.transform;

                    if (SortAscending.length >= 1){
                        if(CurrentCardNumber == SortAscending[0]){
                            NullCardInstance.transform.SetSiblingIndex (CurrentCardNumber-1); 
                            RotatingNumber = CurrentCardNumber-1;
                        }
                    }

                    if (SortAscending.length >= 2){
                        if(CurrentCardNumber == SortAscending[1]){
                            NullCardInstance.transform.SetSiblingIndex (CurrentCardNumber-2); 
                            RotatingNumber = CurrentCardNumber-2;
                        }
                    }

                    if (SortAscending.length >= 3){
                        if(CurrentCardNumber == SortAscending[2]){
                            NullCardInstance.transform.SetSiblingIndex (CurrentCardNumber-3); 
                            RotatingNumber = CurrentCardNumber-3;
                        }
                    }

                    if (SortAscending.length >= 4){
                        if(CurrentCardNumber == SortAscending[3]){
                            NullCardInstance.transform.SetSiblingIndex (CurrentCardNumber-4); 
                            RotatingNumber = CurrentCardNumber-4;
                        }
                    }

                    Important.NumberOfCards += 1;
                }

                if (RotatingNumber == 0){
                    if (NullCardInstance!=null){
                        DestroyImmediate(NullCardInstance);
                        NullCardInstance = null;
                        Important.NumberOfCards -= 1;
                    }
                    NullCardCount = 0;
                }

                if (NullCardCount > 0 && RotatingInProgress == false){
                    AdjustmentCheck = true;
                }
            }
        }
    }
    }

    if (CardSelected.transform.parent == Components.CardsPlayedParent.transform){
        return;
    }

    if(Input.GetMouseButton(0) && (Time.time - clickTime) > 0.05){
        endPosition = Input.mousePosition;
        swipeUpDist = Mathf.Abs(endPosition.y - startPos.y);
        swipeDist = Mathf.Abs(endPosition.x - startPos.x);
        

        if (swipeUpDist - Screen.height/50 > swipeDist){
            swipeDist = 0;
            SwipeUpLerp = 0;
            AssignCardNumber();
            SortRendering(50, CardSelected);

            if (UpdateChecker == false){
                
                Important.NumberOfCards=transform.childCount -1;                          //Don't count pivot point into number of cards. 
                CardGapCloseCheck = false;
                TesterLerp = 0;

                if (Important.NumberOfCards >= 2) {
                    LeftMostCard = transform.GetChild(0).gameObject;
                    RightMostCard = transform.GetChild(Important.NumberOfCards-1).gameObject; //Reference for right most card will be -1 in the [] brackets.
                }

                else if (Important.NumberOfCards == 1){
                    LeftMostCard = transform.GetChild(0).gameObject;
                    RightMostCard = LeftMostCard;
                }
                
                else if (Important.NumberOfCards == 0){
                    LeftMostCard = null;
                    RightMostCard = null;
                }

                UpdateChecker = true;
            }
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[2]);
            SwipeUpDetected = true;
        }

        if (SwipeUpDetected == true)  return;

        if (swipeDist > swipeUpDist - Screen.height/50){
            if (swipeDist < maxSwipeDist){
                startPos = Input.mousePosition;
                maxSwipeDist = 0;
            }           

            CardGapCloseCheck = false;
            StartedSwipingCheck = true;

            var swipeDirection = Mathf.Sign(endPosition.x - startPos.x);
            if (swipeDirection == -1){
                RotateRight = true;
                RotateLeft = false;
                if (swipeDist>maxSwipeDist) maxSwipeDist = swipeDist;
            }

            else if (swipeDirection == 1){ 
                RotateLeft = true;
                RotateRight = false;
                if (swipeDist>maxSwipeDist) maxSwipeDist = swipeDist;
            }
        }
    }
}

private var SignLeft: int;
private var TimeToPause: float = 0.017;
private var RotatingInProgress: Boolean;

function ShowRotateToCentre(RotatingCardNumber: int){

    if (Important.NumberOfCards > 1){
        if (RotatingCardNumber == 0){
            
            while (LeftMostCard.transform.rotation.eulerAngles.y <= 190 || LeftMostCard.transform.rotation.eulerAngles.y > 270){ // LeftMostCard.transform.rotation.eulerAngles.y <= 190
                transform.RotateAround (CentrePoint, Vector3.up, 1 + (190-LeftMostCard.transform.rotation.eulerAngles.y)/6);
                yield WaitForSeconds(TimeToPause);
            } 
            TesterLerp = 0;
            AdjustmentCheck = true;
            RotatingInProgress= false;
            return;
        }

        if (RotatingCardNumber == transform.childCount-2){  //RightMostCard
            while (transform.GetChild(RotatingCardNumber-1).GetComponent.<Transform>().rotation.eulerAngles.y > 180){
                transform.RotateAround (CentrePoint, Vector3.down, 1+ (transform.GetChild(RotatingCardNumber-1).GetComponent.<Transform>().rotation.eulerAngles.y-180)/6);     //Debug.Log("Rotate Right");
                yield WaitForSeconds(TimeToPause);
            }

            TesterLerp = 0;
            AdjustmentCheck = true;
            RotatingInProgress= false;
            return;
        }       

        if (RotatingCardNumber >= 1){
            SignLeft = Mathf.Sign(this.gameObject.transform.GetChild(RotatingCardNumber-1).transform.position.x - -0.36);

            if (SignLeft == 1){
                while (transform.GetChild(RotatingCardNumber-1).GetComponent.<Transform>().rotation.eulerAngles.y < 162){
  
                    transform.RotateAround (CentrePoint, Vector3.up, 1+ (162-transform.GetChild(RotatingCardNumber-1).GetComponent.<Transform>().rotation.eulerAngles.y)/6);     //Debug.Log("Rotate Right");
                    yield WaitForSeconds(TimeToPause);
                }
                TesterLerp = 0;
                AdjustmentCheck = true;
                RotatingInProgress= false;
                return;
            }

            if (SignLeft == -1){

                while (transform.GetChild(RotatingCardNumber-1).GetComponent.<Transform>().rotation.eulerAngles.y > 162){
                    transform.RotateAround (CentrePoint, Vector3.down, 1+ (transform.GetChild(RotatingCardNumber-1).GetComponent.<Transform>().rotation.eulerAngles.y-162)/6);     //Debug.Log("Rotate Right");
                    yield WaitForSeconds(TimeToPause);
                }

                TesterLerp = 0;
                AdjustmentCheck = true;
                RotatingInProgress= false;
                return;
            }               
        }

        AdjustmentCheck = true;
    }
}

private var CardHolding: GameObject;
private var MagnifierClickedCheck: boolean;
var ShownAdditionalInfo: GameObject;
var InstantiatedResourceChange: GameObject;

function CheckRayCast(){
    var rayCheck: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    var hitCheck: RaycastHit;
    if (Physics.Raycast(rayCheck, hitCheck, 100)){
        CardHolding = hitCheck.collider.gameObject;
        if (CardHolding == ClickedCard){
            
            if (CardSelected.tag == "PlayedCards"){
                Components.Magnifier.SetActive(true);
                MagnifierClickedCheck = true;
                Components.Magnifier.transform.GetChild(0).gameObject.SetActive(true);
                Components.ResourceCardZoomIn[0].sprite = CardHolding.GetComponent.<SpriteRenderer>().sprite;
                Components.ResourceCardZoomIn[1].sprite = CardHolding.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite;
                Components.ResourceCardZoomIn[2].sprite = CardHolding.transform.GetChild(1).GetComponent.<SpriteRenderer>().sprite;
                Components.ResourceCardZoomIn[3].sprite = CardHolding.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite;
                Components.ResourceCardTextZoomIn[0].text = CardHolding.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text;
                Components.ResourceCardTextZoomIn[0].transform.localPosition = CardHolding.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().transform.localPosition;
                Components.ResourceCardTextZoomIn[1].text = CardHolding.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text;
                Components.ResourceCardTextZoomIn[1].transform.localPosition = CardHolding.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().transform.localPosition;
                Components.ResourceCardTextZoomIn[2].text = CardHolding.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text;
                Components.ResourceCardTextZoomIn[2].transform.localPosition = CardHolding.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().transform.localPosition;
            }

            if (CardSelected.tag == "EventCards" || CardSelected.tag == "NoOptionEventCard"){
                Components.Magnifier.SetActive(true);
                MagnifierClickedCheck = true;
                Components.Magnifier.transform.GetChild(1).gameObject.SetActive(true);
                Components.EventCardZoomIn[0].sprite = CardHolding.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite;
                Components.EventCardZoomIn[1].sprite = CardHolding.transform.GetChild(1).GetComponent.<SpriteRenderer>().sprite;
                Components.EventCardZoomIn[2].sprite = CardHolding.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite;
                Components.EventCardTextZoomIn[0].text = CardHolding.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text;
                Components.EventCardTextZoomIn[1].text = CardHolding.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text;
                Components.EventCardTextZoomIn[2].text = CardHolding.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text;
            }

            if (CardSelected.tag == "OptionCards"){

                Components.OptionCardAdditionInfo[2].SetActive(true);
                ShownAdditionalInfo = Components.OptionCardAdditionInfo[2];
                CardReference.OptionCardPosition.position.z = 1.0;
                Components.Magnifier.SetActive(true);
                MagnifierClickedCheck = true;
                Components.Magnifier.transform.GetChild(2).gameObject.SetActive(true);
                Components.OptionCardZoomIn[0].sprite = CardHolding.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite;
                Components.OptionCardZoomIn[1].sprite = CardHolding.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite;
                Components.OptionCardZoomIn[2].sprite = CardHolding.transform.GetChild(1).GetComponent.<SpriteRenderer>().sprite;
                Components.OptionCardZoomIn[3].sprite = CardHolding.transform.GetChild(3).GetComponent.<SpriteRenderer>().sprite;
                
                if (Scripts.OptionClickControlScript.ReferencedResourceDisplay[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1].transform.childCount != 0){
                    InstantiatedResourceChange = Instantiate(Scripts.OptionClickControlScript.ReferencedResourceDisplay[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1]);
                    InstantiatedResourceChange.transform.localScale = Vector3(1.6,1.6,1);
                    InstantiatedResourceChange.transform.localRotation.eulerAngles = Vector3(90,180,0);
                    InstantiatedResourceChange.transform.position = Vector3(-0.79,0,2.42);
                    
                    for (var i = 0; i < InstantiatedResourceChange.transform.childCount; i++){
                        if (InstantiatedResourceChange.transform.GetChild(i).GetComponent.<SpriteRenderer>()) InstantiatedResourceChange.transform.GetChild(i).GetComponent.<SpriteRenderer>().sortingOrder += 10;
                    }


                    for (i = 0; i< InstantiatedResourceChange.transform.childCount; i++){
                        if(InstantiatedResourceChange.transform.GetChild(i).GetComponent.<SpriteRenderer>()){
                            InstantiatedResourceChange.transform.GetChild(i).GetComponent.<SpriteRenderer>().sortingLayerName = "Top";
                            InstantiatedResourceChange.transform.parent = Components.Magnifier.transform.GetChild(2).GetComponent.<Transform>();
                        }
                    }
                }

                else{
                    Components.OptionCardTextZoomIn[3].enabled = true;
                }

                Components.OptionCardTextZoomIn[0].text = CardHolding.transform.GetChild(6).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text;
                Components.OptionCardTextZoomIn[1].text = CardHolding.transform.GetChild(6).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text;
                Components.OptionCardTextZoomIn[2].text = CardHolding.transform.GetChild(6).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text;
                Components.OptionCardTextZoomIn[2].font = CardHolding.transform.GetChild(6).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().font; 
                Components.QuoteTextTransform.localScale = CardHolding.transform.GetChild(6).transform.GetChild(2).GetComponent.<Transform>().localScale; 
                Components.OptionCardTextZoomIn[4].text = CardHolding.transform.GetChild(6).transform.GetChild(4).GetComponent.<UnityEngine.UI.Text>().text;

                if (Scripts.OptionClickControlScript.EventDetails.DiscardCards[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1] >0){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[7].transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = "Discard " + Scripts.OptionClickControlScript.EventDetails.DiscardCards[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1] + " random resource \n cards if this option is selected.";
                    Components.OptionCardAdditionInfo[7].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[7];
                }

                if (Scripts.OptionClickControlScript.EventDetails.LoseCheck[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1] == true){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[8].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[8];
                }

                if ("Reshuffle:" in Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1].text){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[9].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[9];
                }

                if ("Add:" in Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1].text){
                    CardReference.OptionCardPosition.position.z = 0.7;
                    var ReferencePath = Scripts.EventCardControlScript.EventDisplay.Normal[Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1]-1].Position[Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1]-1];
                    Components.OptionCardAdditionInfo[0].transform.GetChild(0).gameObject.SetActive(true);
                    Components.OptionCardAdditionInfo[0].transform.GetChild(1).gameObject.SetActive(true);
                    Components.OptionCardAdditionInfo[0].transform.GetChild(2).gameObject.SetActive(false);
                    Components.OptionCardAdditionInfo[0].transform.GetChild(3).gameObject.SetActive(false);

                    if ("Revert:" in Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1].text){
                        Components.OptionCardAdditionInfo[0].transform.GetChild(0).gameObject.SetActive(false);
                        Components.OptionCardAdditionInfo[0].transform.GetChild(1).gameObject.SetActive(false);
                        Components.OptionCardAdditionInfo[0].transform.GetChild(2).gameObject.SetActive(true);
                        Components.OptionCardAdditionInfo[0].transform.GetChild(3).gameObject.SetActive(true);
                    }
                    
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[0].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[0];
                    CardReference.QuickJump.SetActive(true);
                    CardReference.LeftArrow.SetActive(true);
                    CardReference.RightArrow.SetActive(true);
                    Components.OptionCardAdditionInfo[0].transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = 'Prepare to draw' + ' "' + ReferencePath.Name + '"\nnext year if this option is selected.'; 
                    DisplayAddedCard();
                }

                if (Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1] == -1){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[3].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[3];
                }

                if (Scripts.OptionClickControlScript.EventDetails.StatsChange[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1] != 0 || Scripts.OptionClickControlScript.EventDetails.StatsChange[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber+2] !=0){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[4].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                }

                if (Scripts.OptionClickControlScript.EventDetails.UnlockOptionNumber[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1] != 0){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[6].SetActive(true);
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[6];
                }

                if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1].text == "Remove this event"){
                    Components.OptionCardAdditionInfo[2].SetActive(false);
                    Components.OptionCardAdditionInfo[5].SetActive(true);
                    Components.OptionCardAdditionInfo[5].transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text = 'Select this option to permanently remove' +'\n' + 'the event: "' + Scripts.EventCardControlScript.Components.EventCardTitle.text + '".';
                    ShownAdditionalInfo = Components.OptionCardAdditionInfo[5];
                }
            }
        }
        TimePressed = 2;
    }
}

function ClickCloseMagnifier(){
    
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    if (ShownAdditionalInfo) ShownAdditionalInfo.SetActive(false);
    OptionCardClickCheck = false;
    EventCardClickCheck = false;
    NoOptionEventCardClickCheck = false;
    DeselectResourceCardCheck = false;
    CardReference.ButtonSpheresImage[0].color = Color.white;
    CardReference.ButtonSpheresImage[1].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[2].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[3].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[4].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.QuickJump.SetActive(false);
    CardReference.LeftArrow.SetActive(false);
    CardReference.RightArrow.SetActive(false);
    ClearMagnifier();    

}

function ClearMagnifier(){
    Components.MagnifiedSprite.sprite = null;
    if (InstantiatedResourceChange != null) Destroy(InstantiatedResourceChange);
    Components.Magnifier.transform.GetChild(0).gameObject.SetActive(false);
    Components.Magnifier.transform.GetChild(1).gameObject.SetActive(false);
    Components.Magnifier.transform.GetChild(2).gameObject.SetActive(false);
    Components.Magnifier.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(false);
    Components.Magnifier.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(false);
    Components.Magnifier.transform.GetChild(3).gameObject.transform.GetChild(3).gameObject.SetActive(false);
    Components.Magnifier.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
    Components.Magnifier.transform.GetChild(3).gameObject.SetActive(false);
    Components.OptionCardAdditionInfo[1].SetActive(false);
    Components.OptionCardTextZoomIn[3].enabled = false;
    Components.Magnifier.SetActive(false);
    MagnifierClickedCheck = false;

    if (Scripts.MainMenuControllerScript.LevelSelected == 1){
        Important.NoClicking = true;
        Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
        Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Moving on...";
        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
        yield WaitForSeconds(1.0f);
        Scripts.EventCardControlScript.EventCardRemoved();
        return;
    }

}

var CardReference: ReferencedCard;

private var NumberOfResourcesGained = new int[3];
private var NumberOfResourcesLost = new int[3];
private var ReferencedResourceDisplay = new GameObject[3];
private var ResourceDisplay1 = new int[6];
private var ResourceDisplay2 = new int[6];
private var ResourceDisplay3 = new int[6];

function DisplayAddedCard(){

    for (var i = 0; i<3; i++){
        ResourceDisplay1[i] = 0;
        ResourceDisplay2[i] = 0;
        ResourceDisplay3[i] = 0;
        ResourceDisplay1[i+3] = 0;
        ResourceDisplay2[i+3] = 0;
        ResourceDisplay3[i+3] = 0;
        NumberOfResourcesGained[i] = 0;
        NumberOfResourcesLost[i] = 0;
        if (ReferencedResourceDisplay[i]!= null) ReferencedResourceDisplay[i].SetActive(false);
    }

    var ReferencePath = Scripts.EventCardControlScript.EventDisplay.Normal[Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1]-1].Position[Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[CardHolding.GetComponent.<Option_Card_Movement>().OptionCardNumber-1]-1];
 
    CardReference.EventCardSprite.sprite = ReferencePath.SpriteGroup[3];
    CardReference.EventCardText[0].text = ReferencePath.EventCardText[0];
    CardReference.EventCardText[1].text = ReferencePath.EventCardText[1];
    CardReference.EventCardText[1].text = CardReference.EventCardText[1].text.Replace("NWL", "\n");
    CardReference.EventCardText[2].text = ReferencePath.EventCardText[2];

    CardReference.OptionCardSprite[0].sprite = ReferencePath.SpriteGroup[0];
    CardReference.OptionCardSprite[1].sprite = ReferencePath.SpriteGroup[1];
    CardReference.OptionCardSprite[2].sprite = ReferencePath.SpriteGroup[2];

    CardReference.OptionCardText[0].text = ReferencePath.OptionCardTitle[0];
    CardReference.OptionCardText[3].text = ReferencePath.OptionCardTitle[1];
    CardReference.OptionCardText[6].text = ReferencePath.OptionCardTitle[2];
    CardReference.OptionCardText[0].text = CardReference.OptionCardText[0].text.Replace("NWL", "\n");
    CardReference.OptionCardText[3].text = CardReference.OptionCardText[3].text.Replace("NWL", "\n");
    CardReference.OptionCardText[6].text = CardReference.OptionCardText[6].text.Replace("NWL", "\n");

    CardReference.OptionCardText[1].text = ReferencePath.OptionCardQuote[0];
    CardReference.OptionCardText[4].text = ReferencePath.OptionCardQuote[1];
    CardReference.OptionCardText[7].text = ReferencePath.OptionCardQuote[2];
    CardReference.OptionCardText[1].text = CardReference.OptionCardText[1].text.Replace("NWL", "\n");
    CardReference.OptionCardText[4].text = CardReference.OptionCardText[4].text.Replace("NWL", "\n");
    CardReference.OptionCardText[7].text = CardReference.OptionCardText[7].text.Replace("NWL", "\n");

    CardReference.OptionCardText[2].text = ReferencePath.OptionCardEffect[0];
    CardReference.OptionCardText[5].text = ReferencePath.OptionCardEffect[1];
    CardReference.OptionCardText[8].text = ReferencePath.OptionCardEffect[2];
    CardReference.OptionCardText[2].text = CardReference.OptionCardText[2].text.Replace("NWL", "\n");
    CardReference.OptionCardText[5].text = CardReference.OptionCardText[5].text.Replace("NWL", "\n");
    CardReference.OptionCardText[8].text = CardReference.OptionCardText[8].text.Replace("NWL", "\n");

    if (ReferencePath.OptionCardTitle[2] == ""){    // If Option 3 is empty
        CardReference.OptionCardSprite[2].sprite = Scripts.EventCardControlScript.Components.LockedOptionSprite;
        CardReference.OptionCardText[6].text =  "Option Unavailable";
        CardReference.OptionCardText[7].text = "Make an empty space and\ncreativity will instantly fill it.";
    }

    //Option1
    if (ReferencePath.ResourcesUsed[0] != 0){ 
        NumberOfResourcesLost[0] +=1;
        for (var checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = ReferencePath.ResourcesUsed[0];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesUsed[1] != 0){
        NumberOfResourcesLost[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = ReferencePath.ResourcesUsed[1];
                break;
            }
        }
    }
    if (ReferencePath.ResourcesUsed[2] != 0){
        NumberOfResourcesLost[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = ReferencePath.ResourcesUsed[2];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[0]!=0){
        NumberOfResourcesGained[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = ReferencePath.ResourcesGiven[0];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[1]!=0){
        NumberOfResourcesGained[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = ReferencePath.ResourcesGiven[1];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[2]!=0){
        NumberOfResourcesGained[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = ReferencePath.ResourcesGiven[2];
                break;
            }
        }
    }

    ReferencedResourceDisplay[0] = CardReference.ResourceReqDisplay[0].LostDisplay[NumberOfResourcesLost[0]].GainDisplay[NumberOfResourcesGained[0]];
    ReferencedResourceDisplay[0].SetActive(true);

    for(var a=0; a<ReferencedResourceDisplay[0].transform.childCount-2; a++){
        ReferencedResourceDisplay[0].transform.GetChild(a).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[ResourceDisplay1[a]-1];
    }

    //Option2
    if (ReferencePath.ResourcesUsed[3] != 0){ 
        NumberOfResourcesLost[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = ReferencePath.ResourcesUsed[3];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesUsed[4] != 0){
        NumberOfResourcesLost[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = ReferencePath.ResourcesUsed[4];
                break;
            }
        }
    }
    if (ReferencePath.ResourcesUsed[5] != 0){
        NumberOfResourcesLost[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = ReferencePath.ResourcesUsed[5];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[3]!=0){
        NumberOfResourcesGained[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = ReferencePath.ResourcesGiven[3];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[4]!=0){
        NumberOfResourcesGained[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = ReferencePath.ResourcesGiven[4];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[5]!=0){
        NumberOfResourcesGained[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = ReferencePath.ResourcesGiven[5];
                break;
            }
        }
    }

    ReferencedResourceDisplay[1] = CardReference.ResourceReqDisplay[1].LostDisplay[NumberOfResourcesLost[1]].GainDisplay[NumberOfResourcesGained[1]];
    ReferencedResourceDisplay[1].SetActive(true);

    for(a=0; a<ReferencedResourceDisplay[1].transform.childCount-2; a++){
        ReferencedResourceDisplay[1].transform.GetChild(a).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[ResourceDisplay2[a]-1];
    }

    //Option3
    if (ReferencePath.ResourcesUsed[6] != 0){ 
        NumberOfResourcesLost[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = ReferencePath.ResourcesUsed[6];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesUsed[7] != 0){
        NumberOfResourcesLost[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = ReferencePath.ResourcesUsed[7];
                break;
            }
        }
    }
    if (ReferencePath.ResourcesUsed[8] != 0){
        NumberOfResourcesLost[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = ReferencePath.ResourcesUsed[8];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[6]!=0){
        NumberOfResourcesGained[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = ReferencePath.ResourcesGiven[6];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[7]!=0){
        NumberOfResourcesGained[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = ReferencePath.ResourcesGiven[7];
                break;
            }
        }
    }

    if (ReferencePath.ResourcesGiven[8]!=0){
        NumberOfResourcesGained[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = ReferencePath.ResourcesGiven[8];
                break;
            }
        }
    }

    ReferencedResourceDisplay[2] = CardReference.ResourceReqDisplay[2].LostDisplay[NumberOfResourcesLost[2]].GainDisplay[NumberOfResourcesGained[2]];
    ReferencedResourceDisplay[2].SetActive(true);

    for(a=0; a<ReferencedResourceDisplay[2].transform.childCount-2; a++){
        ReferencedResourceDisplay[2].transform.GetChild(a).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[ResourceDisplay3[a]-1];
    }
}

function Select0(){
    CardReference.ButtonSpheresImage[0].color = Color.white;
    CardReference.ButtonSpheresImage[1].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[2].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[3].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[4].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
}

function Select1(){
    CardReference.ButtonSpheresImage[1].color = Color.white;
    CardReference.ButtonSpheresImage[2].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[3].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[4].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[0].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
}

function Select2(){
    CardReference.ButtonSpheresImage[2].color = Color.white;
    CardReference.ButtonSpheresImage[3].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[4].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[0].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[1].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
}

function Select3(){
    CardReference.ButtonSpheresImage[3].color = Color.white;
    CardReference.ButtonSpheresImage[4].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[0].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[1].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[2].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
}

function Select4(){
    CardReference.ButtonSpheresImage[4].color = Color.white;
    CardReference.ButtonSpheresImage[0].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[1].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[2].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
    CardReference.ButtonSpheresImage[3].color = Vector4(0.4784, 0.4431, 0.4431, 0.4431);
}

var AdjustmentCheck: boolean;

function UpdateNumberOfCards(){

    Important.NumberOfCards=transform.childCount -1;                          //Don't count pivot point into number of cards. 
    CardGapCloseCheck = false;
    TesterLerp = 0;
    
    if (Important.NumberOfCards >= 2) {
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = transform.GetChild(Important.NumberOfCards-1).gameObject; //Reference for right most card will be -1 in the [] brackets.
    }

    if (Important.NumberOfCards == 1){
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = LeftMostCard;
    }
                
    if (Important.NumberOfCards == 0){
        LeftMostCard = null;
        RightMostCard = null;
    }
    
    AdjustmentCheck = true;
}

private var CheckIfCardIsPlayed: boolean;
private var StartMovingCheck: boolean;

function SwipeUpReleased(){

    if (Camera.main.WorldToScreenPoint(CardSelected.transform.position).y > Screen.height/4){
        CheckIfCardIsPlayed = true;
    }

    if (CheckIfCardIsPlayed == true){
        if (Components.CardsPlayed[0] == null){
            Components.CardsPlayed[0] = CardSelected;
            CheckIfCardIsPlayed = false; 
            StartMovingCheck = true; 
            LerpProgress = 0.0;
            Components.CardsPlayed[0].tag = "PlayedCards";
            Components.CardsPlayed[0].GetComponent.<ResourceCardControl>().CardPlayPosition = 1;
            return;
        }

        if (Components.CardsPlayed[1] == null){
            Components.CardsPlayed[1] = CardSelected;
            CheckIfCardIsPlayed = false;
            StartMovingCheck = true; 
            LerpProgress = 0.0;
            Components.CardsPlayed[1].tag = "PlayedCards";
            Components.CardsPlayed[1].GetComponent.<ResourceCardControl>().CardPlayPosition = 2;
            return;
        }

        if (Components.CardsPlayed[2] == null){
            Components.CardsPlayed[2] = CardSelected;
            CheckIfCardIsPlayed = false;
            StartMovingCheck = true; 
            LerpProgress = 0.0;
            Components.CardsPlayed[2].tag = "PlayedCards";
            Components.CardsPlayed[2].GetComponent.<ResourceCardControl>().CardPlayPosition = 3;
            return;
        }
    }
        if (NullCardInstance!=null) {
            DestroyImmediate (NullCardInstance);
            NullCardInstance = null;
        }
        NullCardCount = 0;
        CardSelected.transform.parent = this.gameObject.transform;    

        var CurrentCardNumber = CardSelected.GetComponent.<ResourceCardControl>().CardNumber;
        var SortAscending = new Array ();

        if (Components.CardsPlayed[0] != null) SortAscending.push(Components.CardsPlayed[0].GetComponent.<ResourceCardControl>().CardNumber);
        if (Components.CardsPlayed[1] != null) SortAscending.push(Components.CardsPlayed[1].GetComponent.<ResourceCardControl>().CardNumber);
        if (Components.CardsPlayed[2] != null) SortAscending.push(Components.CardsPlayed[2].GetComponent.<ResourceCardControl>().CardNumber);
        SortAscending.push(CurrentCardNumber);
        SortAscending.Sort();

        if (SortAscending.length >= 1){
            if(CurrentCardNumber == SortAscending[0]){
                CardSelected.transform.SetSiblingIndex (CurrentCardNumber-1); 
            }
        }

        if (SortAscending.length >= 2){
            if(CurrentCardNumber == SortAscending[1]){
                CardSelected.transform.SetSiblingIndex (CurrentCardNumber-2); 
            }
        }

        if (SortAscending.length >= 3){
            if(CurrentCardNumber == SortAscending[2]){
                CardSelected.transform.SetSiblingIndex (CurrentCardNumber-3); 
            }
        }

        if (SortAscending.length >= 4){
            if(CurrentCardNumber == SortAscending[3]){
                CardSelected.transform.SetSiblingIndex (CurrentCardNumber-4); 
            }
        }

        Important.NumberOfCards=transform.childCount -1;
        UpdateNumberOfCards();

}

function AssignCardNumber(){
    
    if(Components.CardsPlayed[0] == null && Components.CardsPlayed[1]== null && Components.CardsPlayed[2] == null){
        for (var i: int = 0; i<Important.NumberOfCards;i++)
        {
            if (transform.GetChild(i).gameObject.tag != "Pivot")
            {
            transform.GetChild(i).gameObject.GetComponent.<ResourceCardControl>().CardNumber = i+1;
            }
        }
    }

    RotatingInProgress= true;
    if (AdjustmentCheck == true){
        TesterLerp = 0;
        AdjustmentCheck = false;
    }

    ShowRotateToCentre(CardSelected.transform.GetSiblingIndex());
    CardSelected.transform.parent = Components.CardsPlayedParent.transform;   

    for (i = 0; i<4; i++){

        if (CardsPlayed[i] != null){
            if (CardsPlayed[i].transform.parent != Components.CardsPlayedParent.transform){
                CardsPlayed[i] = CardSelected;
                break;
            }
        }

        if (CardsPlayed[i] == null){
            CardsPlayed[i] = CardSelected;
            break;
        }
    }
}


function DeselectCard(){

    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
    CardSelected.transform.parent = this.gameObject.transform;
    var CurrentCardNumber: int;
    CurrentCardNumber = CardSelected.GetComponent.<ResourceCardControl>().CardNumber;

    var SortAscending = new Array ();

    if (Components.CardsPlayed[0] != null) SortAscending.push(Components.CardsPlayed[0].GetComponent.<ResourceCardControl>().CardNumber);
    if (Components.CardsPlayed[1] != null) SortAscending.push(Components.CardsPlayed[1].GetComponent.<ResourceCardControl>().CardNumber);
    if (Components.CardsPlayed[2] != null) SortAscending.push(Components.CardsPlayed[2].GetComponent.<ResourceCardControl>().CardNumber);
    SortAscending.Sort();

    if (SortAscending.length >= 1){
        if(CurrentCardNumber == SortAscending[0]){
            CardSelected.transform.SetSiblingIndex (CurrentCardNumber-1); 
        }
    }

    if (SortAscending.length >= 2){
        if(CurrentCardNumber == SortAscending[1]){
            CardSelected.transform.SetSiblingIndex (CurrentCardNumber-2); 
        }
    }

    if (SortAscending.length >= 3){
        if(CurrentCardNumber == SortAscending[2]){
            CardSelected.transform.SetSiblingIndex (CurrentCardNumber-3); 
        }
    }

    Components.CardsPlayed[CardSelected.GetComponent.<ResourceCardControl>().CardPlayPosition-1] = null;
    CardSelected.GetComponent.<ResourceCardControl>().CardPlayPosition = 0;


    for (var i = 0; i < Components.CardsPlayed.Length -1; i++){       //Destroy and fill in gap for CardsPlayed Array when a card is deselected
        if (Components.CardsPlayed[i] == null){
            Components.CardsPlayed[i] = Components.CardsPlayed[i+1];  
            Components.CardsPlayed[i+1] = null;
            if (Components.CardsPlayed[i] != null) Components.CardsPlayed[i].GetComponent.<ResourceCardControl>().CardPlayPosition = i+1;
        }
    }
    

    Important.NumberOfCards=transform.childCount -1;
    CardGapCloseCheck = false;
    UpdateNumberOfCards();
    TesterLerp = 0;
    CardSelected.tag = "ResourceCards";

    if (Components.CardsPlayed[0] != null){
        StartMovingCheck = true; 
        LerpProgress = 0.0;
    }
}

function DiscardCards(Card1: int, Card2: int, Card3: int){

    for (var i: int = 0; i<Important.NumberOfCards;i++){
        if (transform.GetChild(i).gameObject.tag != "Pivot"){
            transform.GetChild(i).gameObject.GetComponent.<ResourceCardControl>().CardNumber = i+1;
        }
    }
    
    Components.CardsPlayed[0] = transform.GetChild(Card1).gameObject;
    transform.GetChild(Card1).gameObject.tag = "DestroyedCards";
    
    if (Card2 != -1){
        Components.CardsPlayed[1] = transform.GetChild(Card2).gameObject;
        transform.GetChild(Card2).gameObject.tag = "DestroyedCards";
    }

    if (Card3 != -1){
        Components.CardsPlayed[2] = transform.GetChild(Card3).gameObject;
        transform.GetChild(Card3).gameObject.tag = "DestroyedCards";
    }

    Components.CardsPlayed[0].transform.parent = Components.CardsPlayedParent.transform;
    if (Card2 != -1) Components.CardsPlayed[1].transform.parent = Components.CardsPlayedParent.transform;
    if (Card3 != -1) Components.CardsPlayed[2].transform.parent = Components.CardsPlayedParent.transform;


    StartMovingCheck = true; 
    LerpProgress = 0.0;

    Important.NumberOfCards=transform.childCount -1;                          //Don't count pivot point into number of cards. 
    CardGapCloseCheck = false;
    TesterLerp = 0;
    
    if (Important.NumberOfCards >= 2) {
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = transform.GetChild(Important.NumberOfCards-1).gameObject; //Reference for right most card will be -1 in the [] brackets.
    }

    if (Important.NumberOfCards == 1){
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = LeftMostCard;
    }
                
    if (Important.NumberOfCards == 0){
        LeftMostCard = null;
        RightMostCard = null;
    }
    
    AdjustmentCheck = true;

    yield WaitForSeconds (0.7f);
    Scripts.ParticleEffectControllerScript.DestroyExtraCards();
    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[6]);
    yield WaitForSeconds (0.7f);
    Destroy(Components.CardsPlayed[0]);
    Components.CardsPlayed[0]= null;
    Destroy(Components.CardsPlayed[1]);
    Components.CardsPlayed[1]= null;
    Destroy(Components.CardsPlayed[2]);
    Components.CardsPlayed[2]= null;

    UpdateNumberOfCards();
    yield WaitForSeconds (1.2f);
    Important.NumberOfCards=transform.childCount -1;
    Scripts.UIControllerScript.UpdateResourceCardNumber(Important.NumberOfCards);
}

function DestroyExtra3Cards(Card1: int, Card2: int, Card3: int){
 
    for (var i: int = 0; i<Important.NumberOfCards;i++){
        if (transform.GetChild(i).gameObject.tag != "Pivot"){
            transform.GetChild(i).gameObject.GetComponent.<ResourceCardControl>().CardNumber = i+1;
        }
    }

    Components.CardsPlayed[0] = transform.GetChild(Card1).gameObject;
    Components.CardsPlayed[1] = transform.GetChild(Card2).gameObject;
    Components.CardsPlayed[2] = transform.GetChild(Card3).gameObject;
    transform.GetChild(Card1).gameObject.tag = "DestroyedCards";
    transform.GetChild(Card2).gameObject.tag = "DestroyedCards";
    transform.GetChild(Card3).gameObject.tag = "DestroyedCards";
    Components.CardsPlayed[0].transform.parent = Components.CardsPlayedParent.transform;
    Components.CardsPlayed[1].transform.parent = Components.CardsPlayedParent.transform;
    Components.CardsPlayed[2].transform.parent = Components.CardsPlayedParent.transform;
    StartMovingCheck = true; 
    LerpProgress = 0.0;

    Important.NumberOfCards=transform.childCount -1;                          //Don't count pivot point into number of cards. 
    CardGapCloseCheck = false;
    TesterLerp = 0;
    
    if (Important.NumberOfCards >= 2) {
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = transform.GetChild(Important.NumberOfCards-1).gameObject; //Reference for right most card will be -1 in the [] brackets.
    }

    if (Important.NumberOfCards == 1){
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = LeftMostCard;
    }
                
    if (Important.NumberOfCards == 0){
        LeftMostCard = null;
        RightMostCard = null;
    }
    
    AdjustmentCheck = true;
    Important.NoClicking = false;
}

function DestroyExtra3CardsConfirmed(){
    yield WaitForSeconds (0.5f);
    Destroy(Components.CardsPlayed[0]);
    Components.CardsPlayed[0]= null;
    Destroy(Components.CardsPlayed[1]);
    Components.CardsPlayed[1]= null;
    Destroy(Components.CardsPlayed[2]);
    Components.CardsPlayed[2]= null;

    UpdateNumberOfCards();
    yield WaitForSeconds (1.2f);
    Important.NumberOfCards=transform.childCount -1;
    SortHandCards();
    Scripts.EventCardControlScript.Components.anim.Play("RemoveCard");
    yield WaitForSeconds (1.183f);
    Components.EventCardGameObject.tag = "EventCards";
    Scripts.EventCardControlScript.EventCardManager();
}

function SortHandCards(){ //To insert a line break when inputing from string, add a \n. E.g: "hey\ns" will print 2 lines, hey and s.

    Important.NumberOfCards=transform.childCount -1;  
    Components.Pivot.transform.SetSiblingIndex(Important.NumberOfCards);
    Scripts.UIControllerScript.UpdateResourceCardNumber(Important.NumberOfCards);
    Components.ResourceTypeCardCount[0] = 0;
    Components.ResourceTypeCardCount[1] = 0;
    Components.ResourceTypeCardCount[2] = 0;
    Components.ResourceTypeCardCount[3] = 0;
    Components.ResourceTypeCardCount[4] = 0;

    for (var i = 0; i <Important.NumberOfCards; i++) {       
        Components.CardTypesInHand[i] = transform.GetChild(i).gameObject.GetComponent.<ResourceCardControl>().ResourceType;
        if (Components.CardTypesInHand[i] == 0.01) Components.ResourceTypeCardCount[0] +=1;
        if (Components.CardTypesInHand[i] == 0.1) Components.ResourceTypeCardCount[1] +=1;
        if (Components.CardTypesInHand[i] == 1) Components.ResourceTypeCardCount[2] +=1;
        if (Components.CardTypesInHand[i] == 10) Components.ResourceTypeCardCount[3] +=1;
        if (Components.CardTypesInHand[i] == 100) Components.ResourceTypeCardCount[4] +=1;
    }

    Scripts.TechnologyGaugeControllerScript.IncreaseHumanTech(Components.ResourceTypeCardCount[3]);
    
    for (var a = 0; a < Components.ResourceTypeCardCount[0]; a++){                                             //To count number of each type of card
        transform.GetChild(a).gameObject.GetComponent.<ResourceCardControl>().ResourceType = 0.01;
        transform.GetChild(a).gameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite = Components.SpriteImageResourceCards[0];
        transform.GetChild(a).gameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[0];
        transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceName[0];
        transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceQuote[0];
        transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[0];
        transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[0];
    }
    
    for (var b = 0; b < Components.ResourceTypeCardCount[1]; b++){
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.GetComponent.<ResourceCardControl>().ResourceType = 0.1;
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite = Components.SpriteImageResourceCards[1];
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[1];
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceName[1];
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceQuote[1];
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[1];
        transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[1];
    }

    for (var c = 0; c < Components.ResourceTypeCardCount[2]; c++){
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.GetComponent.<ResourceCardControl>().ResourceType = 1;
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite = Components.SpriteImageResourceCards[2];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[2];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceName[2];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceQuote[2];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[2];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[2];
    }

    for (var d = 0; d < Components.ResourceTypeCardCount[3]; d++){
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.GetComponent.<ResourceCardControl>().ResourceType = 10;
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite = Components.SpriteImageResourceCards[3];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[3];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceName[3];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceQuote[3];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[3];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[3];
    }
    
    for (var e = 0; e < Components.ResourceTypeCardCount[4]; e++){
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.GetComponent.<ResourceCardControl>().ResourceType = 100;
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().sprite = Components.SpriteImageResourceCards[4];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[4];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceName[4];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.ResourceQuote[4];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[4];
        transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[4];
    }

    Scripts.UIControllerScript.UpdateLifeResourceCardNumber(Components.ResourceTypeCardCount[0]);
    Scripts.UIControllerScript.UpdateChaosResourceCardNumber(Components.ResourceTypeCardCount[4]);
    UpdateInformation();
}

private var Sign: String;

function DetermineSign(IntToCheck: int){
    if (IntToCheck > 0) Sign = "+";
    if (IntToCheck <= 0) Sign = "";
    return Sign;
}

function UpdateInformation(){
    var NetFood = Scripts.EventCardControlScript.Important.FoodProduction - Components.ResourceTypeCardCount[0]*3;

    Scripts.UIControllerScript.InformationOverviewText[1].text = "Food: " + DetermineSign(NetFood) + NetFood;
    Scripts.UIControllerScript.InformationOverviewText[2].text = "Morale: " + DetermineSign(Scripts.EventCardControlScript.Important.Morale) + Scripts.EventCardControlScript.Important.Morale;
    Scripts.UIControllerScript.InformationOverviewText[3].text = "Human Ties: " + DetermineSign(Scripts.EventCardControlScript.Important.HumanTies) + Scripts.EventCardControlScript.Important.HumanTies;
    Scripts.UIControllerScript.InformationOverviewText[4].text = "Food Consumption: " + Components.ResourceTypeCardCount[0]*3;
    Scripts.UIControllerScript.InformationOverviewText[5].text = "Food Production: " + Scripts.EventCardControlScript.Important.FoodProduction;
}

function SortRendering(BackgroundRender: int, CardToSort:GameObject){
        CardToSort.transform.GetChild(0).gameObject.GetComponent.<Transform>().localPosition.z = -0.01;
        CardToSort.transform.GetChild(1).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
        CardToSort.transform.GetChild(2).gameObject.GetComponent.<Transform>().localPosition.z = -0.03;
        CardToSort.transform.GetChild(3).gameObject.GetComponent.<Transform>().localPosition.z = -0.02;
}

function RotateToCentre(){
    var CardCount = transform.childCount-1;
    if (CardCount%2 == 1){
        var CardNumberToRotate = (CardCount-1)/2;
        while (transform.GetChild(CardNumberToRotate).GetComponent.<Transform>().rotation.eulerAngles.y < 179.5){
            transform.RotateAround (CentrePoint, Vector3.up, 100 * Time.deltaTime);
        }

        while (transform.GetChild(CardNumberToRotate).GetComponent.<Transform>().rotation.eulerAngles.y > 180.5){
            transform.RotateAround (CentrePoint, Vector3.down, 100 * Time.deltaTime);
        }
    }

    if (CardCount%2 == 0){
        CardNumberToRotate = CardCount/2 -1;
        while (transform.GetChild(CardNumberToRotate).GetComponent.<Transform>().rotation.eulerAngles.y < 170.5){
            transform.RotateAround (CentrePoint, Vector3.up, 100 * Time.deltaTime);
        }

        while (transform.GetChild(CardNumberToRotate).GetComponent.<Transform>().rotation.eulerAngles.y > 171.5){
            transform.RotateAround (CentrePoint, Vector3.down, 100 * Time.deltaTime);
        }
    }

    Components.ResourceCardsReadyCheck = true;
}

class ScriptReference2{
    var OptionClickControlScript: OptionClickControl;
    var EventCardControlScript: EventCardControl;
    var ParticleEffectControllerScript: ParticleEffectController;
    var UIControllerScript: UIController;
    var TechnologyGaugeControllerScript: TechnologyGaugeController; // For Human Tech
    var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
    var CameraControlScript: Camera_Control;
    var MainMenuControllerScript: MainMenuController;
}

class ComponentReference2{
    var CardPositions = new Vector3[15];
    var CardRotations = new Quaternion[15];
    var SpriteImageResourceCards = new Sprite[5];
    var CardTypesInHand = new float[15];
    var ResourceTypeCardCount = new int[5]; // 0 = Life, 1 = Resource, 2 = faith, 3 = human, 4 = destruction
    var CardsPlayed = new GameObject[3];
    var NullCard: GameObject;
    var Magnifier: GameObject;
    var QuoteTextTransform: Transform;
    var MagnifiedSprite: SpriteRenderer;
    var ResourceCardZoomIn = new SpriteRenderer[4];
    var ResourceCardTextZoomIn = new UnityEngine.UI.Text[3];
    var EventCardZoomIn = new SpriteRenderer[3];
    var EventCardTextZoomIn = new UnityEngine.UI.Text[3];
    var OptionCardZoomIn = new SpriteRenderer[4];
    var OptionCardTextZoomIn = new UnityEngine.UI.Text[5];
    var OptionCardAdditionInfo = new GameObject[15];
    var Pivot: GameObject;
    var EventCardGameObject: GameObject;
    var CardsPlayedParent: GameObject;
    var ResourceCardsReadyCheck: Boolean;
}

class ImportantVars1{
    var NoClicking: boolean;
    var GamePaused: boolean;
    var SelectedOptionCardNumber: int;
    var NumberOfCards: int;
}

class ReferencedCard{
    var EventCardSprite: SpriteRenderer;
    var EventCardText = new UnityEngine.UI.Text[2];
    var OptionCardSprite = new SpriteRenderer[3];
    var OptionCardText = new UnityEngine.UI.Text[9];
    var ResourceReqDisplay = new ResourceDisplay1[3];
    var ButtonSpheresImage = new UnityEngine.UI.Image[5];
    var QuickJump: GameObject;
    var LeftArrow: GameObject;
    var RightArrow: GameObject;
    var OptionCardPosition: Transform;
}

class ResourceDisplay1{
    var LostDisplay = new ResourceLostDisplay[4];
}

class ResourceLostDisplay1{
    var GainDisplay = new GameObject[4];
}

