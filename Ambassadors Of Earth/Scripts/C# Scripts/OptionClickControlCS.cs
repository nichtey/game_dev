using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionClickControlCS : MonoBehaviour {

    public CameraControlCS.ScriptReference Scripts;
    public ComponentReference1 Components;
    public FromEventControl EventDetails;
    public CurrentStatusCheck CurrentStatus;

    void Start()
    {
        Components.ResourceQuote[3] = "“It is as inhuman to be totally good\nas it is to be totally evil.”";
        Components.ResourceQuote[4] = "“A nation that destroys its soils\ndestroys itself.”";
    }

    void Update () {
		
	}

    private int OptionCardNumber;
    private Vector3 Scale;
    private Vector3 Pos;
    private int NumberOfResourceCardsToGive;
    private int[] RandomlyDestroyedCardNumbers = new int[3];
    public GameObject[] InstantiatedCards = new GameObject[3];
    private bool CombatVictory = false;
    private string AnnouncerString;

    public IEnumerator CheckMeetRequirements(GameObject Card1, GameObject Card2, GameObject Card3, int OptionCardSelected)
    {
        OptionCardNumber = OptionCardSelected;
        CurrentStatus.MeetOtherRequirement = false;
        CurrentStatus.ResourceRequired = EventDetails.ResourceRequirements[OptionCardSelected - 1];
        CurrentStatus.OtherRequirement = EventDetails.TechnologyRequired[OptionCardSelected - 1];
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

        if (CurrentStatus.CardsPlayed[0] != null) CurrentStatus.ResourcePlaced += CurrentStatus.CardsPlayed[0].GetComponent<ResourceCardControlCS>().ResourceType; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction 
        if (CurrentStatus.CardsPlayed[1] != null) CurrentStatus.ResourcePlaced += CurrentStatus.CardsPlayed[1].GetComponent<ResourceCardControlCS>().ResourceType; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction 
        if (CurrentStatus.CardsPlayed[2] != null) CurrentStatus.ResourcePlaced += CurrentStatus.CardsPlayed[2].GetComponent<ResourceCardControlCS>().ResourceType; // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction 

        if (CurrentStatus.OtherRequirement == 0) { CurrentStatus.MeetOtherRequirement = true; }

        else if (CurrentStatus.OtherRequirement > 0 && CurrentStatus.OtherRequirement < 5)
        {
            for (var i = 0; i < 10; i++)
            {
                if (CurrentStatus.SkillsPossessed[i] == CurrentStatus.OtherRequirement)
                {
                    CurrentStatus.MeetOtherRequirement = true;
                    break;
                }

                if (CurrentStatus.SkillsPossessed[i] == 0)
                {
                    Scripts.UIControllerScript.Announcement("You don't possess the skill required to select this option.");
                    yield break;
                }
            }
        }

        else if (CurrentStatus.OtherRequirement > 100)
        {
            if (Scripts.EventCardControlScript.Important.HumanRelationship + 100 >= CurrentStatus.OtherRequirement)
            {
                CurrentStatus.MeetOtherRequirement = true;
            }

            else
            {
                Scripts.UIControllerScript.Announcement("Your cannot select this option because your human ties is too low.");
                yield break;
            }
        }

        else if (CurrentStatus.OtherRequirement < -100)
        {
            if (Scripts.EventCardControlScript.Important.HumanRelationship - 100 >= CurrentStatus.OtherRequirement)
            {
                CurrentStatus.MeetOtherRequirement = true;
            }

            else
            {
                Scripts.UIControllerScript.Announcement("Your cannot select this option because your human ties is too low.");
                yield break;
            }
        }

        if (EventDetails.Combat.WarCheck[OptionCardSelected-1] == true)
        {
            if (EventDetails.Combat.EnemyDefence[OptionCardSelected - 1] != 0)
            {
                if (Scripts.NatureControllerScript.OffenseLeft == 0)
                {
                    Scripts.UIControllerScript.Announcement("You need at least 1 offensive strength to launch an attack.");
                    yield break;
                }

                else
                {
                    if (Scripts.NatureControllerScript.OffenseLeft <= EventDetails.Combat.EnemyDefence[OptionCardSelected - 1])
                    {
                        //Defeat
                        CombatVictory = false;
                        Components.CombatStuff.VictorySprite.SetActive(false);
                        Components.CombatStuff.DefeatSprite.SetActive(true);
                        Components.CombatStuff.Result.text = "Defeat!";
                        Components.CombatStuff.CombatDetails.text = "Enemy Defence: " + EventDetails.Combat.EnemyDefence[OptionCardSelected - 1] + "\n" + "Our Offense: " + Scripts.NatureControllerScript.OffenseLeft;
                        Components.CombatStuff.WarEffects.text = "Gain 1 Chaos\nResource Card.";
                        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume / 4;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[13]);
                        Components.CombatStuff.BackgroundCombatScene.sprite = Components.CombatStuff.Background[1];
                        Components.CombatStuff.CombatQuote.text = EventDetails.Combat.CombatQuote[OptionCardSelected+2];
                        Components.CombatStuff.CombatQuote.text = Components.CombatStuff.CombatQuote.text.Replace("NWL", "\n");
                    }

                    else if (Scripts.NatureControllerScript.OffenseLeft > EventDetails.Combat.EnemyDefence[OptionCardSelected - 1])
                    {
                        //Victory
                        CombatVictory = true;
                        Components.CombatStuff.DefeatSprite.SetActive(false);
                        Components.CombatStuff.VictorySprite.SetActive(true);
                        Components.CombatStuff.Result.text = "Victory!";
                        Components.CombatStuff.CombatDetails.text = "Enemy Defence: " + EventDetails.Combat.EnemyDefence[OptionCardSelected - 1] + "\n" + "Our Offense: " + Scripts.NatureControllerScript.OffenseLeft;
                        Components.CombatStuff.WarEffects.text = EventDetails.Combat.WarVictory[OptionCardSelected - 1];
                        Components.CombatStuff.WarEffects.text = Components.CombatStuff.WarEffects.text.Replace("NWL", "\n");
                        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume / 4;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[12]);
                        Components.CombatStuff.BackgroundCombatScene.sprite = Components.CombatStuff.Background[0];
                        Components.CombatStuff.CombatQuote.text = EventDetails.Combat.CombatQuote[OptionCardSelected-1];
                        Components.CombatStuff.CombatQuote.text = Components.CombatStuff.CombatQuote.text.Replace("NWL", "\n");
                    }

                    Scripts.NatureControllerScript.OffenseLeft -= EventDetails.Combat.EnemyDefence[OptionCardSelected - 1];
                    if (Scripts.NatureControllerScript.OffenseLeft < 0) Scripts.NatureControllerScript.OffenseLeft = 0;
                    UpdateCombatStats(Scripts.NatureControllerScript.OffenseLeft, Scripts.NatureControllerScript.DefenceLeft);
                }
            }

            else if (EventDetails.Combat.EnemyOffense[OptionCardSelected - 1] != 0)
            {
                if (Scripts.NatureControllerScript.DefenceLeft == 0)
                {
                    Scripts.UIControllerScript.Announcement("You need at least 1 defensive strength to mount a defence.");
                    yield break;
                }

                else
                {

                    if (Scripts.NatureControllerScript.DefenceLeft < EventDetails.Combat.EnemyOffense[OptionCardSelected - 1])
                    {
                        //Defeat
                        CombatVictory = false;
                        Components.CombatStuff.VictorySprite.SetActive(false);
                        Components.CombatStuff.DefeatSprite.SetActive(true);
                        Components.CombatStuff.Result.text = "Defeat!";
                        Components.CombatStuff.CombatDetails.text = "Enemy Offense: " + EventDetails.Combat.EnemyOffense[OptionCardSelected - 1] + "\n" + "Our Defence: " + Scripts.NatureControllerScript.DefenceLeft;
                        Components.CombatStuff.WarEffects.text = "Gain 1 Chaos\nResource Card.";
                        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume / 4;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[13]);
                        Components.CombatStuff.BackgroundCombatScene.sprite = Components.CombatStuff.Background[1];
                        Components.CombatStuff.CombatQuote.text = EventDetails.Combat.CombatQuote[OptionCardSelected + 2];
                        Components.CombatStuff.CombatQuote.text = Components.CombatStuff.CombatQuote.text.Replace("NWL", "\n");
                    }

                    else if (Scripts.NatureControllerScript.DefenceLeft > EventDetails.Combat.EnemyOffense[OptionCardSelected - 1])
                    {
                        //Victory
                        CombatVictory = true;
                        Components.CombatStuff.DefeatSprite.SetActive(false);
                        Components.CombatStuff.VictorySprite.SetActive(true);
                        Components.CombatStuff.Result.text = "Victory!";
                        Components.CombatStuff.CombatDetails.text = "Enemy Offense: " + EventDetails.Combat.EnemyOffense[OptionCardSelected - 1] + "\n" + "Our Defence: " + Scripts.NatureControllerScript.DefenceLeft;
                        Components.CombatStuff.WarEffects.text = EventDetails.Combat.WarVictory[OptionCardSelected - 1];
                        Components.CombatStuff.WarEffects.text = Components.CombatStuff.WarEffects.text.Replace("NWL", "\n");
                        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume / 4;
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[12]);
                        Components.CombatStuff.BackgroundCombatScene.sprite = Components.CombatStuff.Background[0];
                        Components.CombatStuff.CombatQuote.text = EventDetails.Combat.CombatQuote[OptionCardSelected - 1];
                        Components.CombatStuff.CombatQuote.text = Components.CombatStuff.CombatQuote.text.Replace("NWL", "\n");
                    }

                    Scripts.NatureControllerScript.DefenceLeft -= EventDetails.Combat.EnemyOffense[OptionCardSelected - 1];
                    if (Scripts.NatureControllerScript.DefenceLeft < 0) Scripts.NatureControllerScript.DefenceLeft = 0;
                    UpdateCombatStats(Scripts.NatureControllerScript.OffenseLeft, Scripts.NatureControllerScript.DefenceLeft);

                }
            }

            Components.CombatStuff.CombatFinishCheck = false;
            StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0));
            yield return new WaitForSeconds(0.8f);
            Components.CombatStuff.CombatScene.SetActive(true);
            Pos = Camera.main.transform.position;
            Pos.z = 62.3f;
            Camera.main.transform.position = Pos;
            yield return new WaitForSeconds(1.05f);
            Components.CombatStuff.CombatFinishCheck = true;
            yield break;
        }

        if (EventDetails.FoodChange[OptionCardSelected - 1] < 0)
        {
            if (Scripts.NatureControllerScript.CurrentFood < -EventDetails.FoodChange[OptionCardSelected - 1])
            {
                Scripts.UIControllerScript.Announcement("You do not have enough food to select this option.");
                yield break;
            }
        }

        if (EventDetails.DiscardCards[OptionCardSelected - 1] != 0)
        {
            if (Scripts.HandCardRotationScript.Important.NumberOfCards < EventDetails.DiscardCards[OptionCardSelected - 1])
            {
                Scripts.UIControllerScript.Announcement("You do not have enough discardable resource cards.");
                yield break;
            }
        }

        if (Scripts.MainMenuControllerScript.LevelSelected == 1)
        {
            if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22)
            {

                if (OptionCardSelected == 1)
                {
                    Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                    Scale.x = 0.04f;
                    Scale.y = 0.04f;
                    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "After tapping on option 1,\nhold it there a little while.";
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                    Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
                    Pos.x = 20;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                }

                if (OptionCardSelected != 1)
                {
                    Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                    Scale.x = 0.04f;
                    Scale.y = 0.04f;
                    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Wrong option selected!\n(Tap and hold option 1)";
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                    Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
                    Pos.x = 20;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                }

                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                yield break;
            }
        }

        if (CurrentStatus.ResourcePlaced != CurrentStatus.ResourceRequired)
        {
            Scripts.UIControllerScript.Announcement("Please ensure that the correct resource cards are placed.");
            yield break;
        }

        if (CurrentStatus.MeetOtherRequirement == false)
        {
            Scripts.UIControllerScript.Announcement("You don't meet this event card's requirements.");
            yield break;
        }

        if (EventDetails.SpecialCardNumber == 1)
        {

            for (int CheckEmpty = 0; CheckEmpty < Scripts.EventCardControlScript.Important.CardsToDestroy.Length; CheckEmpty++)
            {
                if (Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].TechLevel == 0)
                {
                    Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].TechLevel = EventDetails.CardIdentifier[0];
                    Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].Position = EventDetails.CardIdentifier[1];
                    break;
                }
            }

            if (OptionCardSelected == 1)
            {
                Scripts.NatureControllerScript.Offense = 1;
                Scripts.NatureControllerScript.Defence = 5;
                Scripts.EventCardControlScript.Important.FoodConsumption += 1;
                AnnouncerString = "Limited defensive army established.";
            }

            else if (OptionCardSelected == 2)
            {
                Scripts.NatureControllerScript.Offense = 5;
                Scripts.NatureControllerScript.Defence = 5;
                Scripts.EventCardControlScript.Important.FoodConsumption += 10;
                AnnouncerString = "Standard standing army established.";
            }

            else if (OptionCardSelected == 3)
            {
                Scripts.NatureControllerScript.Offense = 6;
                Scripts.NatureControllerScript.Defence = 8;
                Scripts.EventCardControlScript.Important.FoodConsumption += 20;
                AnnouncerString = "Expanded conscript army established.";
            }

            Scripts.NatureControllerScript.OffenseLeft = Scripts.NatureControllerScript.Offense;
            Scripts.NatureControllerScript.DefenceLeft = Scripts.NatureControllerScript.Defence;
            Scripts.HandCardRotationScript.Important.NoClicking = true;

            Components.CombatStuff.CombatStats.SetActive(true);
            Components.CombatStuff.ShowCombatStats.Play();
            Components.CombatStuff.Attack.text = Scripts.NatureControllerScript.OffenseLeft.ToString();
            Components.CombatStuff.Defence.text = Scripts.NatureControllerScript.DefenceLeft.ToString();
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[10]);
            yield return new WaitForSeconds(0.8f);
            Scripts.UIControllerScript.Announcement(AnnouncerString);
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[11]);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Scripts.EventCardControlScript.EventCardRemoved());
            yield break;
        }

        StartCoroutine(SelectionSuccess(OptionCardSelected));
    }

    public IEnumerator SelectionSuccess(int OptionCardSelected)
    {
        if (CurrentStatus.ResourcePlaced == CurrentStatus.ResourceRequired && CurrentStatus.MeetOtherRequirement == true)
        {
            //Debug.Log("SUCCESS");

            Scripts.HandCardRotationScript.Important.NoClicking = true;

            if (Scripts.MainMenuControllerScript.LevelSelected == 1)
            {
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("FadeCallOut");
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[11].Play("DimScreenFadeOut");
                StartCoroutine(ClearFadeOut());
            }

            if (EventDetails.LoseCheck[OptionCardSelected - 1] == true)
            {
                StartCoroutine(Scripts.CameraControlScript.LoseGame("Defeated", "Avoid being forced to choose such\noptions by making neccessary prep.", 0));
                yield break;
            }

            if (EventDetails.MilitaryChange.OffenseChange[OptionCardSelected - 1] != 0)
            {
                if (EventDetails.MilitaryChange.OffenseChange[OptionCardSelected - 1] > 0)
                {
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[11]);
                    yield return new WaitForSeconds(0.5f);
                    Scripts.NatureControllerScript.TempUpdateOffense(EventDetails.MilitaryChange.OffenseChange[OptionCardSelected - 1]);
                }

                else if (EventDetails.MilitaryChange.OffenseChange[OptionCardSelected - 1] < 0)
                {
                    if (-EventDetails.MilitaryChange.OffenseChange[OptionCardSelected - 1] > Scripts.NatureControllerScript.OffenseLeft)
                    {
                        Scripts.UIControllerScript.Announcement("You offensive strength is too low to select this option.");
                        Scripts.HandCardRotationScript.Important.NoClicking = false;
                        yield break;
                    }

                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield return new WaitForSeconds(0.5f);
                    Scripts.NatureControllerScript.TempUpdateDefence(EventDetails.MilitaryChange.OffenseChange[OptionCardSelected - 1]);
                }
            }

            if (EventDetails.MilitaryChange.DefenceChange[OptionCardSelected - 1] != 0)
            { 
                if (EventDetails.MilitaryChange.DefenceChange[OptionCardSelected - 1] > 0)
                {
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[11]);
                    yield return new WaitForSeconds(0.5f);
                    Scripts.NatureControllerScript.TempUpdateDefence(EventDetails.MilitaryChange.DefenceChange[OptionCardSelected - 1]);
                }

                else if (EventDetails.MilitaryChange.DefenceChange[OptionCardSelected - 1] < 0)
                {
                    if (-EventDetails.MilitaryChange.DefenceChange[OptionCardSelected - 1] > Scripts.NatureControllerScript.DefenceLeft)
                    {
                        Scripts.UIControllerScript.Announcement("Your defensive strength is too low to select this option.");
                        Scripts.HandCardRotationScript.Important.NoClicking = false;
                        yield break;
                    }

                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield return new WaitForSeconds(0.5f);
                    Scripts.NatureControllerScript.TempUpdateDefence(EventDetails.MilitaryChange.DefenceChange[OptionCardSelected - 1]);
                }
            }

            if (EventDetails.MilitaryChange.PermDefenceChange[OptionCardSelected - 1] != 0)
            {
                Scripts.NatureControllerScript.Defence += EventDetails.MilitaryChange.PermDefenceChange[OptionCardSelected - 1];
            }

            if (EventDetails.MilitaryChange.PermOffenseChange[OptionCardSelected - 1] != 0)
            {
                Scripts.NatureControllerScript.Offense += EventDetails.MilitaryChange.PermOffenseChange[OptionCardSelected - 1];
            }

            if (EventDetails.FoodProductionChange[OptionCardSelected - 1] != 0)
            {
                Scripts.EventCardControlScript.Important.FoodProduction += EventDetails.FoodProductionChange[OptionCardSelected - 1];
            }

            if (EventDetails.MoraleProductionChange[OptionCardSelected - 1] != 0)
            {
                Scripts.EventCardControlScript.Important.Morale += EventDetails.MoraleProductionChange[OptionCardSelected - 1];
            }

            if (EventDetails.HumanTiesProductionChange[OptionCardSelected - 1] != 0)
            {
                Scripts.EventCardControlScript.Important.HumanTies += EventDetails.HumanTiesProductionChange[OptionCardSelected - 1];
            }

            if (OptionCardSelected == 1)
            {
                if (EventDetails.ResourcesUsed[0] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[0] - 1] -= 1;
                if (EventDetails.ResourcesUsed[1] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[1] - 1] -= 1;
                if (EventDetails.ResourcesUsed[2] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[2] - 1] -= 1;
            }

            if (OptionCardSelected == 2)
            {
                if (EventDetails.ResourcesUsed[3] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[3] - 1] -= 1;
                if (EventDetails.ResourcesUsed[4] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[4] - 1] -= 1;
                if (EventDetails.ResourcesUsed[5] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[5] - 1] -= 1;
            }

            if (OptionCardSelected == 3)
            {
                if (EventDetails.ResourcesUsed[6] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[6] - 1] -= 1;
                if (EventDetails.ResourcesUsed[7] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[7] - 1] -= 1;
                if (EventDetails.ResourcesUsed[8] != 0) Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[EventDetails.ResourcesUsed[8] - 1] -= 1;
            }

            if (EventDetails.StatsChange[OptionCardSelected - 1] != 0)
            {
                Scripts.NatureControllerScript.UpdateMorale(EventDetails.StatsChange[OptionCardSelected - 1]);
                if (EventDetails.StatsChange[OptionCardSelected - 1] < 0)
                {
                    Components.NatureAvatarAnim.Play("LoseMorale");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield return new WaitForSeconds(0.67f);
                }

                else if (EventDetails.StatsChange[OptionCardSelected - 1] > 0)
                {
                    Components.NatureAvatarAnim.Play("RecoverMorale");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                    yield return new WaitForSeconds(0.75f);
                }
            }

            if (EventDetails.StatsChange[OptionCardSelected + 2] != 0)
            {
                Scripts.HumanControllerScript.UpdateMorale(EventDetails.StatsChange[OptionCardSelected + 2]);
                if (EventDetails.StatsChange[OptionCardSelected + 2] < 0)
                {
                    Components.HumanAvatarAnim.Play("LoseMoraleHuman");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield return new WaitForSeconds(0.67f);
                }

                else if (EventDetails.StatsChange[OptionCardSelected + 2] > 0)
                {
                    Components.HumanAvatarAnim.Play("RecoverMoraleHuman");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                    yield return new WaitForSeconds(0.67f);
                }
            }

            if (EventDetails.FoodChange[OptionCardSelected - 1] != 0)
            {
                Scripts.NatureControllerScript.IncreaseNatureFood(EventDetails.FoodChange[OptionCardSelected - 1] / 100);

                if (EventDetails.FoodChange[OptionCardSelected - 1] < 0)
                {
                    Components.NatureAvatarAnim.Play("LoseFood");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield return new WaitForSeconds(0.67f);
                }

                if (EventDetails.FoodChange[OptionCardSelected - 1] > 0)
                {
                    Components.NatureAvatarAnim.Play("RecoverFood");
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                    yield return new WaitForSeconds(0.75f);
                }
            }

            if (EventDetails.HumanRelationshipChange[OptionCardSelected - 1] != 0)
            {
                Scripts.EventCardControlScript.UpdateHumanRelationship(EventDetails.HumanRelationshipChange[OptionCardSelected - 1]);

                if (EventDetails.HumanRelationshipChange[OptionCardSelected - 1] < 0)
                {
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[7]);
                    yield return new WaitForSeconds(0.67f);
                }

                if (EventDetails.HumanRelationshipChange[OptionCardSelected - 1] > 0)
                {
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[8]);
                    yield return new WaitForSeconds(0.75f);
                }
            }

            if (EventDetails.RemoveEventCheck[OptionCardSelected - 1] == true)
            {
                for (var CheckEmpty = 0; CheckEmpty < Scripts.EventCardControlScript.Important.CardsToDestroy.Length; CheckEmpty++)
                {
                    if (Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].TechLevel == 0)
                    {
                        Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].TechLevel = EventDetails.CardIdentifier[0];
                        Scripts.EventCardControlScript.Important.CardsToDestroy[CheckEmpty].Position = EventDetails.CardIdentifier[1];
                        if (Scripts.EventCardControlScript.Components.OptionCardEffect[OptionCardSelected - 1].text == "Remove this event" || Components.ResourceReqDisplay[OptionCardSelected-1].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text == "Remove This Event" + "\n")
                        {
                            Scripts.EventCardControlScript.Important.DestroyEventCheck = true;
                        }
                        break;
                    }
                }
            }

            if (EventDetails.CalledCardTechLevel[OptionCardSelected - 1] == -1)
            {
                Scripts.EventCardControlScript.Important.SpecialCardActivated = 1;
            }

            if (CurrentStatus.CardsPlayed[0] != null)
            {

                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[4]);
                Scripts.UIControllerScript.AnimateSpriteCheck = 0;
                Scripts.UIControllerScript.DoneShaking = false;
                Scripts.UIControllerScript.Announcer.SetActive(false);
                CurrentStatus.CardsPlayed[0].tag = "DestroyedCards";
                if (CurrentStatus.CardsPlayed[1] == null) Scripts.ParticleEffectControllerScript.DestroyCards();
                StartCoroutine(FadeOutPlayedCards(0));

                if (CurrentStatus.CardsPlayed[1] != null)
                {
                    CurrentStatus.CardsPlayed[1].tag = "DestroyedCards";
                    if (CurrentStatus.CardsPlayed[2] == null) Scripts.ParticleEffectControllerScript.Destroy2Cards();
                    StartCoroutine(FadeOutPlayedCards(1));

                    if (CurrentStatus.CardsPlayed[2] != null)
                    {
                        CurrentStatus.CardsPlayed[2].tag = "DestroyedCards";
                        Scripts.ParticleEffectControllerScript.Destroy3Cards();
                        StartCoroutine(FadeOutPlayedCards(2));
                    }
                }
                yield return new WaitForSeconds(1);
            }

            NumberOfResourceCardsToGive = 0;
            int ResourceTypeTemp;

            if (OptionCardSelected == 1)
            {

                for (int i = 0; i < 3; i++)
                {

                    if (EventDetails.ResourceGained[i] >= 6)
                    {
                        EventDetails.SpecialResourceGained = 1;
                        EventDetails.PromotionResource[0] = EventDetails.ResourceGained[i] - 5;
                    }

                    if (EventDetails.ResourceGained[i] != 0 && EventDetails.ResourceGained[i] < 6)
                    {
                        NumberOfResourceCardsToGive += 1;
                        ResourceTypeTemp = EventDetails.ResourceGained[i] - 1;
                        InstantiatedCards[i] = Instantiate(Components.ResourceCardPrefabs[ResourceTypeTemp]);
                        StartCoroutine(FadeInGivenCards(i));
                    }
                }

                if (NumberOfResourceCardsToGive == 1)
                {
                    InstantiatedCards[0].transform.position = new Vector3(-0.63f, 0, 4.3f);
                }

                if (NumberOfResourceCardsToGive == 2)
                {
                    InstantiatedCards[0].transform.position = new Vector3(0.62f, 0, 4.3f);
                    InstantiatedCards[1].transform.position = new Vector3(-1.88f, 0, 4.3f);
                }

                if (NumberOfResourceCardsToGive == 3)
                {
                    InstantiatedCards[0].transform.position = new Vector3(1.87f, 0, 4.3f);
                    InstantiatedCards[1].transform.position = new Vector3(-0.63f, 0, 4.3f);
                    InstantiatedCards[2].transform.position = new Vector3(-3.13f, 0, 4.3f);
                }
            }

            else if (OptionCardSelected == 2)
            {
                for (int j = 3; j < 6; j++)
                {

                    if (EventDetails.ResourceGained[j] >= 6)
                    {
                        EventDetails.SpecialResourceGained = 1;
                        EventDetails.PromotionResource[1] = EventDetails.ResourceGained[j] - 5;
                    }

                    if (EventDetails.ResourceGained[j] != 0 && EventDetails.ResourceGained[j] < 6)
                    {
                        NumberOfResourceCardsToGive += 1;
                        ResourceTypeTemp = EventDetails.ResourceGained[j] - 1;
                        InstantiatedCards[j - 3] = Instantiate(Components.ResourceCardPrefabs[ResourceTypeTemp]);
                        StartCoroutine(FadeInGivenCards(j - 3));
                    }
                }

                if (NumberOfResourceCardsToGive == 1)
                {
                    InstantiatedCards[0].transform.position = new Vector3(-0.63f, 0, 4.3f);
                }

                if (NumberOfResourceCardsToGive == 2)
                {
                    InstantiatedCards[0].transform.position = new Vector3(0.62f, 0, 4.3f);
                    InstantiatedCards[1].transform.position = new Vector3(-1.88f, 0, 4.3f);
                }

                if (NumberOfResourceCardsToGive == 3)
                {
                    InstantiatedCards[0].transform.position = new Vector3(1.87f, 0, 4.3f);
                    InstantiatedCards[1].transform.position = new Vector3(-0.63f, 0, 4.3f);
                    InstantiatedCards[2].transform.position = new Vector3(-3.13f, 0, 4.3f);
                }
            }

            else if (OptionCardSelected == 3)
            {

                for (int k = 6; k < 9; k++)
                {

                    if (EventDetails.ResourceGained[k] >= 6)
                    {
                        EventDetails.SpecialResourceGained = 1;
                        EventDetails.PromotionResource[2] = EventDetails.ResourceGained[k] - 5;
                    }

                    if (EventDetails.ResourceGained[k] != 0 && EventDetails.ResourceGained[k] < 6)
                    {
                        NumberOfResourceCardsToGive += 1;
                        ResourceTypeTemp = EventDetails.ResourceGained[k] - 1;
                        InstantiatedCards[k - 6] = Instantiate(Components.ResourceCardPrefabs[ResourceTypeTemp]); // Array for Resource Gained 1 more than resource card prefabs
                        StartCoroutine(FadeInGivenCards(k - 6));
                    }
                }

                if (NumberOfResourceCardsToGive == 1)
                {
                    InstantiatedCards[0].transform.position = new Vector3(-0.63f, 0, 4.3f);
                }

                if (NumberOfResourceCardsToGive == 2)
                {
                    InstantiatedCards[0].transform.position = new Vector3(0.62f, 0, 4.3f);
                    InstantiatedCards[1].transform.position = new Vector3(-1.88f, 0, 4.3f);
                }

                if (NumberOfResourceCardsToGive == 3)
                {
                    InstantiatedCards[0].transform.position = new Vector3(1.87f, 0, 4.3f);
                    InstantiatedCards[1].transform.position = new Vector3(-0.63f, 0, 4.3f);
                    InstantiatedCards[2].transform.position = new Vector3(-3.13f, 0, 4.3f);
                }
            }

            if (NumberOfResourceCardsToGive != 0)
            {
                if (Scripts.MiscellaneousGameManagementScript.CurrentSoundEffect != Scripts.MiscellaneousGameManagementScript.SoundEffectClips[4])
                {
                    Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[5]);
                }
                yield return new WaitForSeconds(1);
            }

            if (InstantiatedCards[0] != null)
            {
                InstantiatedCards[0].transform.parent = Components.HandCardRotationTransform;
                float ResourceType = InstantiatedCards[0].GetComponent<ResourceCardControlCS>().ResourceType;

                if (ResourceType == 100)
                {
                    InstantiatedCards[0].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] += 1;
                }

                else if (ResourceType == 10)
                {
                    InstantiatedCards[0].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3] += 1;
                }

                else if (ResourceType == 1)
                {
                    InstantiatedCards[0].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[2] += 1;
                }

                else if (ResourceType == 0.1f)
                {
                    InstantiatedCards[0].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] + Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1] += 1;
                }

                else if (ResourceType == 0.01f)
                {
                    InstantiatedCards[0].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]);
                    Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] += 1;
                }

                Scripts.HandCardRotationScript.Important.NumberOfCards += 1;
                InstantiatedCards[0].GetComponent<ResourceCardControlCS>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards + 1;

                if (InstantiatedCards[1] != null)
                {
                    ResourceType = InstantiatedCards[1].GetComponent<ResourceCardControlCS>().ResourceType;
                    InstantiatedCards[1].transform.parent = Components.HandCardRotationTransform;

                    if (ResourceType == 100)
                    {
                        InstantiatedCards[1].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] += 1;
                    }

                    else if (ResourceType == 10)
                    {
                        InstantiatedCards[1].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3] += 1;
                    }

                    else if (ResourceType == 1)
                    {
                        InstantiatedCards[1].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[2] += 1;
                    }

                    else if (ResourceType == 0.1f)
                    {
                        InstantiatedCards[1].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] + Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1] += 1;
                    }

                    else if (ResourceType == 0.01f)
                    {
                        InstantiatedCards[1].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]);
                        Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] += 1;
                    }

                    Scripts.HandCardRotationScript.Important.NumberOfCards += 1;
                    InstantiatedCards[1].GetComponent<ResourceCardControlCS>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards + 2;

                    if (InstantiatedCards[2] != null)
                    {
                        ResourceType = InstantiatedCards[2].GetComponent<ResourceCardControlCS>().ResourceType;
                        InstantiatedCards[2].transform.parent = Components.HandCardRotationTransform;

                        if (ResourceType == 100)
                        {
                            InstantiatedCards[2].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards);
                            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] += 1;
                        }

                        else if (ResourceType == 10)
                        {
                            InstantiatedCards[2].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4]);
                            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3] += 1;
                        }

                        else if (ResourceType == 1)
                        {
                            InstantiatedCards[2].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] - Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[3]);
                            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[2] += 1;
                        }

                        else if (ResourceType == 0.1f)
                        {
                            InstantiatedCards[2].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] + Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1]);
                            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1] += 1;
                        }

                        else if (ResourceType == 0.01f)
                        {
                            InstantiatedCards[2].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0]);
                            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] += 1;
                        }

                        Scripts.HandCardRotationScript.Important.NumberOfCards += 1;
                        InstantiatedCards[2].GetComponent<ResourceCardControlCS>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards + 3;
                    }
                }
                Scripts.HandCardRotationScript.UpdateNumberOfCards();
            }

            if (EventDetails.ChanceToAddCard[OptionCardSelected - 1] != 0)
            {
                float RandomRoll;
                RandomRoll = Random.Range(0.0f, 1.0f);

                if (RandomRoll > EventDetails.ChanceToAddCard[OptionCardSelected - 1])  //Do not add the card
                {
                    EventDetails.CalledCardTechLevel[OptionCardSelected - 1] = 0;
                }
            }

            if (EventDetails.CalledCardTechLevel[OptionCardSelected - 1] > 0)
            {
                if (InstantiatedCards[0] != null)
                {
                    yield return new WaitForSeconds(0.8f);
                }

                for (int CheckEmpty = 0; CheckEmpty < Scripts.EventCardControlScript.Important.CardsToAddIn.Length; CheckEmpty++)
                {
                    if (Scripts.EventCardControlScript.Important.CardsToAddIn[CheckEmpty].TechLevel == 0)
                    {
                        Scripts.EventCardControlScript.Important.CardsToAddIn[CheckEmpty].TechLevel = EventDetails.CalledCardTechLevel[OptionCardSelected - 1];
                        Scripts.EventCardControlScript.Important.CardsToAddIn[CheckEmpty].Position = EventDetails.CalledCardPosition[OptionCardSelected - 1];
                        if (EventDetails.EnableAnimation == true)
                        {
                            StartCoroutine(PlayShuffleInAnimation(EventDetails.CalledCardTechLevel[OptionCardSelected - 1], EventDetails.CalledCardPosition[OptionCardSelected - 1]));
                            yield return new WaitForSeconds(0.8f);
                        }

                        break;
                    }
                }
            }

            if (EventDetails.PromotionResource[OptionCardSelected - 1] != 0)
            {

                Components.PromotionFinishCheck = false;
                StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0));
                yield return new WaitForSeconds(0.8f);
                Components.PromotionScene.SetActive(true);
                Components.PromotionCard.sprite = EventDetails.PromotionSprite[OptionCardSelected - 1];
                Components.PromotionTextRenderer[0].text = EventDetails.PromotionText[OptionCardSelected - 1];
                Components.PromotionTextRenderer[0].text = Components.PromotionTextRenderer[0].text.Replace("NWL", "\n");
                Components.PromotionTextRenderer[1].text = EventDetails.PromotionText[3 + OptionCardSelected - 1];
                Components.PromotionTextRenderer[1].text = Components.PromotionTextRenderer[1].text.Replace("NWL", "\n");
                Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume / 4;
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[3]);
                Pos = Camera.main.transform.position;
                Pos.z = 62.3f;
                Camera.main.transform.position = Pos;

                for (int i = 0; i < 10; i++)
                {
                    if (CurrentStatus.SkillsPossessed[i] == 0)
                    {
                        CurrentStatus.SkillsPossessed[i] = EventDetails.PromotionResource[OptionCardSelected - 1];
                        break;
                    }
                }

                for (int i = 0; i < 6; i++)
                {
                    if (Scripts.UIControllerScript.InformationSkillSpriteHolders[i].enabled == false)
                    {
                        Scripts.UIControllerScript.InformationSkillSpriteHolders[i].sprite = Components.ResourceSymbol[EventDetails.PromotionResource[OptionCardSelected - 1] + 4];
                        Scripts.UIControllerScript.InformationSkillSpriteHolders[i].enabled = true;
                        Scripts.UIControllerScript.InformationSkillDesign[i].color = Color.white;
                        break;
                    }
                }

                yield return new WaitForSeconds(1.05f);
                Components.PromotionFinishCheck = true;
                yield break;
            }

            if (EventDetails.UnlockOptionNumber[OptionCardSelected - 1] != 0)
            {
                yield return new WaitForSeconds(0.6f);
                var ReferencedPath = Scripts.EventCardControlScript.EventDisplay.Normal[EventDetails.SpriteDisplayIdentifier[0] - 1].Position[EventDetails.SpriteDisplayIdentifier[1] - 1];
                ReferencedPath.UnlockOptionNumber[EventDetails.UnlockOptionNumber[OptionCardSelected - 1] - 1] = 0;
                Scripts.EventCardControlScript.Components.OptionCardRenderers[2].sprite = ReferencedPath.SpriteGroup[2];
                Scripts.ParticleEffectControllerScript.UnlockedCard();
                yield return new WaitForSeconds(1.85f);
            }

            if (EventDetails.DiscardCards[OptionCardSelected - 1] != 0)
            {

                RandomlyDestroyedCardNumbers[0] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards); //random.range not inclusive of max integer
                RandomlyDestroyedCardNumbers[1] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);
                RandomlyDestroyedCardNumbers[2] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);

                while (RandomlyDestroyedCardNumbers[1] == RandomlyDestroyedCardNumbers[0]) RandomlyDestroyedCardNumbers[1] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);
                while (RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[0] || RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[1]) RandomlyDestroyedCardNumbers[2] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);

                if (EventDetails.DiscardCards[OptionCardSelected - 1] == 1)
                {
                    RandomlyDestroyedCardNumbers[1] = -1;
                    RandomlyDestroyedCardNumbers[2] = -1;
                }

                if (EventDetails.DiscardCards[OptionCardSelected - 1] == 2)
                {
                    RandomlyDestroyedCardNumbers[2] = -1;
                }

                StartCoroutine(Scripts.HandCardRotationScript.DiscardCards(RandomlyDestroyedCardNumbers[0], RandomlyDestroyedCardNumbers[1], RandomlyDestroyedCardNumbers[2]));
                yield return new WaitForSeconds(1.8f);
            }

            StartCoroutine(Scripts.EventCardControlScript.EventCardRemoved());
        }
    }

    public void UpdateCombatStats(int NewAttack, int NewDefence)
    {
        Components.CombatStuff.Attack.text = NewAttack.ToString();
        Components.CombatStuff.Defence.text = NewDefence.ToString();
    }

    public IEnumerator FinishedSpecialUpgrade()
    {
        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.1f));
        yield return new WaitForSeconds(0.8f);
        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume * 4;
        Components.PromotionScene.SetActive(false);
        Pos = Camera.main.transform.position;
        Pos.z = 1.84f;
        Camera.main.transform.position = Pos;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Scripts.EventCardControlScript.EventCardRemoved());
    }

    public IEnumerator FinishedCombat()
    {
        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.1f));
        yield return new WaitForSeconds(0.8f);
        Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume = Scripts.MiscellaneousGameManagementScript.MusicAudioPlayer.volume * 4;
        Components.CombatStuff.CombatScene.SetActive(false);
        Pos = Camera.main.transform.position;
        Pos.z = 1.84f;
        Camera.main.transform.position = Pos;
        yield return new WaitForSeconds(1);

        if (CombatVictory == true)
        {
            StartCoroutine(SelectionSuccess(OptionCardNumber));
        }

        else
        {
            //Get 1 Chaos resource if battle is lost.
            InstantiatedCards[0] = Instantiate(Components.ResourceCardPrefabs[4]);
            StartCoroutine(FadeInGivenCards(0));
            InstantiatedCards[0].transform.position = new Vector3(-0.63f, 0, 4.3f);

            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[5]);
            yield return new WaitForSeconds(1);

            InstantiatedCards[0].transform.parent = Components.HandCardRotationTransform;
            InstantiatedCards[0].transform.SetSiblingIndex(Scripts.HandCardRotationScript.Important.NumberOfCards);
            Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] += 1;
            Scripts.HandCardRotationScript.Important.NumberOfCards += 1;
            InstantiatedCards[0].GetComponent<ResourceCardControlCS>().CardNumber = Scripts.HandCardRotationScript.Important.NumberOfCards + 1;
            Scripts.HandCardRotationScript.UpdateNumberOfCards();
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(Scripts.EventCardControlScript.EventCardRemoved());
        }
    }

    public IEnumerator FadeOutPlayedCards(int i)
    {
        Color color;

        if (CurrentStatus.CardsPlayed[i] != null)
        {
            while (CurrentStatus.CardsPlayed[i].GetComponent< SpriteRenderer > ().color.a > 0)
            {
                color = CurrentStatus.CardsPlayed[i].GetComponent<SpriteRenderer>().color;
                color.a -= 0.05f;
                CurrentStatus.CardsPlayed[i].GetComponent<SpriteRenderer>().color = color;

                color = CurrentStatus.CardsPlayed[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                color.a -= 0.1f;
                CurrentStatus.CardsPlayed[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

                color = CurrentStatus.CardsPlayed[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color;
                color.a -= 0.1f;
                CurrentStatus.CardsPlayed[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;

                color = CurrentStatus.CardsPlayed[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color;
                color.a -= 0.1f;
                CurrentStatus.CardsPlayed[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color = color;

                color = CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color;
                color.a -= 0.1f;
                CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = color;

                color = CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color;
                color.a -= 0.1f;
                CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color = color;

                color = CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().color;
                color.a -= 0.1f;
                CurrentStatus.CardsPlayed[i].transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().color = color;

                yield return new WaitForSeconds(0.025f);
            }

            Destroy(CurrentStatus.CardsPlayed[i]);
            CurrentStatus.CardsPlayed[i] = null;
            Scripts.HandCardRotationScript.Components.CardsPlayed[i] = null;
        }
    }

    public IEnumerator FadeInGivenCards(int i)
    {
        Color color;

        color = InstantiatedCards[i].GetComponent<SpriteRenderer>().color;
        color.a = 0;
        InstantiatedCards[i].GetComponent<SpriteRenderer>().color = color;

        color = InstantiatedCards[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        color.a = 0;
        InstantiatedCards[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

        color = InstantiatedCards[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color;
        color.a = 0;
        InstantiatedCards[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;

        color = InstantiatedCards[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color;
        color.a = 0;
        InstantiatedCards[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color = color;

        color = InstantiatedCards[i].transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color;
        color.a = 0;
        InstantiatedCards[i].transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = color;

        color = InstantiatedCards[i].transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color;
        color.a = 0;
        InstantiatedCards[i].transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color = color;

        color = InstantiatedCards[i].transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().color;
        color.a = 0;
        InstantiatedCards[i].transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().color = color;

        while (InstantiatedCards[i].GetComponent< SpriteRenderer > ().color.a < 1)
        {
            color = InstantiatedCards[i].GetComponent<SpriteRenderer>().color;
            color.a += 0.1f;
            InstantiatedCards[i].GetComponent<SpriteRenderer>().color = color;

            color = InstantiatedCards[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a += 0.1f;
            InstantiatedCards[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

            color = InstantiatedCards[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color;
            color.a += 0.1f;
            InstantiatedCards[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;

            color = InstantiatedCards[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color;
            color.a += 0.1f;
            InstantiatedCards[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color = color;

            color = InstantiatedCards[i].transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color;
            color.a += 0.1f;
            InstantiatedCards[i].transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = color;

            color = InstantiatedCards[i].transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color;
            color.a += 0.1f;
            InstantiatedCards[i].transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().color = color;

            color = InstantiatedCards[i].transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().color;
            color.a += 0.1f;
            InstantiatedCards[i].transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().color = color;
            yield return new WaitForSeconds(0.025f);
        }
    }

    private int[] NumberOfResourcesGained = new int[3];
    private int[] NumberOfResourcesLost = new int[3];
    public GameObject[] ReferencedResourceDisplay = new GameObject[3];
    private int[] ResourceDisplay1 = new int[6];
    private int[] ResourceDisplay2 = new int[6];
    private int[] ResourceDisplay3 = new int[6];

    public void UpdateCards(
        int CardIdentifier1,
        int CardIdentifier2)
    {

        EventDetails.CardIdentifier[0] = CardIdentifier1;
        EventDetails.CardIdentifier[1] = CardIdentifier2;

        for (int Clear = 0; Clear < 5; Clear++)
        {
            ResourceDisplay3[Clear] = 0;
            ResourceDisplay2[Clear] = 0;
            ResourceDisplay1[Clear] = 0;
        }

        for (int Clear = 0; Clear < 3; Clear++)
        {
            NumberOfResourcesGained[Clear] = 0;
            NumberOfResourcesLost[Clear] = 0;
            if (ReferencedResourceDisplay[Clear] != null) ReferencedResourceDisplay[Clear].SetActive(false);
        }

        //Option Card 1
        if (EventDetails.ResourcesUsed[0] != 0)
        {
            NumberOfResourcesLost[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = EventDetails.ResourcesUsed[0];
                    break;
                }
            }
        }

        if (EventDetails.ResourcesUsed[1] != 0)
        {
            NumberOfResourcesLost[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = EventDetails.ResourcesUsed[1];
                    break;
                }
            }
        }

        if (EventDetails.ResourcesUsed[2] != 0)
        {
            NumberOfResourcesLost[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = EventDetails.ResourcesUsed[2];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[0] != 0)
        {
            NumberOfResourcesGained[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = EventDetails.ResourceGained[0];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[1] != 0)
        {
            NumberOfResourcesGained[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = EventDetails.ResourceGained[1];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[2] != 0)
        {
            NumberOfResourcesGained[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = EventDetails.ResourceGained[2];
                    break;
                }
            }
        }

        ReferencedResourceDisplay[0] = Components.ResourceReqDisplay[0].LostDisplay[NumberOfResourcesLost[0]].GainDisplay[NumberOfResourcesGained[0]];
        ReferencedResourceDisplay[0].SetActive(true);

        for (int a = 0; a < ReferencedResourceDisplay[0].transform.childCount - 2; a++)
        {
            ReferencedResourceDisplay[0].transform.GetChild(a).GetComponent< SpriteRenderer > ().sprite = Components.ResourceSymbol[ResourceDisplay1[a] - 1];
        }

        //Option Card 2
        if (EventDetails.ResourcesUsed[3] != 0)
        {
            NumberOfResourcesLost[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = EventDetails.ResourcesUsed[3];
                    break;
                }
            }
        }

        if (EventDetails.ResourcesUsed[4] != 0)
        {
            NumberOfResourcesLost[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = EventDetails.ResourcesUsed[4];
                    break;
                }
            }
        }

        if (EventDetails.ResourcesUsed[5] != 0)
        {
            NumberOfResourcesLost[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = EventDetails.ResourcesUsed[5];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[3] != 0)
        {
            NumberOfResourcesGained[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = EventDetails.ResourceGained[3];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[4] != 0)
        {
            NumberOfResourcesGained[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = EventDetails.ResourceGained[4];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[5] != 0)
        {
            NumberOfResourcesGained[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = EventDetails.ResourceGained[5];
                    break;
                }
            }
        }

        ReferencedResourceDisplay[1] = Components.ResourceReqDisplay[1].LostDisplay[NumberOfResourcesLost[1]].GainDisplay[NumberOfResourcesGained[1]];
        ReferencedResourceDisplay[1].SetActive(true);

        for (int a = 0; a < ReferencedResourceDisplay[1].transform.childCount - 2; a++)
        {
            ReferencedResourceDisplay[1].transform.GetChild(a).GetComponent< SpriteRenderer > ().sprite = Components.ResourceSymbol[ResourceDisplay2[a] - 1];
        }

        //Option Card 3
        if (EventDetails.ResourcesUsed[6] != 0)
        {
            NumberOfResourcesLost[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = EventDetails.ResourcesUsed[6];
                    break;
                }
            }
        }

        if (EventDetails.ResourcesUsed[7] != 0)
        {
            NumberOfResourcesLost[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = EventDetails.ResourcesUsed[7];
                    break;
                }
            }
        }
        if (EventDetails.ResourcesUsed[8] != 0)
        {
            NumberOfResourcesLost[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = EventDetails.ResourcesUsed[8];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[6] != 0)
        {
            NumberOfResourcesGained[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = EventDetails.ResourceGained[6];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[7] != 0)
        {
            NumberOfResourcesGained[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = EventDetails.ResourceGained[7];
                    break;
                }
            }
        }

        if (EventDetails.ResourceGained[8] != 0)
        {
            NumberOfResourcesGained[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = EventDetails.ResourceGained[8];
                    break;
                }
            }
        }

        ReferencedResourceDisplay[2] = Components.ResourceReqDisplay[2].LostDisplay[NumberOfResourcesLost[2]].GainDisplay[NumberOfResourcesGained[2]];
        ReferencedResourceDisplay[2].SetActive(true);

        for (int a = 0; a < ReferencedResourceDisplay[2].transform.childCount - 2; a++)
        {
            ReferencedResourceDisplay[2].transform.GetChild(a).GetComponent< SpriteRenderer > ().sprite = Components.ResourceSymbol[ResourceDisplay3[a] - 1];
        }
    }

    public IEnumerator ClearFadeOut()     //For Tutorial
    {    
        yield return new WaitForSeconds(1);
        Scripts.MainMenuControllerScript.TutorialUse.ButtonDimScreen.SetActive(false);
    }

    public IEnumerator PlayShuffleInAnimation(int CardTech, int CardNumber)
    {
        Components.CardToCallAnimationSpriteRenderer[1].sprite = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech - 1].Position[CardNumber - 1].SpriteGroup[3];
        Components.CardToCallAnimationText[0].text = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech - 1].Position[CardNumber - 1].EventCardText[0];
        Components.CardToCallAnimationText[1].text = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech - 1].Position[CardNumber - 1].EventCardText[1];
        Components.CardToCallAnimationText[1].text = Components.CardToCallAnimationText[1].text.Replace("NWL", "\n");
        Components.CardToCallAnimationText[2].text = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech - 1].Position[CardNumber - 1].EventCardText[2];
        Components.CardToCallAnimation.SetActive(true);
        yield return new WaitForSeconds(3);
        Components.CardToCallAnimation.SetActive(false);

        string ShownText = "";
        string ShownText2 = "";

        for (var i = 0; i < 10; i++)
        {
            if (Components.CardAddedNames[i] == "")
            {
                Components.CardAddedNames[i] = Scripts.EventCardControlScript.EventDisplay.Normal[CardTech - 1].Position[CardNumber - 1].Name;
                if (i == 0) ShownText = Components.CardAddedNames[i];
                else if (i > 0 && i < 3) ShownText = ShownText + "\n" + Components.CardAddedNames[i];
                else if (i == 3) ShownText2 = Components.CardAddedNames[i];
                else ShownText2 = ShownText + "\n" + Components.CardAddedNames[i];
                break;
            }

            if (i == 0) ShownText = Components.CardAddedNames[i];
            else if (i > 0 && i < 3) ShownText = ShownText + "\n" + Components.CardAddedNames[i];
            else if (i == 3) ShownText2 = Components.CardAddedNames[i];
            else ShownText2 = ShownText + "\n" + Components.CardAddedNames[i];
        }
        Scripts.UIControllerScript.InformationOverviewText[8].text = ShownText;
        Scripts.UIControllerScript.InformationOverviewText[9].text = ShownText2;
    }


    [System.Serializable]
    public class FromEventControl
    {
        public float[] ResourceRequirements = new float[3];
        public int[] ResourcesUsed = new int[9];
        public int[] TechnologyRequired = new int[3];                              
        public SpriteRenderer[] OptionCardImage = new SpriteRenderer[3];    // Add the 3 sprite renderers from option 1, 2 and 3
        public int[] ResourceGained = new int[9];
        public int[] CardIdentifier = new int[2];
        public int[] StatsChange = new int[6];                              // 1 is health, 10 is atk, 100 is defence
        public int[] UnlockOptionNumber = new int[3];
        public bool[] RemoveEventCheck = new bool[3];
        public int[] CalledCardTechLevel = new int[3];
        public int[] CalledCardPosition = new int[3];
        public float[] ChanceToAddCard = new float[3];
        public bool EnableAnimation;                                        //Card shuffle in animation
        public int SpecialResourceGained;
        public int[] SpriteDisplayIdentifier = new int[2];
        public int[] DiscardCards = new int[3];
        public bool[] LoseCheck = new bool[3];
        public Sprite[] PromotionSprite = new Sprite[3];
        public string[] PromotionText = new string[6];
        public int[] PromotionResource = new int[3];
        public int[] HumanRelationshipChange = new int[3];
        public float[] FoodChange = new float[3];
        public int[] FoodProductionChange = new int[3];
        public int[] MoraleProductionChange = new int[3];
        public int[] HumanTiesProductionChange = new int[3];
        public EventCardControlCS.CombatEffects Combat;
        public EventCardControlCS.MilitaryStrengthChange MilitaryChange;
        public int SpecialCardNumber;
    }

    [System.Serializable]
    public class CurrentStatusCheck
    {
        public GameObject[] CardsPlayed = new GameObject[3];
        public float ResourcePlaced;
        public float ResourceRequired;      // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction, Resource Required = sum of total
        public int OtherRequirement;
        public bool MeetOtherRequirement;
        public int[] SkillsPossessed = new int[10];
    }

    [System.Serializable]
    public class ComponentReference1
    {
        public GameObject[] ResourceCardPrefabs = new GameObject[5];        // 0 is life, 1 is resource, 2 is faith, 3 is humans, 4 is destruction.
        public Sprite[] ResourceSymbol = new Sprite[10];
        public string[] ResourceName = new string[5];
        public string[] ResourceQuote = new string[5];
        public Vector3[] QuotePosition = new Vector3[5];
        public string[] QuoteAuthor = new string[5];
        public ResourceDisplay[] ResourceReqDisplay = new ResourceDisplay[3];
        public Transform HandCardRotationTransform; 
        public GameObject TrapDoorGameObject;
        public GameObject[] AnimationStatsChange = new GameObject[4];
        public GameObject CardToCallAnimation;
        public SpriteRenderer[] CardToCallAnimationSpriteRenderer = new SpriteRenderer[2];
        public UnityEngine.UI.Text[] CardToCallAnimationText = new UnityEngine.UI.Text[2];
        public Animation NatureAvatarAnim;
        public Animation HumanAvatarAnim;
        public string[] CardAddedNames = new string[10];

        public bool PromotionFinishCheck;
        public GameObject PromotionScene;
        public SpriteRenderer PromotionCard;
        public UnityEngine.UI.Text[] PromotionTextRenderer = new UnityEngine.UI.Text[2];

        public CombatComponents CombatStuff;
    }

    [System.Serializable]
    public class CombatComponents
    {
        public bool CombatFinishCheck;
        public GameObject CombatScene;
        public UnityEngine.UI.Text Result;
        public UnityEngine.UI.Text CombatDetails;
        public UnityEngine.UI.Text WarEffects;
        public GameObject DefeatSprite;
        public GameObject VictorySprite;
        public Animation ShowCombatStats;
        public GameObject CombatStats;
        public UnityEngine.UI.Text Attack;
        public UnityEngine.UI.Text Defence;
        public SpriteRenderer BackgroundCombatScene;
        public Sprite[] Background = new Sprite[2];
        public UnityEngine.UI.Text CombatQuote;

    }

    [System.Serializable]
    public class ResourceDisplay
    {
        public ResourceLostDisplay[] LostDisplay = new ResourceLostDisplay[4];
    }

    [System.Serializable]
    public class ResourceLostDisplay
    {
        public GameObject[] GainDisplay = new GameObject[4];
    }
}
