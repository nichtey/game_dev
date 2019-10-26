#pragma strict

var Scripts: ScriptReference1;
var Components: ComponentReference1;
var EventDetails: FromEventControl;
var CurrentStatus: CurrentStatusCheck;

function Start () {
    Components.ResourceQuote[3]  = "“It is as inhuman to be totally good\nas it is to be totally evil.”";
    Components.ResourceQuote[4]  = "“A nation that destroys its soils\ndestroys itself.”";
}

private var FadeTrapDoorCheck: boolean;
private var FadeTrapDoorLerp: float;
private var FadeDoorLerp: float;

function Update () {

    if (FadeTrapDoorCheck == true){
        FadeTrapDoorLerp += Time.deltaTime/0.25;
        FadeDoorLerp = Mathf.Lerp(1,0,FadeTrapDoorLerp);
        Components.TrapDoorGameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color(1,1,1, FadeDoorLerp);
        Components.TrapDoorGameObject.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color(1,1,1, FadeDoorLerp);
        Components.TrapDoorGameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color(1,1,1, FadeDoorLerp);

        if (FadeTrapDoorLerp>=1)
        {
            FadeTrapDoorCheck = false;
            FadeDoorLerp = 0;
            FadeTrapDoorLerp = 0;
            Components.TrapDoorGameObject.transform.GetChild(0).GetComponent.<SpriteRenderer>().color = Color(1,1,1,1);
            Components.TrapDoorGameObject.transform.GetChild(1).GetComponent.<SpriteRenderer>().color = Color(1,1,1,1);
            Components.TrapDoorGameObject.transform.GetChild(2).GetComponent.<SpriteRenderer>().color = Color(1,1,1,1);
            Components.TrapDoorGameObject.SetActive(false);
        }
    }
}

private var NumberOfResourceCardsToGive: int;
private var RandomlyDestroyedCardNumbers = new int[3];
var InstantiatedCards = new GameObject[3];

