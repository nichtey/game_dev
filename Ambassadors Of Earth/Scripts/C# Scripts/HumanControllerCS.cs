using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanControllerCS : MonoBehaviour {

    public GameObject MoraleBar;
    public int Morale;
    public float MoraleLerp = 1;
    public GameObject FoodBar;
    public int Food;
    public float FoodLerp = 1;

    public EventCardControlCS EventCardControlScript;
    public UIControllerCS UIControllerScript;
    private Vector3 Pos;

	void Start () {
		
	}
	
	void Update () {

        if (MoraleLerp < 1)
        {
            MoraleLerp += Time.deltaTime / 0.5f;
            Pos = MoraleBar.transform.localScale;
            Pos.x = Mathf.Lerp(MoraleBar.transform.localScale.x, 0.75f * Morale/100 + 0.25f, MoraleLerp);
            MoraleBar.transform.localScale = Pos;

            if (MoraleLerp >= 1)
            {
                if (Morale >= 40) EventCardControlScript.Important.LowMoraleEvent[0] = 0;
                if (Morale< 40) EventCardControlScript.Important.LowMoraleEvent[0] = 1;
                if (Morale < 10) EventCardControlScript.Important.LowMoraleEvent[0] = 2;
                if (Morale == 0) EventCardControlScript.Important.LowMoraleEvent[0] = 4;

                //UIControllerScript.InformationCharacterText[2].text = MainMenuControllerScript.DetermineMorale(Health);
            }
        }

        if (FoodLerp < 1)
        {
            FoodLerp += Time.deltaTime / 0.5f;
            Pos = FoodBar.transform.localScale;
            Pos.x = Mathf.Lerp(FoodBar.transform.localScale.x, 0.75f * Food/100 + 0.25f, FoodLerp);
            FoodBar.transform.localScale = Pos;

            if (FoodLerp >= 1)
            {
                if (Food >= 40) EventCardControlScript.Important.LowFoodEvent[0] = 0;
                if (Food < 40) EventCardControlScript.Important.LowFoodEvent[0] = 1;
                if (Food < 10) EventCardControlScript.Important.LowFoodEvent[0] = 2;
                if (Food == 0) EventCardControlScript.Important.LowFoodEvent[0] = 4;

                //UIControllerScript.InformationCharacterText[3].text = MainMenuControllerScript.DetermineFood((int)CurrentFood);
            }
        }
	}

    public void UpdateMorale(int MoraleChange)
    {
        Morale += MoraleChange;
        if (Morale > 100) Morale = 100;
        else if (Morale < 0) Morale = 0;
        MoraleLerp = 0;
    }

    public void UpdateFood(int FoodChange)
    {
        Food += FoodChange;
        if (Food > 100) Food = 100;
        else if (Food < 0) Food = 0;
        FoodLerp = 0;
    }
}
