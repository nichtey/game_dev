#pragma strict

var DragPosition: RectTransform;
var TargetPosition: float;
var LerpTarget: float = 1;
var CurrentPosition : float;
var Quotient: int;
var CampaignTitle: UnityEngine.UI.Text;
var CampaignTitleNames = new String[5];
var FadeCampaignTitle: boolean;
var MainMenuControllerScript: MainMenuController;

function Start () {
	
}

function Update () {
	
    if (LerpTarget < 1){
        LerpTarget += Time.deltaTime/ 0.2;
        DragPosition.localPosition.x = Mathf.Lerp(CurrentPosition, TargetPosition, LerpTarget);
        if (FadeCampaignTitle == true) CampaignTitle.color.a = Mathf.Lerp (0, 1, LerpTarget);

        if (LerpTarget >= 1){ 
            FadeCampaignTitle = false;
            CampaignTitle.color.a = 1;
        }
    }
}

function ReleasedDrag(){

    CurrentPosition = DragPosition.localPosition.x;
    
    if (CurrentPosition > -37.4271){        // Prevent Scrolling past the first button.
        CampaignTitle.text = CampaignTitleNames[0];
        Quotient = 0;
        return;    
    }
    
    Quotient = Mathf.Floor((DragPosition.localPosition.x + 37.4271 -37.5)/-75);     //Debug.Log(Quotient);

    var MaxQuotient = DragPosition.childCount -1;
    if (Quotient > MaxQuotient) Quotient = MaxQuotient;

    if (CampaignTitle.text != CampaignTitleNames[Quotient]){
        FadeCampaignTitle = true;
        CampaignTitle.text = CampaignTitleNames[Quotient];
    }

    TargetPosition = -37.4271 - Quotient*75;
    LerpTarget = 0;
}

function ClickRightArrow(){

    Quotient ++;
    CurrentPosition = DragPosition.localPosition.x;
    var MaxQuotient = DragPosition.childCount -1;

    if (Quotient > MaxQuotient){
        Quotient = 0;
        FadeCampaignTitle = true;
        CampaignTitle.text = CampaignTitleNames[Quotient];
        TargetPosition = -37.4271 - Quotient*75;
        LerpTarget = 0;
        return;
    }

    FadeCampaignTitle = true;
    CampaignTitle.text = CampaignTitleNames[Quotient];
    TargetPosition = -37.4271 - Quotient*75;
    LerpTarget = 0;
}

function ClickLeftArrow(){

    Quotient --;
    CurrentPosition = DragPosition.localPosition.x;

    if (Quotient < 0){
        var MaxQuotient = DragPosition.childCount -1;
        Quotient = MaxQuotient;
        FadeCampaignTitle = true;
        CampaignTitle.text = CampaignTitleNames[Quotient];
        TargetPosition = -37.4271 - Quotient*75;
        LerpTarget = 0;
        return;
    }

    FadeCampaignTitle = true;
    CampaignTitle.text = CampaignTitleNames[Quotient];
    TargetPosition = -37.4271 - Quotient*75;
    LerpTarget = 0;
}

function BeginDrag(){
    LerpTarget = 1;
}

function ClickSelect(){
    if (Quotient == 0){MainMenuControllerScript.ClickTutorial(); }
    else if (Quotient ==1){MainMenuControllerScript.ClickLevel2(); }
}