function CheckMeetRequirements(Card1:GameObject, Card2:GameObject, Card3:GameObject, OptionCardSelected: int){
    
    CurrentStatus.MeetOtherRequirement = false;
    CurrentStatus.ResourceRequired = EventDetails.ResourceRequirements[OptionCardSelected -1];
    CurrentStatus.OtherRequirement = EventDetails.TechnologyRequired[OptionCardSelected -1];
    CurrentStatus.ResourcePlaced = 0;
    CurrentStatus.CardsPlayed[0] = Card1;
    CurrentStatus.CardsPlayed[1] = Card2;
    CurrentStatus.CardsPlayed[2] = Card3;
    InstantiatedCards[0] = null;
    InstantiatedCards[1] = null;
    InstantiatedCards[2] = null;
    EventDetails.SpecialResourceGained = 0;
    EventDetails.PromotionResource[0] = 0;
    EventDetails.PromotionResource[1] = 0;
    EventDetails.PromotionResource[2] = 0;

    if (CurrentStatus.CardsPlayed[0] != null) CurrentStatus.ResourcePlaced += CurrentStatus.CardsPlayed[0].GetComponent.<ResourceCardControl>().ResourceType; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction 
    if (CurrentStatus.CardsPlayed[1] != null) CurrentStatus.ResourcePlaced += CurrentStatus.CardsPlayed[1].GetComponent.<ResourceCardControl>().ResourceType; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction 
    if (CurrentStatus.CardsPlayed[2] != null) CurrentStatus.ResourcePlaced += CurrentStatus.CardsPlayed[2].GetComponent.<ResourceCardControl>().ResourceType; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction 
    
    if (CurrentStatus.OtherRequirement == 0){CurrentStatus.MeetOtherRequirement = true;}
    
    if (CurrentStatus.OtherRequirement >0 && CurrentStatus.OtherRequirement <5){
        for (var i = 0; i<10; i++){
            if (CurrentStatus.SkillsPossessed[i] == CurrentStatus.OtherRequirement){
                CurrentStatus.MeetOtherRequirement = true;
                break;
            }

            if (CurrentStatus.SkillsPossessed[i] == 0){
                Scripts.UIControllerScript.Announcement("You don't possess the skill required to select this option.");
                return;
            }
        }
    }

    if (CurrentStatus.OtherRequirement > 100){
        if (Scripts.EventCardControlScript.Important.HumanRelationship + 100 >= CurrentStatus.OtherRequirement){
            CurrentStatus.MeetOtherRequirement = true;
        }

        else {
            Scripts.UIControllerScript.Announcement("Your cannot select this option because your human ties is too low.");
            return;
        }
    }

    if (CurrentStatus.OtherRequirement < -100){
        if (Scripts.EventCardControlScript.Important.HumanRelationship - 100 >= CurrentStatus.OtherRequirement){
            CurrentStatus.MeetOtherRequirement = true;
        }

        else {
            Scripts.UIControllerScript.Announcement("Your cannot select this option because your human ties is too low.");
            return;
        }
    }

    if (EventDetails.FoodChange[OptionCardSelected-1] <0){
        if (Scripts.NatureControllerScript.CurrentFood < -EventDetails.FoodChange[OptionCardSelected-1]){
            Scripts.UIControllerScript.Announcement("You do not have enough food to select this option.");
            return;
        }
    }

    if (EventDetails.DiscardCards[OptionCardSelected - 1] != 0){
        if (Scripts.HandCardRotationScript.Important.NumberOfCards < EventDetails.DiscardCards[OptionCardSelected - 1]){
            Scripts.UIControllerScript.Announcement("You do not have enough discardable resource cards.");
            return;
        }
    }

    if (Scripts.MainMenuControllerScript.LevelSelected == 1){
        if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22){

            if (OptionCardSelected == 1){
                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "After tapping on option 1,\nhold it there a little while.";
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
            }

            if (OptionCardSelected != 1){
                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.x = 0.04;
                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale.y = 0.04;
                Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Wrong option selected!\n(Tap and hold option 1)";
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position.x = 20;
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
            }
                
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
            return;
        }
    }

    if (CurrentStatus.ResourcePlaced == CurrentStatus.ResourceRequired && CurrentStatus.MeetOtherRequirement == true){
        //Debug.Log("SUCCESS");
        
        Scripts.HandCardRotationScript.Important.NoClicking = true;

        if (Scripts.MainMenuControllerScript.LevelSelected == 1){
            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("FadeCallOut");
            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[11].Play("DimScreenFadeOut");
            ClearFadeOut();
        }

        if (EventDetails.LoseCheck[OptionCardSelected-1] == true){
            Scripts.CameraControlScript.LoseGame("Defeated", "Avoid being forced to choose such\noptions by making neccessary prep.");
            return;
        }

        if (EventDetails.FoodProductionChange[OptionCardSelected-1] != 0){
            Scripts.EventCardControlScript.Important.FoodProduction += EventDetails.FoodProductionChange[OptionCardSelected-1];
        }

        if (EventDetails.MoraleProductionChange[OptionCardSelected-1] != 0){
            Scripts.EventCardControlScript.Important.Morale += EventDetails.MoraleProductionChange[OptionCardSelected-1];
        }

        if (EventDetails.HumanTiesProductionChange[OptionCardSelected-1] != 0){
            Scripts.EventCardControlScript.Important.HumanTies += EventDetails.HumanTiesProductionChange[OptionCardSelected-1];
        }

        if (OptionCardSelected == 1){
            if (EventDetails.ResourcesUsed[0] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[0]-1] -=1;
            if (EventDetails.ResourcesUsed[1] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[1]-1] -=1;
            if (EventDetails.ResourcesUsed[2] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[2]-1]-=1;
        }

        if (OptionCardSelected == 2){
            if (EventDetails.ResourcesUsed[3] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[3]-1]-=1;
            if (EventDetails.ResourcesUsed[4] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[4]-1]-=1;
            if (EventDetails.ResourcesUsed[5] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[5]-1]-=1;
        }

        if (OptionCardSelected == 3){
            if (EventDetails.ResourcesUsed[6] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[6]-1]-=1;
            if (EventDetails.ResourcesUsed[7] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[7]-1]-=1;
            if (EventDetails.ResourcesUsed[8] != 0)Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[8]-1]-=1;
        }



        if (EventDetails.StatsChange[OptionCardSelected-1] != 0){
            if (EventDetails.StatsChange[OptionCardSelected-1] > -20 && EventDetails.StatsChange[OptionCardSelected-1] <20){
                
                Scripts.NatureControllerScript.UpdateMorale(EventDetails.StatsChange[OptionCardSelected-1]);
                if (EventDetails.StatsChange[OptionCardSelected-1] <0){
                    Components.NatureAvatarAnim.Play("LoseMorale");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield WaitForSeconds(0.67f);
                }

                if (EventDetails.StatsChange[OptionCardSelected-1] >0){
                    Components.NatureAvatarAnim.Play("RecoverMorale");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                    yield WaitForSeconds(0.75f);
                }
            }

            if (EventDetails.StatsChange[OptionCardSelected-1] > -100 && EventDetails.StatsChange[OptionCardSelected-1] <100){
                Scripts.NatureControllerScript.Attack += EventDetails.StatsChange[OptionCardSelected-1]/10;
                Components.AnimationStatsChange[0].SetActive(true);
                Scripts.NatureControllerScript.AtkChangeAnimation(EventDetails.StatsChange[OptionCardSelected-1]/10);

            }
            if (EventDetails.StatsChange[OptionCardSelected-1] > -1000 && EventDetails.StatsChange[OptionCardSelected-1] <1000){
                Scripts.NatureControllerScript.Defence += EventDetails.StatsChange[OptionCardSelected-1]/100;
                Components.AnimationStatsChange[1].SetActive(true);
                Scripts.NatureControllerScript.DefenceChangeAnimation(EventDetails.StatsChange[OptionCardSelected-1]/100);
            }
        }

        if (EventDetails.StatsChange[OptionCardSelected+2] != 0){
            
            if (EventDetails.StatsChange[OptionCardSelected+2] > -10 && EventDetails.StatsChange[OptionCardSelected+2] <10){ 
                Scripts.HumanControllerScript.Health += EventDetails.StatsChange[OptionCardSelected+2];
            }

            if (EventDetails.StatsChange[OptionCardSelected+2] > -100 && EventDetails.StatsChange[OptionCardSelected+2] <100){
                Scripts.HumanControllerScript.Attack += EventDetails.StatsChange[OptionCardSelected+2]/10;
                Components.AnimationStatsChange[2].SetActive(true);
                Scripts.HumanControllerScript.AtkChangeAnimation(EventDetails.StatsChange[OptionCardSelected+2]/10);
            }
            
            if (EventDetails.StatsChange[OptionCardSelected+2] > -1000 && EventDetails.StatsChange[OptionCardSelected+2] <1000){ 
                Scripts.HumanControllerScript.Defence += EventDetails.StatsChange[OptionCardSelected+2]/100;
                Components.AnimationStatsChange[3].SetActive(true);
                Scripts.HumanControllerScript.DefenceChangeAnimation(EventDetails.StatsChange[OptionCardSelected+2]/100);
            }
        }

        if (EventDetails.FoodChange[OptionCardSelected-1]!=0){
            Scripts.NatureControllerScript.IncreaseNatureFood(EventDetails.FoodChange[OptionCardSelected-1]/100);

            if (EventDetails.FoodChange[OptionCardSelected-1] <0){
                Components.NatureAvatarAnim.Play("LoseFood");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                yield WaitForSeconds(0.67f);
            }

            if (EventDetails.FoodChange[OptionCardSelected-1] >0){
                Components.NatureAvatarAnim.Play("RecoverFood"); 
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                yield WaitForSeconds(0.75f);
            }
        }

        if (EventDetails.HumanRelationshipChange[OptionCardSelected-1]!=0){
            Scripts.EventCardControlScript.UpdateHumanRelationship(EventDetails.HumanRelationshipChange[OptionCardSelected-1]);

            if (EventDetails.HumanRelationshipChange[OptionCardSelected-1] <0){
                //Components.NatureAvatarAnim.Play("LoseFood");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                yield WaitForSeconds(0.67f);
            }

            if (EventDetails.HumanRelationshipChange[OptionCardSelected-1] >0){
                //Components.NatureAvatarAnim.Play("RecoverFood"); 
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                yield WaitForSeconds(0.75f);
            }
        }

        if (EventDetails.RemoveEventCheck[OptionCardSelected-1] == true){
            for(var CheckEmpty = 0; CheckEmpty<Scripts.EventCardControlScript.Important.CardsToDestroy.length; CheckEmpty++){
                if (Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].TechLevel == 0){
                    Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].TechLevel = EventDetails.CardIdentifier[0];
                    Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].Position = EventDetails.CardIdentifier[1];
                    if (Scripts.EventCardControlScript.Components.OptionCardEffect[OptionCardSelected-1].text == "Remove this event") Scripts.EventCardControlScript.Important.DestroyEventCheck = true;
                    break;
                } 
            }
        }

        if (EventDetails.CalledCardTechLevel[OptionCardSelected-1] == -1){
            Scripts.EventCardControlScript.Important.SpecialCardActivated = 1;
        }

        if (CurrentStatus.CardsPlayed[0] != null){
            
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[4]);
            Scripts.UIControllerScript.AnimateSpriteCheck = 0;
            Scripts.UIControllerScript.DoneShaking = false;
            Scripts.UIControllerScript.Announcer.SetActive (false);
            CurrentStatus.CardsPlayed[0].tag = "DestroyedCards";
            if (CurrentStatus.CardsPlayed[1] == null) Scripts.ParticleEffectControllerScript.DestroyCards();
            FadeOutPlayedCards(0);

            if (CurrentStatus.CardsPlayed[1] != null){
                CurrentStatus.CardsPlayed[1].tag = "DestroyedCards";
                if (CurrentStatus.CardsPlayed[2] == null) Scripts.ParticleEffectControllerScript.Destroy2Cards();
                FadeOutPlayedCards(1);

                if (CurrentStatus.CardsPlayed[2] != null){
                    CurrentStatus.CardsPlayed[2].tag = "DestroyedCards";
                    Scripts.ParticleEffectControllerScript.Destroy3Cards();
                    FadeOutPlayedCards(2);
                }
            }
            yield WaitForSeconds(1);
        }

        NumberOfResourceCardsToGive = 0;
        var ResourceTypeTemp: int;

        if (OptionCardSelected == 1){

            for (i = 0; i < 3; i++){

                if (EventDetails.ResourceGained[i] >= 6){
                    EventDetails.SpecialResourceGained = 1;
                    EventDetails.PromotionResource[0] = EventDetails.ResourceGained[i] - 5;
                }

                if (EventDetails.ResourceGained[i] != 0 && EventDetails.ResourceGained[i] < 6){
                    NumberOfResourceCardsToGive += 1;
                    ResourceTypeTemp = EventDetails.ResourceGained[i] -1;
                    InstantiatedCards[i] = Instantiate(Components.ResourceCardPrefabs[ResourceTypeTemp]);
                    FadeInGivenCards(i);
                }
            }

            if (NumberOfResourceCardsToGive == 1){
                InstantiatedCards[0].transform.position = Vector3(-0.63,0,4.3);
            }

            if (NumberOfResourceCardsToGive == 2){
                InstantiatedCards[0].transform.position = Vector3(0.62,0,4.3);
                InstantiatedCards[1].transform.position = Vector3(-1.88,0,4.3);
            }

            if (NumberOfResourceCardsToGive == 3){
                InstantiatedCards[0].transform.position = Vector3(1.87,0,4.3);
                InstantiatedCards[1].transform.position = Vector3(-0.63,0,4.3);
                InstantiatedCards[2].transform.position = Vector3(-3.13,0,4.3);
            }
        }

        if (OptionCardSelected == 2){
            for (var j = 3; j < 6; j++){

                if (EventDetails.ResourceGained[j] >= 6){
                    EventDetails.SpecialResourceGained = 1;
                    EventDetails.PromotionResource[1] = EventDetails.ResourceGained[j] - 5;
                }

                if (EventDetails.ResourceGained[j] != 0  && EventDetails.ResourceGained[j] < 6){
                    NumberOfResourceCardsToGive += 1;
                    ResourceTypeTemp = EventDetails.ResourceGained[j] -1;
                    InstantiatedCards[j-3] = Instantiate(Components.ResourceCardPrefabs[ResourceTypeTemp]); 
                    FadeInGivenCards(j-3);
                }
            }

            if (NumberOfResourceCardsToGive == 1){
                InstantiatedCards[0].transform.position = Vector3(-0.63,0,4.3);
            }

            if (NumberOfResourceCardsToGive == 2){
                InstantiatedCards[0].transform.position = Vector3(0.62,0,4.3);
                InstantiatedCards[1].transform.position = Vector3(-1.88,0,4.3);
            }

            if (NumberOfResourceCardsToGive == 3){
                InstantiatedCards[0].transform.position = Vector3(1.87,0,4.3);
                InstantiatedCards[1].transform.position = Vector3(-0.63,0,4.3);
                InstantiatedCards[2].transform.position = Vector3(-3.13,0,4.3);
            }
        }

        if (OptionCardSelected == 3){

            for (var k = 6; k < 9; k++){

                if (EventDetails.ResourceGained[k] >= 6){
                    EventDetails.SpecialResourceGained = 1;
                    EventDetails.PromotionResource[2] = EventDetails.ResourceGained[k] - 5;
                }

                if (EventDetails.ResourceGained[k] != 0 && EventDetails.ResourceGained[k] < 6){
                    NumberOfResourceCardsToGive += 1;
                    ResourceTypeTemp = EventDetails.ResourceGained[k] -1;
                    InstantiatedCards[k-6] = Instantiate(Components.ResourceCardPrefabs[ResourceTypeTemp]); // Array for Resource Gained 1 more than resource card prefabs
                    FadeInGivenCards(k-6);
                }
            }

            if (NumberOfResourceCardsToGive == 1){
                InstantiatedCards[0].transform.position = Vector3(-0.63,0,4.3);
            }

            if (NumberOfResourceCardsToGive == 2){
                InstantiatedCards[0].transform.position = Vector3(0.62,0,4.3);
                InstantiatedCards[1].transform.position = Vector3(-1.88,0,4.3);
            }

            if (NumberOfResourceCardsToGive == 3){
                InstantiatedCards[0].transform.position = Vector3(1.87,0,4.3);
                InstantiatedCards[1].transform.position = Vector3(-0.63,0,4.3);
                InstantiatedCards[2].transform.position = Vector3(-3.13,0,4.3);
            }
        }

        if (NumberOfResourceCardsToGive != 0){
            if (Scripts.MiscellaneousGameManagementScript.CurrentSoundEffect != Scripts.MiscellaneousGameManagementScript.SoundEffectClips[4]){
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[5]);
            }
            yield WaitForSeconds(1);
        }

        if (InstantiatedCards[0] != null){
            InstantiatedCards[0].transform.parent = Components.HandCardRotationTransform; 
            var ResourceType = InstantiatedCards[0].GetComponent.<ResourceCardControl>().ResourceType;
            if (ResourceType == 100){
                InstantiatedCards[0].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards);
                Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]+=1;
            }

            if (ResourceType == 10){
                InstantiatedCards[0].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]);
                Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]+=1;
            }

            if (ResourceType == 1){
                InstantiatedCards[0].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]);
                Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[2]+=1;
            }

            if (ResourceType == 0.1){
                InstantiatedCards[0].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] + Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]);
                Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]+=1;
            }

            if (ResourceType == 0.01){
                InstantiatedCards[0].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]);
                Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]+=1;
            }

            Scripts.HandCardRotationScript.Important.NumberOfCards +=1;
            InstantiatedCards[0].GetComponent.<ResourceCardControl>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards+1;

            if (InstantiatedCards[1] != null){

                ResourceType = InstantiatedCards[1].GetComponent.<ResourceCardControl>().ResourceType;
                InstantiatedCards[1].transform.parent = Components.HandCardRotationTransform; 

                if (ResourceType == 100){
                    InstantiatedCards[1].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]+=1;
                }

                if (ResourceType == 10){
                    InstantiatedCards[1].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]+=1;
                }

                if (ResourceType == 1){
                    InstantiatedCards[1].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[2]+=1;
                }

                if (ResourceType == 0.1){
                    InstantiatedCards[1].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] + Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]+=1;
                }

                if (ResourceType == 0.01){
                    InstantiatedCards[1].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]+=1;
                }

                Scripts.HandCardRotationScript.Important.NumberOfCards +=1;
                InstantiatedCards[1].GetComponent.<ResourceCardControl>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards+2;

                if (InstantiatedCards[2] != null){
                    ResourceType = InstantiatedCards[2].GetComponent.<ResourceCardControl>().ResourceType;
                    InstantiatedCards[2].transform.parent = Components.HandCardRotationTransform; 
                    
                    if (ResourceType == 100){
                        InstantiatedCards[2].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]+=1;
                    }

                    if (ResourceType == 10){
                        InstantiatedCards[2].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]+=1;
                    }

                    if (ResourceType == 1){
                        InstantiatedCards[2].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[2]+=1;
                    }

                    if (ResourceType == 0.1){
                        InstantiatedCards[2].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] + Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]+=1;
                    }

                    if (ResourceType == 0.01){
                        InstantiatedCards[2].transform.SetSiblingIndex (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]+=1;
                    }
                    
                    Scripts.HandCardRotationScript.Important.NumberOfCards +=1;
                    InstantiatedCards[2].GetComponent.<ResourceCardControl>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards+3;
                }
            }
           
            Scripts.HandCardRotationScript.UpdateNumberOfCards();
        }

        if (EventDetails.ChanceToAddCard[OptionCardSelected-1] != 0){
            var RandomRoll: float;
            RandomRoll = Random.Range(0.0f, 1.0f);
            //Debug.Log(RandomRoll);
            if (RandomRoll > EventDetails.ChanceToAddCard[OptionCardSelected-1]){   //Do not add the card
                EventDetails.CalledCardTechLevel[OptionCardSelected-1] = 0;
            }
        }

        if (EventDetails.CalledCardTechLevel[OptionCardSelected-1] > 0){

            if (InstantiatedCards[0] != null){
                yield WaitForSeconds(0.8);
            }

            for(CheckEmpty = 0; CheckEmpty<Scripts.EventCardControlScript.Important.CardsToAddIn.length; CheckEmpty++){
                if (Scripts.EventCardControlScript.Important.CardsToAddIn[CheckEmpty].TechLevel == 0){
                    Scripts.EventCardControlScript.Important.CardsToAddIn[CheckEmpty].TechLevel = EventDetails.CalledCardTechLevel[OptionCardSelected-1];
                    Scripts.EventCardControlScript.Important.CardsToAddIn[CheckEmpty].Position = EventDetails.CalledCardPosition[OptionCardSelected-1];
                    if (EventDetails.EnableAnimation == true){
                        PlayShuffleInAnimation(EventDetails.CalledCardTechLevel[OptionCardSelected-1], EventDetails.CalledCardPosition[OptionCardSelected-1]);
                        yield WaitForSeconds(0.8);
                    }

                    break;
                } 
            }
        }

        if (EventDetails.PromotionResource[OptionCardSelected-1] != 0){
            
            Components.PromotionFinishCheck = false;
            Scripts.CameraControlScript.FadeThroughBlack(0);
            yield WaitForSeconds(0.8);
            Components.PromotionScene.SetActive(true);
            Components.PromotionCard.sprite = EventDetails.PromotionSprite[OptionCardSelected-1];
            Components.PromotionTextRenderer[0].text = EventDetails.PromotionText[OptionCardSelected-1];
            Components.PromotionTextRenderer[0].text = Components.PromotionTextRenderer[0].text.Replace("NWL", "\n");
            Components.PromotionTextRenderer[1].text = EventDetails.PromotionText[3+OptionCardSelected-1];
            Components.PromotionTextRenderer[1].text = Components.PromotionTextRenderer[1].text.Replace("NWL", "\n");
            Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume/4;
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[3]);
            Camera.main.transform.position.z = 62.3;

            for (i = 0; i<10; i++){
                if (CurrentStatus.SkillsPossessed[i] == 0){
                    CurrentStatus.SkillsPossessed[i] = EventDetails.PromotionResource[OptionCardSelected-1];
                    break;
                }
            }

            for (i=0; i<6; i++){
                if (Scripts.UIControllerScript.InformationSkillSpriteHolders[i].enabled == false){
                    Scripts.UIControllerScript.InformationSkillSpriteHolders[i].sprite = Components.ResourceSymbol[EventDetails.PromotionResource[OptionCardSelected-1] + 4];
                    Scripts.UIControllerScript.InformationSkillSpriteHolders[i].enabled = true;
                    Scripts.UIControllerScript.InformationSkillDesign[i].color = Color.white;
                    break;
                }
            }

            yield WaitForSeconds(1.05);
            Components.PromotionFinishCheck = true;
            return;
        }

        if (EventDetails.UnlockOptionNumber[OptionCardSelected-1] != 0){
            yield WaitForSeconds(0.6);
            var ReferencedPath = Scripts.EventCardControlScript.EventDisplay.Normal[EventDetails.SpriteDisplayIdentifier[0]-1].Position[EventDetails.SpriteDisplayIdentifier[1]-1];
            ReferencedPath.UnlockOptionNumber[EventDetails.UnlockOptionNumber[OptionCardSelected-1]-1] = 0;
            Scripts.EventCardControlScript.Components.OptionCardRenderers[2].sprite = ReferencedPath.SpriteGroup[2]; 
            Scripts.ParticleEffectControllerScript.UnlockedCard();
            yield WaitForSeconds(1.85);
        }

        if (EventDetails.DiscardCards[OptionCardSelected - 1] != 0){

            RandomlyDestroyedCardNumbers[0] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards); //random.range not inclusive of max integer
            RandomlyDestroyedCardNumbers[1] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);
            RandomlyDestroyedCardNumbers[2] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);

            while ( RandomlyDestroyedCardNumbers[1] == RandomlyDestroyedCardNumbers[0])RandomlyDestroyedCardNumbers[1] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);
            while ( RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[0] || RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[1])RandomlyDestroyedCardNumbers[2] = Random.Range(0,Scripts.HandCardRotationScript.Important.NumberOfCards);
            
            if (EventDetails.DiscardCards[OptionCardSelected - 1] == 1){
                RandomlyDestroyedCardNumbers[1] = -1;
                RandomlyDestroyedCardNumbers[2] = -1;
            }

            if (EventDetails.DiscardCards[OptionCardSelected - 1] == 2){
                RandomlyDestroyedCardNumbers[2] = -1;
            }

            Scripts.HandCardRotationScript.DiscardCards(RandomlyDestroyedCardNumbers[0], RandomlyDestroyedCardNumbers[1], RandomlyDestroyedCardNumbers[2]);
            yield WaitForSeconds(1.8f);
        }

        Scripts.EventCardControlScript.EventCardRemoved();

    }
    
    if (CurrentStatus.ResourcePlaced != CurrentStatus.ResourceRequired){
        Scripts.UIControllerScript.Announcement("Please ensure that the correct resource cards are placed.");
    }

    if (CurrentStatus.MeetOtherRequirement == false){
        Scripts.UIControllerScript.Announcement("You don't meet this event card's requirements.");
    }
}

    function FinishedSpecialUpgrade(){
        Scripts.CameraControlScript.FadeThroughBlack(0.1);
        yield WaitForSeconds(0.8);
        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume*4;
        Components.PromotionScene.SetActive(false);
        Camera.main.transform.position.z = 1.84;        
        yield WaitForSeconds(0.5);
        Scripts.EventCardControlScript.EventCardRemoved();
    }

    function FadeOutPlayedCards(i: int){

        if (CurrentStatus.CardsPlayed[i]!= null){
            while(CurrentStatus.CardsPlayed[i].GetComponent.<SpriteRenderer>().color.a>0){
                CurrentStatus.CardsPlayed[i].GetComponent.<SpriteRenderer>().color.a -= 0.05;
                CurrentStatus.CardsPlayed[i].transform.GetChild(0).GetComponent.<SpriteRenderer>().color.a -= 0.1;
                CurrentStatus.CardsPlayed[i].transform.GetChild(1).GetComponent.<SpriteRenderer>().color.a -= 0.1;
                CurrentStatus.CardsPlayed[i].transform.GetChild(2).GetComponent.<SpriteRenderer>().color.a -= 0.1;
                CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().color.a -= 0.1;
                CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().color.a -= 0.1;
                CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().color.a -= 0.1;
                yield WaitForSeconds(0.025);
            }
        
            Destroy (CurrentStatus.CardsPlayed[i]);
            CurrentStatus.CardsPlayed[i] = null;
            Scripts.HandCardRotationScript.Components.CardsPlayed[i]=null;
        }
    }

    function FadeInGivenCards(i: int){
        InstantiatedCards[i].GetComponent.<SpriteRenderer>().color.a = 0;
        InstantiatedCards[i].transform.GetChild(0).GetComponent.<SpriteRenderer>().color.a = 0;
        InstantiatedCards[i].transform.GetChild(1).GetComponent.<SpriteRenderer>().color.a = 0;
        InstantiatedCards[i].transform.GetChild(2).GetComponent.<SpriteRenderer>().color.a = 0;
        InstantiatedCards[i].transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().color.a = 0;
        InstantiatedCards[i].transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().color.a = 0;
        InstantiatedCards[i].transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().color.a = 0;

        while(InstantiatedCards[i].GetComponent.<SpriteRenderer>().color.a<1){
            InstantiatedCards[i].GetComponent.<SpriteRenderer>().color.a += 0.1;
            InstantiatedCards[i].transform.GetChild(0).GetComponent.<SpriteRenderer>().color.a += 0.1;
            InstantiatedCards[i].transform.GetChild(1).GetComponent.<SpriteRenderer>().color.a += 0.1;
            InstantiatedCards[i].transform.GetChild(2).GetComponent.<SpriteRenderer>().color.a += 0.1;
            InstantiatedCards[i].transform.GetChild(3).transform.GetChild(0).GetComponent.<UnityEngine.UI.Text>().color.a += 0.1;
            InstantiatedCards[i].transform.GetChild(3).transform.GetChild(1).GetComponent.<UnityEngine.UI.Text>().color.a += 0.1;
            InstantiatedCards[i].transform.GetChild(3).transform.GetChild(2).GetComponent.<UnityEngine.UI.Text>().color.a += 0.1;
            yield WaitForSeconds(0.025);
        }
    }

