using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureControllerCS : MonoBehaviour {

    public SpriteRenderer[] WarningLow = new SpriteRenderer[2];
    private float ColorChange;
    private int TargetColor = 0;
    private float LerpColourProgress = 0;

    public GameObject HealthBar;
    public float HealthLerp = 1;
    public float MaxHealth = 20;

    public GameObject ScienceBar;
    public float CurrentScienceProgress;
    public float TargetScienceProgress;
    public float PercentageToGrow;
    public float ScienceLerp = 1;
    public float CurrentFood;

    public UIControllerCS UIControllerScript;
    public MainMenuControllerCS MainMenuControllerScript;
    public EventCardControlCS EventCardControlScript;
    public OptionClickControlCS OptionClickControlScript;

    public int Health = 20;
    public int Offense = 0;
    public int Defence = 0;
    public int OffenseLeft;
    public int DefenceLeft;

    private Vector3 Pos;

    void Start () {

    }
	
	void Update ()
    {
        if (Health < 8 || CurrentFood < 40)
        {
            if (LerpColourProgress >= 1 && TargetColor == 1)
            {
                LerpColourProgress = 0;
                TargetColor = 0;
            }

            else if (LerpColourProgress >= 1 && TargetColor == 0)
            {
                LerpColourProgress = 0;
                TargetColor = 1;
            }

            LerpColourProgress += Time.deltaTime / 0.33f;
        }

        if (Health < 8)
        {
            Color Temp;
            ColorChange = Mathf.Lerp(WarningLow[0].color.a, TargetColor, LerpColourProgress);
            Temp = WarningLow[0].color;
            Temp.a = ColorChange;
            WarningLow[0].color = Temp;
        }

        if (CurrentFood < 40)
        {
            Color Temp;
            ColorChange = Mathf.Lerp(WarningLow[1].color.a, TargetColor, LerpColourProgress);
            Temp = WarningLow[1].color;
            Temp.a = ColorChange;
            WarningLow[1].color = Temp;
        }

        if (Health >= 8)
        {
            Color Temp;
            Temp = WarningLow[0].color;
            Temp.a = 0;
            WarningLow[0].color = Temp;
        }

        if (CurrentFood >= 40)
        {
            Color Temp;
            Temp = WarningLow[1].color;
            Temp.a = 0;
            WarningLow[1].color = Temp;
        }

        if (ScienceLerp < 1)
        {
            ScienceLerp += Time.deltaTime / 0.2f;
            Pos = ScienceBar.transform.localScale;
            Pos.x = Mathf.Lerp(CurrentScienceProgress, TargetScienceProgress, ScienceLerp);
            ScienceBar.transform.localScale = Pos;

            if (ScienceLerp >= 1)
            {
                CurrentScienceProgress = ScienceBar.transform.localScale.x;
                CurrentFood = Mathf.RoundToInt((CurrentScienceProgress - 0.25f) * 400 / 3);
                if (CurrentFood >= 40) EventCardControlScript.Important.LowFoodEvent[0] = 0;
                if (CurrentFood < 40) EventCardControlScript.Important.LowFoodEvent[0] = 1;
                if (CurrentFood < 10) EventCardControlScript.Important.LowFoodEvent[0] = 2;
                if (CurrentFood == 0) EventCardControlScript.Important.LowFoodEvent[0] = 4;
                
                UIControllerScript.InformationCharacterText[3].text = MainMenuControllerScript.DetermineFood((int) CurrentFood);
            }
        }

        if (HealthLerp < 1)
        {
            HealthLerp += Time.deltaTime / 0.5f;
            Pos = HealthBar.transform.localScale;
            Pos.x = Mathf.Lerp(HealthBar.transform.localScale.x, 0.75f * (Health / MaxHealth) + 0.25f, HealthLerp);
            HealthBar.transform.localScale = Pos;

            if (HealthLerp >= 1)
            {
                if (Health >= 8) EventCardControlScript.Important.LowMoraleEvent[0] = 0;
                if (Health < 8) EventCardControlScript.Important.LowMoraleEvent[0] = 1;
                if (Health < 2) EventCardControlScript.Important.LowMoraleEvent[0] = 2;
                if (Health == 0) EventCardControlScript.Important.LowMoraleEvent[0] = 4;

                UIControllerScript.InformationCharacterText[2].text = MainMenuControllerScript.DetermineMorale(Health);
            }
        }
    }

    public void ResetOffenseDefence()
    {
        OffenseLeft = Offense;
        DefenceLeft = Defence;

        OptionClickControlScript.Components.CombatStuff.Attack.text = Offense.ToString();
        OptionClickControlScript.Components.CombatStuff.Defence.text = Defence.ToString();
    }

    public void TempUpdateOffense(int ValueChanged)
    {
        OffenseLeft += ValueChanged;
        OptionClickControlScript.Components.CombatStuff.Attack.text = OffenseLeft.ToString();
    }

    public void TempUpdateDefence(int ValueChanged)
    {
        DefenceLeft += ValueChanged;
        OptionClickControlScript.Components.CombatStuff.Defence.text = DefenceLeft.ToString();
    }

    public void IncreaseNatureFood(float PercentageToGrow)
    {
        TargetScienceProgress = PercentageToGrow * 0.75f + CurrentScienceProgress;
        if (TargetScienceProgress < 0.25) TargetScienceProgress = 0.25f;
        if (TargetScienceProgress > 1) TargetScienceProgress = 1;
        ScienceLerp = 0;
    }

    public void UpdateMorale(int MoraleChange)
    {
        Health += MoraleChange;
        if (Health > MaxHealth) Health = (int)MaxHealth;
        if (Health < 0) Health = 0;
        HealthLerp = 0;
    }
}
