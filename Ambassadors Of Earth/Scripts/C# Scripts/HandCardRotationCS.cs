using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardRotationCS : MonoBehaviour {

    public CameraControlCS.ScriptReference Scripts;
    public ComponentReference2 Components;
    public ImportantVars1 Important;
    public ReferencedCard CardReference;
    public GameObject[] CardsPlayed = new GameObject[4];

    private Vector3 CentrePoint;
    private bool RotateLeft;
    private GameObject LeftMostCard;
    private bool RotateRight;
    private GameObject RightMostCard;
    private Vector3 MidpointVector;

    private bool EventCardClickCheck;
    private bool OptionCardClickCheck;
    private bool NoOptionEventCardClickCheck;
    private bool DeselectResourceCardCheck;
    private bool LockedOptionClickCheck;
    private bool NoticeClickCheck;
    private bool AnnualReportClickCheck;
    private bool FinalReportClickCheck;
    private bool NatureAvatarClickCheck;
    private bool NatureMoraleClickCheck;
    private bool HumanMoraleClickCheck;
    private bool NumberCardsClickCheck;
    private bool NumberLifeClickCheck;
    private bool NumberChaosClickCheck;
    private bool HumanTiesClickCheck;
    private bool CombatStatsClickCheck;
    private bool CheckIfCardIsPlayed;
    private bool StartMovingCheck;

	void Start () {
        CentrePoint = Components.Pivot.GetComponent<Renderer>().bounds.center;
        MidpointVector = new Vector3(-0.36f, 4.12f, 9.4235f) - CentrePoint;
        Components.CardPositions[0] = new Vector3(0.5f, -9.96f, 0.56f);
        Components.CardPositions[1] = new Vector3(-0.09f, -6.74f, 0.52f);
        Components.CardPositions[2] = new Vector3(0.38f, -3.31f, 0.48f);
        Components.CardPositions[3] = new Vector3(1.82f, -0.26f, 0.44f);
        Components.CardPositions[4] = new Vector3(4.2f, 2.14f, 0.4f);
        Components.CardPositions[5] = new Vector3(7.23f, 3.81f, 0.36f);
        Components.CardPositions[6] = new Vector3(10.6f, 4.39f, 0.32f);
        Components.CardPositions[7] = new Vector3(14.15f, 3.97f, 0.28f);
        Components.CardPositions[8] = new Vector3(17.19f, 2.36f, 0.24f);
        Components.CardPositions[9] = new Vector3(19.66f, -0.19f, 0.20f);
        Components.CardPositions[10] = new Vector3(21.22f, -3.27f, 0.16f);
        Components.CardPositions[11] = new Vector3(21.69f, -6.68f, 0.12f);
        Components.CardPositions[12] = new Vector3(21.1f, -10.16f, 0.08f);
        Components.CardPositions[13] = new Vector3(19.5f, -13.1f, 0.04f);
        Components.CardPositions[14] = new Vector3(17.01f, -15.47f, 0.00f);

        Components.CardRotations[0] = new Quaternion(0.0f, 0.0f, 0.809f, 0.5878f);
        Components.CardRotations[1] = new Quaternion(0.0f, 0.0f, 0.7071f, 0.7071f);
        Components.CardRotations[2] = new Quaternion(0.0f, 0.0f, 0.5878f, 0.809f);
        Components.CardRotations[3] = new Quaternion(0.0f, 0.0f, 0.454f, 0.891f);
        Components.CardRotations[4] = new Quaternion(0.0f, 0.0f, 0.309f, 0.951f);
        Components.CardRotations[5] = new Quaternion(0.0f, 0.0f, 0.1564345f, 0.9876884f);
        Components.CardRotations[6] = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        Components.CardRotations[7] = new Quaternion(0.0f, 0.0f, 0.1564f, -0.9877f);
        Components.CardRotations[8] = new Quaternion(0.0f, 0.0f, 0.309f, -0.951f);
        Components.CardRotations[9] = new Quaternion(0.0f, 0.0f, 0.454f, -0.891f);
        Components.CardRotations[10] = new Quaternion(0.0f, 0.0f, 0.5878f, -0.809f);
        Components.CardRotations[11] = new Quaternion(0.0f, 0.0f, 0.7071f, -0.7071f);
        Components.CardRotations[12] = new Quaternion(0.0f, 0.0f, -0.809f, 0.5878f);
        Components.CardRotations[13] = new Quaternion(0.0f, 0.0f, -0.891f, 0.454f);
        Components.CardRotations[14] = new Quaternion(0.0f, 0.0f, -0.95106f, 0.309f);
    }

    public void InitialiseNewGame()
    {
        SortHandCards();
        Important.NumberOfCards = transform.childCount - 1;                          //Don't count pivot point into number of cards. 
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = transform.GetChild(Important.NumberOfCards - 1).gameObject; //Reference for right most card will be -1 in the [] brackets.
        RotateLeft = false;
        RotateRight = false;
        CardSelected = null;
        RotateToCentre();
    }

    public IEnumerator LoadSavedCards()
    {
        Scripts.UIControllerScript.UpdateRoundNumber(Scripts.EventCardControlScript.ShuffleCount.NumberOfShuffles + 1);
        LeftMostCard = transform.GetChild(0).gameObject;
        RightMostCard = transform.GetChild(Important.NumberOfCards - 1).gameObject;
        RotateLeft = false;
        RotateRight = false;
        CardSelected = null;
        Scripts.UIControllerScript.UpdateLifeResourceCardNumber(Components.ResourceTypeCardCount[0]);
        Scripts.UIControllerScript.UpdateChaosResourceCardNumber(Components.ResourceTypeCardCount[4]);
        Scripts.UIControllerScript.UpdateResourceCardNumber(Important.NumberOfCards);
        yield return new WaitForSeconds(0.1f);
        RotateToCentre();
    }

    public GameObject CardSelected;
    public float TesterLerp = 0.0f;
    public bool NeedToCloseCheck = true;
    public bool AdjustmentCheck;

    private float clickTime;
    private bool RotatingInProgress;
    private Vector2 startPos;
    private Vector2 endPosition;
    private float StartingAngle;
    private float swipeDist;
    private float swipeUpDist;
    private float maxSwipeDist;
    private bool ResourceCardsSelected;
    private float ElasticSpringBack;
    private bool SwipeUpDetected;
    private bool CardGapCloseCheck;
    private bool UpdateChecker;
    private float ElasticSpringLerp = 0.0f;
    private float SwipeUpLerp = 0.0f;
    private float ElasticSwipeCorrectionLerp = 0.0f;
    private float LerpProgress = 0.0f;
    private bool StartedSwipingCheck;
    private GameObject NullCardInstance;
    private int NullCardCount;
    private int RotatingNumber;
    private GameObject ClickedCard;
    private float TimePressed;
    private Vector3 Pos;
    private Vector3 Scale;
    private Quaternion Quat;

    private GameObject CardHolding;
    private bool MagnifierClickedCheck;
    public GameObject ShownAdditionalInfo;
    public GameObject InstantiatedResourceChange;

    void Update()
    {
        if (RotateLeft == true && LeftMostCard != null)
        {
            if (LeftMostCard.transform.rotation.eulerAngles.y <= 190 || LeftMostCard.transform.rotation.eulerAngles.y > 270)
            {
                transform.RotateAround(CentrePoint, Vector3.up, (swipeDist / Screen.width) * 800 * Time.deltaTime);
            }
            else { RotateLeft = false; } //Debug.Log("Stop Rotating Left");
        }

        if (RotateRight == true && RightMostCard != null)
        {
            if (RightMostCard.transform.rotation.eulerAngles.y >= 170 || RightMostCard.transform.rotation.eulerAngles.y < 90)       //RightMostCard.transform.rotation.eulerAngles.y >= 170
            { 
                transform.RotateAround(CentrePoint, Vector3.down, (swipeDist / Screen.width) * 800 * Time.deltaTime);
            }
            else { RotateRight = false; }  //Debug.Log("Stop Rotating Right");
        }

        if (ClickedCard != null)
        {
            TimePressed += Time.deltaTime;
            if (TimePressed >= 0.7f && TimePressed < 2)
            {
                if (Scripts.MainMenuControllerScript.LevelSelected == 1 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22)
                {
                    if (CardSelected.GetComponent<OptionCardMovementCS>().OptionCardNumber != 1)
                    {
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;

                        Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                        Scale.x = 0.04f;
                        Scale.y = 0.04f;
                        Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Wrong option selected!\n(Tap and hold option 1)";
                        Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                        return;
                    }
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("FadeCallOut");
                }
                CheckRayCast();
            }
        }

        if (Input.GetMouseButtonUp(0) && MagnifierClickedCheck == false)
        {
            TimePressed = 0;
            ResourceCardsSelected = false;
            swipeDist = 0;

            if (Camera.main.transform.position.z == 62.3f)
            {
                if (Scripts.OptionClickControlScript.Components.PromotionFinishCheck == true)
                {
                    StartCoroutine(Scripts.OptionClickControlScript.FinishedSpecialUpgrade());
                }

                else if (Scripts.OptionClickControlScript.Components.CombatStuff.CombatFinishCheck == true)
                {
                    StartCoroutine(Scripts.OptionClickControlScript.FinishedCombat());
                }
            }

            else if (SwipeUpDetected == true)
            {
                if (NullCardInstance != null)
                {
                    DestroyImmediate(NullCardInstance);
                    NullCardInstance = null;
                }

                SwipeUpReleased();
                if (CardSelected.transform.parent == Components.CardsPlayedParent.transform)
                {
                    Scale = CardSelected.transform.localScale;
                    Scale.x = 0.35f;
                    Scale.y = 0.35f;
                    CardSelected.transform.localScale = Scale;
                }
                if (CardSelected.transform.parent == this.gameObject.transform)
                {
                    Scale = CardSelected.transform.localScale;
                    Scale.x = 1;
                    Scale.y = 1;
                    CardSelected.transform.localScale = Scale;
                }
                SwipeUpDetected = false;
            }

            else if (ClickedCard != null)
            {
                switch (ClickedCard.tag)
                {
                    case "PlayedCards":
                        ClickedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "EventCards":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "NoOptionEventCard":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "OptionCards":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "LockedOption":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "Notice":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "AnnualReport":
                    case "FinalReport":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "NatureAvatar":
                        ClickedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "NatureStats":
                    case "HumanStats":
                        ClickedCard.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "HumanTies":
                    case "NumberOfChaosCards":
                    case "NumberOfLifeCards":
                    case "NumberOfCards":
                        ClickedCard.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                        ClickedCard = null;
                        break;

                    case "CombatStats":
                        ClickedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        CardSelected.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        CardSelected.transform.GetChild(0).transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                        CardSelected.transform.GetChild(0).transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
                        CardSelected.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        ClickedCard = null;
                        break;
                }
            }

            UpdateChecker = false;
            StartedSwipingCheck = false;

            Ray rayMouseUp = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitMouseUp;

            if (Physics.Raycast(rayMouseUp, out hitMouseUp, 100))
            {
                CardSelected = hitMouseUp.collider.gameObject;

                switch (CardSelected.tag)
                {

                    case "EventCards":
                        if (Scripts.MainMenuControllerScript.LevelSelected != 1) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 13 || Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 18) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);

                        if (EventCardClickCheck == true)
                        {
                            EventCardClickCheck = false;

                            if (Scripts.EventCardControlScript.Important.EventCardSelected == false)
                            {
                                Scripts.EventCardControlScript.EventCardClicked();
                                return;
                            }
                            if (Scripts.EventCardControlScript.Important.EventCardSelected == true)
                            {
                                Scripts.EventCardControlScript.EventCardDeselected();
                                return;
                            }
                        }
                        break;

                    case "OptionCards":
                        if (Scripts.MainMenuControllerScript.LevelSelected != 1) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        if (Scripts.MainMenuControllerScript.LevelSelected == 1 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 16) Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);

                        if (OptionCardClickCheck == true)
                        {
                            OptionCardClickCheck = false;
                            Important.SelectedOptionCardNumber = CardSelected.GetComponent<OptionCardMovementCS>().OptionCardNumber;
                            StartCoroutine(Scripts.OptionClickControlScript.CheckMeetRequirements(Components.CardsPlayed[0], Components.CardsPlayed[1], Components.CardsPlayed[2], Important.SelectedOptionCardNumber)); //Debug.DrawLine(ray.origin, hit.point);
                        }
                        break;

                    case "NoOptionEventCard":
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        if (NoOptionEventCardClickCheck == true)
                        {
                            NoOptionEventCardClickCheck = false;
                            Important.NoClicking = true;
                            Scripts.ParticleEffectControllerScript.DestroyExtraCards();
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[6]);
                            StartCoroutine(DestroyExtra3CardsConfirmed());
                        }
                        break;

                    case "PlayedCards":
                        if (DeselectResourceCardCheck == true)
                        {
                            DeselectResourceCardCheck = false;
                            DeselectCard();
                        }
                        break;

                    case "LockedOption":
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        if (LockedOptionClickCheck == true)
                        {
                            LockedOptionClickCheck = false;
                            Scripts.UIControllerScript.Announcement("This option cannot be selected currently");
                        }
                        break;

                    case "Notice":
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        if (NoticeClickCheck == true)
                        {
                            NoticeClickCheck = false;
                            Important.NoClicking = true;
                            StartCoroutine(Scripts.MiscellaneousGameManagementScript.NoticeRemoved());
                        }
                        break;

                    case "AnnualReport":
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        if (AnnualReportClickCheck == true)
                        {
                            AnnualReportClickCheck = false;
                            Important.NoClicking = true;
                            StartCoroutine(Scripts.EventCardControlScript.AnnualReportRemoved());
                        }
                        break;

                    case "FinalReport":
                        if (FinalReportClickCheck == true)
                        {
                            FinalReportClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Important.NoClicking = true;
                            StartCoroutine(Scripts.EventCardControlScript.FinalReportRemoved());
                        }
                        break;

                    case "NatureAvatar":
                        if (NatureAvatarClickCheck == true)
                        {
                            NatureAvatarClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.Information.SetActive(true);
                            Important.GamePaused = true;
                        }
                        break;

                    case "NatureStats":
                        if (NatureMoraleClickCheck == true)
                        {
                            NatureMoraleClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayNatureMoraleExplanation();
                        }
                        break;

                    case "HumanStats":
                        if (HumanMoraleClickCheck == true)
                        {
                            HumanMoraleClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayHumanMoraleExplanation();
                        }
                        break;

                    case "NumberOfCards":
                        
                        if (NumberCardsClickCheck == true)
                        {
                            NumberCardsClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayCardNumberExplanation();
                        }
                        break;

                    case "NumberOfLifeCards":
                        
                        if (NumberLifeClickCheck == true)
                        {
                            NumberLifeClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayLifeCardNumberExplanation();
                        }
                        break;

                    case "NumberOfChaosCards":
                        
                        if (NumberChaosClickCheck == true)
                        {
                            NumberChaosClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayDestructionCardNumberExplanation();
                        }
                        break;

                    case "HumanTies":
                        
                        if (HumanTiesClickCheck == true)
                        {
                            HumanTiesClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayHumanTiesExplanation();
                        }
                        break;

                    case "CombatStats":
                        
                        if (CombatStatsClickCheck == true)
                        {
                            CombatStatsClickCheck = false;
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.UIControllerScript.DisplayCombatExplation();
                        }
                        break;
                }
            }

            if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 15 && CardSelected.GetComponent<ResourceCardControlCS>() != null && Components.CardsPlayed[0] != null)
            {

                if (CardSelected.GetComponent<ResourceCardControlCS>().ResourceType == 100)
                {
                    StartCoroutine(Scripts.MainMenuControllerScript.ClearPrevious());
                    return;
                }

                else if (CardSelected.GetComponent<ResourceCardControlCS>().ResourceType != 100)
                {
                    Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                    Scale.x = 0.035f;
                    Scale.y = 0.04f;
                    Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "That's not the right card.\n(Swipe up the Chaos Resource)";
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                    Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
                    Pos.x = 20;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;
                    Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                    NeedToCloseCheck = true;
                    DeselectCard();
                    return;
                }
            }
        }

        if (StartMovingCheck == true)
        {

            if (Components.CardsPlayed[0] == null)
            {
                StartMovingCheck = false;
                return;
            }

            if (Components.CardsPlayed[0] != null && Components.CardsPlayed[1] == null && Components.CardsPlayed[2] == null)    // if 1 card played
            { 
                LerpProgress += Time.deltaTime * (1.0f / 0.5f);     // Check Completeion of Lerp
                Components.CardsPlayed[0].transform.position = Vector3.Lerp(Components.CardsPlayed[0].transform.position, new Vector3(-0.63f, 0, 4.3f), LerpProgress);
                Quat = Components.CardsPlayed[0].transform.rotation;
                Quat.eulerAngles = Vector3.Lerp(Components.CardsPlayed[0].transform.rotation.eulerAngles, new Vector3(90, 180, 0), LerpProgress);
                Components.CardsPlayed[0].transform.rotation = Quat;

                if (LerpProgress >= 0.7f)
                {
                    Components.CardsPlayed[0].GetComponent<Renderer>().sortingOrder = 1;
                    SortRendering(0, Components.CardsPlayed[0]);
                }

                if (LerpProgress >= 1)
                {
                    StartMovingCheck = false;
                }
            }

            else if (Components.CardsPlayed[1] != null && Components.CardsPlayed[2] == null)
            { // if 2 cards played
                LerpProgress += Time.deltaTime * (1.0f / 0.5f); // Check Completeion of Lerp
                Components.CardsPlayed[0].transform.position = Vector3.Lerp(Components.CardsPlayed[0].transform.position, new Vector3(0.62f, 0, 4.3f), LerpProgress);
                Quat = Components.CardsPlayed[0].transform.rotation;
                Quat.eulerAngles = Vector3.Lerp(Components.CardsPlayed[0].transform.rotation.eulerAngles, new Vector3(90, 180, 0), LerpProgress);
                Components.CardsPlayed[0].transform.rotation = Quat;

                Components.CardsPlayed[1].transform.position = Vector3.Lerp(Components.CardsPlayed[1].transform.position, new Vector3(-1.88f, 0, 4.3f), LerpProgress);
                Quat = Components.CardsPlayed[1].transform.rotation;
                Quat.eulerAngles = Vector3.Lerp(Components.CardsPlayed[1].transform.rotation.eulerAngles, new Vector3(90, 180, 0), LerpProgress);
                Components.CardsPlayed[1].transform.rotation = Quat;

                if (LerpProgress >= 0.7f)
                {
                    Components.CardsPlayed[1].GetComponent<Renderer>().sortingOrder = 1;
                    SortRendering(0, Components.CardsPlayed[1]);
                }

                if (LerpProgress >= 1)
                {
                    StartMovingCheck = false;
                }
            }

            else if (Components.CardsPlayed[1] != null && Components.CardsPlayed[2] != null)
            { //if 3 cards played
                LerpProgress += Time.deltaTime * (1.0f / 0.5f); // Check Completeion of Lerp
                Components.CardsPlayed[0].transform.position = Vector3.Lerp(Components.CardsPlayed[0].transform.position, new Vector3(1.87f, 0, 4.3f), LerpProgress);
                Quat = Components.CardsPlayed[0].transform.rotation;
                Quat.eulerAngles = Vector3.Lerp(Components.CardsPlayed[0].transform.rotation.eulerAngles, new Vector3(90, 180, 0), LerpProgress);
                Components.CardsPlayed[0].transform.rotation = Quat;

                Components.CardsPlayed[1].transform.position = Vector3.Lerp(Components.CardsPlayed[1].transform.position, new Vector3(-0.63f, 0, 4.3f), LerpProgress);
                Quat = Components.CardsPlayed[1].transform.rotation;
                Quat.eulerAngles = Vector3.Lerp(Components.CardsPlayed[1].transform.rotation.eulerAngles, new Vector3(90, 180, 0), LerpProgress);
                Components.CardsPlayed[1].transform.rotation = Quat;

                Components.CardsPlayed[2].transform.position = Vector3.Lerp(Components.CardsPlayed[2].transform.position, new Vector3(-3.13f, 0, 4.3f), LerpProgress);
                Quat = Components.CardsPlayed[2].transform.rotation;
                Quat.eulerAngles = Vector3.Lerp(Components.CardsPlayed[2].transform.rotation.eulerAngles, new Vector3(90, 180, 0), LerpProgress);
                Components.CardsPlayed[2].transform.rotation = Quat;

                if (LerpProgress >= 0.7f)
                {
                    Components.CardsPlayed[2].GetComponent<Renderer>().sortingOrder = 1;
                    SortRendering(0, Components.CardsPlayed[2]);
                }

                if (LerpProgress >= 1)
                {
                    StartMovingCheck = false;
                }
            }
        }

        if (TesterLerp != 0 && TesterLerp < 1 && AdjustmentCheck == false)
        {
            TesterLerp += Time.deltaTime * (1.0f / 0.5f); // Check Completeion of Lerp
            for (var a = 0; a < Important.NumberOfCards; a++)
            {
                if (transform.GetChild(a).gameObject.tag != "Pivot")
                {
                    transform.GetChild(a).gameObject.transform.localPosition = Vector3.Lerp(transform.GetChild(a).gameObject.transform.localPosition, Components.CardPositions[a], TesterLerp);        //Squeeze in to fill space taken by removed card.
                    transform.GetChild(a).gameObject.transform.localRotation = Quaternion.Lerp(transform.GetChild(a).gameObject.transform.localRotation, Components.CardRotations[a], TesterLerp);

                    if (transform.GetChild(a).gameObject.transform.childCount > 0)
                    {
                        Pos = transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
                        Pos.z = -0.01f;
                        transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition = Pos;

                        Pos = transform.GetChild(a).gameObject.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition;
                        Pos.z = -0.02f;
                        transform.GetChild(a).gameObject.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition = Pos;

                        Pos = transform.GetChild(a).gameObject.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition;
                        Pos.z = -0.03f;
                        transform.GetChild(a).gameObject.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition = Pos;

                        Pos = transform.GetChild(a).gameObject.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition;
                        Pos.z = -0.02f;
                        transform.GetChild(a).gameObject.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition = Pos;
                    }
                }
            }
        }

        if (AdjustmentCheck == true)
        {
            if (TesterLerp < 1)
            {
                TesterLerp += Time.deltaTime * (1.0f / 0.5f); // Check Completeion of Lerp

                for (var i = 0; i < Important.NumberOfCards; i++)
                {
                    if (transform.GetChild(i).gameObject.tag != "Pivot")
                    {
                        Pos = transform.GetChild(i).gameObject.transform.localPosition;
                        Pos.z = Components.CardPositions[i].z;
                        Pos.x = Mathf.Lerp(transform.GetChild(i).gameObject.transform.localPosition.x, Components.CardPositions[i].x, TesterLerp);        //Squeeze in to fill space taken by removed card.
                        Pos.y = Mathf.Lerp(transform.GetChild(i).gameObject.transform.localPosition.y, Components.CardPositions[i].y, TesterLerp);
                        transform.GetChild(i).gameObject.transform.localPosition = Pos;
                        transform.GetChild(i).gameObject.transform.localRotation = Quaternion.Lerp(transform.GetChild(i).gameObject.transform.localRotation, Components.CardRotations[i], TesterLerp);

                        if (transform.GetChild(i).gameObject.transform.childCount > 0)
                        {
                            Pos = transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
                            Pos.z = -0.01f;
                            transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition = Pos;

                            Pos = transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition;
                            Pos.z = -0.02f;
                            transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition = Pos;

                            Pos = transform.GetChild(i).gameObject.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition;
                            Pos.z = -0.03f;
                            transform.GetChild(i).gameObject.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition = Pos;

                            Pos = transform.GetChild(i).gameObject.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition;
                            Pos.z = -0.02f;
                            transform.GetChild(i).gameObject.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition = Pos;
                        }
                    }
                }
            }

            else if (CardGapCloseCheck == false && TesterLerp >= 1)
            {
                ElasticSpringLerp = 0;
                TesterLerp = 0;
                AdjustmentCheck = false;
            }
        }

        if (Important.NumberOfCards > 1 && LeftMostCard != null)
        {
            if (swipeDist == 0 && SwipeUpDetected == false && AdjustmentCheck == false && RotatingInProgress == false)      //Check if  cards rotated too much to left  or too much to right and correct it
            {

                Vector3 RightCardVector = RightMostCard.transform.position - CentrePoint;                   // Vector of Centre of rightmost card and centre card
                var SignOfRotationRight = Mathf.Sign(RightMostCard.transform.position.x - -0.36f);      // Determines which side of the middle point the right most card falls on
                float AngleToDeterminePositionRight = Vector3.Angle(RightCardVector, MidpointVector);     // Angle between the middle point of centre card and rightmost card

                if (AngleToDeterminePositionRight > 0 && AngleToDeterminePositionRight < 40 && SignOfRotationRight == 1)    //RightMostCard.transform.rotation.eulerAngles.y < 180
                {
                    ElasticSwipeCorrectionLerp += Time.deltaTime / 0.5f;
                    ElasticSpringBack = Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                    transform.RotateAround(CentrePoint, Vector3.up, ElasticSpringBack);
                }

                Vector3 LeftCardVector = LeftMostCard.transform.position - CentrePoint;
                var SignOfRotationLeft = Mathf.Sign(LeftMostCard.transform.position.x - -0.36f);
                float AngleToDeterminePositionLeft = Vector3.Angle(LeftCardVector, MidpointVector);

                if (AngleToDeterminePositionLeft > 0 && AngleToDeterminePositionLeft < 40 && SignOfRotationLeft == -1)      //LeftMostCard.transform.rotation.eulerAngles.y > 180
                {
                    ElasticSwipeCorrectionLerp += Time.deltaTime / 0.5f;
                    ElasticSpringBack = Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                    transform.RotateAround(CentrePoint, Vector3.down, ElasticSpringBack);
                }
            }
        }

        else if (Important.NumberOfCards == 1 && LeftMostCard != null)
        {
            if (swipeDist == 0 && SwipeUpDetected == false && AdjustmentCheck == false)
            {
                if (Mathf.Abs(180 - LeftMostCard.transform.rotation.eulerAngles.y) < 40)        //Left Most Card should be in between 160 to 200.
                {
                    if (LeftMostCard.transform.rotation.eulerAngles.y > 185)
                    {
                        ElasticSwipeCorrectionLerp += Time.deltaTime / 0.5f;
                        ElasticSpringBack = Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                        transform.RotateAround(CentrePoint, Vector3.down, ElasticSpringBack);
                    }

                    else if (LeftMostCard.transform.rotation.eulerAngles.y < 175)
                    {
                        ElasticSwipeCorrectionLerp += Time.deltaTime / 0.5f;
                        ElasticSpringBack = Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                        transform.RotateAround(CentrePoint, Vector3.up, ElasticSpringBack);
                    }
                }

                if (Mathf.Abs(-180 - LeftMostCard.transform.rotation.eulerAngles.y) < 40)       // Left Most Card should be in between -160 to -200
                {
                    if (LeftMostCard.transform.rotation.eulerAngles.y > -175)
                    {
                        ElasticSwipeCorrectionLerp += Time.deltaTime / 0.5f;
                        ElasticSpringBack = Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                        transform.RotateAround(CentrePoint, Vector3.down, ElasticSpringBack);
                    }

                    else if (LeftMostCard.transform.rotation.eulerAngles.y < -185)
                    {
                        ElasticSwipeCorrectionLerp += Time.deltaTime / 0.5f;
                        ElasticSpringBack = Mathf.Lerp(0, 7, ElasticSwipeCorrectionLerp);
                        transform.RotateAround(CentrePoint, Vector3.up, ElasticSpringBack);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && MagnifierClickedCheck == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Scripts.UIControllerScript.ResourceCardExplanation.SetActive(false);

            if (Physics.Raycast(ray, out hit, 100))
            {
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
                FinalReportClickCheck = false;

                if (Scripts.MainMenuControllerScript.LevelSelected == 1)
                {
                    if (Scripts.MainMenuControllerScript.TutorialUse.FinishLoadCheck == false) return;

                    if (CardSelected.tag == "TutorialPrevious")
                    {
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.ClickPrevious();
                        return;
                    }

                    else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 16 || Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22)
                    {
                        if (CardSelected.tag != "OptionCards")
                        {

                            if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 16)
                            {
                                Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                                Scale.x = 0.035f;
                                Scale.y = 0.035f;
                                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
                                Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Tap on the option that you\nbelieve is the most beneficial\nto the well-being of the forest.";
                                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[11].Play("DelayedDimScreenFadeIn");
                            }

                            else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 22)
                            {
                                Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                                Scale.x = 0.04f;
                                Scale.y = 0.04f;
                                Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
                                Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Let's give it a try.\n(Tap and hold option 1)";
                            }

                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                            Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
                            Pos.x = 20;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;

                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            return;
                        }
                    }

                    else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 15)
                    {
                        if (CardSelected.tag != "ResourceCards")
                        {
                            Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                            Scale.x = 0.035f;
                            Scale.y = 0.04f;
                            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;

                            Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Locate your Chaos resource\ncard and swipe up.";
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                            Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
                            Pos.x = 20;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                            NeedToCloseCheck = true;
                            return;
                        }

                        else if (CardSelected.tag == "ResourceCards")
                        {
                            if (NeedToCloseCheck == true)
                            {
                                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("FadeCallOut");
                                Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                            }
                            NeedToCloseCheck = false;
                        }
                    }

                    else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 13 || Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 18)
                    {
                        if (CardSelected.tag == "EventCards")
                        {
                            EventCardClickCheck = true;
                            CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                            CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                            ClickedCard = CardSelected;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                            StartCoroutine(Scripts.MainMenuControllerScript.ClearPrevious());
                        }

                        else
                        {
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
                            Scale.x = 0.035f;
                            Scale.y = 0.04f;
                            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;

                            if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 13) Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Please select the event I made!\n(Tap on the card above)";
                            if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial == 18) Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Let me show you something!\n(Tap on the card above)";
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
                            Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
                            Pos.x = 20;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("DelayedShowButtons");
                        }
                        return;
                    }

                    else if (Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial != 15 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial != 16 && Scripts.MainMenuControllerScript.TutorialUse.PositionInTutorial != 22)
                    {

                        if (CardSelected.tag == "TutorialNext")
                        {
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            StartCoroutine(Scripts.MainMenuControllerScript.ClearPrevious());
                            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[10].Play("FadeButtons");
                        }
                        return;
                    }
                }

                else if (Scripts.MainMenuControllerScript.LevelSelected != 1 && Scripts.MainMenuControllerScript.LevelUse.FinishTalk == false)
                {
                    if (Scripts.MainMenuControllerScript.LevelUse.SpeechLoaded == false) return;

                    if (CardSelected.tag == "TutorialNext")
                    {
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[0].Play("FadeCallOut");
                        Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[1].Play("FadeButtons");
                        StartCoroutine(Scripts.MainMenuControllerScript.NextSpeech());
                    }

                    else if (CardSelected.tag == "TutorialPrevious")
                    {
                        if (Scripts.MainMenuControllerScript.LevelUse.PositionInTalking > 1)
                        {
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Pos = Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position;
                            Pos.x = 20;
                            Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position = Pos;
                            Scripts.MainMenuControllerScript.LevelUse.PositionInTalking -= 2;
                            Scripts.MainMenuControllerScript.PreviousSpeech();
                        }
                    }
                    return;
                }

                else if (Scripts.MainMenuControllerScript.LevelSelected != 1 && Scripts.MainMenuControllerScript.LevelUse.FinishTalkEnding == false)
                {
                    if (Scripts.MainMenuControllerScript.LevelUse.SpeechLoaded == false) return;

                    if (CardSelected.tag == "TutorialNext")
                    {
                        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                        Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[0].Play("FadeCallOut");
                        Scripts.MainMenuControllerScript.LevelUse.SpeechAnimations[1].Play("FadeButtons");
                        StartCoroutine(Scripts.MainMenuControllerScript.NextSpeechEnding());
                    }

                    else if (CardSelected.tag == "TutorialPrevious")
                    {
                        if (Scripts.MainMenuControllerScript.LevelUse.PositionInEnding == 4) return;

                        if (Scripts.MainMenuControllerScript.LevelUse.PositionInEnding > 1)
                        {
                            Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
                            Pos = Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position;
                            Pos.x = 20;
                            Scripts.MainMenuControllerScript.LevelUse.SpeechObjects[2].transform.position = Pos;
                            Scripts.MainMenuControllerScript.LevelUse.PositionInEnding -= 2;
                            Scripts.MainMenuControllerScript.PreviousSpeechEnding();
                        }
                    }
                    return;
                }

                switch (CardSelected.tag)
                {
                    case "AnnualReport":
                        AnnualReportClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "FinalReport":
                        FinalReportClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "Notice":
                        NoticeClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "BattleCard":
                        /*if (Scripts.EventCardControlScript.Important.EventCardSelected == false)
                        {
                            Scripts.EventCardControlScript.BattleCardClicked();
                            return;
                        }
                        else if (Scripts.EventCardControlScript.Important.EventCardSelected == true)
                        {
                            Scripts.EventCardControlScript.BattleCardDeselected();
                            return;
                        }*/
                        break;

                    case "ResourceCards":
                        ResourceCardsSelected = true; //Debug.DrawLine(ray.origin, hit.point);
                        ElasticSwipeCorrectionLerp = 0;
                        break;

                    case "NoOptionEventCard":
                        NoOptionEventCardClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "PlayedCards":
                        DeselectResourceCardCheck = true;
                        CardSelected.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "OptionCards":
                        OptionCardClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "EventCards":
                        EventCardClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "LockedOption":
                        LockedOptionClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "NatureAvatar":
                        NatureAvatarClickCheck = true;
                        CardSelected.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "NatureStats":
                        NatureMoraleClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "HumanStats":
                        HumanMoraleClickCheck = true;
                        CardSelected.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "NumberOfCards":
                        NumberCardsClickCheck = true;
                        CardSelected.GetComponent<UnityEngine.UI.Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "NumberOfLifeCards":
                        NumberLifeClickCheck = true;
                        CardSelected.GetComponent<UnityEngine.UI.Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "NumberOfChaosCards":
                        NumberChaosClickCheck = true;
                        CardSelected.GetComponent<UnityEngine.UI.Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "HumanTies":
                        HumanTiesClickCheck = true;
                        CardSelected.GetComponent<UnityEngine.UI.Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;

                    case "CombatStats":
                        CombatStatsClickCheck = true;
                        CardSelected.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(0).transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(0).transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        CardSelected.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                        ClickedCard = CardSelected;
                        break;
                }
            }

            clickTime = 0;
            clickTime = Time.time;
            startPos = Input.mousePosition;
            if (LeftMostCard != null) StartingAngle = LeftMostCard.transform.rotation.eulerAngles.y;
        }

        if (ResourceCardsSelected == false) return;

        if (SwipeUpDetected == true)
        {
            Pos = Input.mousePosition;
            Pos.z = 46.4f;
            CardSelected.transform.position = Camera.main.ScreenToWorldPoint(Pos);
            SwipeUpLerp += Time.deltaTime / 0.5f;

            Quat = CardSelected.transform.rotation;
            Vector3 Angle = Quat.eulerAngles;
            Angle.y = Mathf.Lerp(CardSelected.transform.rotation.eulerAngles.y, 180, SwipeUpLerp);
            Quat.eulerAngles = Angle;
            CardSelected.transform.rotation = Quat;

            Scale = CardSelected.transform.localScale;
            Scale.x = 0.55f;
            Scale.y = 0.55f;
            CardSelected.transform.localScale = Scale;
        }

        for (int i = 0; i < 4; i++)
        {
            if (CardsPlayed[i] != null)
            {
                if (CardsPlayed[i].transform.parent == Components.CardsPlayedParent.transform)
                {

                    if (Camera.main.WorldToScreenPoint(CardsPlayed[i].transform.position).y > Screen.height / 4)
                    {
                        if (NullCardInstance != null)
                        {
                            DestroyImmediate(NullCardInstance);
                            NullCardInstance = null;
                            TesterLerp = 0;
                            Important.NumberOfCards -= 1;
                        }
                        NullCardCount = 0;
                    }

                    else if (Camera.main.WorldToScreenPoint(CardsPlayed[i].transform.position).y < Screen.height / 4)
                    {

                        if (NullCardCount == 0)
                        {

                            int CurrentCardNumber = CardsPlayed[i].GetComponent<ResourceCardControlCS>().CardNumber;
                            ArrayList SortAscending = new ArrayList();

                            if (Components.CardsPlayed[0] != null) SortAscending.Add(Components.CardsPlayed[0].GetComponent<ResourceCardControlCS>().CardNumber);
                            if (Components.CardsPlayed[1] != null) SortAscending.Add(Components.CardsPlayed[1].GetComponent<ResourceCardControlCS>().CardNumber);
                            if (Components.CardsPlayed[2] != null) SortAscending.Add(Components.CardsPlayed[2].GetComponent<ResourceCardControlCS>().CardNumber);
                            SortAscending.Add(CurrentCardNumber);
                            SortAscending.Sort();

                            TesterLerp = 0;
                            NullCardCount += 1;
                            NullCardInstance = Instantiate(Components.NullCard);
                            NullCardInstance.transform.parent = this.gameObject.transform;
                            int temp;

                            if (SortAscending.Count >= 1)
                            {
                                temp = System.Convert.ToInt32(SortAscending[0]);

                                if (CurrentCardNumber == temp)
                                {
                                    NullCardInstance.transform.SetSiblingIndex(CurrentCardNumber - 1);
                                    RotatingNumber = CurrentCardNumber - 1;
                                }
                            }

                            if (SortAscending.Count >= 2)
                            {
                                temp = System.Convert.ToInt32(SortAscending[1]);

                                if (CurrentCardNumber == temp)
                                {
                                    NullCardInstance.transform.SetSiblingIndex(CurrentCardNumber - 2);
                                    RotatingNumber = CurrentCardNumber - 2;
                                }
                            }

                            if (SortAscending.Count >= 3)
                            {
                                temp = System.Convert.ToInt32(SortAscending[2]);

                                if (CurrentCardNumber == temp)
                                {
                                    NullCardInstance.transform.SetSiblingIndex(CurrentCardNumber - 3);
                                    RotatingNumber = CurrentCardNumber - 3;
                                }
                            }

                            if (SortAscending.Count >= 4)
                            {
                                temp = System.Convert.ToInt32(SortAscending[3]);

                                if (CurrentCardNumber == temp)
                                {
                                    NullCardInstance.transform.SetSiblingIndex(CurrentCardNumber - 4);
                                    RotatingNumber = CurrentCardNumber - 4;
                                }
                            }

                            Important.NumberOfCards += 1;
                        }

                        if (RotatingNumber == 0)
                        {
                            if (NullCardInstance != null)
                            {
                                DestroyImmediate(NullCardInstance);
                                NullCardInstance = null;
                                Important.NumberOfCards -= 1;
                            }
                            NullCardCount = 0;
                        }

                        if (NullCardCount > 0 && RotatingInProgress == false)
                        {
                            AdjustmentCheck = true;
                        }
                    }
                }
            }
        }

        if (CardSelected.transform.parent == Components.CardsPlayedParent.transform)
        {
            return;
        }

        if (Input.GetMouseButton(0) && (Time.time - clickTime) > 0.05f)
        {
            endPosition = Input.mousePosition;
            swipeUpDist = Mathf.Abs(endPosition.y - startPos.y);
            swipeDist = Mathf.Abs(endPosition.x - startPos.x);


            if (swipeUpDist - Screen.height / 50 > swipeDist)
            {
                swipeDist = 0;
                SwipeUpLerp = 0;
                AssignCardNumber();
                SortRendering(50, CardSelected);

                if (UpdateChecker == false)
                {

                    Important.NumberOfCards = transform.childCount - 1;                          //Don't count pivot point into number of cards. 
                    CardGapCloseCheck = false;
                    TesterLerp = 0;

                    if (Important.NumberOfCards >= 2)
                    {
                        LeftMostCard = transform.GetChild(0).gameObject;
                        RightMostCard = transform.GetChild(Important.NumberOfCards - 1).gameObject; //Reference for right most card will be -1 in the [] brackets.
                    }

                    else if (Important.NumberOfCards == 1)
                    {
                        LeftMostCard = transform.GetChild(0).gameObject;
                        RightMostCard = LeftMostCard;
                    }

                    else if (Important.NumberOfCards == 0)
                    {
                        LeftMostCard = null;
                        RightMostCard = null;
                    }

                    UpdateChecker = true;
                }
                Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[2]);
                SwipeUpDetected = true;
            }

            if (SwipeUpDetected == true) return;

            if (swipeDist > swipeUpDist - Screen.height / 50)
            {
                if (swipeDist < maxSwipeDist)
                {
                    startPos = Input.mousePosition;
                    maxSwipeDist = 0;
                }

                CardGapCloseCheck = false;
                StartedSwipingCheck = true;

                if ( Mathf.Abs(endPosition.x - startPos.x) < Screen.width/40)
                {
                    RotateRight = false;
                    RotateLeft = false;
                }

                else {
                    var swipeDirection = Mathf.Sign(endPosition.x - startPos.x);
                    if (swipeDirection == -1)
                    {
                        RotateRight = true;
                        RotateLeft = false;
                        if (swipeDist > maxSwipeDist) maxSwipeDist = swipeDist;
                    }

                    else if (swipeDirection == 1)
                    {
                        RotateLeft = true;
                        RotateRight = false;
                        if (swipeDist > maxSwipeDist) maxSwipeDist = swipeDist;
                    }
                }
            }
        }
    }

    private float SignLeft;
    private float TimeToPause = 0.017f;

    public IEnumerator ShowRotateToCentre(int RotatingCardNumber)
    {

        if (Important.NumberOfCards > 1)
        {
            if (RotatingCardNumber == 0)
            {
                while (LeftMostCard.transform.rotation.eulerAngles.y <= 190 || LeftMostCard.transform.rotation.eulerAngles.y > 270)     // LeftMostCard.transform.rotation.eulerAngles.y <= 190
                { 
                    transform.RotateAround(CentrePoint, Vector3.up, 1 + (190 - LeftMostCard.transform.rotation.eulerAngles.y) / 6);
                    yield return new WaitForSeconds(TimeToPause);
                }

                TesterLerp = 0;
                AdjustmentCheck = true;
                RotatingInProgress = false;
                yield break;
            }

            if (RotatingCardNumber == transform.childCount - 2)          //RightMostCard
            { 
                while (transform.GetChild(RotatingCardNumber - 1).GetComponent< Transform > ().rotation.eulerAngles.y > 180)
                {
                    transform.RotateAround(CentrePoint, Vector3.down, 1 + (transform.GetChild(RotatingCardNumber - 1).GetComponent< Transform > ().rotation.eulerAngles.y - 180) / 6);     //Debug.Log("Rotate Right");
                    yield return new WaitForSeconds(TimeToPause);
                }

                TesterLerp = 0;
                AdjustmentCheck = true;
                RotatingInProgress = false;
                yield break;
            }

            if (RotatingCardNumber >= 1)
            {
                SignLeft = Mathf.Sign(this.gameObject.transform.GetChild(RotatingCardNumber - 1).transform.position.x - -0.36f);

                if (SignLeft > 0)
                {
                    while (transform.GetChild(RotatingCardNumber - 1).GetComponent< Transform > ().rotation.eulerAngles.y < 162)
                    {

                        transform.RotateAround(CentrePoint, Vector3.up, 1 + (162 - transform.GetChild(RotatingCardNumber - 1).GetComponent< Transform > ().rotation.eulerAngles.y) / 6);     //Debug.Log("Rotate Right");
                        yield return new WaitForSeconds(TimeToPause);
                    }
                    TesterLerp = 0;
                    AdjustmentCheck = true;
                    RotatingInProgress = false;
                    yield break;
                }

                if (SignLeft < 0)
                {

                    while (transform.GetChild(RotatingCardNumber - 1).GetComponent< Transform > ().rotation.eulerAngles.y > 162)
                    {
                        transform.RotateAround(CentrePoint, Vector3.down, 1 + (transform.GetChild(RotatingCardNumber - 1).GetComponent< Transform > ().rotation.eulerAngles.y - 162) / 6);     //Debug.Log("Rotate Right");
                        yield return new WaitForSeconds(TimeToPause);
                    }

                    TesterLerp = 0;
                    AdjustmentCheck = true;
                    RotatingInProgress = false;
                    yield break;
                }
            }
            AdjustmentCheck = true;
        }
    }

    public void CheckRayCast()
    {
        Ray rayCheck = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitCheck;
        if (Physics.Raycast(rayCheck, out hitCheck, 100))
        {
            CardHolding = hitCheck.collider.gameObject;
            if (CardHolding == ClickedCard)
            {
                if (CardSelected.tag == "OptionCards")
                {
                    Pos = CardReference.OptionCardPosition.position;
                    Pos.z = 1.0f;
                    CardReference.OptionCardPosition.position = Pos;
                    Pos = Components.OptionCardZoomIn[3].transform.localPosition;
                    Pos.z = 1.43f;
                    Components.OptionCardZoomIn[3].transform.localPosition = Pos;

                    Components.Magnifier.SetActive(true);
                    MagnifierClickedCheck = true;
                    Components.Magnifier.transform.GetChild(2).gameObject.SetActive(true);
                    Components.OptionCardZoomIn[0].sprite = CardHolding.transform.GetChild(2).GetComponent< SpriteRenderer > ().sprite;
                    Components.OptionCardZoomIn[1].sprite = CardHolding.transform.GetChild(0).GetComponent< SpriteRenderer > ().sprite;
                    Components.OptionCardZoomIn[2].sprite = CardHolding.transform.GetChild(1).GetComponent< SpriteRenderer > ().sprite;
                    Components.OptionCardZoomIn[3].sprite = CardHolding.transform.GetChild(3).GetComponent< SpriteRenderer > ().sprite;

                    if (Scripts.OptionClickControlScript.ReferencedResourceDisplay[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1].transform.childCount != 0)
                    {
                        InstantiatedResourceChange = Instantiate(Scripts.OptionClickControlScript.ReferencedResourceDisplay[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1]);
                        InstantiatedResourceChange.transform.localScale = new Vector3(1.6f, 1.6f, 1);
                        Quat = InstantiatedResourceChange.transform.localRotation;
                        Quat.eulerAngles = new Vector3(90, 180, 0);
                        InstantiatedResourceChange.transform.localRotation = Quat;
                        InstantiatedResourceChange.transform.position = new Vector3(-0.79f, 0, 2.42f);

                        for (int i = 0; i < InstantiatedResourceChange.transform.childCount; i++)
                        {
                            if (InstantiatedResourceChange.transform.GetChild(i).GetComponent< SpriteRenderer > ()) InstantiatedResourceChange.transform.GetChild(i).GetComponent< SpriteRenderer > ().sortingOrder += 10;
                        }


                        for (int i = 0; i < InstantiatedResourceChange.transform.childCount; i++)
                        {
                            if (InstantiatedResourceChange.transform.GetChild(i).GetComponent< SpriteRenderer > ())
                            {
                                InstantiatedResourceChange.transform.GetChild(i).GetComponent< SpriteRenderer > ().sortingLayerName = "Top";
                                InstantiatedResourceChange.transform.parent = Components.Magnifier.transform.GetChild(2).GetComponent< Transform > ();
                            }
                        }
                    }

                    else if (Scripts.OptionClickControlScript.EventDetails.SpecialCardNumber == 1)
                    {
                        Pos = Components.OptionCardZoomIn[3].transform.localPosition;
                        Pos.z = 2.02f;
                        Components.OptionCardZoomIn[3].transform.localPosition = Pos;

                        Scale = Components.OptionCardTextZoomIn[3].transform.localScale;
                        Scale.x = 0.075f;
                        Scale.y = 0.075f;
                        Components.OptionCardTextZoomIn[3].transform.localScale = Scale;       
                        Pos = Components.OptionCardTextZoomIn[3].rectTransform.localPosition;
                        Pos.y = -130;
                        Components.OptionCardTextZoomIn[3].transform.localPosition = Pos;
                        Components.OptionCardTextZoomIn[3].text = CardHolding.transform.GetChild(6).transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text;
                        Components.OptionCardTextZoomIn[3].enabled = true;
                    }

                    else {

                        Pos = Components.OptionCardTextZoomIn[3].rectTransform.localPosition;
                        Pos.y = -120.7f;
                        Components.OptionCardTextZoomIn[3].transform.localPosition = Pos;

                        if (Scripts.OptionClickControlScript.EventDetails.Combat.WarCheck[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1] == true)
                        {
                            Components.OptionCardTextZoomIn[3].text = CardHolding.transform.GetChild(6).transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text;
                            Scale = Components.OptionCardTextZoomIn[3].transform.localScale;
                            Scale.x = 0.08f;
                            Scale.y = 0.08f;
                            Components.OptionCardTextZoomIn[3].transform.localScale = Scale;
                            Components.OptionCardTextZoomIn[3].enabled = true;
                        }

                        else
                        {
                            Components.OptionCardTextZoomIn[3].text = CardHolding.transform.GetChild(6).transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text;
                            Scale = Components.OptionCardTextZoomIn[3].transform.localScale;
                            Scale.x = 0.1f;
                            Scale.y = 0.1f;
                            Components.OptionCardTextZoomIn[3].transform.localScale = Scale;
                            Components.OptionCardTextZoomIn[3].enabled = true;
                        }
                    }

                    Components.OptionCardTextZoomIn[0].text = CardHolding.transform.GetChild(6).transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text;
                    Components.OptionCardTextZoomIn[1].text = CardHolding.transform.GetChild(6).transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text;
                    Components.OptionCardTextZoomIn[2].text = CardHolding.transform.GetChild(6).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().text;
                    Components.OptionCardTextZoomIn[2].font = CardHolding.transform.GetChild(6).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().font;
                    Components.QuoteTextTransform.localScale = CardHolding.transform.GetChild(6).transform.GetChild(2).GetComponent< Transform > ().localScale;
                    Components.OptionCardTextZoomIn[4].text = CardHolding.transform.GetChild(6).transform.GetChild(4).GetComponent< UnityEngine.UI.Text > ().text;

                    switch (Components.OptionCardEffectExplanation[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1])
                    {
                        case 0: //No Effect
                            Components.OptionCardAdditionInfo[2].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[2];
                            break;

                        case 1: //Nature Morale Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "The morale of our community will\nincrease if this option is selected.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 2: //Nature Morale Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "The morale of our community will\ndecrease if this option is selected.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 3: //Human Morale Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will increase\nthe morale of humans.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 4: //Human Morale Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will decrease\nthe morale of humans.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 5: //Nature Food and Morale Affected
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "The morale and food of our community\nwill be affected if this option is selected.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 6: //Unlock Option Number
                            Components.OptionCardAdditionInfo[2].SetActive(false);
                            Components.OptionCardAdditionInfo[6].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[6];
                            break;

                        case 7: //Add Card
                            Pos = CardReference.OptionCardPosition.position;
                            Pos.z = 0.7f;
                            CardReference.OptionCardPosition.position = Pos;
                            var ReferencePath = Scripts.EventCardControlScript.EventDisplay.Normal[Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1] - 1].Position[Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1] - 1];
                            Components.OptionCardAdditionInfo[0].transform.GetChild(0).gameObject.SetActive(true);
                            Components.OptionCardAdditionInfo[0].transform.GetChild(1).gameObject.SetActive(true);
                            Components.OptionCardAdditionInfo[0].transform.GetChild(2).gameObject.SetActive(false);
                            Components.OptionCardAdditionInfo[0].transform.GetChild(3).gameObject.SetActive(false);

                            if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Revert:"))
                            {
                                Components.OptionCardAdditionInfo[0].transform.GetChild(0).gameObject.SetActive(false);
                                Components.OptionCardAdditionInfo[0].transform.GetChild(1).gameObject.SetActive(false);
                                Components.OptionCardAdditionInfo[0].transform.GetChild(2).gameObject.SetActive(true);
                                Components.OptionCardAdditionInfo[0].transform.GetChild(3).gameObject.SetActive(true);
                            }

                            Components.OptionCardAdditionInfo[0].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[0];
                            CardReference.QuickJump.SetActive(true);
                            CardReference.LeftArrow.SetActive(true);
                            CardReference.RightArrow.SetActive(true);
                            Components.OptionCardAdditionInfo[0].transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Prepare to draw" + " '" + ReferencePath.Name + "'\nnext year if this option is selected.";
                            DisplayAddedCard();
                            break;

                        case 8: //Remove Event
                            Components.OptionCardAdditionInfo[5].SetActive(true);
                            Components.OptionCardAdditionInfo[5].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Select this option to permanently remove" + '\n' + "the event: '" + Scripts.EventCardControlScript.Components.EventCardTitle.text + "'.";
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[5];
                            break;

                        case 9: //Discard Cards
                            Components.OptionCardAdditionInfo[7].transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "Discard " + Scripts.OptionClickControlScript.EventDetails.DiscardCards[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1] + " random resource \n cards if this option is selected.";
                            Components.OptionCardAdditionInfo[7].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[7];
                            break;

                        case 10: //Lose
                            Components.OptionCardAdditionInfo[8].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[8];
                            break;

                        case 11: //Promotion
                            break;

                        case 12: //Human Ties +ve
                            Components.OptionCardAdditionInfo[12].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[12];
                            break;

                        case 13: //Human Ties -ve
                            Components.OptionCardAdditionInfo[13].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[13];
                            break;

                        case 14: //Nature Food Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will increase\nour community's food reserves.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 15: //Nature Food Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will decrease\nour community's food reserves.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 16: //Human Food increase
                            break;

                        case 17: //Human Food decrease
                            break;

                        case 18: //Food Production change
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will affect our\nannual production of food.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 19: //Morale Production Change
                            break;

                        case 20: //Human Ties Production Change
                            break;

                        case 21: //War Nature Attacks
                            Components.OptionCardAdditionInfo[11].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[11];
                            break;

                        case 22: //War Nature Defends
                            Components.OptionCardAdditionInfo[10].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[10];
                            break;

                        case 23: //Perm Offense Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will permanentlyNWLincrease our army's offesnive strength.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 24: //Perm Offense Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will permanentlyNWLdecrease our army's offensive strength.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 25: //Perm Defence Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will permanentlyNWLincrease our army's defensive strength.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 26: //Perm Defence Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will permanentlyNWLdecrease our army's defensive strength.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 27: //Temp Offense Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will increase ourNWLarmy's offesnive strength for this year.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 28: //Temp Offense Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will decrease ourNWLarmy's offensive strength for this year.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 29: //Temp Defence Increase
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will increase ourNWLarmy's defensive strength for this year.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 30: //Temp Defence Decrease
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will decrease ourNWLarmy's defensive strength for this year.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 31: //Remove random event
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this option will removeNWLthe mentioned event permanently.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 32: //Remove random event and lose food
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Selecting this will remove the mentionedNWLevent and reduce our food reserves.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;

                        case 33: //Set Up Army
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Armies provide offensive and defensive\nstrength per year, at the cost of food.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                            break;
                    }


                    /*if (Scripts.OptionClickControlScript.EventDetails.DiscardCards[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] > 0)
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[7].transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = "Discard " + Scripts.OptionClickControlScript.EventDetails.DiscardCards[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] + " random resource \n cards if this option is selected.";
                        Components.OptionCardAdditionInfo[7].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[7];
                    }

                    else if (Scripts.OptionClickControlScript.EventDetails.LoseCheck[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] == true)
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[8].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[8];
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1].text.Contains("Reshuffle:"))
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[9].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[9];
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1].text.Contains("Add:"))
                    {
                        Pos = CardReference.OptionCardPosition.position;
                        Pos.z = 0.7f;
                        CardReference.OptionCardPosition.position = Pos;
                        var ReferencePath = Scripts.EventCardControlScript.EventDisplay.Normal[Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] - 1].Position[Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] - 1];
                        Components.OptionCardAdditionInfo[0].transform.GetChild(0).gameObject.SetActive(true);
                        Components.OptionCardAdditionInfo[0].transform.GetChild(1).gameObject.SetActive(true);
                        Components.OptionCardAdditionInfo[0].transform.GetChild(2).gameObject.SetActive(false);
                        Components.OptionCardAdditionInfo[0].transform.GetChild(3).gameObject.SetActive(false);

                        if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1].text.Contains("Revert:")){
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
                        Components.OptionCardAdditionInfo[0].transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = "Prepare to draw" + " '" + ReferencePath.Name + "'\nnext year if this option is selected.";
                        DisplayAddedCard();
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Enemy Defence:"))
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[11].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[11];
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Enemy Offense:"))
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[10].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[10];
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Human Ties"))
                    {
                        if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("+"))
                        {
                            Components.OptionCardAdditionInfo[2].SetActive(false);
                            Components.OptionCardAdditionInfo[12].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[12];
                        }

                        else
                        {
                            Components.OptionCardAdditionInfo[2].SetActive(false);
                            Components.OptionCardAdditionInfo[13].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[13];
                        }
                    }

                    else if (Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] == -1)
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[3].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[3];
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Morale"))
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);

                        if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Human Morale"))
                        {
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Choosing this option will affect\nthe morale of humans.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                        }

                        else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent<OptionCardMovementCS>().OptionCardNumber - 1].text.Contains("Food"))
                        {
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "The morale and food of our community\nwill be affected if this option is selected.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                        }

                        else
                        {
                            Components.OptionCardAdditionInfo[4].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "The morale of our community will be\naffected if this option is selected.";
                            Components.OptionCardAdditionInfo[4].SetActive(true);
                            ShownAdditionalInfo = Components.OptionCardAdditionInfo[4];
                        }
                        
                    }

                    else if (Scripts.OptionClickControlScript.EventDetails.UnlockOptionNumber[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] != 0)
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[6].SetActive(true);
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[6];
                    }

                    else if (Scripts.EventCardControlScript.Components.OptionCardEffect[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1].text == "Remove this event")
                    {
                        Components.OptionCardAdditionInfo[2].SetActive(false);
                        Components.OptionCardAdditionInfo[5].SetActive(true);
                        Components.OptionCardAdditionInfo[5].transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text = "Select this option to permanently remove" + '\n' + "the event: '" + Scripts.EventCardControlScript.Components.EventCardTitle.text + "'.";
                        ShownAdditionalInfo = Components.OptionCardAdditionInfo[5];
                    }*/
                }

                else if (CardSelected.tag == "PlayedCards")
                {
                    Components.Magnifier.SetActive(true);
                    MagnifierClickedCheck = true;
                    Components.Magnifier.transform.GetChild(0).gameObject.SetActive(true);
                    Components.ResourceCardZoomIn[0].sprite = CardHolding.GetComponent<SpriteRenderer>().sprite;
                    Components.ResourceCardZoomIn[1].sprite = CardHolding.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    Components.ResourceCardZoomIn[2].sprite = CardHolding.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
                    Components.ResourceCardZoomIn[3].sprite = CardHolding.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite;
                    Components.ResourceCardTextZoomIn[0].text = CardHolding.transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text;
                    Components.ResourceCardTextZoomIn[0].transform.localPosition = CardHolding.transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().transform.localPosition;
                    Components.ResourceCardTextZoomIn[1].text = CardHolding.transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text;
                    Components.ResourceCardTextZoomIn[1].transform.localPosition = CardHolding.transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().transform.localPosition;
                    Components.ResourceCardTextZoomIn[2].text = CardHolding.transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text;
                    Components.ResourceCardTextZoomIn[2].transform.localPosition = CardHolding.transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().transform.localPosition;
                }

                else if (CardSelected.tag == "EventCards" || CardSelected.tag == "NoOptionEventCard")
                {
                    Components.Magnifier.SetActive(true);
                    MagnifierClickedCheck = true;
                    Components.Magnifier.transform.GetChild(1).gameObject.SetActive(true);
                    Components.EventCardZoomIn[0].sprite = CardHolding.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    Components.EventCardZoomIn[1].sprite = CardHolding.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
                    Components.EventCardZoomIn[2].sprite = CardHolding.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite;
                    Components.EventCardTextZoomIn[0].text = CardHolding.transform.GetChild(3).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text;
                    Components.EventCardTextZoomIn[1].text = CardHolding.transform.GetChild(3).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text;
                    Components.EventCardTextZoomIn[2].text = CardHolding.transform.GetChild(3).transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text;
                }
            }
            TimePressed = 2;
        }
    }

    public void ClickCloseMagnifier()
    {
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        if (ShownAdditionalInfo) ShownAdditionalInfo.SetActive(false);
        OptionCardClickCheck = false;
        EventCardClickCheck = false;
        NoOptionEventCardClickCheck = false;
        DeselectResourceCardCheck = false;
        CardReference.ButtonSpheresImage[0].color = Color.white;
        CardReference.ButtonSpheresImage[1].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[2].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[3].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[4].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.QuickJump.SetActive(false);
        CardReference.LeftArrow.SetActive(false);
        CardReference.RightArrow.SetActive(false);
        StartCoroutine(ClearMagnifier());
    }

    public IEnumerator ClearMagnifier()
    {
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

        if (Scripts.MainMenuControllerScript.LevelSelected == 1)
        {
            Important.NoClicking = true;
            Pos = Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position;
            Pos.x = 20;
            Scripts.MainMenuControllerScript.TutorialUse.TutorialButtons.transform.position = Pos;
            Scale = Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale;
            Scale.x = 0.04f;
            Scale.y = 0.04f;
            Scripts.MainMenuControllerScript.TutorialUse.Tutorial[1].transform.localScale = Scale;
            Scripts.MainMenuControllerScript.TutorialUse.TutorialText.text = "Moving on...";
            Scripts.MainMenuControllerScript.TutorialUse.TutorialAnim[0].Play("CallOutWobble");
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(Scripts.EventCardControlScript.EventCardRemoved());
            yield break;
        }
    }

    private int[] NumberOfResourcesGained = new int[3];
    private int[] NumberOfResourcesLost = new int[3];
    private GameObject[] ReferencedResourceDisplay = new GameObject[3];
    private int[] ResourceDisplay1 = new int[6];
    private int[] ResourceDisplay2 = new int[6];
    private int[] ResourceDisplay3 = new int[6];

    public void DisplayAddedCard()
    {
        for (int i = 0; i < 3; i++)
        {
            ResourceDisplay1[i] = 0;
            ResourceDisplay2[i] = 0;
            ResourceDisplay3[i] = 0;
            ResourceDisplay1[i + 3] = 0;
            ResourceDisplay2[i + 3] = 0;
            ResourceDisplay3[i + 3] = 0;
            NumberOfResourcesGained[i] = 0;
            NumberOfResourcesLost[i] = 0;
            if (ReferencedResourceDisplay[i] != null) ReferencedResourceDisplay[i].SetActive(false);
        }

        var ReferencePath = Scripts.EventCardControlScript.EventDisplay.Normal[Scripts.OptionClickControlScript.EventDetails.CalledCardTechLevel[CardHolding.GetComponent< OptionCardMovementCS > ().OptionCardNumber - 1] - 1].Position[Scripts.OptionClickControlScript.EventDetails.CalledCardPosition[CardHolding.GetComponent< OptionCardMovementCS> ().OptionCardNumber - 1] - 1];

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

        if (ReferencePath.OptionCardTitle[2] == "")         // If Option 3 is empty
        {    
            CardReference.OptionCardSprite[2].sprite = Scripts.EventCardControlScript.Components.LockedOptionSprite;
            CardReference.OptionCardText[6].text = "Option Unavailable";
            CardReference.OptionCardText[7].text = "Make an empty space and\ncreativity will instantly fill it.";
        }

        //Option1
        if (ReferencePath.ResourcesUsed[0] != 0)
        {
            NumberOfResourcesLost[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = ReferencePath.ResourcesUsed[0];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesUsed[1] != 0)
        {
            NumberOfResourcesLost[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = ReferencePath.ResourcesUsed[1];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesUsed[2] != 0)
        {
            NumberOfResourcesLost[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = ReferencePath.ResourcesUsed[2];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[0] != 0)
        {
            NumberOfResourcesGained[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = ReferencePath.ResourcesGiven[0];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[1] != 0)
        {
            NumberOfResourcesGained[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = ReferencePath.ResourcesGiven[1];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[2] != 0)
        {
            NumberOfResourcesGained[0] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay1[checkempty] == 0)
                {
                    ResourceDisplay1[checkempty] = ReferencePath.ResourcesGiven[2];
                    break;
                }
            }
        }

        ReferencedResourceDisplay[0] = CardReference.ResourceReqDisplay[0].LostDisplay[NumberOfResourcesLost[0]].GainDisplay[NumberOfResourcesGained[0]];
        ReferencedResourceDisplay[0].SetActive(true);

        for (int a = 0; a < ReferencedResourceDisplay[0].transform.childCount - 2; a++)
        {
            ReferencedResourceDisplay[0].transform.GetChild(a).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[ResourceDisplay1[a] - 1];
        }

        //Option2
        if (ReferencePath.ResourcesUsed[3] != 0)
        {
            NumberOfResourcesLost[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = ReferencePath.ResourcesUsed[3];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesUsed[4] != 0)
        {
            NumberOfResourcesLost[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = ReferencePath.ResourcesUsed[4];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesUsed[5] != 0)
        {
            NumberOfResourcesLost[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = ReferencePath.ResourcesUsed[5];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[3] != 0)
        {
            NumberOfResourcesGained[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = ReferencePath.ResourcesGiven[3];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[4] != 0)
        {
            NumberOfResourcesGained[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = ReferencePath.ResourcesGiven[4];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[5] != 0)
        {
            NumberOfResourcesGained[1] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay2[checkempty] == 0)
                {
                    ResourceDisplay2[checkempty] = ReferencePath.ResourcesGiven[5];
                    break;
                }
            }
        }

        ReferencedResourceDisplay[1] = CardReference.ResourceReqDisplay[1].LostDisplay[NumberOfResourcesLost[1]].GainDisplay[NumberOfResourcesGained[1]];
        ReferencedResourceDisplay[1].SetActive(true);

        for (int a = 0; a < ReferencedResourceDisplay[1].transform.childCount - 2; a++)
        {
            ReferencedResourceDisplay[1].transform.GetChild(a).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[ResourceDisplay2[a] - 1];
        }

        //Option3
        if (ReferencePath.ResourcesUsed[6] != 0)
        {
            NumberOfResourcesLost[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = ReferencePath.ResourcesUsed[6];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesUsed[7] != 0)
        {
            NumberOfResourcesLost[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = ReferencePath.ResourcesUsed[7];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesUsed[8] != 0)
        {
            NumberOfResourcesLost[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = ReferencePath.ResourcesUsed[8];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[6] != 0)
        {
            NumberOfResourcesGained[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = ReferencePath.ResourcesGiven[6];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[7] != 0)
        {
            NumberOfResourcesGained[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = ReferencePath.ResourcesGiven[7];
                    break;
                }
            }
        }

        if (ReferencePath.ResourcesGiven[8] != 0)
        {
            NumberOfResourcesGained[2] += 1;
            for (int checkempty = 0; checkempty < 6; checkempty++)
            {
                if (ResourceDisplay3[checkempty] == 0)
                {
                    ResourceDisplay3[checkempty] = ReferencePath.ResourcesGiven[8];
                    break;
                }
            }
        }

        ReferencedResourceDisplay[2] = CardReference.ResourceReqDisplay[2].LostDisplay[NumberOfResourcesLost[2]].GainDisplay[NumberOfResourcesGained[2]];
        ReferencedResourceDisplay[2].SetActive(true);

        for (int a = 0; a < ReferencedResourceDisplay[2].transform.childCount - 2; a++)
        {
            ReferencedResourceDisplay[2].transform.GetChild(a).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[ResourceDisplay3[a] - 1];
        }
    }

    public void Select0()
    {
        CardReference.ButtonSpheresImage[0].color = Color.white;
        CardReference.ButtonSpheresImage[1].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[2].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[3].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[4].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
    }

    public void Select1()
    {
        CardReference.ButtonSpheresImage[1].color = Color.white;
        CardReference.ButtonSpheresImage[2].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[3].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[4].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[0].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
    }

    public void Select2()
    {
        CardReference.ButtonSpheresImage[2].color = Color.white;
        CardReference.ButtonSpheresImage[3].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[4].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[0].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[1].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
    }

    public void Select3()
    {
        CardReference.ButtonSpheresImage[3].color = Color.white;
        CardReference.ButtonSpheresImage[4].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[0].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[1].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[2].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
    }

    public void Select4()
    {
        CardReference.ButtonSpheresImage[4].color = Color.white;
        CardReference.ButtonSpheresImage[0].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[1].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[2].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
        CardReference.ButtonSpheresImage[3].color = new Vector4(0.4784f, 0.4431f, 0.4431f, 0.4431f);
    }

    public void UpdateNumberOfCards()
    {
        Important.NumberOfCards = transform.childCount - 1;                          //Don't count pivot point into number of cards. 
        CardGapCloseCheck = false;
        TesterLerp = 0;

        if (Important.NumberOfCards >= 2)
        {
            LeftMostCard = transform.GetChild(0).gameObject;
            RightMostCard = transform.GetChild(Important.NumberOfCards - 1).gameObject; //Reference for right most card will be -1 in the [] brackets.
        }

        if (Important.NumberOfCards == 1)
        {
            LeftMostCard = transform.GetChild(0).gameObject;
            RightMostCard = LeftMostCard;
        }

        if (Important.NumberOfCards == 0)
        {
            LeftMostCard = null;
            RightMostCard = null;
        }
        AdjustmentCheck = true;
    }

    public void SwipeUpReleased()
    {

        if (Camera.main.WorldToScreenPoint(CardSelected.transform.position).y > Screen.height / 4)
        {
            CheckIfCardIsPlayed = true;
        }

        if (CheckIfCardIsPlayed == true)
        {
            if (Components.CardsPlayed[0] == null)
            {
                Components.CardsPlayed[0] = CardSelected;
                CheckIfCardIsPlayed = false;
                StartMovingCheck = true;
                LerpProgress = 0.0f;
                Components.CardsPlayed[0].tag = "PlayedCards";
                Components.CardsPlayed[0].GetComponent< ResourceCardControlCS > ().CardPlayPosition = 1;
                return;
            }

            if (Components.CardsPlayed[1] == null)
            {
                Components.CardsPlayed[1] = CardSelected;
                CheckIfCardIsPlayed = false;
                StartMovingCheck = true;
                LerpProgress = 0.0f;
                Components.CardsPlayed[1].tag = "PlayedCards";
                Components.CardsPlayed[1].GetComponent< ResourceCardControlCS > ().CardPlayPosition = 2;
                return;
            }

            if (Components.CardsPlayed[2] == null)
            {
                Components.CardsPlayed[2] = CardSelected;
                CheckIfCardIsPlayed = false;
                StartMovingCheck = true;
                LerpProgress = 0.0f;
                Components.CardsPlayed[2].tag = "PlayedCards";
                Components.CardsPlayed[2].GetComponent< ResourceCardControlCS > ().CardPlayPosition = 3;
                return;
            }
        }
        if (NullCardInstance != null)
        {
            DestroyImmediate(NullCardInstance);
            NullCardInstance = null;
        }
        NullCardCount = 0;
        CardSelected.transform.parent = this.gameObject.transform;

        var CurrentCardNumber = CardSelected.GetComponent< ResourceCardControlCS > ().CardNumber;
        ArrayList SortAscending = new ArrayList();

        if (Components.CardsPlayed[0] != null) SortAscending.Add(Components.CardsPlayed[0].GetComponent< ResourceCardControlCS > ().CardNumber);
        if (Components.CardsPlayed[1] != null) SortAscending.Add(Components.CardsPlayed[1].GetComponent< ResourceCardControlCS > ().CardNumber);
        if (Components.CardsPlayed[2] != null) SortAscending.Add(Components.CardsPlayed[2].GetComponent< ResourceCardControlCS > ().CardNumber);
        SortAscending.Add(CurrentCardNumber);
        SortAscending.Sort();
        int temp;

        if (SortAscending.Count >= 1)
        {
            temp = System.Convert.ToInt32(SortAscending[0]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 1);
            }
        }

        if (SortAscending.Count >= 2)
        {
            temp = System.Convert.ToInt32(SortAscending[1]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 2);
            }
        }

        if (SortAscending.Count >= 3)
        {
            temp = System.Convert.ToInt32(SortAscending[2]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 3);
            }
        }

        if (SortAscending.Count >= 4)
        {
            temp = System.Convert.ToInt32(SortAscending[3]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 4);
            }
        }

        Important.NumberOfCards = transform.childCount - 1;
        UpdateNumberOfCards();
    }

    public void AssignCardNumber()
    {

        if (Components.CardsPlayed[0] == null && Components.CardsPlayed[1] == null && Components.CardsPlayed[2] == null)
        {
            for (int i = 0; i < Important.NumberOfCards; i++)
        {
                if (transform.GetChild(i).gameObject.tag != "Pivot")
                {
                    transform.GetChild(i).gameObject.GetComponent< ResourceCardControlCS > ().CardNumber = i + 1;
                }
            }
        }

        RotatingInProgress = true;
        if (AdjustmentCheck == true)
        {
            TesterLerp = 0;
            AdjustmentCheck = false;
        }

        StartCoroutine(ShowRotateToCentre(CardSelected.transform.GetSiblingIndex()));
        CardSelected.transform.parent = Components.CardsPlayedParent.transform;

        for (int i = 0; i < 4; i++)
        {
            if (CardsPlayed[i] != null)
            {
                if (CardsPlayed[i].transform.parent != Components.CardsPlayedParent.transform)
                {
                    CardsPlayed[i] = CardSelected;
                    break;
                }
            }

            if (CardsPlayed[i] == null)
            {
                CardsPlayed[i] = CardSelected;
                break;
            }
        }
    }

    public void DeselectCard()
    {

        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        CardSelected.transform.parent = this.gameObject.transform;
        int CurrentCardNumber;
        CurrentCardNumber = CardSelected.GetComponent< ResourceCardControlCS > ().CardNumber;

        ArrayList SortAscending = new ArrayList();

        if (Components.CardsPlayed[0] != null) SortAscending.Add(Components.CardsPlayed[0].GetComponent< ResourceCardControlCS > ().CardNumber);
        if (Components.CardsPlayed[1] != null) SortAscending.Add(Components.CardsPlayed[1].GetComponent< ResourceCardControlCS > ().CardNumber);
        if (Components.CardsPlayed[2] != null) SortAscending.Add(Components.CardsPlayed[2].GetComponent< ResourceCardControlCS > ().CardNumber);
        SortAscending.Sort();
        int temp;

        if (SortAscending.Count >= 1)
        {
            temp = System.Convert.ToInt32(SortAscending[0]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 1);
            }
        }

        if (SortAscending.Count >= 2)
        {
            temp = System.Convert.ToInt32(SortAscending[1]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 2);
            }
        }

        if (SortAscending.Count >= 3)
        {
            temp = System.Convert.ToInt32(SortAscending[2]);

            if (CurrentCardNumber == temp)
            {
                CardSelected.transform.SetSiblingIndex(CurrentCardNumber - 3);
            }
        }

        Components.CardsPlayed[CardSelected.GetComponent< ResourceCardControlCS > ().CardPlayPosition - 1] = null;
        CardSelected.GetComponent< ResourceCardControlCS > ().CardPlayPosition = 0;


        for (var i = 0; i < Components.CardsPlayed.Length - 1; i++)         //Destroy and fill in gap for CardsPlayed Array when a card is deselected
        {       
            if (Components.CardsPlayed[i] == null)
            {
                Components.CardsPlayed[i] = Components.CardsPlayed[i + 1];
                Components.CardsPlayed[i + 1] = null;
                if (Components.CardsPlayed[i] != null) Components.CardsPlayed[i].GetComponent< ResourceCardControlCS > ().CardPlayPosition = i + 1;
            }
        }


        Important.NumberOfCards = transform.childCount - 1;
        CardGapCloseCheck = false;
        UpdateNumberOfCards();
        TesterLerp = 0;
        CardSelected.tag = "ResourceCards";

        if (Components.CardsPlayed[0] != null)
        {
            StartMovingCheck = true;
            LerpProgress = 0.0f;
        }
    }

    public IEnumerator DiscardCards(int Card1, int Card2, int Card3)
    {
        for (int i = 0; i < Important.NumberOfCards; i++)
        {
            if (transform.GetChild(i).gameObject.tag != "Pivot")
            {
                transform.GetChild(i).gameObject.GetComponent<ResourceCardControlCS>().CardNumber = i + 1;
            }
        }

        Components.CardsPlayed[0] = transform.GetChild(Card1).gameObject;
        transform.GetChild(Card1).gameObject.tag = "DestroyedCards";

        if (Card2 != -1)
        {
            Components.CardsPlayed[1] = transform.GetChild(Card2).gameObject;
            transform.GetChild(Card2).gameObject.tag = "DestroyedCards";
        }

        if (Card3 != -1)
        {
            Components.CardsPlayed[2] = transform.GetChild(Card3).gameObject;
            transform.GetChild(Card3).gameObject.tag = "DestroyedCards";
        }

        Components.CardsPlayed[0].transform.parent = Components.CardsPlayedParent.transform;
        if (Card2 != -1) Components.CardsPlayed[1].transform.parent = Components.CardsPlayedParent.transform;
        if (Card3 != -1) Components.CardsPlayed[2].transform.parent = Components.CardsPlayedParent.transform;

        StartMovingCheck = true;
        LerpProgress = 0.0f;

        Important.NumberOfCards = transform.childCount - 1;                                 //Don't count pivot point into number of cards. 
        CardGapCloseCheck = false;
        TesterLerp = 0;

        if (Important.NumberOfCards >= 2)
        {
            LeftMostCard = transform.GetChild(0).gameObject;
            RightMostCard = transform.GetChild(Important.NumberOfCards - 1).gameObject;     //Reference for right most card will be -1 in the [] brackets.
        }

        if (Important.NumberOfCards == 1)
        {
            LeftMostCard = transform.GetChild(0).gameObject;
            RightMostCard = LeftMostCard;
        }

        if (Important.NumberOfCards == 0)
        {
            LeftMostCard = null;
            RightMostCard = null;
        }

        AdjustmentCheck = true;

        yield return new WaitForSeconds (0.7f);
        Scripts.ParticleEffectControllerScript.DestroyExtraCards();
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[6]);
        yield return new WaitForSeconds (0.7f);
        Destroy(Components.CardsPlayed[0]);
        Components.CardsPlayed[0] = null;
        Destroy(Components.CardsPlayed[1]);
        Components.CardsPlayed[1] = null;
        Destroy(Components.CardsPlayed[2]);
        Components.CardsPlayed[2] = null;

        UpdateNumberOfCards();
        yield return new WaitForSeconds (1.2f);
        Important.NumberOfCards = transform.childCount - 1;
        Scripts.UIControllerScript.UpdateResourceCardNumber(Important.NumberOfCards);
    }

    public void DestroyExtra3Cards(int Card1, int Card2, int Card3)
    {

        for (int i = 0; i < Important.NumberOfCards; i++){
            if (transform.GetChild(i).gameObject.tag != "Pivot")
            {
                transform.GetChild(i).gameObject.GetComponent< ResourceCardControlCS > ().CardNumber = i + 1;
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
        LerpProgress = 0.0f;

        Important.NumberOfCards = transform.childCount - 1;                          //Don't count pivot point into number of cards. 
        CardGapCloseCheck = false;
        TesterLerp = 0;

        if (Important.NumberOfCards >= 2)
        {
            LeftMostCard = transform.GetChild(0).gameObject;
            RightMostCard = transform.GetChild(Important.NumberOfCards - 1).gameObject; //Reference for right most card will be -1 in the [] brackets.
        }

        if (Important.NumberOfCards == 1)
        {
            LeftMostCard = transform.GetChild(0).gameObject;
            RightMostCard = LeftMostCard;
        }

        if (Important.NumberOfCards == 0)
        {
            LeftMostCard = null;
            RightMostCard = null;
        }

        AdjustmentCheck = true;
        Important.NoClicking = false;
    }

    public IEnumerator DestroyExtra3CardsConfirmed()
    {
        yield return new WaitForSeconds (0.5f);
        Destroy(Components.CardsPlayed[0]);
        Components.CardsPlayed[0] = null;
        Destroy(Components.CardsPlayed[1]);
        Components.CardsPlayed[1] = null;
        Destroy(Components.CardsPlayed[2]);
        Components.CardsPlayed[2] = null;

        UpdateNumberOfCards();
        yield return new WaitForSeconds (1.2f);
        Important.NumberOfCards = transform.childCount - 1;
        SortHandCards();
        Scripts.EventCardControlScript.Components.anim.Play("RemoveCard");
        yield return new WaitForSeconds (1.183f);
        Components.EventCardGameObject.tag = "EventCards";
        StartCoroutine(Scripts.EventCardControlScript.EventCardManager());
    }

    public void SortHandCards()         //To insert a line break when inputing from string, add a \n. E.g: "hey\ns" will print 2 lines, hey and s.
    {
        Important.NumberOfCards = transform.childCount - 1;
        Components.Pivot.transform.SetSiblingIndex(Important.NumberOfCards);
        Scripts.UIControllerScript.UpdateResourceCardNumber(Important.NumberOfCards);
        Components.ResourceTypeCardCount[0] = 0;
        Components.ResourceTypeCardCount[1] = 0;
        Components.ResourceTypeCardCount[2] = 0;
        Components.ResourceTypeCardCount[3] = 0;
        Components.ResourceTypeCardCount[4] = 0;

        for (int i = 0; i < Important.NumberOfCards; i++)
        {
            Components.CardTypesInHand[i] = transform.GetChild(i).gameObject.GetComponent<ResourceCardControlCS>().ResourceType;
            if (Components.CardTypesInHand[i] == 0.01f)
            {
                Components.ResourceTypeCardCount[0] += 1;
            }
            else if (Components.CardTypesInHand[i] == 0.1f)
            {
                Components.ResourceTypeCardCount[1] += 1;
            }
            else if (Components.CardTypesInHand[i] == 1)
            {
                Components.ResourceTypeCardCount[2] += 1;
            }
            else if (Components.CardTypesInHand[i] == 10)
            {
                Components.ResourceTypeCardCount[3] += 1;
            }
            else if (Components.CardTypesInHand[i] == 100)
            {
                Components.ResourceTypeCardCount[4] += 1;
            }
        }

        for (int a = 0; a < Components.ResourceTypeCardCount[0]; a++)       //To count number of each type of card
        {                                             
            transform.GetChild(a).gameObject.GetComponent< ResourceCardControlCS > ().ResourceType = 0.01f;
            transform.GetChild(a).gameObject.transform.GetChild(0).GetComponent< SpriteRenderer > ().sprite = Components.SpriteImageResourceCards[0];
            transform.GetChild(a).gameObject.transform.GetChild(2).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[0];
            transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceName[0];
            transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceQuote[0];
            transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[0];
            transform.GetChild(a).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[0];
        }

        for (int b = 0; b < Components.ResourceTypeCardCount[1]; b++)
        {
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.GetComponent< ResourceCardControlCS > ().ResourceType = 0.1f;
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(0).GetComponent< SpriteRenderer > ().sprite = Components.SpriteImageResourceCards[1];
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(2).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[1];
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceName[1];
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceQuote[1];
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[1];
            transform.GetChild(Components.ResourceTypeCardCount[0] + b).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[1];
        }

        for (int c = 0; c < Components.ResourceTypeCardCount[2]; c++)
        {
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.GetComponent< ResourceCardControlCS > ().ResourceType = 1;
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(0).GetComponent< SpriteRenderer > ().sprite = Components.SpriteImageResourceCards[2];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(2).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[2];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceName[2];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceQuote[2];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[2];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + c).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[2];
        }

        for (int d = 0; d < Components.ResourceTypeCardCount[3]; d++)
        {
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.GetComponent< ResourceCardControlCS > ().ResourceType = 10;
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(0).GetComponent< SpriteRenderer > ().sprite = Components.SpriteImageResourceCards[3];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(2).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[3];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceName[3];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceQuote[3];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[3];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + d).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[3];
        }

        for (int e = 0; e < Components.ResourceTypeCardCount[4]; e++)
        {
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.GetComponent< ResourceCardControlCS > ().ResourceType = 100;
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(0).GetComponent< SpriteRenderer > ().sprite = Components.SpriteImageResourceCards[4];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(2).GetComponent< SpriteRenderer > ().sprite = Scripts.OptionClickControlScript.Components.ResourceSymbol[4];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceName[4];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(1).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.ResourceQuote[4];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(1).transform.localPosition = Scripts.OptionClickControlScript.Components.QuotePosition[4];
            transform.GetChild(Components.ResourceTypeCardCount[0] + Components.ResourceTypeCardCount[1] + Components.ResourceTypeCardCount[2] + Components.ResourceTypeCardCount[3] + e).gameObject.transform.GetChild(3).transform.GetChild(2).GetComponent< UnityEngine.UI.Text > ().text = Scripts.OptionClickControlScript.Components.QuoteAuthor[4];
        }

        Scripts.UIControllerScript.UpdateLifeResourceCardNumber(Components.ResourceTypeCardCount[0]);
        Scripts.UIControllerScript.UpdateChaosResourceCardNumber(Components.ResourceTypeCardCount[4]);
        UpdateInformation();
    }

    private string Sign;

    public string DetermineSign(int IntToCheck)
    {
        if (IntToCheck > 0) Sign = "+";
        if (IntToCheck <= 0) Sign = "";
        return Sign;
    }

    public void UpdateInformation()
    {
        int NetFood = Scripts.EventCardControlScript.Important.FoodProduction - Components.ResourceTypeCardCount[0] * 3;

        Scripts.UIControllerScript.InformationOverviewText[1].text = "Food: " + DetermineSign(NetFood) + NetFood;
        Scripts.UIControllerScript.InformationOverviewText[2].text = "Morale: " + DetermineSign(Scripts.EventCardControlScript.Important.Morale) + Scripts.EventCardControlScript.Important.Morale;
        Scripts.UIControllerScript.InformationOverviewText[3].text = "Human Ties: " + DetermineSign(Scripts.EventCardControlScript.Important.HumanTies) + Scripts.EventCardControlScript.Important.HumanTies;
        Scripts.UIControllerScript.InformationOverviewText[4].text = "Food Consumption: " + Components.ResourceTypeCardCount[0] * 3;
        Scripts.UIControllerScript.InformationOverviewText[5].text = "Food Production: " + Scripts.EventCardControlScript.Important.FoodProduction;
        Scripts.UIControllerScript.InformationCharacterText[0].text = "Attack: " + Scripts.NatureControllerScript.OffenseLeft + " (+" + Scripts.NatureControllerScript.Offense + ")";
        Scripts.UIControllerScript.InformationCharacterText[1].text = "Defence: " + Scripts.NatureControllerScript.OffenseLeft + " (+" + Scripts.NatureControllerScript.Offense + ")";
    }

    public void SortRendering(int BackgroundRender, GameObject CardToSort)
    {
        Pos = CardToSort.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
        Pos.z = -0.01f;
        CardToSort.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition = Pos;

        Pos = CardToSort.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition;
        Pos.z = -0.02f;
        CardToSort.transform.GetChild(1).gameObject.GetComponent<Transform>().localPosition = Pos;

        Pos = CardToSort.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition;
        Pos.z = -0.03f;
        CardToSort.transform.GetChild(2).gameObject.GetComponent<Transform>().localPosition = Pos;

        Pos = CardToSort.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition;
        Pos.z = -0.02f;
        CardToSort.transform.GetChild(3).gameObject.GetComponent<Transform>().localPosition = Pos;
    }

    public void RotateToCentre()    //Problem
    {

        int CardCount = transform.childCount - 1;
        if (CardCount % 2 == 1)
        {
            int CardNumberToRotate = (CardCount - 1) / 2;
            while (transform.GetChild(CardNumberToRotate).GetComponent<Transform>().rotation.x > 0)     //rotation.eulerAngles.y > 180.5f
            {
                transform.RotateAround(CentrePoint, Vector3.down, 100 * Time.deltaTime);
            }

            while (transform.GetChild(CardNumberToRotate).GetComponent<Transform>().rotation.x < 0)     //rotation.eulerAngles.y < 179.5f
            {
                transform.RotateAround(CentrePoint, Vector3.up, 10 * Time.deltaTime);
            }
        }

        if (CardCount % 2 == 0)
        {
            int CardNumberToRotate = CardCount / 2 - 1;
            while (transform.GetChild(CardNumberToRotate).GetComponent<Transform>().rotation.x > -0.05547891f)  //rotation.eulerAngles.y > 171.5f
            {
                transform.RotateAround(CentrePoint, Vector3.down, 100 * Time.deltaTime);
            }

            while (transform.GetChild(CardNumberToRotate).GetComponent< Transform > ().rotation.x < -0.05547891f)   //rotation.eulerAngles.y < 170.5f
            {
                transform.RotateAround(CentrePoint, Vector3.up, 10 * Time.deltaTime);
            }
        }

        if (Scripts.MainMenuControllerScript.LevelSelected != 1)    //Note: Cannot use transform.RotateAround if ResourceCards.Position != 0.
        {
            Pos = Scripts.MainMenuControllerScript.TutorialUse.ResourceCards.position;
            Pos.z = 15;
            Scripts.MainMenuControllerScript.TutorialUse.ResourceCards.position = Pos;
        }
        Components.ResourceCardsReadyCheck = true;
    }

    [System.Serializable]
    public class ComponentReference2
    {
        public Vector3[] CardPositions = new Vector3[15];
        public Quaternion[] CardRotations = new Quaternion[15];
        public Sprite[] SpriteImageResourceCards = new Sprite[5];
        public float[] CardTypesInHand = new float[15];
        public int[] ResourceTypeCardCount = new int[5];        // 0 = Life, 1 = Resource, 2 = faith, 3 = human, 4 = destruction
        public GameObject[] CardsPlayed = new GameObject[3];
        public GameObject NullCard;
        public GameObject Magnifier;
        public Transform QuoteTextTransform;
        public SpriteRenderer MagnifiedSprite;
        public SpriteRenderer[] ResourceCardZoomIn = new SpriteRenderer[4];
        public UnityEngine.UI.Text[] ResourceCardTextZoomIn = new UnityEngine.UI.Text[3];
        public SpriteRenderer[] EventCardZoomIn = new SpriteRenderer[3];
        public UnityEngine.UI.Text[] EventCardTextZoomIn = new UnityEngine.UI.Text[3];
        public SpriteRenderer[] OptionCardZoomIn = new SpriteRenderer[4];
        public UnityEngine.UI.Text[] OptionCardTextZoomIn = new UnityEngine.UI.Text[5];
        public GameObject[] OptionCardAdditionInfo = new GameObject[15];
        public GameObject Pivot;
        public GameObject EventCardGameObject;
        public GameObject CardsPlayedParent;
        public bool ResourceCardsReadyCheck;
        public int[] OptionCardEffectExplanation = new int[3];
}

    [System.Serializable]
    public class ImportantVars1
    {
        public bool NoClicking;
        public bool GamePaused;
        public int SelectedOptionCardNumber;
        public int NumberOfCards;
    }

    [System.Serializable]
    public class ReferencedCard
    {
        public SpriteRenderer EventCardSprite;
        public UnityEngine.UI.Text[] EventCardText = new UnityEngine.UI.Text[3];
        public SpriteRenderer[] OptionCardSprite = new SpriteRenderer[3];
        public UnityEngine.UI.Text[] OptionCardText = new UnityEngine.UI.Text[9];
        public ResourceDisplay[] ResourceReqDisplay = new ResourceDisplay[3];
        public UnityEngine.UI.Image[] ButtonSpheresImage = new UnityEngine.UI.Image[5];
        public GameObject QuickJump;
        public GameObject LeftArrow;
        public GameObject RightArrow;
        public Transform OptionCardPosition;
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