private var NumberOfResourcesGained = new int[3];
private var NumberOfResourcesLost = new int[3];
var ReferencedResourceDisplay = new GameObject[3];
private var ResourceDisplay1 = new int[6];
private var ResourceDisplay2 = new int[6];
private var ResourceDisplay3 = new int[6];


function UpdateCards(
        CardIdentifier1: int, 
        CardIdentifier2: int){

    EventDetails.CardIdentifier[0] = CardIdentifier1;
    EventDetails.CardIdentifier[1] = CardIdentifier2;

    for (var Clear=0; Clear<5; Clear++){
        ResourceDisplay3[Clear] = 0;
        ResourceDisplay2[Clear] = 0;
        ResourceDisplay1[Clear] = 0;
    }

    for (Clear=0; Clear<3; Clear++){
        NumberOfResourcesGained[Clear] = 0;
        NumberOfResourcesLost[Clear] = 0;
        if (ReferencedResourceDisplay[Clear] != null) ReferencedResourceDisplay[Clear].SetActive(false);
    }

    //Option Card 1
    if (EventDetails.ResourcesUsed[0] != 0){ 
        NumberOfResourcesLost[0] +=1;
        for (var checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = EventDetails.ResourcesUsed[0];
                break;
            }
        }
    }

    if (EventDetails.ResourcesUsed[1] != 0){
        NumberOfResourcesLost[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = EventDetails.ResourcesUsed[1];
                break;
            }
        }
    }
    if (EventDetails.ResourcesUsed[2] != 0){
        NumberOfResourcesLost[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = EventDetails.ResourcesUsed[2];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[0]!=0){
        NumberOfResourcesGained[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = EventDetails.ResourceGained[0];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[1]!=0){
        NumberOfResourcesGained[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = EventDetails.ResourceGained[1];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[2]!=0){
        NumberOfResourcesGained[0] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay1[checkempty] == 0){
                ResourceDisplay1[checkempty] = EventDetails.ResourceGained[2];
                break;
            }
        }
    }

    ReferencedResourceDisplay[0] = Components.ResourceReqDisplay[0].LostDisplay[NumberOfResourcesLost[0]].GainDisplay[NumberOfResourcesGained[0]];
    ReferencedResourceDisplay[0].SetActive(true);

    for(var a=0; a<ReferencedResourceDisplay[0].transform.childCount-2; a++){
        ReferencedResourceDisplay[0].transform.GetChild(a).GetComponent.<SpriteRenderer>().sprite = Components.ResourceSymbol[ResourceDisplay1[a]-1];
    }

    //Option Card 2

    if (EventDetails.ResourcesUsed[3] != 0){ 
        NumberOfResourcesLost[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = EventDetails.ResourcesUsed[3];
                break;
            }
        }
    }

    if (EventDetails.ResourcesUsed[4] != 0){
        NumberOfResourcesLost[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = EventDetails.ResourcesUsed[4];
                break;
            }
        }
    }
    if (EventDetails.ResourcesUsed[5] != 0){
        NumberOfResourcesLost[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = EventDetails.ResourcesUsed[5];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[3]!=0){
        NumberOfResourcesGained[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = EventDetails.ResourceGained[3];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[4]!=0){
        NumberOfResourcesGained[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = EventDetails.ResourceGained[4];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[5]!=0){
        NumberOfResourcesGained[1] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay2[checkempty] == 0){
                ResourceDisplay2[checkempty] = EventDetails.ResourceGained[5];
                break;
            }
        }
    }

    ReferencedResourceDisplay[1] = Components.ResourceReqDisplay[1].LostDisplay[NumberOfResourcesLost[1]].GainDisplay[NumberOfResourcesGained[1]];
    ReferencedResourceDisplay[1].SetActive(true);

    for(a=0; a<ReferencedResourceDisplay[1].transform.childCount-2; a++){
        ReferencedResourceDisplay[1].transform.GetChild(a).GetComponent.<SpriteRenderer>().sprite = Components.ResourceSymbol[ResourceDisplay2[a]-1];
    }

    //Option Card 3

    if (EventDetails.ResourcesUsed[6] != 0){ 
        NumberOfResourcesLost[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = EventDetails.ResourcesUsed[6];
                break;
            }
        }
    }

    if (EventDetails.ResourcesUsed[7] != 0){
        NumberOfResourcesLost[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = EventDetails.ResourcesUsed[7];
                break;
            }
        }
    }
    if (EventDetails.ResourcesUsed[8] != 0){
        NumberOfResourcesLost[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = EventDetails.ResourcesUsed[8];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[6]!=0){
        NumberOfResourcesGained[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = EventDetails.ResourceGained[6];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[7]!=0){
        NumberOfResourcesGained[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = EventDetails.ResourceGained[7];
                break;
            }
        }
    }

    if (EventDetails.ResourceGained[8]!=0){
        NumberOfResourcesGained[2] +=1;
        for (checkempty=0; checkempty<6; checkempty++){
            if (ResourceDisplay3[checkempty] == 0){
                ResourceDisplay3[checkempty] = EventDetails.ResourceGained[8];
                break;
            }
        }
    }

    ReferencedResourceDisplay[2] = Components.ResourceReqDisplay[2].LostDisplay[NumberOfResourcesLost[2]].GainDisplay[NumberOfResourcesGained[2]];
    ReferencedResourceDisplay[2].SetActive(true);

    for(a=0; a<ReferencedResourceDisplay[2].transform.childCount-2; a++){
        ReferencedResourceDisplay[2].transform.GetChild(a).GetComponent.<SpriteRenderer>().sprite = Components.ResourceSymbol[ResourceDisplay3[a]-1];
    }


}

