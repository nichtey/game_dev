#pragma strict

var HealthBar: GameObject;
var HealthLerp: float = 1;
var MaxHealth: float = 20;

var ScienceBar: GameObject;
var CurrentScienceProgress: float;
var TargetScienceProgress: float;
var PercentageToGrow: float;
var ScienceLerp: float;

var NatureControllerScript: NatureController;

var DefendChangeAnimation:Animation;
var AttackChangeAnimation:Animation;

function AtkChangeAnimation(ChangeAmount:int){
    if (ChangeAmount<0) AttackChangeAnimation.Play("DecreaseDefence");
    if (ChangeAmount>0) AttackChangeAnimation.Play("IncreaseDefence");
}

function DefenceChangeAnimation(ChangeAmount: int){
    if (ChangeAmount<0) DefendChangeAnimation.Play("DecreaseDefence");
    if (ChangeAmount>0) DefendChangeAnimation.Play("IncreaseDefence");
}


function Start () {

    ChooseBattleOption();
    CurrentScienceProgress = 0.25;
    ScienceLerp = 1;
    ScienceBar.transform.localScale.x = 0.25; //Default 0 Science x scale is 0.25.
    yield WaitForSeconds(0.5);
    IncreaseHumanScience(0.5);
}

function Update () {
    if(ScienceLerp < 1){
        ScienceLerp += Time.deltaTime/0.7;
        ScienceBar.transform.localScale.x = Mathf.Lerp(0.25, TargetScienceProgress, ScienceLerp);

        if(ScienceLerp >=1){
            CurrentScienceProgress = ScienceBar.transform.localScale.x;
        }
    }

    if(HealthLerp <1){
        HealthLerp += Time.deltaTime/0.5;
        HealthBar.transform.localScale.x = Mathf.Lerp(HealthBar.transform.localScale.x, 0.75*(Health/MaxHealth)+0.25, HealthLerp);
    }

}

function IncreaseHumanScience(PercentageToGrow: float){
    TargetScienceProgress = PercentageToGrow*0.75 + CurrentScienceProgress;
    ScienceLerp = 0;
}

var DamageDealt: int;
var Health: int = 20;
var Attack: int = 1;
var Defence: int = 1;

var PotentialDamageDealt = new int[2];
var PotentialDamageBlocked = new int [2];
var AttackingPotential: float;
var DefendingPotential: float;

var Choice: int; // 0 = Attack, 1 = Defend

function ChooseBattleOption(){

    PotentialDamageBlocked[0] = 0;                                  // If Both Players Defend
    PotentialDamageDealt[0] = Attack;                               // If Both Players Attack

    if (NatureControllerScript.Attack >= Defence)   PotentialDamageBlocked[1] = Defence;                        //If Nature Attacks and Human defends
    if (NatureControllerScript.Attack < Defence) PotentialDamageBlocked[1] = NatureControllerScript.Attack;     //If Nature Attacks and Human defends

    var NetDamageDealt: int;    
    NetDamageDealt = Attack - NatureControllerScript.Defence;
    if (NetDamageDealt > 0) PotentialDamageDealt[1] = NetDamageDealt;   //If Human Attacks and Nature defends
    if (NetDamageDealt <= 0) PotentialDamageDealt[1] = 0;               //If Human Attacks and Nature defends (Successful Defence)

    AttackingPotential = (PotentialDamageDealt[0]+PotentialDamageDealt[1])/2;
    DefendingPotential = (PotentialDamageBlocked[0]+PotentialDamageBlocked[1])/2;
    
    if (DefendingPotential > AttackingPotential){
        Choice = 1; //Defend
    }

    if (AttackingPotential > DefendingPotential){
        Choice = 0; //Attack
    }

    if (AttackingPotential == DefendingPotential){
        if (Health >= NatureControllerScript.Health) Choice = 0;    //Choice = Attack
        if (Health < NatureControllerScript.Health) Choice = 1;     //Choice = Defend
    }
}

function DamageReceived(AmountOfDamage: int){
    if (Choice == 0) yield WaitForSeconds(0.5);
    if (Choice == 1) yield WaitForSeconds(1.0);
    Health -= AmountOfDamage;
    HealthLerp = 0;
}