using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCampaignDragCS : MonoBehaviour {

    public RectTransform DragPosition;
    public float TargetPosition;
    public float LerpTarget = 1;
    public float CurrentPosition;
    public int Quotient;
    public UnityEngine.UI.Text CampaignTitle;
    public string[] CampaignTitleNames = new string[5];
    public bool FadeCampaignTitle;
    public MainMenuControllerCS MainMenuControllerScript;

    private Vector3 Pos;
    private Color color;

	void Start () {
		
	}
	
	void Update () {
        if (LerpTarget < 1)
        {
            LerpTarget += Time.deltaTime / 0.2f;
            Pos = DragPosition.localPosition;
            Pos.x = Mathf.Lerp(CurrentPosition, TargetPosition, LerpTarget);
            DragPosition.localPosition = Pos;

            if (FadeCampaignTitle == true)
            {
                color = CampaignTitle.color;
                color.a = Mathf.Lerp(0, 1, LerpTarget);
                CampaignTitle.color = color;
            }

            if (LerpTarget >= 1)
            {
                FadeCampaignTitle = false;
                color = CampaignTitle.color;
                color.a = 1;
                CampaignTitle.color = color;
            }
        }
    }

    public void ReleasedDrag()
    {

        CurrentPosition = DragPosition.localPosition.x;

        if (CurrentPosition > -37.4271)          // Prevent Scrolling past the first button.
        {       
            CampaignTitle.text = CampaignTitleNames[0];
            Quotient = 0;
            return;
        }

        Quotient = Mathf.FloorToInt((DragPosition.localPosition.x + 37.4271f - 37.5f) / -75);     //Debug.Log(Quotient);

        var MaxQuotient = DragPosition.childCount - 1;
        if (Quotient > MaxQuotient) Quotient = MaxQuotient;

        if (CampaignTitle.text != CampaignTitleNames[Quotient])
        {
            FadeCampaignTitle = true;
            CampaignTitle.text = CampaignTitleNames[Quotient];
        }

        TargetPosition = -37.4271f - Quotient * 75;
        LerpTarget = 0;
    }

    public void JumpToSelected(int QuotientValue)
    {
        Quotient = QuotientValue;
        FadeCampaignTitle = true;
        CampaignTitle.text = CampaignTitleNames[Quotient];
        TargetPosition = -37.4271f - Quotient * 75;
        LerpTarget = 0;
    }

    public void ClickRightArrow()
    {

        Quotient++;
        CurrentPosition = DragPosition.localPosition.x;
        int MaxQuotient = DragPosition.childCount - 1;

        if (Quotient > MaxQuotient)
        {
            Quotient = 0;
            FadeCampaignTitle = true;
            CampaignTitle.text = CampaignTitleNames[Quotient];
            TargetPosition = -37.4271f - Quotient * 75;
            LerpTarget = 0;
            return;
        }

        FadeCampaignTitle = true;
        CampaignTitle.text = CampaignTitleNames[Quotient];
        TargetPosition = -37.4271f - Quotient * 75;
        LerpTarget = 0;
    }

    public void ClickLeftArrow()
    {

        Quotient--;
        CurrentPosition = DragPosition.localPosition.x;

        if (Quotient < 0)
        {
            var MaxQuotient = DragPosition.childCount - 1;
            Quotient = MaxQuotient;
            FadeCampaignTitle = true;
            CampaignTitle.text = CampaignTitleNames[Quotient];
            TargetPosition = -37.4271f - Quotient * 75;
            LerpTarget = 0;
            return;
        }

        FadeCampaignTitle = true;
        CampaignTitle.text = CampaignTitleNames[Quotient];
        TargetPosition = -37.4271f - Quotient * 75;
        LerpTarget = 0;
    }

    public void BeginDrag()
    {
        LerpTarget = 1;
    }

    public void ClickSelect()
    {
        if (Quotient == 0) { MainMenuControllerScript.ClickTutorial(); }
        else if (Quotient == 1) { MainMenuControllerScript.ClickLevel2(); }
        else if (Quotient ==2) { MainMenuControllerScript.ClickLevel3(); }
    }

}
