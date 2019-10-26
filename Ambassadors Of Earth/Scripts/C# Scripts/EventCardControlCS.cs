using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCardControlCS : MonoBehaviour {

    public CameraControlCS.ScriptReference Scripts;
    public ComponentReference Components;
    public CardCounting ShuffleCount;
    public ImportantVars Important;
    public AllEventCards[] AllEventCardsInGame = new AllEventCards[1];
    public AllEventCardSprites EventDisplay;
    public ImportantVars ImportantReset;
    public AllEventCards AllEventCardsReset;
    public VictoryCondition Victory;
    public bool ShuffleCheck = false;

    private bool FinalYearShownCheck;
    private int EventCardPlayed;
    private Vector3 MovePoint;
    private Vector3 Pos;
    private Vector3 Scale;
    private int FirstEventFixedCheck;

    private int DisplayedFoodConsumption;

    void Start () {
		
	}

    public void LaunchTutorial()
    {
        ShuffleCount.NumberOfShuffles = 0;
        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[8].SetActive(false);
        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[9].SetActive(false);
        Scripts.UIControllerScript.UpdateRoundNumber(ShuffleCount.NumberOfShuffles + 1);
        Pos = Scripts.MainMenuControllerScript.TutorialUse.ResourceCards.position;
        Pos.z = 15;
        Scripts.MainMenuControllerScript.TutorialUse.ResourceCards.position = Pos;

        for (int ClearCard = 0; ClearCard < Important.CardToUse.Length; ClearCard++)
        {
            Important.CardToUse[ClearCard].CardIdentifier[0] = 0;
            Important.CardToUse[ClearCard].CardIdentifier[1] = 0;
            Important.CardToUse[ClearCard].SpriteDisplayIdentifier[0] = 0;
            Important.CardToUse[ClearCard].SpriteDisplayIdentifier[1] = 0;
        }

        for (int ClearCard = 0; ClearCard < 10; ClearCard++)
        {
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

    public IEnumerator InitialiseNewGame()
    {
        ShuffleCount.NumberOfShuffles = 0;
        ShuffleCount.FinishingLastCards = false;
        Components.EventCard.transform.position = new Vector3(22.25f, 0.05f, 1.51f);

        Important.PositiveHumanEvent[0] = 0;
        Important.NegativeHumanEvent[0] = 0;
        Important.LowMoraleEvent[0] = 0;
        Important.LowFoodEvent[0] = 0;
        Important.PositiveHumanEvent[1] = 0;
        Important.NegativeHumanEvent[1] = 0;
        Important.LowMoraleEvent[1] = 0;
        Important.LowFoodEvent[1] = 0;
        Important.FoodProduction = 0;
        Important.FoodConsumption = 0;
        Important.Morale = 0;
        Important.HumanTies = 0;
        UpdateHumanRelationship(0);
        FinalYearShownCheck = false;

        if (Scripts.MainMenuControllerScript.LevelSelected == 1)
        {
            ShuffleCheck = false;
            StartCoroutine(ShuffleCard());
            yield return new WaitForSeconds(0.1f);
            while (ShuffleCheck == false)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Scripts.HandCardRotationScript.Important.NoClicking = false;
        }
    }

    private bool StartMovingCheck = false;
    private float LerpProgress = 0;

    void Update () {

        if (StartMovingCheck == true)
        {
            LerpProgress += Time.deltaTime / 1.5f;
            Components.EventCard.transform.localPosition = Vector3.Lerp(Components.EventCard.transform.localPosition, MovePoint, LerpProgress);

            if (LerpProgress >= 1)
            {
                StartMovingCheck = false;
            }
        }
    }

    public void EventCardClicked()              // Clicking is taken from HandCardRotation
    { 
        Important.EventCardSelected = true;
        UpdateOptionCardInfo();
        LerpProgress = 0;
        StartMovingCheck = true;
        MovePoint = new Vector3(-0.61f, 0, -10.2f);
        Scripts.OptionCardScripts[0].MoveOnClick();
        Scripts.OptionCardScripts[1].MoveOnClick();
        Scripts.OptionCardScripts[2].MoveOnClick();
    }

    public void EventCardDeselected()              // Clicking is taken from HandCardRotation
    { 
        LerpProgress = 0;
        Important.EventCardSelected = false;
        StartMovingCheck = true;
        MovePoint = new Vector3(-0.61f, 0.092f, -3.9f);
        Scripts.OptionCardScripts[0].MoveBackOnClick();
        Scripts.OptionCardScripts[1].MoveBackOnClick();
        Scripts.OptionCardScripts[2].MoveBackOnClick();
    }

    public void BattleCardClicked()
    {
        LerpProgress = 0;
        //Scripts.CharacterBattleControllerScript[0].MoveToActivePosition();
        //Scripts.CharacterBattleControllerScript[1].MoveToActivePosition();
        Important.EventCardSelected = true;
        StartMovingCheck = true;
        MovePoint = new Vector3(-0.61f, 0, -10.2f);
    }

    public IEnumerator BattleCardDeselected()                 // Clicking is taken from HandCardRotation
    { 

        //if (Scripts.NatureControllerScript.BattleOptionChooseCheck == true) return;
        LerpProgress = 0;
        //Scripts.CharacterBattleControllerScript[0].MoveToInactivePosition();
        //Scripts.CharacterBattleControllerScript[1].MoveToInactivePosition();
        Important.EventCardSelected = false;
        yield return new WaitForSeconds(0.5f);
        StartMovingCheck = true;
        MovePoint = new Vector3(-0.61f, 0.092f, -3.9f);
    }

    public IEnumerator BattleCardRemoved()
    {
        Scripts.HandCardRotationScript.SortHandCards();
        LerpProgress = 0;
        this.gameObject.tag = "EventCards";
        Important.SpecialCardActivated = 0;
        Important.EventCardSelected = false;
        //Scripts.CharacterBattleControllerScript[0].MoveToInactivePosition();
        //Scripts.CharacterBattleControllerScript[1].MoveToInactivePosition();
        yield return new WaitForSeconds(0.5f);
        StartMovingCheck = true;
        MovePoint = new Vector3(-0.61f, 0.092f, -3.9f);
        yield return new WaitForSeconds (1);
        Components.anim.Play("RemoveCard");
        yield return new WaitForSeconds (1.183f);
        //EventCardManager();
    }

    public IEnumerator EventCardRemoved()
    {

        Scripts.HandCardRotationScript.SortHandCards();
        LerpProgress = 0;
        Important.EventCardSelected = false;
        StartMovingCheck = true;
        MovePoint = new Vector3(-0.61f, 0.092f, -3.9f);
        Scripts.OptionCardScripts[0].MoveBackOnClick();
        Scripts.OptionCardScripts[1].MoveBackOnClick();
        Scripts.OptionCardScripts[2].MoveBackOnClick();
        yield return new WaitForSeconds (1);

        if (Important.ImmidiateDrawCardNumber == 0)
        {
            ShuffleCount.PositionInShuffle += 1;
            Scripts.UIControllerScript.InformationOverviewText[7].text = "Events This Year: " + (ShuffleCount.PositionInShuffle + 1) + "/10";
        }

        if (Important.ImmidiateDrawCardNumber != 0 && Scripts.HandCardRotationScript.Important.SelectedOptionCardNumber == 2)
        {
            StartCoroutine(Scripts.CameraControlScript.LoseGame("Defeated", "Always keep track of your life and\nchaos cards to avoid this fate!",0));
            Components.anim.Play("RemoveCard");
            yield break;
        }

        if (Important.DestroyEventCheck == false)
        {
            Components.anim.Play("RemoveCard");
        }

        if (Important.DestroyEventCheck == true)
        {
            Important.DestroyEventCheck = false;
            Components.anim.Play("DestroyCard");
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds (1.183f);
        if (Scripts.MainMenuControllerScript.LevelSelected == 1)
        {
            StartCoroutine(Scripts.MainMenuControllerScript.ClearPrevious());
            yield break;
        }
        StartCoroutine(EventCardManager());
    }

    public IEnumerator AnnualReportRemoved()
    {
        float TempFloat = Important.FoodProduction - DisplayedFoodConsumption;
        Scripts.NatureControllerScript.IncreaseNatureFood(TempFloat / 100);
        UpdateHumanRelationship(Important.HumanTies);
        Scripts.NatureControllerScript.UpdateMorale(Important.Morale);

        Components.AnnualReportAnim.Play("RemoveAnnualReport");
        ShuffleCount.PositionInShuffle++;

        yield return new WaitForSeconds(0.50f);
        Components.AnnualReportAnim.Play("RemoveCard");
        yield return new WaitForSeconds(1.183f);
        StartCoroutine(EventCardManager());
    }

    public IEnumerator FinalReportRemoved()
    {
        Components.FinalReportAnim.Play("RemoveAnnualReport");
        yield return new WaitForSeconds(0.50f);
        Components.FinalReportAnim.Play("RemoveCard");
        yield return new WaitForSeconds(1.183f);
        EndGameVictoryCheck();
    }

    private int[] RandomlyDestroyedCardNumbers = new int[3];

    public IEnumerator EventCardManager()
    {
        if (Important.ImmidiateDrawCardNumber != 0) Important.ImmidiateDrawCardNumber = 0;

        Scripts.OptionClickControlScript.Components.AnimationStatsChange[0].SetActive(false);
        Scripts.OptionClickControlScript.Components.AnimationStatsChange[1].SetActive(false);
        Scripts.OptionClickControlScript.Components.AnimationStatsChange[2].SetActive(false);
        Scripts.OptionClickControlScript.Components.AnimationStatsChange[3].SetActive(false);
        Components.MissionSpriteHolder.SetActive(false);

        Components.OptionCardEffect[0].text = "";
        Components.OptionCardEffect[1].text = "";
        Components.OptionCardEffect[2].text = "";

        if (Scripts.HandCardRotationScript.Important.NumberOfCards > 12)
        {
            RandomlyDestroyedCardNumbers[0] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards); //random.range not inclusive of max integer
            RandomlyDestroyedCardNumbers[1] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);
            RandomlyDestroyedCardNumbers[2] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);

            while (RandomlyDestroyedCardNumbers[1] == RandomlyDestroyedCardNumbers[0]) RandomlyDestroyedCardNumbers[1] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);
            while (RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[0] || RandomlyDestroyedCardNumbers[2] == RandomlyDestroyedCardNumbers[1]) RandomlyDestroyedCardNumbers[2] = Random.Range(0, Scripts.HandCardRotationScript.Important.NumberOfCards);
            Components.EventCardRenderer.sprite = EventDisplay.Miscellaneous.Position[0].SpriteGroup[3];
            Components.EventCardTitle.text = "Overload";
            Components.EventCardDescription.text = "Discard 3 random resource cards\nfrom your hand";
            Components.EventCardType.text = "Special Card";
            this.gameObject.tag = "NoOptionEventCard";
            Components.anim.Play("DrawCard");
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
            yield return new WaitForSeconds(1.183f);
            Scripts.HandCardRotationScript.DestroyExtra3Cards(RandomlyDestroyedCardNumbers[0], RandomlyDestroyedCardNumbers[1], RandomlyDestroyedCardNumbers[2]);
            yield break;
        }

        if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] == 0)
        {
            Important.ImmidiateDrawCardNumber = 3;
            StartCoroutine(LoadCardInfo(3));
            yield break;
        }

        if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[4] >= 3)
        {
            Important.ImmidiateDrawCardNumber = 4;
            StartCoroutine(LoadCardInfo(4));
            yield break;
        }

        if (Important.SpecialCardActivated == 1)
        {
            Components.EventCardRenderer.sprite = EventDisplay.Miscellaneous.Position[1].SpriteGroup[3];
            Components.EventCardTitle.text = "The Struggle";
            Components.EventCardDescription.text = "Humans and beasts face off in\nan intense skirmish.";
            Components.EventCardType.text = "Special Card";
            this.gameObject.tag = "BattleCard";
            Components.anim.Play("DrawCard");
            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
            yield return new WaitForSeconds(1.183f);
            Scripts.HandCardRotationScript.Important.NoClicking = false;
            yield break;
        }

        if (ShuffleCount.PositionInShuffle > ShuffleCount.NumberOfCardsBeforeShuffle)
        {

            ShuffleCount.NumberOfShuffles++;
            ShuffleCheck = false;
            StartCoroutine(ShuffleCard());
            yield return new WaitForSeconds(3);
            Components.HideWhileShuffling[0].SetActive(true);
            Components.HideWhileShuffling[1].SetActive(true);
            Components.HideWhileShuffling[2].SetActive(true);
            Components.ShufflingCardsAnimationObject.SetActive(false);

            if (Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].RoundsNumber <= ShuffleCount.NumberOfShuffles)
            {
                yield break;
            }

            while (ShuffleCheck == false)
            {
                yield return null;
            }
        }

        if (ShuffleCount.PositionInShuffle == ShuffleCount.NumberOfCardsBeforeShuffle)
        {
            if (ShuffleCount.FinishingLastCards == true)
            {
                StartCoroutine(FinalReport());//EndGameVictoryCheck();
                ShuffleCount.FinishingLastCards = false;
                yield break;
            }

            string Sign;
            
            Components.AnnualReportText[0].text = "Food Production: " + Important.FoodProduction;

            Important.TempFoodConsumption = Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0];
            DisplayedFoodConsumption = Important.TempFoodConsumption + Important.FoodConsumption;
            Components.AnnualReportText[1].text = "Food Consumption: " + DisplayedFoodConsumption;

            if (Important.FoodProduction - DisplayedFoodConsumption > 0) Sign = "+";
            else Sign = "";
            Components.AnnualReportText[2].text = "Food: " + Sign + (Important.FoodProduction - DisplayedFoodConsumption);

            if (Important.Morale > 0) Sign = "+";
            else Sign = "";
            Components.AnnualReportText[3].text = "Morale: " + Sign + Important.Morale;

            if (Important.HumanTies > 0) Sign = "+";
            else Sign = "";
            Components.AnnualReportText[4].text = "Human Ties: " + Sign + Important.HumanTies;

            Components.AnnualReportAnim.Play("DrawCard");
            yield return new WaitForSeconds(1.183f);
            Components.AnnualReportAnim.Play("ShowAnnualReport");
            yield return new WaitForSeconds(0.3f);
            Scripts.HandCardRotationScript.Important.NoClicking = false;
            yield break;
        }

        if (Scripts.MainMenuControllerScript.LevelSelected != 1)
        {
            if (ShuffleCount.NumberOfShuffles != 0 && ShuffleCount.NumberOfShuffles + 1 == Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].RoundsNumber && FinalYearShownCheck == false)
            {
                StartCoroutine(Scripts.MiscellaneousGameManagementScript.NewNotice(1, "Final Year", "Be sure to meet the objectives\nby the end of this year!", "Notice"));
                FinalYearShownCheck = true;
                yield break;
            }
        }

        EventCardPlayed = Important.EventCardOrder[ShuffleCount.PositionInShuffle];

        while (Important.CardToUse[EventCardPlayed - 1].CardIdentifier[0] == 0)     //Check if it is a blank card
        {
            ShuffleCount.PositionInShuffle += 1;
            ShuffleCount.NumberOfCardsBeforeShuffle += 1;
            EventCardPlayed = Important.EventCardOrder[ShuffleCount.PositionInShuffle];
        }

        if (Important.HumanRelationship <= -10 && Important.CardToUse[EventCardPlayed - 1].CardPriority > -1 && Important.NegativeHumanEvent[1] < Important.NegativeHumanEvent[0])         //Negative Human Relation
        {
            float RollLuck = Random.Range(0.0f, 1.0f);
            if (RollLuck >= 0.9f + 0.02f * (Important.HumanRelationship + 10))
            {
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[0] = 3;
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[1] = 1;
                Important.CardToUse[EventCardPlayed - 1].CardPriority = -1;
                Important.NegativeHumanEvent[1]++;
            }
        }

        if (Important.HumanRelationship >= 10 && Important.CardToUse[EventCardPlayed - 1].CardPriority > -1 && Important.PositiveHumanEvent[1] < Important.PositiveHumanEvent[0])          //Positive Relation
        {
            float RollLuck = Random.Range(0.0f, 1.0f);

            if (RollLuck >= 0.9f - 0.02f * (Important.HumanRelationship - 10))
            {
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[0] = 4;
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[1] = 1;
                Important.CardToUse[EventCardPlayed - 1].CardPriority = -1;
                Important.PositiveHumanEvent[1]++;
            }
        }

        if (Scripts.NatureControllerScript.Health < 8 && Important.CardToUse[EventCardPlayed - 1].CardPriority > -1 && Important.LowMoraleEvent[1] < Important.LowMoraleEvent[0])          //Low Morale
        { 
            float RollLuck = Random.Range(0.0f, 1.0f);
            if (Scripts.NatureControllerScript.Health == 0) RollLuck = 1;   //100% chance to draw low morale card if morale = 0.

            if (RollLuck >= 0.9f - 0.1f * (7.5f - Scripts.NatureControllerScript.Health))
            {
                int RollNumberMorale;
                RollNumberMorale = Random.Range(1, 4);
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[0] = 2;
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[1] = RollNumberMorale;
                Important.CardToUse[EventCardPlayed - 1].CardPriority = -1;
                Important.LowMoraleEvent[1]++;
            }
        }

        if (Scripts.NatureControllerScript.CurrentFood < 40 && Important.CardToUse[EventCardPlayed - 1].CardPriority > -1 && Important.LowFoodEvent[1] < Important.LowFoodEvent[0])
        {
            float RollLuck = Random.Range(0.0f, 1.0f);
            if (Scripts.NatureControllerScript.CurrentFood == 0) RollLuck = 1;   //100% chance to draw low food card if morale = 0.

            if (RollLuck >= 0.9f - 0.02f * (35 - Scripts.NatureControllerScript.CurrentFood))
            {

                int RollNumberFood;
                RollNumberFood = Random.Range(16, 19);
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[0] = 2;
                Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[1] = RollNumberFood;
                Important.CardToUse[EventCardPlayed - 1].CardPriority = -1;
                Important.LowFoodEvent[1]++;
            }
        }

        int TechLevel;
        int SpritePosition;
        TechLevel = Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[0];
        SpritePosition = Important.CardToUse[EventCardPlayed - 1].SpriteDisplayIdentifier[1];

        Components.EventCardRenderer.sprite = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SpriteGroup[3];
        Components.EventCardTitle.text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].EventCardText[0];
        Components.EventCardDescription.text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].EventCardText[1];
        Components.EventCardDescription.text = Components.EventCardDescription.text.Replace("NWL", "\n");
        Components.EventCardType.text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].EventCardText[2];
        Scripts.OptionClickControlScript.EventDetails.EnableAnimation = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].CalledCard.EnableAnimation; //Shuffle in animation
        Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[0] = TechLevel;
        Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[1] = SpritePosition;

        if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MissionProgress != 0)
        {
            Components.MissionSpriteHolder.SetActive(true);
            Components.MissionSpriteRenderers[0].sprite = Components.MissionSprites[1];

            if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MissionProgress == 1)
            {
                Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[0];
                Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
            }

            if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MissionProgress == 2)
            {
                Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
                Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
            }

            if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MissionProgress == 3)
            {
                Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
                Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[1];
            }
        }

        for (var i = 0; i < 3; i++)
        {    
            Scripts.OptionClickControlScript.EventDetails.TechnologyRequired[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SkillsRequirements[i];
            Scripts.OptionClickControlScript.EventDetails.ResourceRequirements[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].CardRequirements[i];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].ResourcesUsed[i];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i + 3] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].ResourcesUsed[i + 3];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i + 6] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].ResourcesUsed[i + 6];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].ResourcesGiven[i];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i + 3] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].ResourcesGiven[i + 3];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i + 6] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].ResourcesGiven[i + 6];
            Scripts.OptionClickControlScript.EventDetails.StatsChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].StatsChange[i];
            Scripts.OptionClickControlScript.EventDetails.StatsChange[i + 3] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].StatsChange[i + 3];
            Scripts.OptionClickControlScript.EventDetails.UnlockOptionNumber[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].UnlockOptionNumber[i];
            Scripts.OptionClickControlScript.EventDetails.RemoveEventCheck[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].RemoveEventCheck[i];
            Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].CalledCard.TechLevel[i];
            Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].CalledCard.Position[i];
            Scripts.OptionClickControlScript.EventDetails.ChanceToAddCard[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].CalledCard.ChanceToAddCard[i];
            Scripts.OptionClickControlScript.EventDetails.DiscardCards[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].DiscardCards[i];
            Scripts.OptionClickControlScript.EventDetails.LoseCheck[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].LoseCheck[i];
            Scripts.OptionClickControlScript.EventDetails.PromotionSprite[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].PromotionSprite[i];
            Scripts.OptionClickControlScript.EventDetails.PromotionText[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].PromotionText[i];
            Scripts.OptionClickControlScript.EventDetails.PromotionText[i + 3] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].PromotionText[i + 3];
            Scripts.OptionClickControlScript.EventDetails.HumanRelationshipChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].HumanRelationshipChange[i];
            Scripts.OptionClickControlScript.EventDetails.FoodChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].FoodChange[i];
            Scripts.OptionClickControlScript.EventDetails.Combat.WarCheck[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarCheck[i];
            Scripts.OptionClickControlScript.EventDetails.Combat.EnemyOffense[i] = Random.Range(EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.EnemyOffense[i], EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.EnemyOffense[i+3]+1);    //Makes min and max inclusive
            Scripts.OptionClickControlScript.EventDetails.Combat.EnemyDefence[i] = Random.Range(EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.EnemyDefence[i], EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.EnemyDefence[i+3]+1);    //Makes min and max inclusive
            Scripts.OptionClickControlScript.EventDetails.Combat.WarVictory[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarVictory[i];
            Scripts.OptionClickControlScript.EventDetails.Combat.CombatQuote[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.CombatQuote[i];
            Scripts.OptionClickControlScript.EventDetails.Combat.CombatQuote[i+3] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.CombatQuote[i+3];
            Scripts.OptionClickControlScript.EventDetails.MilitaryChange.OffenseChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MilitaryChange.OffenseChange[i];
            Scripts.OptionClickControlScript.EventDetails.MilitaryChange.DefenceChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MilitaryChange.DefenceChange[i];
            Scripts.OptionClickControlScript.EventDetails.MilitaryChange.PermOffenseChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MilitaryChange.PermOffenseChange[i];
            Scripts.OptionClickControlScript.EventDetails.MilitaryChange.PermDefenceChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MilitaryChange.PermDefenceChange[i];

            Scripts.OptionClickControlScript.EventDetails.FoodProductionChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].FoodProductionChange[i];
            Scripts.OptionClickControlScript.EventDetails.MoraleProductionChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].MoraleProductionChange[i];
            Scripts.OptionClickControlScript.EventDetails.HumanTiesProductionChange[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].HumanTiesProductionChange[i];

            Scripts.HandCardRotationScript.Components.OptionCardEffectExplanation[i] = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].OptionCardEffectExplanation[i];

            Components.OptionCardRenderers[i].sprite = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SpriteGroup[i];
            Components.LockedOptionBlacken[i].SetActive(false);
            Components.OptionCardTitleRenderers[i].text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].OptionCardTitle[i];
            Components.OptionCardTitleRenderers[i].text = Components.OptionCardTitleRenderers[i].text.Replace("NWL", "\n");
            Components.OptionCardHolder[i].tag = "OptionCards";
            Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
            Scale = Components.OptionCardQuoteSize[i].localScale;
            Scale.x = 0.07f;
            Scale.y = 0.07f;
            Components.OptionCardQuoteSize[i].localScale = Scale;
            Components.OptionCardQuoteRenderers[i].text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].OptionCardQuote[i];
            Components.OptionCardQuoteRenderers[i].text = Components.OptionCardQuoteRenderers[i].text.Replace("NWL", "\n");
            Components.OptionCardEffect[i].text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].OptionCardEffect[i];
            Components.OptionCardEffect[i].text = Components.OptionCardEffect[i].text.Replace("NWL", "\n");

            if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarCheck[i] == true)     //Change appearance of any military effect cards
            {
                Pos = Components.ResourceChangeGameObject[i].transform.localPosition;
                Pos.z = 2.13f;
                Components.ResourceChangeGameObject[i].transform.localPosition = Pos;
                Components.ResourceChangeSprite[i].sprite = Components.ResourceChange[1];

                if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarEffect[i] != "")
                {
                    Pos = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale;
                    Pos.x = 0.08f;
                    Pos.y = 0.08f;
                    Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale = Pos;
                    Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarEffect[i] + "\n";
                }

            }

            else if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarCheck[i] == false)
            {
                Pos = Components.ResourceChangeGameObject[i].transform.localPosition;
                Pos.z = 2.18f;
                Components.ResourceChangeGameObject[i].transform.localPosition = Pos;
                Components.ResourceChangeSprite[i].sprite = Components.ResourceChange[0];
                Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text = "– NA –" + "\n";
                Pos = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale;
                Pos.x = 0.1f;
                Pos.y = 0.1f;
                Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale = Pos;
            }

            if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].UnlockOptionNumber[i] == -1)    //Check if any option is locked
            {   
                Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
                Components.OptionCardHolder[i].tag = "LockedOption";
            }

            if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SpriteGroup[i] == null)      //For Standard Cards to check if any option is not present.
            {      
                Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
                Components.OptionCardTitleRenderers[i].text = "Option Unavailable";
                Components.OptionCardHolder[i].tag = "LockedOption";
                Scale = Components.OptionCardQuoteSize[i].localScale;
                Scale.x = 0.07f;
                Scale.y = 0.07f;
                Components.OptionCardQuoteSize[i].localScale = Scale;
                Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
                Components.OptionCardQuoteRenderers[i].text = "<i>'Make an empty space and\ncreativity will instantly fill it.'</i>";
            }
        }

        Scripts.OptionClickControlScript.EventDetails.SpecialCardNumber = EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SpecialCardNumber;

        if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SpecialCardNumber == 1) // For Military Model Event Card, SpecialCardNumber == 1
        {
            for (int i = 0; i<3; i++)
            {
                Components.ResourceChangeSprite[i].sprite = Components.ResourceChange[2];
                Pos = Components.ResourceChangeGameObject[i].transform.localPosition;
                Pos.z = 2.85f;
                Components.ResourceChangeGameObject[i].transform.localPosition = Pos;
                Scale = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale;
                Scale.x = 0.075f;
                Scale.y = 0.075f;
                Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale = Scale;
                Pos = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localPosition;
                Pos.y = -136.5f;
                Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localPosition = Pos;
            }

            Scripts.OptionClickControlScript.Components.ResourceReqDisplay[0].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text = "Defensive Strength: 5" + "\n" + "Offensive Strength: 1" + "\n" + "Food Consumption: 1";
            Scripts.OptionClickControlScript.Components.ResourceReqDisplay[1].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text = "Defensive Strength: 5" + "\n" + "Offensive Strength: 5" + "\n" + "Food Consumption: 10";
            Scripts.OptionClickControlScript.Components.ResourceReqDisplay[2].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text = "Defensive Strength: 8" + "\n" + "Offensive Strength: 6" + "\n" + "Food Consumption: 20";
        }

        if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].SpecialCardNumber != 1)
        {
            for (int i = 0; i < 3; i++)
            {
  
                if (EventDisplay.Normal[TechLevel - 1].Position[SpritePosition - 1].Combat.WarCheck[i] == true)
                {
                    Components.ResourceChangeSprite[i].sprite = Components.ResourceChange[1];
                    Pos = Components.ResourceChangeGameObject[i].transform.localPosition;
                    Pos.z = 2.13f;
                    Components.ResourceChangeGameObject[i].transform.localPosition = Pos;
                    Pos = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale;
                    Pos.x = 0.08f;
                    Pos.y = 0.08f;
                    Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale = Pos;
                }

                else
                {
                    Components.ResourceChangeSprite[i].sprite = Components.ResourceChange[0];
                    Pos = Components.ResourceChangeGameObject[i].transform.localPosition;
                    Pos.z = 2.18f;
                    Components.ResourceChangeGameObject[i].transform.localPosition = Pos;
                    Scale = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale;
                    Scale.x = 0.1f;
                    Scale.y = 0.1f;
                    Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale = Scale;
                }
                
                Pos = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localPosition;
                Pos.y = -120.7f;
                Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localPosition = Pos;
            }
        }

        if (Scripts.MainMenuControllerScript.TutorialUse.UpdateEventClickPrevious == true)
        {
            UpdateOptionCardInfo();
            yield break;
        }

        Components.anim.Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        if (Scripts.MainMenuControllerScript.LevelSelected != 1) Important.CardsInDeck[SpritePosition - 1].CardPriority = 0;

        yield return new WaitForSeconds(1.183f);
        Scripts.HandCardRotationScript.Important.NoClicking = false;
    }

    public IEnumerator LoadCardInfo(int PositionNumber)
    {
        for (int i = 0; i < 3; i++)     //Make them normal (Non combat) resource change holders
        {
            Pos = Components.ResourceChangeGameObject[i].transform.localPosition;
            Pos.z = 2.18f;
            Components.ResourceChangeGameObject[i].transform.localPosition = Pos;
            Components.ResourceChangeSprite[i].sprite = Components.ResourceChange[0];
            Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].GetComponent<UnityEngine.UI.Text>().text = "– NA –" + "\n";
            Pos = Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale;
            Pos.x = 0.1f;
            Pos.y = 0.1f;
            Scripts.OptionClickControlScript.Components.ResourceReqDisplay[i].LostDisplay[0].GainDisplay[0].transform.localScale = Pos;
        }

        var ReferencePosition = EventDisplay.Miscellaneous.Position[PositionNumber - 1];
        Components.EventCardRenderer.sprite = ReferencePosition.SpriteGroup[3];
        Components.EventCardTitle.text = ReferencePosition.EventCardText[0];
        Components.EventCardDescription.text = ReferencePosition.EventCardText[1];
        Components.EventCardDescription.text = Components.EventCardDescription.text.Replace("NWL", "\n");
        Components.EventCardType.text = ReferencePosition.EventCardText[2];
        Scripts.OptionClickControlScript.EventDetails.EnableAnimation = ReferencePosition.CalledCard.EnableAnimation; //Shuffle in animation
        Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[0] = 0;
        Scripts.OptionClickControlScript.EventDetails.SpriteDisplayIdentifier[1] = PositionNumber;

        if (ReferencePosition.MissionProgress != 0)
        {
            Components.MissionSpriteHolder.SetActive(true);
            Components.MissionSpriteRenderers[0].sprite = Components.MissionSprites[1];

            if (ReferencePosition.MissionProgress == 1)
            {
                Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[0];
                Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
            }

            if (ReferencePosition.MissionProgress == 2)
            {
                Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
                Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[0];
            }

            if (ReferencePosition.MissionProgress == 3)
            {
                Components.MissionSpriteRenderers[1].sprite = Components.MissionSprites[1];
                Components.MissionSpriteRenderers[2].sprite = Components.MissionSprites[1];
            }
        }

        for (var i = 0; i < 3; i++)
        {

            Scripts.OptionClickControlScript.EventDetails.TechnologyRequired[i] = ReferencePosition.SkillsRequirements[i];
            Scripts.OptionClickControlScript.EventDetails.ResourceRequirements[i] = ReferencePosition.CardRequirements[i];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i] = ReferencePosition.ResourcesUsed[i];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i + 3] = ReferencePosition.ResourcesUsed[i + 3];
            Scripts.OptionClickControlScript.EventDetails.ResourcesUsed[i + 6] = ReferencePosition.ResourcesUsed[i + 6];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i] = ReferencePosition.ResourcesGiven[i];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i + 3] = ReferencePosition.ResourcesGiven[i + 3];
            Scripts.OptionClickControlScript.EventDetails.ResourceGained[i + 6] = ReferencePosition.ResourcesGiven[i + 6];
            Scripts.OptionClickControlScript.EventDetails.StatsChange[i] = ReferencePosition.StatsChange[i];
            Scripts.OptionClickControlScript.EventDetails.StatsChange[i + 3] = ReferencePosition.StatsChange[i + 3];
            Scripts.OptionClickControlScript.EventDetails.UnlockOptionNumber[i] = ReferencePosition.UnlockOptionNumber[i];
            Scripts.OptionClickControlScript.EventDetails.RemoveEventCheck[i] = ReferencePosition.RemoveEventCheck[i];
            Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[i] = ReferencePosition.CalledCard.TechLevel[i];
            Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[i] = ReferencePosition.CalledCard.Position[i];
            Scripts.OptionClickControlScript.EventDetails.ChanceToAddCard[i] = ReferencePosition.CalledCard.ChanceToAddCard[i];
            Scripts.OptionClickControlScript.EventDetails.DiscardCards[i] = ReferencePosition.DiscardCards[i];
            Scripts.OptionClickControlScript.EventDetails.LoseCheck[i] = ReferencePosition.LoseCheck[i];
            Scripts.OptionClickControlScript.EventDetails.PromotionSprite[i] = ReferencePosition.PromotionSprite[i];
            Scripts.OptionClickControlScript.EventDetails.PromotionText[i] = ReferencePosition.PromotionText[i];
            Scripts.OptionClickControlScript.EventDetails.PromotionText[i + 3] = ReferencePosition.PromotionText[i + 3];
            Scripts.OptionClickControlScript.EventDetails.HumanRelationshipChange[i] = ReferencePosition.HumanRelationshipChange[i];
            Scripts.OptionClickControlScript.EventDetails.FoodChange[i] = ReferencePosition.FoodChange[i];
            Scripts.OptionClickControlScript.EventDetails.Combat.WarCheck[i] = ReferencePosition.Combat.WarCheck[i];
            Scripts.OptionClickControlScript.EventDetails.MilitaryChange.OffenseChange[i] = ReferencePosition.MilitaryChange.OffenseChange[i];
            Scripts.OptionClickControlScript.EventDetails.MilitaryChange.DefenceChange[i] = ReferencePosition.MilitaryChange.DefenceChange[i];

            Scripts.OptionClickControlScript.EventDetails.FoodProductionChange[i] = ReferencePosition.FoodProductionChange[i];
            Scripts.OptionClickControlScript.EventDetails.MoraleProductionChange[i] = ReferencePosition.MoraleProductionChange[i];
            Scripts.OptionClickControlScript.EventDetails.HumanTiesProductionChange[i] = ReferencePosition.HumanTiesProductionChange[i];

            Components.OptionCardRenderers[i].sprite = ReferencePosition.SpriteGroup[i];
            Components.LockedOptionBlacken[i].SetActive(false);
            Components.OptionCardTitleRenderers[i].text = ReferencePosition.OptionCardTitle[i];
            Components.OptionCardTitleRenderers[i].text = Components.OptionCardTitleRenderers[i].text.Replace("NWL", "\n");
            Components.OptionCardHolder[i].tag = "OptionCards";
            Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
            Scale = Components.OptionCardQuoteSize[i].localScale;
            Scale.x = 0.07f;
            Scale.y = 0.07f;
            Components.OptionCardQuoteSize[i].localScale = Scale;
            Components.OptionCardQuoteRenderers[i].text = ReferencePosition.OptionCardQuote[i];
            Components.OptionCardQuoteRenderers[i].text = Components.OptionCardQuoteRenderers[i].text.Replace("NWL", "\n");
            Components.OptionCardEffect[i].text = ReferencePosition.OptionCardEffect[i];
            Components.OptionCardEffect[i].text = Components.OptionCardEffect[i].text.Replace("NWL", "\n");

            if (ReferencePosition.UnlockOptionNumber[i] == -1)          //Check if any option is locked
            {   
                Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
                Components.OptionCardHolder[i].tag = "LockedOption";
            }

            if (ReferencePosition.SpriteGroup[i] == null)                //For Standard Cards to check if any option is not present.
            {      
                Components.OptionCardRenderers[i].sprite = Components.LockedOptionSprite;
                Components.OptionCardTitleRenderers[i].text = "Option Unavailable";
                Components.OptionCardHolder[i].tag = "LockedOption";
                Scale = Components.OptionCardQuoteSize[i].localScale;
                Scale.x = 0.07f;
                Scale.y = 0.07f;
                Components.OptionCardQuoteSize[i].localScale = Scale;
                Components.OptionCardQuoteRenderers[i].font = Components.Fonts[1];
                Components.OptionCardQuoteRenderers[i].text = "<i>'Make an empty space and\ncreativity will instantly fill it.'</i>";
            }
        }

        Components.anim.Play("DrawCard");
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[1]);
        yield return new WaitForSeconds(1.183f);
        Scripts.HandCardRotationScript.Important.NoClicking = false;
    }

    public IEnumerator FinalReport()
    {
        switch (Scripts.MainMenuControllerScript.LevelSelected)
        {
            case 2:
                Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].VictoryCheck = true;
                Components.FinalReportText[1].text = "Possess at least 5 life\nresources by 9997BC.";
                Components.FinalReportText[3].text = "Secure at least 3 mineral\nresources by 9997BC.";

                if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[0] < 5)
                {
                    Components.ObjectiveCheck[0].sprite = Components.TickCross[1];
                    Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].VictoryCheck = false;
                }
                else
                {
                    Components.ObjectiveCheck[0].sprite = Components.TickCross[0];
                }

                if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[1] < 3)
                {
                    Components.ObjectiveCheck[1].sprite = Components.TickCross[1];
                    Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].VictoryCheck = false;
                }
                else
                {
                    Components.ObjectiveCheck[1].sprite = Components.TickCross[0];
                }

                break;

            case 3:
                Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].VictoryCheck = false;
                Components.FinalReportText[1].text = "Either reduce human\nmorale to 0 by 15 AD";
                Components.FinalReportText[3].text = "Or become their allies\n(+30 Human Ties) by 15 AD.";

                if (Scripts.HumanControllerScript.Morale <= 0)
                {
                    Components.ObjectiveCheck[0].sprite = Components.TickCross[0];
                    Scripts.MainMenuControllerScript.LevelUse.SpeechBubble[1].SpeechEnding[0].Text = "With humans still reeling\nfrom defeat, they should\nnot disturb us for now.";
                    Scripts.MainMenuControllerScript.LevelUse.SpeechBubble[1].SpeechEnding[1].Text = "The strength displayed\ntoday will ensure that\nwe are safe tomorrow.";
                    Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].VictoryCheck = true;
                }
                else if (Scripts.HumanControllerScript.Morale > 0)
                {
                    Components.ObjectiveCheck[0].sprite = Components.TickCross[1];
                }

                if (Important.HumanRelationship >= 30)
                {

                    Components.ObjectiveCheck[1].sprite = Components.TickCross[0];
                    Scripts.MainMenuControllerScript.LevelUse.SpeechBubble[1].SpeechEnding[0].Text = "Great! I'm sure that as allies,\nthe humans will cooperate\nand learn to live in peace.";
                    Scripts.MainMenuControllerScript.LevelUse.SpeechBubble[1].SpeechEnding[1].Text =  "Your wisdom has ensured\nthat no unnecessary blood\nneeds to be shed.";
                    Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2].VictoryCheck = true;
                }
                else if (Important.HumanRelationship < 30)
                {
                    Components.ObjectiveCheck[1].sprite = Components.TickCross[1];
                }
                break;
        }

        Components.FinalReportAnim.Play("DrawCard");
        yield return new WaitForSeconds(1.183f);
        Components.FinalReportAnim.Play("ShowAnnualReport");
        yield return new WaitForSeconds(0.3f);
        Scripts.HandCardRotationScript.Important.NoClicking = false;
    }

    public void EndGameVictoryCheck()
    {
        var VictoryPath = Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2];
        Scripts.HandCardRotationScript.Important.NoClicking = false;
        
        /*VictoryPath.VictoryCheck = true;

        for (int i = 0; i < 5; i++)
        {
            if (VictoryPath.CardsHolding[i] != 0)
            {
                if (Scripts.HandCardRotationScript.Components.ResourceTypeCardCount[i] < VictoryPath.CardsHolding[i])
                {
                    VictoryPath.VictoryCheck = false;
                    break;
                }
            }
        }

        if (VictoryPath.HumanTies != 0 && VictoryPath.HumanMorale != 0)
        {
            if (Important.HumanRelationship < VictoryPath.HumanTies)
            {
                VictoryPath.VictoryCheck = false;
            }
        }*/

        if (VictoryPath.VictoryCheck == true)
        {
            //Debug.Log("WinLevel");
            Scripts.MainMenuControllerScript.LevelUse.FinishTalkEnding = false;
            Scripts.MainMenuControllerScript.LevelUse.PositionInEnding = 0;
            Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[0].SetActive(true);
            Pos = Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position;
            Pos.x = 20;
            Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position = Pos;
            StartCoroutine(Scripts.MainMenuControllerScript.AngieTalkEnding());
            return;
        }

        if (VictoryPath.VictoryCheck == false)
        {
            //Debug.Log("LoseLevel");
            Scripts.MainMenuControllerScript.LevelUse.FinishTalkEnding = false;
            Scripts.MainMenuControllerScript.LevelUse.PositionInEnding = 3;
            Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[0].SetActive(true);
            Pos = Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position;
            Pos.x = 20;
            Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position = Pos;
            StartCoroutine(Scripts.MainMenuControllerScript.AngieTalkEnding());
            return;
        }
    }

    private int[] HighestPriorityCards = new int[20];
    private int[] HighPriorityCards = new int[20];
    private int[] MidPriorityCards = new int[20];
    private int[] LowPriorityCards = new int[20];

    public IEnumerator ShuffleCard()
    {
        Important.TempFoodConsumption = 0;
        Scripts.NatureControllerScript.ResetOffenseDefence();
        ShuffleCount.NumberOfCardsBeforeShuffle = 10;
        Important.PositiveHumanEvent[1] = 0;
        Important.NegativeHumanEvent[1] = 0;
        Important.LowMoraleEvent[1] = 0;
        Important.LowFoodEvent[1] = 0;
        Scripts.UIControllerScript.InformationOverviewText[8].text = "You have not added any events for next year (yet).";
        Scripts.UIControllerScript.InformationOverviewText[9].text = "";

        for (var i = 0; i < 10; i++)
        {
            Scripts.OptionClickControlScript.Components.CardAddedNames[i] = "";
        }

        if (ShuffleCount.NumberOfShuffles != 0)
        {
            Scripts.UIControllerScript.UpdateRoundNumber(ShuffleCount.NumberOfShuffles);

            var VictoryPath = Victory.ResourceVictory[Scripts.MainMenuControllerScript.LevelSelected - 2];

            if (VictoryPath.RoundsNumber == ShuffleCount.NumberOfShuffles)
            {
                if (Important.CardsToAddIn[0].TechLevel == 0) StartCoroutine(FinalReport());//EndGameVictoryCheck();

                else
                {
                    StartCoroutine(Scripts.MiscellaneousGameManagementScript.NewNotice(1, "Final Decisions", "Please resolve all events\n that were added in the final year.", "Notice"));

                    for (int ClearCard = 0; ClearCard < Important.CardToUse.Length; ClearCard++)
                    {
                        Important.CardToUse[ClearCard].CardIdentifier[0] = 0;
                        Important.CardToUse[ClearCard].CardIdentifier[1] = 0;
                        Important.CardToUse[ClearCard].SpriteDisplayIdentifier[0] = 0;
                        Important.CardToUse[ClearCard].SpriteDisplayIdentifier[1] = 0;
                        Important.CardToUse[ClearCard].Name = "";
                        Important.CardToUse[ClearCard].CardPriority = 0;
                    }

                    for (int i = 0; i < Important.EventCardOrder.Length; i++)
                    {
                        Important.EventCardOrder[i] = 0;
                    }

                    ShuffleCount.PositionInShuffle = 0;
                    int NumberOfCardsToShuffle = 0;

                    for (int i = 0; i < 10; i++)
                    {
                        if (Important.CardsToAddIn[i].TechLevel != 0)
                        {
                            Important.CardToUse[i].CardIdentifier[0] = 1;
                            Important.CardToUse[i].CardIdentifier[1] = i + 1;
                            Important.CardToUse[i].SpriteDisplayIdentifier[0] = Important.CardsToAddIn[i].TechLevel;
                            Important.CardToUse[i].SpriteDisplayIdentifier[1] = Important.CardsToAddIn[i].Position;
                            Important.CardToUse[i].Name = EventDisplay.Normal[Important.CardsToAddIn[i].TechLevel - 1].Position[Important.CardsToAddIn[i].Position - 1].Name;
                            Important.CardToUse[i].CardPriority = -1;
                            Important.EventCardOrder[i] = i + 1;
                        }

                        else
                        {
                            NumberOfCardsToShuffle = i;
                            break;
                        }
                    }

                    for (int x = NumberOfCardsToShuffle - 1; x > 0; x--)
                    {
                        int Temp = Important.EventCardOrder[x];
                        int randIndex;
                        randIndex = Random.Range(0, x + 1);                                       //A random number is generated, to be smaller than or equal to number of unshuffled elements in array
                        Important.EventCardOrder[x] = Important.EventCardOrder[randIndex];      //The last element is made equal to the random element in the array
                        Important.EventCardOrder[randIndex] = Temp;                             //Random Array stores the value of the last element
                    }

                    ShuffleCount.NumberOfCardsBeforeShuffle = NumberOfCardsToShuffle;
                    ShuffleCount.FinishingLastCards = true;
                }

                yield break;
            }

            Components.ShufflingCardsAnimationObject.SetActive(true);
            StartCoroutine(Scripts.ShufflingCardAnimationScript.PlayShuffle());
            Components.HideWhileShuffling[0].SetActive(false);
            Components.HideWhileShuffling[1].SetActive(false);
            Components.HideWhileShuffling[2].SetActive(false);
        }

        for (int ClearCard = 0; ClearCard < Important.CardToUse.Length; ClearCard++)
        {
            Important.CardToUse[ClearCard].CardIdentifier[0] = 0;
            Important.CardToUse[ClearCard].CardIdentifier[1] = 0;
            Important.CardToUse[ClearCard].SpriteDisplayIdentifier[0] = 0;
            Important.CardToUse[ClearCard].SpriteDisplayIdentifier[1] = 0;
            Important.CardToUse[ClearCard].Name = "";
            Important.CardToUse[ClearCard].CardPriority = 0;
        }

        for (int ClearCard = 0; ClearCard < 20; ClearCard++)
        {
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

        for (int DE = 0; DE < Important.CardsToDestroy.Length; DE++)        //Delete all cards that are to be deleted during shuffling.
        {  
            if (Important.CardsToDestroy[DE].TechLevel != 0)
            {
                ReferencePath[Important.CardsToDestroy[DE].Position - 1].CardIdentifier[0] = 0;
                ReferencePath[Important.CardsToDestroy[DE].Position - 1].CardIdentifier[1] = 0;
                ReferencePath[Important.CardsToDestroy[DE].Position - 1].SpriteDisplayIdentifier[0] = 0;
                ReferencePath[Important.CardsToDestroy[DE].Position - 1].SpriteDisplayIdentifier[1] = 0;
                ReferencePath[Important.CardsToDestroy[DE].Position - 1].CardPriority = 0;
                ReferencePath[Important.CardsToDestroy[DE].Position - 1].Name = "";
                Important.CardsToDestroy[DE].TechLevel = 0;
                Important.CardsToDestroy[DE].Position = 0;
            }
        }

        int FG = 0;

        for (var AD = 0; AD < ReferencePath.Length; AD++)           //Add all cards that are to be added during shuffling.
        {  

            if (ReferencePath[AD].CardIdentifier[0] == 0)           //If the slot is empty
            {   
                if (Important.CardsToAddIn[FG].TechLevel != 0)
                {
                    ReferencePath[AD].SpriteDisplayIdentifier[0] = Important.CardsToAddIn[FG].TechLevel;
                    ReferencePath[AD].SpriteDisplayIdentifier[1] = Important.CardsToAddIn[FG].Position;
                    ReferencePath[AD].CardIdentifier[0] = 1;
                    ReferencePath[AD].CardIdentifier[1] = AD + 1;
                    ReferencePath[AD].Name = EventDisplay.Normal[Important.CardsToAddIn[FG].TechLevel - 1].Position[Important.CardsToAddIn[FG].Position - 1].Name;
                    ReferencePath[AD].CardPriority = -1; //Change later to custom made
                    Important.CardsToAddIn[FG].TechLevel = 0;
                    Important.CardsToAddIn[FG].Position = 0;
                    FG++;
                }
            }
        }

        yield return new WaitForSeconds (0.5f);

        if (Scripts.MainMenuControllerScript.LevelSelected >= 2)
        {

            for (var a = 0; a < ReferencePath.Length; a++)
            {
                if (ReferencePath[a].CardIdentifier[0] != 0)        //if card Identifier [0] == 0, that card is voided. 
                {       
                    if (ReferencePath[a].CardPriority < 3 && ReferencePath[a].CardPriority > -1) ReferencePath[a].CardPriority++;
                    Important.CardToUse[a].CardPriority = ReferencePath[a].CardPriority;

                    Important.CardToUse[a].Name = ReferencePath[a].Name;
                    Important.CardToUse[a].CardIdentifier[0] = ReferencePath[a].CardIdentifier[0];
                    Important.CardToUse[a].CardIdentifier[1] = ReferencePath[a].CardIdentifier[1];
                    Important.CardToUse[a].SpriteDisplayIdentifier[0] = ReferencePath[a].SpriteDisplayIdentifier[0];
                    Important.CardToUse[a].SpriteDisplayIdentifier[1] = ReferencePath[a].SpriteDisplayIdentifier[1];
                }
            }
        }

        FirstEventFixedCheck = 0;

        for (int x = 0; x < Important.CardToUse.Length; x++)
        {
            if (Important.CardToUse[x].CardPriority == -2)  //Card to be drawn first.
            {
                Important.EventCardOrder[0] = x + 1;
                FirstEventFixedCheck = 1;
            }

            else if (Important.CardToUse[x].CardPriority == -1)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (HighestPriorityCards[i] == 0)
                    {
                        HighestPriorityCards[i] = x + 1;
                        break;
                    }
                }
            }

            else if (Important.CardToUse[x].CardPriority == 3)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (HighPriorityCards[i] == 0)
                    {
                        HighPriorityCards[i] = x + 1;
                        break;
                    }
                }
            }

            else if (Important.CardToUse[x].CardPriority == 2)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (MidPriorityCards[i] == 0)
                    {
                        MidPriorityCards[i] = x + 1;
                        break;
                    }
                }
            }

            else if (Important.CardToUse[x].CardPriority == 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (LowPriorityCards[i] == 0)
                    {
                        LowPriorityCards[i] = x + 1;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < 20; i++)
        {
            if (HighestPriorityCards[i] != 0)
            {
                for (int CheckZero = 0; CheckZero < 20; CheckZero++)
                {
                    if (Important.EventCardOrder[CheckZero] == 0)
                    {
                        Important.EventCardOrder[i] = HighestPriorityCards[i];
                        break;
                    }
                }
            }
            else break;
        }

        for (int i = 0; i < 20; i++)
        {
            if (HighPriorityCards[i] != 0)
            {
                for (int CheckZero = 0; CheckZero < 20; CheckZero++)
                {
                    if (Important.EventCardOrder[CheckZero] == 0)
                    {
                        Important.EventCardOrder[CheckZero] = HighPriorityCards[i];
                        break;
                    }
                }
            }
            else break;
        }

        for (int i = 0; i < 20; i++)
        {
            if (MidPriorityCards[i] != 0)
            {
                for (int CheckZero = 0; CheckZero < 20; CheckZero++)
                {
                    if (Important.EventCardOrder[CheckZero] == 0)
                    {
                        Important.EventCardOrder[CheckZero] = MidPriorityCards[i];
                        break;
                    }
                }
            }
            else break;
        }

        for (int i = 0; i < 20; i++)
        {
            if (LowPriorityCards[i] != 0)
            {
                for (int CheckZero = 0; CheckZero < 20; CheckZero++)
                {
                    if (Important.EventCardOrder[CheckZero] == 0)
                    {
                        Important.EventCardOrder[CheckZero] = LowPriorityCards[i];
                        break;
                    }
                }
            }
            else break;
        }

        for (int x = 9; x > 0 + FirstEventFixedCheck; x--)
        {                                                                           //Fisher-Yates shuffle http://en.wikipedia.org/wiki/Fisher–Yates_shuffle, code from https://forum.unity.com/threads/shuffling-an-array.46234/
            int Temp = Important.EventCardOrder[x];                                 //Last element in the unshuffled array is stored temporarily
            int randIndex;
            randIndex = Random.Range(0 + FirstEventFixedCheck, x + 1);              //A random number is generated, to be smaller than or equal to number of unshuffled elements in array
            Important.EventCardOrder[x] = Important.EventCardOrder[randIndex];      //The last element is made equal to the random element in the array
            Important.EventCardOrder[randIndex] = Temp;                             //Random Array stores the value of the last element
        }

        for (int x = 10; x < 20; x++)
        {
            Important.EventCardOrder[x] = 0;
        }

        ShuffleCheck = true;
    }

    // Card Requirements 0, 1, 2 for resource requirements and 3, 4, 5 for technology requirements of the option cards.
    // Card Sprite 0, 1, 2 for option cards, 3 for event card
    // CardsGiven 0,1,2 for resource given from first option card, 3,4,5 from second and 6,7,8 from third.
    // Change stats [0, 1 and 2] will affect nature's health, attack and defence. [3, 4 and 5] will affect that of humans.

    // A card from Cards To Shuffle in will be transfered to the next empty slot in the HumanTech0 array when they are required to be shuffled into deck.
    // Card To Call is for a card to tell another card to be shuffled into deck -- If CardToCall[0, 1 or 2] = -1, that card is taken out of the deck. 0, 1, 2 corresponds to option card chosen.
    // CardIdentifier[0] is to check human tech level, CardIdentifier[1] is to check position in the tech level array.

    public void UpdateOptionCardInfo()          //Send info of the options of Event card to Option Card Controller
    {
        if (Important.ImmidiateDrawCardNumber != 0)         //To Override the queue and immidiately draw certain cards (eg. Desolation)
        {      
            Scripts.OptionClickControlScript.UpdateCards(
                AllEventCardsInGame[0].Miscellaneous[Important.ImmidiateDrawCardNumber - 1].CardIdentifier[0],
                AllEventCardsInGame[0].Miscellaneous[Important.ImmidiateDrawCardNumber - 1].CardIdentifier[1]);
            return;
        }

        Scripts.OptionClickControlScript.UpdateCards(
            Important.CardToUse[EventCardPlayed - 1].CardIdentifier[0],
            Important.CardToUse[EventCardPlayed - 1].CardIdentifier[1]);
    }

    public void UpdateHumanRelationship(int Change)
    {
        string TypeofRelationship;
        Important.HumanRelationship += Change;

        if (Important.HumanRelationship >= 10 && Important.HumanRelationship < 30)      //Max number of positive human events = 1
        {
            TypeofRelationship = "  Friendly";
            Important.PositiveHumanEvent[0] = 1;
            Important.NegativeHumanEvent[0] = 0;
            Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[3];
            Pos = Components.HumanRelationSpritePosition.localPosition;
            Pos.x = -13.2f;
            Components.HumanRelationSpritePosition.localPosition = Pos;
        }

        else if (Important.HumanRelationship >= 30)
        {
            TypeofRelationship = " Allies";
            Important.PositiveHumanEvent[0] = 2;
            Important.NegativeHumanEvent[0] = 0;
            Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[4];
            Pos = Components.HumanRelationSpritePosition.localPosition;
            Pos.x = -12.5f;
            Components.HumanRelationSpritePosition.localPosition = Pos;
        }

        else if (Important.HumanRelationship <= -10 && Important.HumanRelationship > -30)
        {
            TypeofRelationship = "  Guarded";
            Important.PositiveHumanEvent[0] = 0;
            Important.NegativeHumanEvent[0] = 1;
            Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[1];
            Pos = Components.HumanRelationSpritePosition.localPosition;
            Pos.x = -14.7f;
            Components.HumanRelationSpritePosition.localPosition = Pos; 
        }

        else if (Important.HumanRelationship <= -30)
        {
            TypeofRelationship = " Hostile";
            Important.PositiveHumanEvent[0] = 0;
            Important.NegativeHumanEvent[0] = 2;
            Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[0];
            Pos = Components.HumanRelationSpritePosition.localPosition;
            Pos.x = -13.98f;
            Components.HumanRelationSpritePosition.localPosition = Pos;
        }

        else
        {
            TypeofRelationship = "  Neutral";
            Important.PositiveHumanEvent[0] = 0;
            Important.NegativeHumanEvent[0] = 0;
            Components.HumanRelationSpriteRenderer.sprite = Components.HumanRelationSprites[2];
            Pos = Components.HumanRelationSpritePosition.localPosition;
            Pos.x = -12.5f;
            Components.HumanRelationSpritePosition.localPosition = Pos;
        }

        Components.HumanRelationDisplay.text = TypeofRelationship + " (" + Important.HumanRelationship + ")";
        Scripts.UIControllerScript.InformationCharacterText[8].text = Scripts.MainMenuControllerScript.DetermineHumanTies(Important.HumanRelationship);
    }

    [System.Serializable]
    public class AllEventCards
    {
        public CardInfo[] HumanTech0 = new CardInfo[20];
        public CardInfo[] HumanTech1 = new CardInfo[4];
        public CardInfo[] HumanTech2 = new CardInfo[6];
        public CardInfo[] HumanTech3 = new CardInfo[4];
        public CardInfo[] Miscellaneous = new CardInfo[3];
        public CardInfo[] CardsToShuffleIn = new CardInfo[5];
        public LevelSelected[] LevelChosen = new LevelSelected[5];
    }

    [System.Serializable]
    public class LevelSelected
    {
        public string Name;
        public CardInfo[] CardsInLevel = new CardInfo[20];
    }

    [System.Serializable]
    public class CardInfo
    {                             //Class Provides the Structure for the array "CardToUse"
        public string Name; 
        public int[] CardIdentifier = new int[2];
        public int[] SpriteDisplayIdentifier = new int[2];
        public int CardPriority;
    }

    [System.Serializable]
    public class CardsDestroyedAfterShuffle
    {
        public int TechLevel;
        public int Position;
    }

    [System.Serializable]
    public class CardsAddedAfterShuffle
    {
        public int TechLevel;
        public int Position;
    }

    [System.Serializable]
    public class ComponentReference
    {
        public UnityEngine.Video.VideoPlayer EndSceneVideoPlayer;
        public Animation anim;
        public GameObject EventCard;
        public GameObject ShufflingCardsAnimationObject;
        public SpriteRenderer EventCardRenderer;
        public UnityEngine.UI.Text EventCardTitle;
        public UnityEngine.UI.Text EventCardDescription;
        public UnityEngine.UI.Text EventCardType;

        public SpriteRenderer[] OptionCardRenderers = new SpriteRenderer[3];
        public UnityEngine.UI.Text[] OptionCardTitleRenderers = new UnityEngine.UI.Text[3];
        public UnityEngine.UI.Text[] OptionCardQuoteRenderers = new UnityEngine.UI.Text[3];
        public Transform[] OptionCardQuoteSize = new Transform[3];
        public UnityEngine.UI.Text[] OptionCardEffect = new UnityEngine.UI.Text[3];
        public Font[] Fonts = new Font[5];
        public Sprite LockedOptionSprite;
        public GameObject[] LockedOptionBlacken = new GameObject[3];
        public GameObject[] OptionCardHolder = new GameObject[3];
        public GameObject MissionSpriteHolder;
        public SpriteRenderer[] MissionSpriteRenderers = new SpriteRenderer[3];
        public Sprite[] MissionSprites = new Sprite[2];
        public GameObject[] HideWhileShuffling = new GameObject[3];
        public UnityEngine.UI.Text HumanRelationDisplay;
        public UnityEngine.UI.Image HumanRelationSpriteRenderer;
        public Sprite[] HumanRelationSprites = new Sprite[5];
        public Transform HumanRelationSpritePosition;

        public GameObject AnnualReport;
        public Animation AnnualReportAnim;
        public UnityEngine.UI.Text[] AnnualReportText = new UnityEngine.UI.Text[5];

        public GameObject FinalReport;
        public Animation FinalReportAnim;
        public UnityEngine.UI.Text[] FinalReportText = new UnityEngine.UI.Text[4];
        public UnityEngine.UI.Image[] ObjectiveCheck = new UnityEngine.UI.Image[2];
        public Sprite[] TickCross = new Sprite[2];
        public Sprite[] ResourceChange = new Sprite[2];
        public GameObject[] ResourceChangeGameObject = new GameObject[3];
        public SpriteRenderer[] ResourceChangeSprite = new SpriteRenderer[3];
    }

    [System.Serializable]
    public class CardCounting
    {
        public int NumberOfShuffles;
        public int PositionInShuffle;
        public int NumberOfCardsBeforeShuffle = 10;
        public bool FinishingLastCards = false;
    }

    [System.Serializable]
    public class ImportantVars
    {
        public int[] PositiveHumanEvent = new int[2];
        public int[] NegativeHumanEvent = new int[2];
        public int[] LowMoraleEvent = new int[2];
        public int[] LowFoodEvent = new int[2];

        public bool EventCardSelected;
        public bool DestroyEventCheck;
        public float HumanRelationship;
        public int SpecialCardActivated = 0;    // SpecialCardActivated= 1 : Costly Struggle. 
        public int ImmidiateDrawCardNumber;   // Draw this card immidiately inside Miscellaneous cards
        public int[] EventCardOrder = new int[20];
        public CardInfo[] CardToUse = new CardInfo[30];                //Array size = Number of Event Cards
        public CardsDestroyedAfterShuffle[] CardsToDestroy = new CardsDestroyedAfterShuffle[10];
        public CardsAddedAfterShuffle[] CardsToAddIn = new CardsAddedAfterShuffle[10];
        public CardInfo[] CardsInDeck = new CardInfo[30];

        public int TempFoodConsumption;
        public int FoodProduction;
        public int FoodConsumption;
        public int Morale;
        public int HumanTies;
    }

    [System.Serializable]
    public class AllEventCardSprites
    {
        public OneTech[] Normal = new OneTech[5];    // Tech Level 0, 1, 2, 3 and cards to shuffle in
        public OneTech Miscellaneous;
    }

    [System.Serializable]
    public class OneTech
    {
        public string Name;
        public OneEventCardSprite[] Position = new OneEventCardSprite[10];
    }

    [System.Serializable]
    public class OneEventCardSprite
    {
        public string Name;
        public Sprite[] SpriteGroup = new Sprite[4];
        public string[] EventCardText = new string[3];
        public string[] OptionCardTitle = new string[3];
        public string[] OptionCardQuote = new string[3];
        public string[] OptionCardEffect = new string[3];
        public int[] OptionCardEffectExplanation = new int[3];
        public float[] CardRequirements = new float[3];
        public int[] SkillsRequirements = new int[3];
        public int[] ResourcesUsed = new int[9];
        public int[] ResourcesGiven = new int[9];
        public int[] StatsChange = new int[6];
        public int[] UnlockOptionNumber = new int[3];
        public CardCalled CalledCard;
        public bool[] RemoveEventCheck = new bool[3];
        public int[] DiscardCards = new int[3];
        public bool[] LoseCheck = new bool[3];
        public Sprite[] PromotionSprite = new Sprite[3];
        public string[] PromotionText = new string[6];
        public int[] HumanRelationshipChange = new int[3];
        public float[] FoodChange = new float[3];
        public int[] FoodProductionChange = new int[3];
        public int[] MoraleProductionChange = new int[3];
        public int[] HumanTiesProductionChange = new int[3];
        public CombatEffects Combat;
        public MilitaryStrengthChange MilitaryChange;
        public int MissionProgress;
        public int SpecialCardNumber;
    }

    [System.Serializable]
    public class CardCalled
    {
        public int[] TechLevel = new int[3];
        public int[] Position = new int[3];
        public float[] ChanceToAddCard = new float[3];
        public bool EnableAnimation;                    //CardShuffleInAnimation
    }

    [System.Serializable]
    public class VictoryCondition
    {
        public ResourceGatheringVictory[] ResourceVictory = new ResourceGatheringVictory[10];
    }

    [System.Serializable]
    public class ResourceGatheringVictory
    {
        public string Name;
        public bool VictoryCheck;
        public int RoundsNumber;
        public int[] CardsHolding = new int[5];
        public int HumanTies;
        public int HumanMorale;
    }

    [System.Serializable]
    public class CombatEffects
    {
        public string[] WarEffect = new string[3];
        public string[] WarVictory = new string[3];
        public bool[] WarCheck = new bool[3];
        public int[] EnemyOffense = new int[6];
        public int[] EnemyDefence = new int[6];
        public string[] CombatQuote = new string[6];
    }

    [System.Serializable]
    public class MilitaryStrengthChange
    {
        public int[] OffenseChange = new int[3];
        public int[] DefenceChange = new int[3];
        public int[] PermOffenseChange = new int[3];
        public int[] PermDefenceChange = new int[3];
    }
}
