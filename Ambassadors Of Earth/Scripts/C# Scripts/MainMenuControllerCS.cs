using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControllerCS : MonoBehaviour {

    public CameraControlCS.ScriptReference Scripts;
    public TutorialStuff TutorialUse;
    public LevelStuff LevelUse;
    public StartingCardDeal StartingCards;
    public GameObject[] MainMenuGameObjectsToDisable = new GameObject[4];
    public GameObject[] Layers = new GameObject[3];
    public int LevelSelected;
    public GameObject CardsPlayedParentGameObject;
    public UnityEngine.UI.Button ContinueButtonDisplay;
    public GameObject InstantiatedCard;
    public Transform HandCardRotationTransform;
    public int PreviousNumberOfCards;
    public GameObject StartingImages;
    public GameObject[] MainMenuCanvas = new GameObject[3];
    public UnityEngine.UI.Button[] MainMenuButtons = new UnityEngine.UI.Button[10];
    public int CurrentPage;
    private int PreviousPage;
    public PlayerProgressData PlayerData;
    public ComponentReference Components;

    void Start () {
        Vector3 Pos = Camera.main.transform.position;
        Pos.z = -23.8f;
        Camera.main.transform.position = Pos;
        SetContinueButtonDisplay();
        CurrentPage = 1;
    }

    void SetContinueButtonDisplay()
    {
        /*if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            ContinueButtonDisplay.interactable = true;
        }

        else
        {
            ContinueButtonDisplay.interactable = false;
        }*/
    }

    public void ClickCredits()
    {
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        Components.BackgroundMainMenu.sprite = Components.BackgroundMainMenuImages[1];

        Vector3 Temp = Components.BackgroundMainMenuTransform.localScale;
        Temp.x = 18;
        Temp.y = 18;
        Components.BackgroundMainMenuTransform.localScale = Temp;

        //Open Credits
        Components.SettingsPage.SetActive(false);
        Components.CreditsPage.SetActive(true);

        if (CurrentPage == 4) return;

        PreviousPage = CurrentPage;

        if (CurrentPage == 1)       //Starting Main Menu
        {
            Layers[0].SetActive(false);
            StartingImages.SetActive(false);
            MainMenuCanvas[1].SetActive(false);

            Layers[1].SetActive(true);
            MainMenuCanvas[2].SetActive(true);
            CurrentPage = 4;
        }

        else if (CurrentPage == 2)  //Mode Selection
        {
            MainMenuCanvas[2].SetActive(true);
            MainMenuCanvas[0].SetActive(false);
            CurrentPage = 4;
        }

        else if (CurrentPage == 3)  //CampaignMenu
        {
            MainMenuCanvas[2].SetActive(true);
            MainMenuCanvas[1].SetActive(false);
            CurrentPage = 4;
        }
    }

    public void ClickSettings()
    {
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        Components.BackgroundMainMenu.sprite = Components.BackgroundMainMenuImages[0];

        Vector3 Temp = Components.BackgroundMainMenuTransform.localScale;
        Temp.x = 31.2f;
        Temp.y = 30;
        Components.BackgroundMainMenuTransform.localScale = Temp;

        //Open Settings
        Components.SettingsPage.SetActive(true);
        Components.CreditsPage.SetActive(false);

        if (CurrentPage == 4) return;

        PreviousPage = CurrentPage;

        if (CurrentPage == 1)       //Starting Main Menu
        {  
            Layers[0].SetActive(false);
            StartingImages.SetActive(false);
            MainMenuCanvas[1].SetActive(false);

            Layers[1].SetActive(true);
            MainMenuCanvas[2].SetActive(true);
            CurrentPage = 4;
        }

        else if (CurrentPage == 2)  //Mode Selection
        {   
            MainMenuCanvas[2].SetActive(true);
            MainMenuCanvas[0].SetActive(false);
            CurrentPage = 4;
        }

        else if (CurrentPage == 3)  //CampaignMenu
        {     
            MainMenuCanvas[2].SetActive(true);
            MainMenuCanvas[1].SetActive(false);
            CurrentPage = 4;
        }
    }

    public void ClickNewGame()     // To ModeSelection
    {    
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        Layers[0].SetActive(false);
        Layers[1].SetActive(true);
        StartingImages.SetActive(false);
        MainMenuCanvas[0].SetActive(true);
        CurrentPage = 2;
    }

    public void ClickCampaign()        //To CampaignMenu
    {
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        MainMenuCanvas[1].SetActive(true);
        MainMenuCanvas[0].SetActive(false);
        CurrentPage = 3;

        for (int i = 0; i < PlayerData.ShowUnlockAnimCheck.Length; i++)
        {
            if (PlayerData.ShowUnlockAnimCheck[i] == true && PlayerData.ClearedLevelCheck[i] == false)
            {
                Scripts.MainMenuCampaignDragScript.JumpToSelected(i);
                StartCoroutine(ShowUnlockLevelAnimation(i));
                break;
            }

            else if (PlayerData.ShowUnlockAnimCheck[i] == true && PlayerData.ClearedLevelCheck[i] == true)
            {
                LevelUse.LevelGreyed[i].SetActive(false);
                LevelUse.LevelLocks[i].SetActive(false);
            }

            else if (PlayerData.ShowUnlockAnimCheck[i] == false && PlayerData.ClearedLevelCheck[i] == false && i!=0)
            {
                LevelUse.LevelGreyed[i].SetActive(true);
                LevelUse.LevelLocks[i].SetActive(true);
            }
        }
    }

    public void ClickBack1()       //To StartingMainMenu
    {  
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        Layers[0].SetActive(true);
        Layers[1].SetActive(false);
        MainMenuCanvas[0].SetActive(false);
        StartingImages.SetActive(true);
        CurrentPage = 1;
    }

    public void ClickBack2()       //To ModeSelection
    {  
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        MainMenuCanvas[0].SetActive(true);
        MainMenuCanvas[1].SetActive(false);
        CurrentPage = 2;
    }

    public void ClickBack3()       //From SettingsPage to previous page
    {  
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        Components.BackgroundMainMenu.sprite = Components.BackgroundMainMenuImages[0];
        Vector3 Temp = Components.BackgroundMainMenuTransform.localScale;
        Temp.x = 31.2f;
        Temp.y = 30;
        Components.BackgroundMainMenuTransform.localScale = Temp;

        if (PreviousPage == 1)      //Starting Main Menu
        { 
            MainMenuCanvas[2].SetActive(false);
            Layers[0].SetActive(true);
            Layers[1].SetActive(false);
            StartingImages.SetActive(true);
            CurrentPage = 1;
        }

        if (PreviousPage == 2)      // Mode Selection
        { 
            MainMenuCanvas[2].SetActive(false);
            MainMenuCanvas[0].SetActive(true);
            CurrentPage = 2;
        }

        if (PreviousPage == 3)       //Campaign Menu
        {  
            MainMenuCanvas[2].SetActive(false);
            MainMenuCanvas[1].SetActive(true);
            CurrentPage = 3;
        }
    }

    public IEnumerator ShowUnlockLevelAnimation(int LevelUnlocked)
    {
        yield return new WaitForSeconds(0.3f);
        PlayerData.UnlockAnimDisplay[LevelUnlocked].Play();
        yield return new WaitForSeconds(1);
        PlayerData.ClearedLevelCheck[LevelUnlocked] = true;
        Scripts.MiscellaneousGameManagementScript.Save();
    }

    public void ClickLevel3()
    {
        if (PlayerData.ClearedLevelCheck[2] == true)
        {
            LevelUse.LevelButtons[2].interactable = false;
            LevelSelected = 3;
            LevelUse.PositionInTalking = 0;
            LevelUse.PositionInEnding = 0;
            StartCoroutine(MoveCameraClickPlay());
            LevelUse.SpeechLoaded = false;
            LevelUse.FinishTalk = false;
            LevelUse.FinishTalkEnding = true;
            Scripts.UIControllerScript.YearDetails(LevelUse.SpeechBubble[LevelSelected - 2].CurrentYear, LevelUse.SpeechBubble[LevelSelected - 2].BcOrAd);

            Scripts.HandCardRotationScript.Components.ResourceCardsReadyCheck = false;
            StartCoroutine(ResetGame(LevelSelected));
            StartCoroutine(StandardLevelSetUp());
        }
    }

    public void ClickLevel2()
    {
        if (PlayerData.ClearedLevelCheck[1] == true)
        {
            LevelUse.LevelButtons[1].interactable = false;
            LevelSelected = 2;
            LevelUse.PositionInTalking = 0;
            LevelUse.PositionInEnding = 0;
            StartCoroutine(MoveCameraClickPlay());
            LevelUse.SpeechLoaded = false;
            LevelUse.FinishTalk = false;
            LevelUse.FinishTalkEnding = true;
            Scripts.UIControllerScript.YearDetails(LevelUse.SpeechBubble[LevelSelected - 2].CurrentYear, LevelUse.SpeechBubble[LevelSelected - 2].BcOrAd);

            Scripts.ParticleEffectControllerScript.SetWeather(1);
            Scripts.HandCardRotationScript.Components.ResourceCardsReadyCheck = false;
            StartCoroutine(ResetGame(LevelSelected));
            StartCoroutine(StandardLevelSetUp());
        }
    }

    public IEnumerator AngieTalkEnding()
    {
        var ReferencePath = LevelUse.SpeechBubble[LevelSelected - 2].SpeechEnding[LevelUse.PositionInEnding];
        if (ReferencePath.Text == "")
        {
            LevelUse.FinishTalkEnding = true;
            LevelUse.SpeechObjects[0].SetActive(false);
            if (LevelUse.PositionInEnding <= 2) StartCoroutine(Scripts.CameraControlScript.WinGame(LevelSelected));
            if (LevelUse.PositionInEnding > 2) StartCoroutine(Scripts.CameraControlScript.LoseGame("","", LevelSelected));
            yield break;
        }

        Vector3 Scale = LevelUse.SpeechObjects[1].transform.localScale;
        Scale.x = ReferencePath.Size[0];
        Scale.y = ReferencePath.Size[1];
        LevelUse.SpeechObjects[1].transform.localScale = Scale;
        ReferencePath.Text = ReferencePath.Text.Replace("NWL", "\n");
        LevelUse.SpeechObjects[1].GetComponent<UnityEngine.UI.Text>().text = ReferencePath.Text;
        LevelUse.SpeechAnimations[0].Play("CallOutWobble");

        yield return new WaitForSeconds(0.5f);
        LevelUse.PositionInEnding += 1;
        LevelUse.SpeechLoaded = true;
        LevelUse.SpeechAnimations[1].Play("ShowButtons");
    }

    public IEnumerator NextSpeechEnding()
    {
        LevelUse.SpeechLoaded = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(AngieTalkEnding());
    }

    public void PreviousSpeechEnding()
    {
        LevelUse.SpeechLoaded = false;
        StartCoroutine(AngieTalkEnding());
    }

    public IEnumerator AngieTalk()
    {

        var ReferencePath = LevelUse.SpeechBubble[LevelSelected - 2].Speech[LevelUse.PositionInTalking];
        if (ReferencePath.Text == "")
        {
            LevelUse.FinishTalk = true;
            LevelUse.SpeechObjects[0].SetActive(false);
            StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
            yield break;
        }

        Vector3 Scale = LevelUse.SpeechObjects[1].transform.localScale;
        Scale.x = ReferencePath.Size[0];
        Scale.y = ReferencePath.Size[1];
        LevelUse.SpeechObjects[1].transform.localScale = Scale;
        ReferencePath.Text = ReferencePath.Text.Replace("NWL", "\n");
        LevelUse.SpeechObjects[1].GetComponent<UnityEngine.UI.Text>().text = ReferencePath.Text;
        LevelUse.SpeechAnimations[0].Play("CallOutWobble");

        yield return new WaitForSeconds(0.5f);
        LevelUse.PositionInTalking += 1;
        LevelUse.SpeechLoaded = true;
        LevelUse.SpeechAnimations[1].Play("ShowButtons");
    }

    public IEnumerator NextSpeech()
    {
        LevelUse.SpeechLoaded = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(AngieTalk());
    }

    public void PreviousSpeech()
    {
        LevelUse.SpeechLoaded = false;
        StartCoroutine(AngieTalk());
    }

    private string Sign;

    public string DetermineSign(int IntToCheck)
    {
        if (IntToCheck > 0) Sign = "+";
        if (IntToCheck <= 0) Sign = "";
        return Sign;
    }

    public string DetermineHumanTies(float HumanTieToCheck)
    {
        string DisplaySpeech;
        if (HumanTieToCheck <= -30) { DisplaySpeech = "Humans view us with contempt (Human Ties: " + HumanTieToCheck + ").\nThey will attempt to oppose our every move."; }
        else if (HumanTieToCheck <= -10 && HumanTieToCheck > -30) { DisplaySpeech = "Humans remain suspicious of us (Human Ties: " + HumanTieToCheck + ").\nThey may occasionally disrupt our community."; }
        else if (HumanTieToCheck > -10 && HumanTieToCheck < 10) { DisplaySpeech = "Humans view us in a neutral light (Human Ties: " + HumanTieToCheck + ").\nThey will neither help nor hinder us intentionally."; }
        else if (HumanTieToCheck >= 10 && HumanTieToCheck < 30) { DisplaySpeech = "Humans are happy to work with us (Human Ties: " + HumanTieToCheck + ").\nThey will occasionally offer to help our community."; }
        else { DisplaySpeech = "Close relations have been established (Human Ties: " + HumanTieToCheck + ").\nHumans will work with us for the betterment of Earth."; }
        return DisplaySpeech;
    }

    public string DetermineMorale(int MoraleToCheck)
    {
        string DisplaySpeech;
        if (MoraleToCheck >= 8) { DisplaySpeech = "Morale: " + MoraleToCheck * 5 + "% (Satisfactory)"; }
        else if (MoraleToCheck < 8 && MoraleToCheck >= 2) { DisplaySpeech = "Morale: " + MoraleToCheck * 5 + "% (Low)"; }
        else if (MoraleToCheck < 2) { DisplaySpeech = "Morale: " + MoraleToCheck * 5 + "% (Abyssal)"; }
        else { DisplaySpeech = "Morale: " + MoraleToCheck * 5 + "% (Non-existent)"; }
        return DisplaySpeech;
    }

    public string DetermineFood(int FoodToCheck)
    {
        string DisplaySpeech;
        if (FoodToCheck >= 40) { DisplaySpeech = "Food: " + FoodToCheck + " (Adequate)"; }
        else if (FoodToCheck < 40 && FoodToCheck >= 10) { DisplaySpeech = "Food: " + FoodToCheck + " (Low)"; }
        else if (FoodToCheck < 10) { DisplaySpeech = "Food: " + FoodToCheck + " (Scarce)"; }
        else { DisplaySpeech = "Food: " + FoodToCheck + " (None)"; }
        return DisplaySpeech;
    }

    public IEnumerator StandardLevelSetUp()
    {
        Vector3 ResetPos = TutorialUse.ResourceCards.position;
        ResetPos.z = 0;
        TutorialUse.ResourceCards.position = ResetPos;
        LevelUse.SpeechObjects[0].SetActive(false);
        TutorialUse.Tutorial[0].SetActive(false);
        TutorialUse.Tutorial[8].SetActive(false);
        TutorialUse.Tutorial[9].SetActive(false);
        Scripts.UIControllerScript.Information.SetActive(false);

        Scripts.UIControllerScript.InformationOverviewText[0].text = LevelUse.SpeechBubble[LevelSelected - 2].LevelObjective;
        Scripts.UIControllerScript.InformationOverviewText[0].text = Scripts.UIControllerScript.InformationOverviewText[0].text.Replace("NWL", "\n");

        int NetFood = LevelUse.SpeechBubble[LevelSelected - 2].FoodProduction - LevelUse.SpeechBubble[LevelSelected - 2].FoodConsumption;
        Scripts.UIControllerScript.InformationOverviewText[1].text = "Food: " + DetermineSign(NetFood) + NetFood;
        Scripts.UIControllerScript.InformationOverviewText[2].text = "Morale: " + DetermineSign(LevelUse.SpeechBubble[LevelSelected - 2].Morale) + LevelUse.SpeechBubble[LevelSelected - 2].Morale;
        Scripts.UIControllerScript.InformationOverviewText[3].text = "Human Ties: " + DetermineSign(LevelUse.SpeechBubble[LevelSelected - 2].HumanTies) + LevelUse.SpeechBubble[LevelSelected - 2].HumanTies;
        Scripts.UIControllerScript.InformationOverviewText[4].text = "Food Consumption: " + LevelUse.SpeechBubble[LevelSelected - 2].FoodConsumption;
        Scripts.UIControllerScript.InformationOverviewText[5].text = "Food Production: " + LevelUse.SpeechBubble[LevelSelected - 2].FoodProduction;
        Scripts.UIControllerScript.InformationOverviewText[6].text = "Year: " + LevelUse.SpeechBubble[LevelSelected - 2].CurrentYear + " " + LevelUse.SpeechBubble[LevelSelected - 2].BcOrAd;
        Scripts.UIControllerScript.InformationOverviewText[7].text = "Events This Year: 1/10";
        Scripts.UIControllerScript.InformationOverviewText[8].text = "You have not added any events for next year (yet).";
        Scripts.UIControllerScript.InformationOverviewText[9].text = "";

        Scripts.UIControllerScript.InformationCharacterText[0].text = "Attack: 0 (+0)";
        Scripts.UIControllerScript.InformationCharacterText[1].text = "Defence: 0 (+0)";

        Scripts.UIControllerScript.InformationCharacterText[2].text = DetermineMorale(LevelUse.SpeechBubble[LevelSelected - 2].StartingMorale);
        Scripts.UIControllerScript.InformationCharacterText[3].text = DetermineFood(LevelUse.SpeechBubble[LevelSelected - 2].StartingFood);
        Scripts.UIControllerScript.InformationCharacterText[6].text = DetermineMorale(LevelUse.SpeechBubble[LevelSelected - 2].StartingMoraleHuman/5);
        Scripts.UIControllerScript.InformationCharacterText[7].text = DetermineFood(LevelUse.SpeechBubble[LevelSelected - 2].StartingFoodHuman);

        Scripts.EventCardControlScript.Important.HumanRelationship = 0;
        Scripts.EventCardControlScript.UpdateHumanRelationship(LevelUse.SpeechBubble[LevelSelected - 2].StartingHumanTies);        
        Scripts.UIControllerScript.InformationCharacterText[8].text = DetermineHumanTies(LevelUse.SpeechBubble[LevelSelected - 2].StartingHumanTies);

        Scripts.NatureControllerScript.CurrentScienceProgress = 0.25f;
        float FoodStartingFrac = (float)LevelUse.SpeechBubble[LevelSelected - 2].StartingFood / 100;
        Scripts.NatureControllerScript.IncreaseNatureFood(FoodStartingFrac);
        Scripts.NatureControllerScript.Health = 0;
        Scripts.NatureControllerScript.UpdateMorale(LevelUse.SpeechBubble[LevelSelected - 2].StartingMorale);

        Scripts.HumanControllerScript.UpdateMorale(LevelUse.SpeechBubble[LevelSelected - 2].StartingMoraleHuman);
        Scripts.HumanControllerScript.UpdateFood(LevelUse.SpeechBubble[LevelSelected - 2].StartingFoodHuman);

        Scripts.UIControllerScript.ClearMedals();
        yield return new WaitForSeconds(1.5f);

        while (Scripts.HandCardRotationScript.Components.ResourceCardsReadyCheck == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Vector3 Position = TutorialUse.ResourceCards.position;
        Position.z = 0;
        TutorialUse.ResourceCards.position = Position;

        for (int i = 0; i < StartingCards.CardDistribution[0] + StartingCards.CardDistribution[1] + StartingCards.CardDistribution[2] + StartingCards.CardDistribution[3] + StartingCards.CardDistribution[4]; i++)
        {
            Vector3 Pos = new Vector3();
            Pos.Set(-0.84f, 3.44f, 17.5f);
            TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.position = Pos;
            Quaternion newQuaternion = new Quaternion();
            newQuaternion.Set(0.0f, -0.7071f, 0.7071f, 0.0f);
            TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.rotation = newQuaternion;
        }

        Scripts.HandCardRotationScript.AdjustmentCheck = true;
        TutorialUse.Tutorial[8].SetActive(true);

        Color color = new Color();

        for (int i = 0; i < 3; i++)
        {
            color = TutorialUse.CounterSprite[i].color;
            color.a = 0;
            TutorialUse.CounterSprite[i].color = color;

            color = TutorialUse.CounterText[i].color;
            color.a = 0;
            TutorialUse.CounterText[i].color = color;
        }
        TutorialUse.TutorialAnim[8].Play("PopOutCardCounter");

        TutorialUse.Tutorial[9].SetActive(true);
        color = TutorialUse.YearSprite.color;
        color.a = 0;
        TutorialUse.YearSprite.color = color;
        color = TutorialUse.YearText.color;
        color.a = 0;
        TutorialUse.YearText.color = color;

        yield return new WaitForSeconds(0.25f);
        TutorialUse.TutorialAnim[9].Play("PopOutYear");

        yield return new WaitForSeconds(0.5f);
        LevelUse.SpeechObjects[0].SetActive(true);
        Position = LevelUse.SpeechObjects[2].transform.position;
        Position.x = 20;
        LevelUse.SpeechObjects[2].transform.position = Position;
        StartCoroutine(AngieTalk());

        for (int i = 0; i < Scripts.EventCardControlScript.Important.CardsInDeck.Length; i++)
        {
            Scripts.EventCardControlScript.Important.CardsInDeck[i].Name = "";
            Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[0] = 0;
            Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[1] = 0;
            Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[0] = 0;
            Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[1] = 0;
            Scripts.EventCardControlScript.Important.CardsInDeck[i].CardPriority = 0;
        }

        var ReferencePath = Scripts.EventCardControlScript.AllEventCardsInGame[0].LevelChosen[LevelSelected - 2].CardsInLevel;
        for (int i = 0; i < ReferencePath.Length; i++)
        {
            if (ReferencePath[i].CardIdentifier[0] == 0) break;
            Scripts.EventCardControlScript.Important.CardsInDeck[i].Name = ReferencePath[i].Name;
            Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[0] = ReferencePath[i].CardIdentifier[0];
            Scripts.EventCardControlScript.Important.CardsInDeck[i].CardIdentifier[1] = ReferencePath[i].CardIdentifier[1];
            Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[0] = ReferencePath[i].SpriteDisplayIdentifier[0];
            Scripts.EventCardControlScript.Important.CardsInDeck[i].SpriteDisplayIdentifier[1] = ReferencePath[i].SpriteDisplayIdentifier[1];
            Scripts.EventCardControlScript.Important.CardsInDeck[i].CardPriority = ReferencePath[i].CardPriority;
        }

        Scripts.EventCardControlScript.ShuffleCheck = false;
        StartCoroutine(Scripts.EventCardControlScript.ShuffleCard());
        yield return new WaitForSeconds(0.1f);
        while (Scripts.EventCardControlScript.ShuffleCheck == false)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Scripts.HandCardRotationScript.Important.NoClicking = false;
    }

    public void ClickTutorial()
    {
        LevelSelected = 1;

        Vector3 Pos = new Vector3();
        Pos.Set(22.25f, 0.05f, 1.51f);
        TutorialUse.TopCardPosition.position = Pos;                 // OutofScene
        Pos.Set(16, 5.41f, -2.64f);
        TutorialUse.OptionCardTransforms[0].position = Pos;         // OutOfScene
        Pos.Set(21, 7.05f, -2.64f);
        TutorialUse.OptionCardTransforms[1].position = Pos;         // OutOfScene
        Pos.Set(26.31f, 9.35f, -2.64f);
        TutorialUse.OptionCardTransforms[2].position = Pos;         // OutOfScene

        LevelUse.SpeechObjects[0].SetActive(false);
        TutorialUse.Tutorial[0].SetActive(false);
        TutorialUse.Tutorial[8].SetActive(false);
        TutorialUse.Tutorial[9].SetActive(false);

        Scripts.UIControllerScript.RoundNumberDisplayText.text = "10000 BC";
        StartCoroutine(MoveCameraClickPlay());

        Vector3 Scale;
        Scale = TutorialUse.Tutorial[1].transform.localScale;
        Scale.x = 0.1f;
        Scale.y = 0.1f;
        TutorialUse.Tutorial[1].transform.localScale = Scale;
        TutorialUse.TutorialText.text = "Hey!";

        Vector3 Position;
        Position = TutorialUse.TutorialButtons.transform.position;
        Position.x = 20;
        TutorialUse.TutorialButtons.transform.position = Position;

        StartCoroutine(FirstTimeBlink());
        StartCoroutine(ResetGame(LevelSelected));
        Scripts.HandCardRotationScript.Important.NoClicking = false;
        Scripts.NatureControllerScript.CurrentScienceProgress = 0.25f;
        Scripts.NatureControllerScript.IncreaseNatureFood(1);
        Scripts.NatureControllerScript.UpdateMorale(20);
    }

    public IEnumerator FirstTimeBlink()
    {
        yield return new WaitForSeconds(0.8f);
        Scripts.CameraControlScript.Blink();
        yield return new WaitForSeconds(7.5f);
        TutorialUse.Tutorial[0].SetActive(true);
        TutorialUse.ButtonDimScreen.SetActive(false);
        TutorialUse.PositionInTutorial = 0;
        StartCoroutine(TutorialClickNext());
    }

    public IEnumerator TutorialResetHand()
    {

        int PreviousNumberOfCards = HandCardRotationTransform.childCount - 1;
        for (int Delete = PreviousNumberOfCards - 1; Delete > -1; Delete--)
        {
            Destroy(HandCardRotationTransform.GetChild(Delete).gameObject);
        }

        yield return new WaitForSeconds(0.1f);

        int life;
        int Resource;
        int Faith;
        int Human;
        int Chaos;
          
        for (life = 0; life < StartingCards.CardDistribution[0]; life++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[0]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(life);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[life];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[life];
        }

        for (Resource = life; Resource < StartingCards.CardDistribution[1] + life; Resource++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[1]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Resource);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Resource];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Resource];
        }

        for (Faith = Resource; Faith < StartingCards.CardDistribution[2] + Resource; Faith++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[2]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Faith);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Faith];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Faith];
        }

        for (Human = Faith; Human < StartingCards.CardDistribution[3] + Faith; Human++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[3]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Human);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Human];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Human];
        }

        for (Chaos = Human; Chaos < StartingCards.CardDistribution[4] + Human; Chaos++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[4]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Chaos);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Chaos];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Chaos];
        }

        if (TutorialUse.PositionInTutorial == 17)
        {
            GameObject InstantiatedCards = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[4]);
            Vector3 Pos = new Vector3();
            Pos.Set(-0.63f, 0, 4.3f);
            InstantiatedCards.transform.position = Pos;
            InstantiatedCards.transform.parent = Scripts.HandCardRotationScript.Components.CardsPlayedParent.transform;
            Scripts.HandCardRotationScript.Components.CardsPlayed[0] = InstantiatedCards;
            Scripts.HandCardRotationScript.Components.CardsPlayed[0].tag = "PlayedCards";
        }

        TutorialUse.PositionInTutorial -= 2;
        StartCoroutine(TutorialClickNext());
    }

    public IEnumerator ClearPrevious()
    {
        TutorialUse.FinishLoadCheck = false;

        if (TutorialUse.RemoveTextCheck == true)
        {
            TutorialUse.RemoveTextCheck = false;
            TutorialUse.TutorialAnim[0].Play("FadeCallOut");
            yield return new WaitForSeconds(0.5f);
        }

        if (TutorialUse.RemoveTopCardCheck == true)
        {
            TutorialUse.RemoveTopCardCheck = false;
            Scripts.EventCardControlScript.Components.anim.Play("RemoveCard");
            yield return new WaitForSeconds(1.25f);
        }

        if (TutorialUse.RemoveResourceCardCheck == true)
        {
            TutorialUse.RemoveResourceCardCheck = false;
            TutorialUse.TutorialAnim[TutorialUse.ResourceCardFadeOutNumber - 1].Play("RemoveCard");
            yield return new WaitForSeconds(1.25f);
        }
        StartCoroutine(TutorialClickNext());
    }

    public void ClickPrevious()
    {

        if (TutorialUse.PositionInTutorial == 1) return;
        TutorialUse.RemoveTextCheck = false;
        TutorialUse.RemoveTopCardCheck = false;
        TutorialUse.RemoveResourceCardCheck = false;
        TutorialUse.EventCardClickCheck = false;
        TutorialUse.FinishLoadCheck = false;
        Vector3 Pos;
        Pos = TutorialUse.TutorialButtons.transform.position;
        Pos.x = 20;
        TutorialUse.TutorialButtons.transform.position = Pos;

        if (TutorialUse.PositionInTutorial == 16)
        {
            Scripts.HandCardRotationScript.Components.CardsPlayed[0].GetComponent<ResourceCardControlCS>().CardNumber = 5;
            Scripts.HandCardRotationScript.Components.CardsPlayed[0].GetComponent<ResourceCardControlCS>().CardPlayPosition = 1;
            Scripts.HandCardRotationScript.CardSelected = Scripts.HandCardRotationScript.Components.CardsPlayed[0];
            Scripts.HandCardRotationScript.DeselectCard();
        }

        if (TutorialUse.PositionInTutorial == 17)
        {
            StartingCards.CardDistribution[0] = 1;
            StartingCards.CardDistribution[1] = 1;
            StartingCards.CardDistribution[2] = 1;
            StartingCards.CardDistribution[3] = 1;
            StartingCards.CardDistribution[4] = 0;
            StartCoroutine(TutorialResetHand());
            return;
        }

        TutorialUse.PositionInTutorial -= 2;
        StartCoroutine(TutorialClickNext());
    }

    public IEnumerator TutorialClickNext()
    {
        Vector3 Pos = new Vector3();

        if (TutorialUse.EventCardClickCheck == false)
        {
            Pos.Set(22.25f, 0.05f, 1.51f);
            TutorialUse.TopCardPosition.position = Pos;                 // OutofScene
            Pos.Set(16.0f, 5.41f, -2.64f);
            TutorialUse.OptionCardTransforms[0].position = Pos;         // OutOfScene
            Pos.Set(21.0f, 7.05f, -2.64f);
            TutorialUse.OptionCardTransforms[1].position = Pos;         // OutOfScene
            Pos.Set(26.31f, 9.35f, -2.64f);
            TutorialUse.OptionCardTransforms[2].position = Pos;         // OutOfScene
        }

        if (TutorialUse.PositionInTutorial >= 7)
        {
            Pos = TutorialUse.ResourceCards.position;
            Pos.z = 0;
            TutorialUse.ResourceCards.position = Pos;
        }

        if (TutorialUse.PositionInTutorial <= 31) TutorialUse.Tutorial[8].SetActive(false);
        if (TutorialUse.PositionInTutorial <= 23) TutorialUse.Tutorial[9].SetActive(false);
        TutorialUse.ButtonDimScreen.SetActive(false);

        Pos.Set(22.25f, 0.05f, 1.51f);
        TutorialUse.Tutorial[2].transform.position = Pos;    // Out of Scene
        TutorialUse.Tutorial[3].transform.position = Pos;    // Out of Scene
        TutorialUse.Tutorial[4].transform.position = Pos;    // Out of Scene
        TutorialUse.Tutorial[5].transform.position = Pos;    // Out of Scene
        TutorialUse.Tutorial[6].transform.position = Pos;    // Out of Scene
        TutorialUse.Tutorial[7].transform.position = Pos;    // Out of Scene

        TutorialUse.RemoveTextCheck = false;
        TutorialUse.RemoveTopCardCheck = false;
        TutorialUse.RemoveResourceCardCheck = false;
        TutorialUse.UpdateEventClickPrevious = false;
        Vector3 Scale;
        Scale = TutorialUse.Tutorial[1].transform.localScale;

        switch (TutorialUse.PositionInTutorial)
        {
            case 33:
                StartCoroutine(Scripts.CameraControlScript.WinGame(1));
                break;

            case 32:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Right, we are all set! Best\nof luck and prepare for your\nfirst challenge!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 33;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 31:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "These numbers are tracked\nand displayed right in the\nmiddle. You can't miss them!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 32;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");

                TutorialUse.Tutorial[8].SetActive(true);
                Color color;
                for (int i=0; i<4; i++){
                    color = TutorialUse.CounterSprite[i].color;
                    color.a = 0;
                    TutorialUse.CounterSprite[i].color = color;
                    color = TutorialUse.CounterText[i].color;
                    color.a = 0;
                    TutorialUse.CounterText[i].color = color;
                }
                color = TutorialUse.CounterSprite[4].color;
                color.a = 0;
                TutorialUse.CounterSprite[4].color = color;
                yield return new WaitForSeconds(0.25f);
                TutorialUse.TutorialAnim[8].Play("PopOutCardCounter");
                break;

            case 30:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.033f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Hold no more than 12 resources.\nKeep chaos level below 3.\nAnd NEVER let life go extinct!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 31;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 29:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "What are missions, you ask?\nLets not confuse you first!\nFor now, just remember...";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 30;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 28:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveTopCardCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Likewise, 'Mission Cards' will\nnever be repeated unless options\nin the mission say otherwise.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 4;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());

                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 29;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 27:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveTopCardCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Events labeled 'Special cards'\nwill never be repeated unless\nthey are added by an option.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 3;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());

                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 28;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 26:
                TutorialUse.RemoveResourceCardCheck = true;
                TutorialUse.ResourceCardFadeOutNumber = 8;
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Selecting certain options\ncan remove certain event\ncards permanently.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                TutorialUse.TutorialAnim[7].Play("DrawCard");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 27;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 25:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "All events can be repeated\nthe following year. However,\nthere are a few exceptions.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 26;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 24:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "A year passes after every 10\nevents, and you may or may not\nget the same events next year.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 25;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 23:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.04f;
                Scale.y = 0.04f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Well, the current year is\ndisplayed on your top left.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 24;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");

                TutorialUse.Tutorial[9].SetActive(true);
                color = TutorialUse.YearSprite.color;
                color.a = 0;
                TutorialUse.YearSprite.color = color;
                color = TutorialUse.YearText.color;
                color.a = 0;
                TutorialUse.YearText.color = color;
                yield return new WaitForSeconds(0.25f);
                TutorialUse.TutorialAnim[9].Play("PopOutYear");
                break;

            case 22:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.04f;
                Scale.y = 0.04f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "... Oh yes that's right!\nWhat does a year mean?";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 23;
                Scripts.HandCardRotationScript.Important.NoClicking = false;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 21:
                Pos.Set(-0.61f, 0, -10.2f);
                TutorialUse.TopCardPosition.position = Pos;
                Pos.Set(180, 180, 0);
                Quaternion Quat;
                Quat = TutorialUse.TopCardPosition.transform.rotation;
                Quat.eulerAngles = Pos;    // In Scene
                TutorialUse.TopCardPosition.transform.rotation = Quat;
                Pos.Set(3.09f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[0].position = Pos;
                Pos.Set(-0.61f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[1].position = Pos;
                Pos.Set(-4.31f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[2].position = Pos;
                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                Scale.x = 0.04f;
                Scale.y = 0.04f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Try it for yourself!\n(Tap and hold option 1)";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 22;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 20:
                Pos.Set(-0.61f, 0, -10.2f);
                TutorialUse.TopCardPosition.position = Pos;
                Pos.Set(180, 180, 0);
                Quat = TutorialUse.TopCardPosition.transform.rotation;
                Quat.eulerAngles = Pos;    // In Scene
                TutorialUse.TopCardPosition.transform.rotation = Quat;
                Pos.Set(3.09f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[0].position = Pos;
                Pos.Set(-0.61f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[1].position = Pos;
                Pos.Set(-4.31f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[2].position = Pos;
                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "You can learn more about\nan option's effect by tapping\nand holding an option card.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 21;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 19:
                Pos.Set(-0.61f, 0, -10.2f);
                TutorialUse.TopCardPosition.position = Pos;
                Pos.Set(180, 180, 0);
                Quat = TutorialUse.TopCardPosition.transform.rotation;
                Quat.eulerAngles = Pos;    // In Scene
                TutorialUse.TopCardPosition.transform.rotation = Quat;
                Pos.Set(3.09f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[0].position = Pos;
                Pos.Set(-0.61f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[1].position = Pos;
                Pos.Set(-4.31f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[2].position = Pos;
                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Side effects are displayed in\nlarge, bold characters, as\ncan be seen in option 1.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 20;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 18:
                if (TutorialUse.EventCardClickCheck == false)
                {
                    Pos.Set(-0.61f, 0, -10.2f);
                    TutorialUse.TopCardPosition.position = Pos;
                    Pos.Set(180, 180, 0);
                    Quat = TutorialUse.TopCardPosition.transform.rotation;
                    Quat.eulerAngles = Pos;    // In Scene
                    TutorialUse.TopCardPosition.transform.rotation = Quat;
                    Pos.Set(3.09f, 0.1f, -3.6f);
                    TutorialUse.OptionCardTransforms[0].position = Pos;
                    Pos.Set(-0.61f, 0.1f, -3.6f);
                    TutorialUse.OptionCardTransforms[1].position = Pos;
                    Pos.Set(-4.31f, 0.1f, -3.6f);
                    TutorialUse.OptionCardTransforms[2].position = Pos;
                }

                if (TutorialUse.EventCardClickCheck == true)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Responding to many real\nworld events frequently\nresults in side effects.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");

                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 19;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 17:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.EventCardClickCheck = true;
                Scripts.EventCardControlScript.Important.EventCardSelected = false;
                Scale.x = 0.04f;
                Scale.y = 0.04f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Let's take a look at a real\nevent this time, shall we?";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 2;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());

                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 18;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 16:
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] == 0) TutorialUse.TutorialText.text = "Well done. 1 faith is roughly\nequivalent to 2 life resources.\nYour strategies matter!";
                if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] == 2) TutorialUse.TutorialText.text = "Oh dear! We don't need more\nchaos in the forest! Be more\ncareful next time.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 17;
                Scripts.HandCardRotationScript.Important.NoClicking = false;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 15:
                Pos.Set(-0.61f, 0, -10.2f);
                TutorialUse.TopCardPosition.position = Pos;
                Pos.Set(180, 180, 0);
                Quat = TutorialUse.TopCardPosition.transform.rotation;
                Quat.eulerAngles = Pos;    // In Scene
                TutorialUse.TopCardPosition.transform.rotation = Quat;
                Pos.Set(3.09f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[0].position = Pos;
                Pos.Set(-0.61f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[1].position = Pos;
                Pos.Set(-4.31f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[2].position = Pos;
                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                Scale.x = 0.033f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Great job! Well, I trust you can\nnow choose an option that is\nbeneficial for us all?";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.25f);
                TutorialUse.ButtonDimScreen.SetActive(true);
                TutorialUse.TutorialAnim[11].Play("DelayedDimScreenFadeIn");
                yield return new WaitForSeconds(0.25f);
                TutorialUse.PositionInTutorial = 16;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 14:
                Pos.Set(-0.61f, 0, -10.2f);
                TutorialUse.TopCardPosition.position = Pos;
                Pos.Set(180, 180, 0);
                Quat = TutorialUse.TopCardPosition.transform.rotation;
                Quat.eulerAngles = Pos;    // In Scene
                TutorialUse.TopCardPosition.transform.rotation = Quat;
                Pos.Set(3.09f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[0].position = Pos;
                Pos.Set(-0.61f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[1].position = Pos;
                Pos.Set(-4.31f, 0.1f, -3.6f);
                TutorialUse.OptionCardTransforms[2].position = Pos;
                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                Scripts.HandCardRotationScript.NeedToCloseCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Right, now since all 3 options\nconsume a Chaos resource,\nswipe up that card!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 15;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 13:
                if (TutorialUse.EventCardClickCheck == false)
                {
                    Pos.Set(-0.61f, 0, -10.2f);
                    TutorialUse.TopCardPosition.position = Pos;
                    Pos.Set(180, 180, 0);
                    Quat = TutorialUse.TopCardPosition.transform.rotation;
                    Quat.eulerAngles = Pos;    // In Scene
                    TutorialUse.TopCardPosition.transform.rotation = Quat;
                    Pos.Set(3.09f, 0.1f, -3.6f);
                    TutorialUse.OptionCardTransforms[0].position = Pos;
                    Pos.Set(-0.61f, 0.1f, -3.6f);
                    TutorialUse.OptionCardTransforms[1].position = Pos;
                    Pos.Set(-4.31f, 0.1f, -3.6f);
                    TutorialUse.OptionCardTransforms[2].position = Pos;
                }

                if (TutorialUse.EventCardClickCheck == true)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                TutorialUse.UpdateEventClickPrevious = true;
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                TutorialUse.UpdateEventClickPrevious = false;

                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.033f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Events can consume or provide\nresources. This is indicated by red\nand green arrows respectively.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 14;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.EventCardClickCheck = false;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 12:
                TutorialUse.RemoveTextCheck = true;
                Scripts.EventCardControlScript.Important.EventCardSelected = false;
                TutorialUse.EventCardClickCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Every year, around 10 events\nhappen in the forest. Click the\nevent that I made for you!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 1;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());

                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 13;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 11:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveResourceCardCheck = true;
                TutorialUse.ResourceCardFadeOutNumber = 7;
                Scale.x = 0.034f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Lastly, Chaos is something we\nabsolutely do not want in the\nforest! Keep as little as possible.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                TutorialUse.TutorialAnim[6].Play("DrawCard");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 12;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 10:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveResourceCardCheck = true;
                TutorialUse.ResourceCardFadeOutNumber = 6;
                Scale.x = 0.034f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Humans are troublemakers!\nBut lets not forget the good ones.\nThese will fight for our interests.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                TutorialUse.TutorialAnim[5].Play("DrawCard");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 11;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 9:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveResourceCardCheck = true;
                TutorialUse.ResourceCardFadeOutNumber = 5;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Faith represents messengers\nthat spread your wisdom.\nAND ... I'm one of them!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                TutorialUse.TutorialAnim[4].Play("DrawCard");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 10;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 8:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveResourceCardCheck = true;
                TutorialUse.ResourceCardFadeOutNumber = 4;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Minerals are rare, valuable\nresources. They are used to\ntrade and build facilities.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                TutorialUse.TutorialAnim[3].Play("DrawCard");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 9;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 7:
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveResourceCardCheck = true;
                TutorialUse.ResourceCardFadeOutNumber = 3;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Life is the backbone of our\ncommunity. A highly versatile\nand replenishable resource.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                TutorialUse.TutorialAnim[2].Play("DrawCard");
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 8;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 6:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                for (int i = 0; i < 5; i++)
                {
                    Pos = TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localPosition;
                    Pos.x = 22.4f;
                    Pos.y = -9.4f;
                    TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localPosition = Pos;

                    Quat = TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localRotation;
                    Vector3 Angles = Quat.eulerAngles;
                    Angles.z = 72;
                    Quat.eulerAngles = Angles;
                    TutorialUse.ResourceCards.GetChild(0).transform.GetChild(i).transform.localRotation = Quat;
                }
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 0;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards

                Scripts.EventCardControlScript.Components.EventCardRenderer.sprite = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].SpriteGroup[3];
                Scripts.EventCardControlScript.Components.EventCardTitle.text = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].EventCardText[0];
                Scripts.EventCardControlScript.Components.EventCardDescription.text = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].EventCardText[1];
                Scripts.EventCardControlScript.Components.EventCardDescription.text = Scripts.EventCardControlScript.Components.EventCardDescription.text.Replace("NWL", "\n");
                Scripts.EventCardControlScript.Components.EventCardType.text = Scripts.EventCardControlScript.EventDisplay.Normal[6].Position[0].EventCardText[2];
                Pos.Set(-0.62f, 0.05f, -3.9f);
                TutorialUse.TopCardPosition.position = Pos;    // In Scene
                Pos.Set(180, 180, 0);
                Quat = TutorialUse.TopCardPosition.transform.rotation;
                Quat.eulerAngles = Pos;    // In Scene
                TutorialUse.TopCardPosition.transform.rotation = Quat;
                TutorialUse.RemoveTextCheck = true;
                TutorialUse.RemoveTopCardCheck = true;
                Scale.x = 0.037f;
                Scale.y = 0.04f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Resources that you currently\nhave are displayed below.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.9f);
                Scripts.HandCardRotationScript.AdjustmentCheck = true;
                TutorialUse.PositionInTutorial = 7;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 5:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Maintaining peace in your\nforest is a matter of balancing\nfive types of resources.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                Scripts.EventCardControlScript.ShuffleCount.PositionInShuffle = 0;
                StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
                yield return new WaitForSeconds(1.25f);
                TutorialUse.PositionInTutorial = 6;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 4:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.035f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "But since you may have\nforgotten what that means...\nLet me refresh your memory!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 5;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 3:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.038f;
                Scale.y = 0.038f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "As a tree elder, you are the\nnatural ruler of this forest.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 4;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 2:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.05f;
                Scale.y = 0.05f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "... Well, that's ok.";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 3;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 1:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.04f;
                Scale.y = 0.035f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "You do know that I am\nyour faithful and loyal\nservant, Angie, right?";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 2;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;

            case 0:
                Pos = TutorialUse.ResourceCards.position;
                Pos.z = 15;
                TutorialUse.ResourceCards.position = Pos;          //Hide Resource Cards
                TutorialUse.RemoveTextCheck = true;
                Scale.x = 0.1f;
                Scale.y = 0.1f;
                TutorialUse.Tutorial[1].transform.localScale = Scale;
                TutorialUse.TutorialText.text = "Hey!";
                TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                yield return new WaitForSeconds(0.5f);
                TutorialUse.PositionInTutorial = 1;
                TutorialUse.FinishLoadCheck = true;
                TutorialUse.TutorialAnim[10].Play("ShowButtons");
                break;
        }
    }

    public void ClickContinue()
    {
        /*StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.1f));
        StartCoroutine(MoveCameraClickPlay());
        //Scripts.MiscellaneousGameManagementScript.Load();
        Scripts.OptionCardScripts[0].MoveBackOnClick();
        Scripts.OptionCardScripts[1].MoveBackOnClick();
        Scripts.OptionCardScripts[2].MoveBackOnClick();
        Scripts.EventCardControlScript.Important.EventCardSelected = false;*/
    }

    public IEnumerator MoveCameraClickPlay()
    {
        Scripts.MiscellaneousGameManagementScript.FinalYearParticleEffect.SetActive(false);
        Scripts.MiscellaneousGameManagementScript.BackgroundDim.Play("RestoreLightingBackground");

        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.1f));
        yield return new WaitForSeconds(0.8f);
        MainMenuGameObjectsToDisable[3].GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
        MainMenuGameObjectsToDisable[0].SetActive(false);
        MainMenuGameObjectsToDisable[1].SetActive(false);
        MainMenuGameObjectsToDisable[2].SetActive(false);
        Vector3 Pos = Camera.main.transform.position;
        Pos.z = 1.84f;
        Camera.main.transform.position = Pos;
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[1]);

        StartingImages.SetActive(true);
        MainMenuCanvas[1].SetActive(false);
        MainMenuCanvas[0].SetActive(false);
        Layers[0].SetActive(true);
        Layers[1].SetActive(false);
        CurrentPage = 1;
        for (int i = 0; i < MainMenuButtons.Length; i++)
        {       // Make the level selection buttons interactable again
            if (MainMenuButtons[i] == null) break;
            MainMenuButtons[i].interactable = true;
        }
    }

    public void ClickQuit()
    {
        Confirmation("Are you sure you want to quit?", 1);
    }

    public void ClickReset()
    {
        Confirmation("Delete game data permanently?", 2);
    }

    public UnityEngine.UI.Text ConfirmationText;
    public Animation ConfirmationAnim;
    public GameObject DimScreen;
    private int ConfirmationNumber;

    public void Confirmation(string TextDisplay, int ConfirmationNumberInput)
    {
        ConfirmationNumber = ConfirmationNumberInput;
        ConfirmationText.text = TextDisplay;
        DimScreen.SetActive(true);
        ConfirmationAnim.Play("Confirmation");
    }

    public void CloseConfirmation()
    {
        ConfirmationAnim.Play("RemoveConfirmation");
        StartCoroutine(WaitTime());
    }

    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.30f);
        DimScreen.SetActive(false);
    }

    public void ExecuteConfirmation()
    {
        if (ConfirmationNumber == 1) { Application.Quit(); }
        if (ConfirmationNumber == 2)
        {
            for (int i = 0; i<3; i++)
            {
                PlayerData.ShowUnlockAnimCheck[i] = false;
                PlayerData.ClearedLevelCheck[i] = false;
            }

            Scripts.MiscellaneousGameManagementScript.DeleteData();
            CloseConfirmation();
        }
    }

    public IEnumerator ResetGame(int Level)
    {
        for (int x = 0; x < 5; x++)
        {
            StartingCards.CardDistribution[x] = StartingCards.LevelNumber[Level - 1].Distribution[x];
        }

        int CardsToDelete;
        CardsToDelete = CardsPlayedParentGameObject.transform.childCount;

        if (CardsToDelete > 0)
        {
            for (int del = CardsToDelete - 1; del > -1; del--)
            {
                Destroy(CardsPlayedParentGameObject.transform.GetChild(del).gameObject);
            }
        }

        Scripts.OptionCardScripts[0].MoveBackOnClick();
        Scripts.OptionCardScripts[1].MoveBackOnClick();
        Scripts.OptionCardScripts[2].MoveBackOnClick();

        PreviousNumberOfCards = HandCardRotationTransform.childCount - 1;
        for (int Delete = PreviousNumberOfCards - 1; Delete > -1; Delete--)
        {
            Destroy(HandCardRotationTransform.GetChild(Delete).gameObject);
        }

        yield return new WaitForSeconds(0.1f);

        int life;
        int Resource;
        int Faith;
        int Human;
        int Chaos;

        for (life = 0; life < StartingCards.CardDistribution[0]; life++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[0]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(life);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[life];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[life];
        }

        for (Resource = life; Resource < StartingCards.CardDistribution[1] + life; Resource++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[1]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Resource);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Resource];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Resource];
        }

        for (Faith = Resource; Faith < StartingCards.CardDistribution[2] + Resource; Faith++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[2]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Faith);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Faith];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Faith];
        }

        for (Human = Faith; Human < StartingCards.CardDistribution[3] + Faith; Human++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[3]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Human);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Human];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Human];
        }

        for (Chaos = Human; Chaos < StartingCards.CardDistribution[4] + Human; Chaos++)
        {
            InstantiatedCard = Instantiate(Scripts.OptionClickControlScript.Components.ResourceCardPrefabs[4]);
            InstantiatedCard.transform.parent = HandCardRotationTransform;
            InstantiatedCard.transform.SetSiblingIndex(Chaos);
            InstantiatedCard.transform.localPosition = Scripts.HandCardRotationScript.Components.CardPositions[Chaos];
            InstantiatedCard.transform.localRotation = Scripts.HandCardRotationScript.Components.CardRotations[Chaos];
        }

        //Cannot reset by class because it will make a "shallow" copy e.g. cannot do: Scripts.EventCardControlScript.AllEventCardsInGame[0] = Scripts.EventCardControlScript.AllEventCardsReset;   

        //Things that need to be reset from EventCardControl (Important) 
        Scripts.EventCardControlScript.Important.EventCardSelected = false;
        Scripts.EventCardControlScript.Important.SpecialCardActivated = 0;

        for (int i = 0; i < 10; i++)
        {
            Scripts.EventCardControlScript.Important.CardsToAddIn[i].TechLevel = 0;
            Scripts.EventCardControlScript.Important.CardsToAddIn[i].Position = 0;
            Scripts.EventCardControlScript.Important.CardsToDestroy[i].TechLevel = 0;
            Scripts.EventCardControlScript.Important.CardsToDestroy[i].Position = 0;
        }

        Vector3 Pos;

        for (int i = 0; i < HandCardRotationTransform.childCount - 1; i++)
        {
            if (HandCardRotationTransform.GetChild(i).gameObject.transform.childCount > 0)
            {
                Pos = HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
                Pos.z = -0.01f;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition = Pos;

                Pos = HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition;
                Pos.z = -0.02f;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition = Pos;

                Pos = HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition;
                Pos.z = -0.03f;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition = Pos;

                Pos = HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition;
                Pos.z = -0.02f;
                HandCardRotationTransform.GetChild(i).gameObject.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition = Pos;
            }
        }

        Scripts.HandCardRotationScript.InitialiseNewGame();

        if (LevelSelected != 1) StartCoroutine(Scripts.EventCardControlScript.InitialiseNewGame());
        else if (LevelSelected == 1) Scripts.EventCardControlScript.LaunchTutorial();
    }

    [System.Serializable]
    public class StartingCardDeal
    {
        public int[] CardDistribution = new int[5];          //number of life, resource, faith, human and chaos cards
        public CardDistributionPerLevel[] LevelNumber = new CardDistributionPerLevel[10];
    }

    [System.Serializable]
    public class CardDistributionPerLevel
    {
        public int[] Distribution = new int[5];
    }

    [System.Serializable]
    public class TutorialStuff
    {
        public int PositionInTutorial;
        public int ResourceCardFadeOutNumber;
        public bool RemoveResourceCardCheck = false;
        public bool RemoveTextCheck = false;
        public bool RemoveTopCardCheck = false;
        public bool EventCardClickCheck = false;
        public bool UpdateEventClickPrevious = false;
        public bool ClickMagnifierCheck = false;
        public bool FinishLoadCheck;
        public Transform[] OptionCardTransforms = new Transform[3];
        public UnityEngine.UI.Image[] CounterSprite = new UnityEngine.UI.Image[3];
        public UnityEngine.UI.Text[] CounterText = new UnityEngine.UI.Text[3];
        public GameObject[] Tutorial = new GameObject[10];
        public Animation[] TutorialAnim = new Animation[12];
        public UnityEngine.UI.Image YearSprite;
        public UnityEngine.UI.Text YearText;
        public GameObject TutorialButtons;
        public GameObject ButtonDimScreen;
        public UnityEngine.UI.Text TutorialText;
        public Transform TopCardPosition;
        public Transform ResourceCards;
    }

    [System.Serializable]
    public class LevelStuff
    {
        public int PositionInTalking;
        public int PositionInEnding;
        public bool FinishTalk;
        public bool FinishTalkEnding;
        public bool SpeechLoaded;
        public OneLevelSpeech[] SpeechBubble = new OneLevelSpeech[10];
        public GameObject[] SpeechObjects = new GameObject[3];
        public Animation[] SpeechAnimations = new Animation[3];
        public UnityEngine.UI.Button[] LevelButtons = new UnityEngine.UI.Button[3];
        public GameObject[] LevelLocks = new GameObject[3];
        public GameObject[] LevelGreyed = new GameObject[3];
    }

    [System.Serializable]
    public class OneLevelSpeech
    {
        public string Level;
        public OneSpeechInfo[] Speech = new OneSpeechInfo[10];
        public OneSpeechInfo[] SpeechEnding = new OneSpeechInfo[3];
        public int CurrentYear;
        public int TargetYear;
        public string BcOrAd;
        public string LevelObjective;
        public int Morale;
        public int HumanTies;
        public int FoodConsumption;
        public int FoodProduction;
        public int StartingMorale;
        public int StartingFood;
        public int StartingMoraleHuman;
        public int StartingFoodHuman;
        public int StartingHumanTies;
    }

    [System.Serializable]
    public class OneSpeechInfo
    {
        public string Text;
        public float[] Size = new float[2];
    }

    [System.Serializable]
    public class PlayerProgressData
    {
        public Animation[] UnlockAnimDisplay = new Animation[3];
        public bool[] ClearedLevelCheck = new bool[3];
        public bool[] ShowUnlockAnimCheck = new bool[3];
    }

    [System.Serializable]
    public class ComponentReference
    {
        public GameObject CreditsPage;
        public GameObject SettingsPage;
        public SpriteRenderer BackgroundMainMenu;
        public Sprite[] BackgroundMainMenuImages = new Sprite[2];
        public Transform BackgroundMainMenuTransform;
    }
}