function ClearFadeOut(){    //For Tutorial
    yield WaitForSeconds(1);
    Scripts.MainMenuControllerScript.TutorialUse.ButtonDimScreen.SetActive(false);
}

function PlayShuffleInAnimation(CardTech: int, CardNumber:int){
    Components.CardToCallAnimationSpriteRenderer[1].sprite = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech-1].Position[CardNumber-1].SpriteGroup[3];
    Components.CardToCallAnimationText[0].text = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech-1].Position[CardNumber-1].EventCardText[0];
    Components.CardToCallAnimationText[1].text = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech-1].Position[CardNumber-1].EventCardText[1];
    Components.CardToCallAnimationText[1].text = Components.CardToCallAnimationText[1].text.Replace("NWL", "\n");
    Components.CardToCallAnimationText[2].text = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech-1].Position[CardNumber-1].EventCardText[2];
    Components.CardToCallAnimation.SetActive(true);
    yield WaitForSeconds(3);
    Components.CardToCallAnimation.SetActive(false);

    var ShownText: String = "";
    var ShownText2: String = "";

    for (var i=0; i<10; i++){
        if (Components.CardAddedNames[i] == ""){
            Components.CardAddedNames[i] = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech-1].Position[CardNumber-1].Name;
            if (i== 0) ShownText = Components.CardAddedNames[i];
            else if (i> 0 && i<3) ShownText = ShownText + "\n" + Components.CardAddedNames[i];
            else if (i== 3) ShownText2 = Components.CardAddedNames[i];
            else ShownText2 = ShownText + "\n" + Components.CardAddedNames[i];
            break;
        }

        if (i== 0) ShownText = Components.CardAddedNames[i];
        else if (i> 0 && i<3) ShownText = ShownText + "\n" + Components.CardAddedNames[i];
        else if (i== 3) ShownText2 = Components.CardAddedNames[i];
        else ShownText2 = ShownText + "\n" + Components.CardAddedNames[i];
    }
    Scripts.UIControllerScript.InformationOverviewText[8].text = ShownText;
    Scripts.UIControllerScript.InformationOverviewText[9].text = ShownText2;
}

