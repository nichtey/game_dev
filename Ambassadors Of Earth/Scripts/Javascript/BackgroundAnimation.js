#pragma strict

var flyingbird1 = new Sprite[15];
var flyingbirdImage: SpriteRenderer;
var PositionNumber: int;

var HumanKing = new Sprite[15];
var HumanKingImage: SpriteRenderer;

function Start () {
    InvokeRepeating("Fly", 2, 0.08);
}

function Update () {
	
}

function Fly(){
    PositionNumber++;
    flyingbirdImage.sprite = flyingbird1[PositionNumber%15];
    HumanKingImage.sprite = HumanKing[PositionNumber%15];
}

