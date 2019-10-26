#pragma strict

var BarToMove: GameObject;
var XScaleOfBar: float;
var LengthToGrow: float;
var LerpGrowth: float;
var TechnologyLevel:int;
var HumanTechCheck: boolean;
var EventCardControlScript: EventCardControl;
var IncreaseTechAnimation: AnimationClip;

function Start () {
    XScaleOfBar = BarToMove.transform.localScale.y;  // Max length = 0.65
    LengthToGrow = 0;
    LerpGrowth = 1;
    HumanTechLevelRequirements[0] = 10;
    HumanTechLevelRequirements[1] = 20;
    HumanTechLevelRequirements[2] = 40;
}

function Update () {
    if (LerpGrowth <1){
        LerpGrowth += Time.deltaTime/0.5;
        BarToMove.transform.localScale.y = Mathf.Lerp(XScaleOfBar, LengthToGrow, LerpGrowth);
    }

    if (BarToMove.transform.localScale.y >= 0.65){
        HumanTechnologyLevel += 1;
        LengthToGrow = 0;
        XScaleOfBar = BarToMove.transform.localScale.y;
        LerpGrowth = 0;
        //EventCardControlScript.ShuffleCount.HumanTechLevel =  HumanTechnologyLevel; 
    }
}

/*function Grow (TechnologyLevel: int){
    if (TechnologyLevel == 1) LengthToGrow = 0.217;
    if (TechnologyLevel == 2) LengthToGrow = 0.433;
    if (TechnologyLevel == 3) LengthToGrow = 0.65;
    XScaleOfBar = BarToMove.transform.localScale.y;
    LerpGrowth = 0;
}*/

var HumanTechLevelRequirements = new int[3];
var HumanTechnologyLevel: int;

function IncreaseHumanTech (NumberOfHumanCards: float){
    if (HumanTechnologyLevel == 0){
        LengthToGrow += (NumberOfHumanCards/HumanTechLevelRequirements[0]) * 0.65;
        XScaleOfBar = BarToMove.transform.localScale.y;
        LerpGrowth = 0;
    }

    if (HumanTechnologyLevel == 1){
        LengthToGrow += (NumberOfHumanCards/HumanTechLevelRequirements[1]) * 0.65;
        XScaleOfBar = BarToMove.transform.localScale.y;
        LerpGrowth = 0;
    }

    if (HumanTechnologyLevel == 2){
        LengthToGrow += (NumberOfHumanCards/HumanTechLevelRequirements[2]) * 0.65;
        XScaleOfBar = BarToMove.transform.localScale.y;
        LerpGrowth = 0;
    }
}