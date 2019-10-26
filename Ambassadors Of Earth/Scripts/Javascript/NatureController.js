#pragma strict

var BattleOptionChooseCheck: boolean;
var HealthBar: GameObject;
var HealthLerp: float = 1;
var MaxHealth: float = 20;

var ScienceBar: GameObject;
var CurrentScienceProgress: float;
var TargetScienceProgress: float;
var PercentageToGrow: float;
var ScienceLerp: float;
var CurrentFood: float;

var UIControllerScript: UIController;
var HumanControllerScript: HumanController;
var MainMenuControllerScript: MainMenuController;
var CharacterBattleControllerScript = new CharacterBattleController[2]; // CharacterBattleControllerScript[0] is for nature is CharacterBattleControllerScript[1] is for humans

var DefendChangeAnimation:Animation;
var AttackChangeAnimation:Animation;
var NatureTechResearched = new Boolean[3];

function AtkChangeAnimation(ChangeAmount:int){
    if (ChangeAmount<0) AttackChangeAnimation.Play("DecreaseDefence");
    if (ChangeAmount>0) AttackChangeAnimation.Play("IncreaseDefence");
}

function DefenceChangeAnimation(ChangeAmount: int){
    if (ChangeAmount<0) DefendChangeAnimation.Play("DecreaseDefence");
    if (ChangeAmount>0) DefendChangeAnimation.Play("IncreaseDefence");
}

function Start () {
    ScienceLerp = 1;
    ScienceBar.transform.localScale.x = 0.25; //Default 0 Science x scale is 0.25.
}

function Update () {
    if(ScienceLerp < 1){
        ScienceLerp += Time.deltaTime/0.2;
        ScienceBar.transform.localScale.x = Mathf.Lerp(CurrentScienceProgress, TargetScienceProgress, ScienceLerp);

        if(ScienceLerp >=1){
            CurrentScienceProgress = ScienceBar.transform.localScale.x;
            CurrentFood = Mathf.RoundToInt((CurrentScienceProgress - 0.25)*400/3);
            if (CurrentFood >= 40) EventCardControlScript.Important.LowFoodEvent[0] = 0;
            if (CurrentFood < 40) EventCardControlScript.Important.LowFoodEvent[0] = 1;
            if (CurrentFood < 10) EventCardControlScript.Important.LowFoodEvent[0] = 2;
            if (CurrentFood == 0) EventCardControlScript.Important.LowFoodEvent[0] = 4;

            UIControllerScript.InformationCharacterText[3].text = MainMenuControllerScript.DetermineFood(CurrentFood);
        }
    }

    if(HealthLerp <1){
        HealthLerp += Time.deltaTime/0.5;
        HealthBar.transform.localScale.x = Mathf.Lerp(HealthBar.transform.localScale.x, 0.75*(Health/MaxHealth)+0.25, HealthLerp);

        if (HealthLerp >=1){
            if (Health >= 8) EventCardControlScript.Important.LowMoraleEvent[0] = 0;
            if (Health < 8) EventCardControlScript.Important.LowMoraleEvent[0] = 1;
            if (Health < 2) EventCardControlScript.Important.LowMoraleEvent[0] = 2;
            if (Health == 0) EventCardControlScript.Important.LowMoraleEvent[0] = 4;

            UIControllerScript.InformationCharacterText[2].text = MainMenuControllerScript.DetermineMorale(Health);
        }
    }

}

function IncreaseNatureFood(PercentageToGrow: float){
    TargetScienceProgress = PercentageToGrow*0.75 + CurrentScienceProgress;
    if (TargetScienceProgress < 0.25) TargetScienceProgress = 0.25;
    if (TargetScienceProgress > 1) TargetScienceProgress = 1;
    ScienceLerp = 0;
}

function UpdateMorale(MoraleChange: int){
    Health += MoraleChange;
    if (Health > MaxHealth) Health = MaxHealth;
    if (Health < 0) Health = 0;
    HealthLerp = 0;
}

var DamageDealt: int;
var Health: int = 20;
var Attack: int = 3;
var Defence: int = 4;
var Choice: int;

function ClickAttack(){

    if (BattleOptionChooseCheck == true) return;
    BattleOptionChooseCheck = true;
    Choice = 0;
    CharacterBattleControllerScript[0].NatureOptionsButtonSprite[0].color = Color.green;
    CharacterBattleControllerScript[1].MoveEnemyOptionsToActivePosition(Choice, HumanControllerScript.Choice);

    if (HumanControllerScript.Choice == 0){
        DamageDealt = Attack;
        HumanControllerScript.DamageReceived(DamageDealt);
        DamageReceived(HumanControllerScript.Attack);
    }
    
    if (HumanControllerScript.Choice == 1){ 
        DamageDealt = Attack - HumanControllerScript.Defence;
        if (DamageDealt > 0) HumanControllerScript.DamageReceived(DamageDealt);
    }
    Finished();
}

function ClickDefend(){
    
    if (BattleOptionChooseCheck == true) return;
    BattleOptionChooseCheck = true;
    Choice = 1;
    CharacterBattleControllerScript[0].NatureOptionsButtonSprite[1].color = Color.green;
    CharacterBattleControllerScript[1].MoveEnemyOptionsToActivePosition(Choice, HumanControllerScript.Choice);

    if (HumanControllerScript.Choice == 0){
        if (HumanControllerScript.Attack - Defence > 0) DamageReceived(HumanControllerScript.Attack - Defence);
    }
    Finished();
}

function DamageReceived(AmountOfDamage: int){
    yield WaitForSeconds(1.0);
    Health -= AmountOfDamage;
    if (Health > MaxHealth) Health = MaxHealth;
    if (Health < 0) Health = 0;
    HealthLerp = 0;
}

var EventCardControlScript: EventCardControl;

function Finished(){
    yield WaitForSeconds(2);
    CharacterBattleControllerScript[1].NatureAnimations[0].SetActive(false);
    CharacterBattleControllerScript[1].NatureAnimations[1].SetActive(false);
    CharacterBattleControllerScript[1].EnemyAnimations[0].SetActive(false);
    CharacterBattleControllerScript[1].EnemyAnimations[1].SetActive(false);
    CharacterBattleControllerScript[0].NatureOptionsButtonSprite[0].color = Color.white;
    CharacterBattleControllerScript[0].NatureOptionsButtonSprite[1].color = Color.white;
    CharacterBattleControllerScript[0].EnemyOptionsButtonSprite[0].color = Color.white;
    CharacterBattleControllerScript[0].EnemyOptionsButtonSprite[1].color = Color.white;
    BattleOptionChooseCheck = false;
    EventCardControlScript.BattleCardRemoved();
}