class ScriptReference1{
    var HandCardRotationScript: HandCardRotation;
    var EventCardControlScript: EventCardControl;
    var TechnologyGaugeControllerScript: TechnologyGaugeController;
    var NatureControllerScript: NatureController;
    var HumanControllerScript: HumanController;
    var UIControllerScript: UIController;
    var ParticleEffectControllerScript: ParticleEffectController;
    var CameraControlScript: Camera_Control;
    var MiscellaneousGameManagementScript: MiscellaneousGameManagement;
    var MainMenuControllerScript: MainMenuController;
}

class FromEventControl{
    var ResourceRequirements = new float[3];
    var ResourcesUsed = new int[9];
    var TechnologyRequired = new int[3]; // Technology Requirements and other misc. Beast technology = 1,2... Human <x = -1, -2..., and Human Technology = 10, 20...
    var OptionCardImage = new SpriteRenderer[3]; // Add the 3 sprite renderers from option 1, 2 and 3
    var ResourceGained =  new int[9];
    var CardIdentifier= new int[2];
    var StatsChange = new int[6];   // 1 is health, 10 is atk, 100 is defence
    var UnlockOptionNumber = new int[3];
    var RemoveEventCheck = new Boolean[3];
    var CalledCardTechLevel= new int[3];
    var CalledCardPosition = new int[3];
    var ChanceToAddCard = new float[3];
    var EnableAnimation: Boolean;   //Card shuffle in animation
    var SpecialResourceGained: int;
    var SpriteDisplayIdentifier = new int[2];
    var DiscardCards = new int[3];
    var LoseCheck = new Boolean[3];
    var PromotionSprite = new Sprite[3];
    var PromotionText = new String[6];
    var PromotionResource = new int[3];
    var HumanRelationshipChange = new int[3];
    var FoodChange = new float[3];
    var FoodProductionChange = new int[3];
    var MoraleProductionChange = new int[3];
    var HumanTiesProductionChange = new int[3];
}

