#pragma strict

var OptionCardNumber: int;
var OptionCard: GameObject;
var Speed: float =1.0;
var MovePoint: Vector3;

function Start () {
    MovePoint = OptionCard.transform.localPosition;
}

var LerpProgress: float = 0;

function Update () {
    if (LerpProgress <= 1){
        LerpProgress += Time.deltaTime/1.5;
        OptionCard.transform.localPosition = Vector3.Lerp(OptionCard.transform.localPosition,MovePoint,LerpProgress);
    }
}

function MoveOnClick(){
    LerpProgress = 0;
    if (OptionCardNumber == 1){MovePoint = Vector3(3.09,0.1,-3.6);}
    if (OptionCardNumber == 2){MovePoint = Vector3(-0.61,0.1,-3.6);}
    if (OptionCardNumber == 3){MovePoint = Vector3(-4.31,0.1,-3.6);}
}

function MoveBackOnClick(){
    LerpProgress = 0;
    if (OptionCardNumber == 1){MovePoint = Vector3(16,5.41,-2.64);}
    if (OptionCardNumber == 2){MovePoint = Vector3(21,7.05,-2.64);}
    if (OptionCardNumber == 3){MovePoint = Vector3(26.31,9.35,-2.64);}
}
