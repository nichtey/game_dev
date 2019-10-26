using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerCS : MonoBehaviour {

    public CameraControlCS.ScriptReference Scripts;
    public Text ResourceCardCounterText;
    public Text LifeResourceCardCounterText;
    public Text ChaosResourceCardCounterText;
    public GameObject ResourceCardExplanation;
    public Text ResourceCardExplanationText;
    public GameObject PauseButton;
    public GameObject PauseMenu;
    public GameObject DimScreen;
    public Animation anim;
    public GameObject RoundNumberDisplay;
    public Text RoundNumberDisplayText;
    public string BcOrAd;
    public int CurrentYear;
    public GameObject Information;
    public GameObject[] InformationText = new GameObject[2];
    public Text[] InformationOverviewText = new Text[10];
    public Text[] InformationCharacterText = new Text[9];
    public Image[] InformationSkillSpriteHolders = new Image[6];
    public SpriteRenderer[] InformationSkillDesign = new SpriteRenderer[6];
    public Animation TabAnim;

	void Start () {
		
	}

    public void ClearMedals()
    {
        for (int i = 0; i < 6; i++)
        {
            InformationSkillSpriteHolders[i].enabled = false;
            InformationSkillDesign[i].color = Color.black;
        }
    }

    public void ClickOverview()
    {
        if (InformationText[0].activeSelf == true) return;
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        TabAnim.Play("ClickOverviewTab");
        InformationText[0].SetActive(true);
        InformationText[1].SetActive(false);
    }

    public void ClickCharacter()
    {
        if (InformationText[1].activeSelf == true) return;
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        TabAnim.Play("ClickCharacterTab");
        InformationText[0].SetActive(false);
        InformationText[1].SetActive(true);
    }

    public void CloseInformation()
    {
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        Information.SetActive(false);
        Scripts.HandCardRotationScript.Important.GamePaused = false;
    }

    public void YearDetails(int YearNow, string BCADCheck)
    {
        BcOrAd = BCADCheck;
        CurrentYear = YearNow;
        RoundNumberDisplayText.text = CurrentYear + " " + BcOrAd;
    }

    public void UpdateRoundNumber(int RoundNumber)
    {
        if (BcOrAd == "BC") RoundNumberDisplayText.text = (CurrentYear - RoundNumber) + " " + BcOrAd;
        if (BcOrAd == "AD") RoundNumberDisplayText.text = (CurrentYear + RoundNumber) + " " + BcOrAd;
        InformationOverviewText[6].text = "Year: " + RoundNumberDisplayText.text;
        InformationOverviewText[7].text = "Events This Year: 1/10";
    }

    public void PauseClicked()
    {
        if (Scripts.HandCardRotationScript.Important.GamePaused == true) return;
        DimScreen.SetActive(true);
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        PauseMenu.SetActive(true);
        anim.Play("Pause Menu");
        Scripts.HandCardRotationScript.Important.GamePaused = true;
    }

    public void ResumeClicked()
    {
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        anim.Play("RemovePauseMenu");
        Scripts.HandCardRotationScript.Important.GamePaused = false;
        StartCoroutine(RemovePauseMenu());
    }

    public void MainMenuClicked()
    {

        if (Scripts.ParticleEffectControllerScript.WeatherInPlay != 0)
        {
            Scripts.ParticleEffectControllerScript.Weather[Scripts.ParticleEffectControllerScript.WeatherInPlay - 1].Stop();
            Scripts.ParticleEffectControllerScript.WeatherInPlay = 0;
        }
        Scripts.MiscellaneousGameManagementScript.PlayMusic(Scripts.MiscellaneousGameManagementScript.MusicClips[0]);
        Scripts.MiscellaneousGameManagementScript.PlaySoundEffect(Scripts.MiscellaneousGameManagementScript.SoundEffectClips[0]);
        anim.Play("RemovePauseMenu");
        StartCoroutine(MainMenuLoad());
        StartCoroutine(RemovePauseMenu());
        //Scripts.MiscellaneousGameManagementScript.Save();
        //Scripts.MainMenuControllerScript.SetContinueButtonDisplay();
    }

    public IEnumerator RemovePauseMenu()
    {
        yield return new WaitForSeconds(0.3f);
        PauseMenu.SetActive(false);
    }

    public IEnumerator MainMenuLoad()
    {
        yield return new WaitForSeconds(0.3f);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[0].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[1].SetActive(true);
        Scripts.MainMenuControllerScript.MainMenuGameObjectsToDisable[2].SetActive(true);
        StartCoroutine(Scripts.CameraControlScript.FadeThroughBlack(0.25f));
        Scripts.CameraControlScript.MainMenuVideo.clip = Scripts.CameraControlScript.VideoClips[1];
        Scripts.CameraControlScript.MainMenuVideo.frame = 0;

        yield return new WaitForSeconds(0.8f);
        Scripts.HandCardRotationScript.Important.GamePaused = false;
        Vector3 Pos = Camera.main.transform.position;
        Pos.z = -23.8f;
        Camera.main.transform.position = Pos;
        yield return new WaitForSeconds(0.2f);
        Scripts.CameraControlScript.MainMenuVideo.Play();
    }

    public GameObject Announcer;
    public float LerpFadeProgress;
    public float LerpMotionProgress;
    public float StartTimer;
    public int AnimateSpriteCheck = 0;
    public bool DoneShaking;
    private float Fade = 0;
    private float LerpFadeOutProgress = 0;

    void Update () {
        if (AnimateSpriteCheck == 1)
        {
            if (LerpFadeProgress < 1)
            {
                LerpFadeProgress += Time.deltaTime / 0.2f;
                Fade = Mathf.Lerp(0, 1, LerpFadeProgress);
                Vector4 colorTemp = new Vector4(); 
                colorTemp.Set(1, 1, 1, Fade);
                Announcer.GetComponent<Image>().color = colorTemp;
                Vector4 colorTemp3 = new Vector4(0, 0, 0, LerpFadeProgress);
                Announcer.GetComponentInChildren<Text>().color = colorTemp3;
            }

            if (LerpFadeProgress >= 1)
            {
                Vector3 Pos = Announcer.transform.position;

                if (LerpMotionProgress <= 0.8f)
                {
                    LerpMotionProgress += Time.deltaTime / 0.06f;
                    Pos.x = Mathf.Lerp(-0.72f, -0.52f, LerpMotionProgress);
                    Announcer.transform.position = Pos;
                }

                if (LerpMotionProgress > 0.8f && LerpMotionProgress <= 1.7f)
                {
                    LerpMotionProgress += Time.deltaTime / 0.12f;
                    Pos.x = Mathf.Lerp(-0.52f, -0.92f, LerpMotionProgress - 0.8f);
                    Announcer.transform.position = Pos;
                }

                if (LerpMotionProgress > 1.7f && LerpMotionProgress <= 2.7f)
                {
                    LerpMotionProgress += Time.deltaTime / 0.06f;
                    Pos.x = Mathf.Lerp(-0.92f, -0.72f, LerpMotionProgress - 1.7f);
                    Announcer.transform.position = Pos;
                }
                if (LerpMotionProgress > 2.7f)
                {
                    StartTimer += Time.deltaTime;
                    if (StartTimer >= 2.0f)
                    {
                        LerpMotionProgress = 0;
                        LerpFadeOutProgress = 0;
                        AnimateSpriteCheck = 0;
                        DoneShaking = true;
                    }
                }
            }
        }

        if (DoneShaking == true)
        {
            LerpFadeOutProgress += Time.deltaTime / 0.5f;
            Fade = Mathf.Lerp(1, 0, LerpFadeOutProgress);
            Vector4 colorTemp2 = new Vector4();
            colorTemp2.Set(1, 1, 1, Fade);
            Announcer.GetComponent<Image>().color = colorTemp2;
            Vector4 colorTemp4 = new Vector4(0, 0, 0, Fade);
            Announcer.GetComponentInChildren<Text>().color = colorTemp4;

            if (LerpFadeOutProgress >= 1)
            {
                DoneShaking = false;
                Announcer.SetActive(false);
            }
        }
    }

    public void Announcement(string TextDisplay)
    {
        Announcer.SetActive(true);
        Announcer.transform.GetChild(0).gameObject.GetComponent<Text>().text = TextDisplay;
        LerpFadeProgress = 0;
        LerpMotionProgress = 0;
        StartTimer = 0.0f;
        if (AnimateSpriteCheck == 1 || DoneShaking == true)
        {
            LerpFadeProgress = 1;
            DoneShaking = false;
            Vector4 colorTemp = new Vector4();
            colorTemp.Set(1, 1, 1, 1);
            Announcer.GetComponent<Image>().color = colorTemp;
            colorTemp = new Vector4(0, 0, 0, 1);
            Announcer.GetComponentInChildren<Text>().color = colorTemp;
        }
        AnimateSpriteCheck = 1;
    }

    public void DisplayCardNumberExplanation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "This is the number of resource cards you have." + '\n' + "You cannot hold more than 12 cards.";
    }

    public void  UpdateResourceCardNumber(int NumberOfResourceCards)
    {
        ResourceCardCounterText.text = NumberOfResourceCards.ToString();
    }

    public void DisplayLifeCardNumberExplanation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "This is the number of life cards you have." + '\n' + "You must have at least 1 life card.";
    }

    public void UpdateLifeResourceCardNumber(int NumberOfResourceCards)
    {
        LifeResourceCardCounterText.text = NumberOfResourceCards.ToString();
    }

    public void DisplayDestructionCardNumberExplanation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "This is the number of chaos cards you have." + '\n' + "You cannot hold 3 or more chaos cards.";
    }

    public void UpdateChaosResourceCardNumber(int NumberOfResourceCards)
    {
        ChaosResourceCardCounterText.text = NumberOfResourceCards.ToString();
    }

    public void DisplayHumanTiesExplanation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "This is our relationship with the humans." + '\n' + "Events may occur based on our relationship.";
    }

    public void DisplayNatureMoraleExplanation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "Morale (Red): " + Scripts.NatureControllerScript.Health * 5 + "%" + "\n" + "Food reserves (Blue): " + Scripts.NatureControllerScript.CurrentFood + "%";
    }

    public void DisplayHumanMoraleExplanation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "Morale (Red): " + Scripts.HumanControllerScript.Morale + "%" + "\n" + "Food reserves (Blue): " + Scripts.HumanControllerScript.Food + "%";   
    }

    public void DisplayCombatExplation()
    {
        ResourceCardExplanation.SetActive(true);
        ResourceCardExplanationText.text = "Offensive strength left this year: " + Scripts.NatureControllerScript.OffenseLeft + "\n" + "Defensive strength left this year: " + Scripts.NatureControllerScript.DefenceLeft;
    }
}