class CurrentStatusCheck{
    var CardsPlayed = new GameObject[3];
    var ResourcePlaced: float;
    var ResourceRequired: float; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction, Resource Required = sum of total
    var OtherRequirement : int;
    var MeetOtherRequirement: boolean;
    var SkillsPossessed = new int[10];
}

class ComponentReference1{
    var ResourceCardPrefabs = new GameObject[5]; // 0 is life, 1 is resource, 2 is faith, 3 is humans, 4 is destruction.
    var ResourceSymbol = new Sprite [10];
    var ResourceName = new String [5];
    var ResourceQuote = new String [5];
    var QuotePosition = new Vector3[5];
    var QuoteAuthor = new String[5];
    var ResourceReqDisplay = new ResourceDisplay[3];
    var HandCardRotationTransform: Transform; 
    var TrapDoorGameObject: GameObject;
    var AnimationStatsChange = new GameObject[4];
    var CardToCallAnimation: GameObject;
    var CardToCallAnimationSpriteRenderer = new SpriteRenderer[2];
    var CardToCallAnimationText = new UnityEngine.UI.Text[2];
    var PromotionScene: GameObject;
    var PromotionCard: SpriteRenderer;
    var PromotionTextRenderer = new UnityEngine.UI.Text[2];
    var PromotionFinishCheck: Boolean;
    var NatureAvatarAnim: Animation;
    var CardAddedNames = new String[10];
}

class ResourceDisplay{
    var LostDisplay = new ResourceLostDisplay[4];
}

class ResourceLostDisplay{
    var GainDisplay = new GameObject[4];
}