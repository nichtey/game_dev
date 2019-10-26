#pragma strict

var CharacterNumber: int;
var InitialPosition: Vector3;
var ActivePosition: Vector3;
var MoveLerp: float = 1;

function Start () {
    
    if (CharacterNumber == 1) {
        InitialPosition = Vector3(30.38, 0, -7.1);
        NatureOptionsInitialPosition = Vector3(0,0,0);
    }

    if (CharacterNumber == 2){
        InitialPosition = Vector3(-31.62, 0, -7.1);
        EnemyOptionsInitialPosition = Vector3(0,0,0);
    }
}

var NatureOptions: GameObject;
var NatureOptionsInitialPosition: Vector3;
var NatureOptionsLerp: float = 1;
var NatureOptionsActivePosition: Vector3;

var EnemyOptions: GameObject;
var EnemyOptionsInitialPosition: Vector3;
var EnemyOptionsLerp: float = 1;
var EnemyOptionsActivePosition: Vector3;

function Update () {
    
    if (MoveLerp <1){
        MoveLerp += Time.deltaTime/0.5;
        this.gameObject.transform.position = Vector3.Lerp(transform.position, ActivePosition, MoveLerp);
    }

    if (NatureOptionsLerp <1 && CharacterNumber == 1){
        NatureOptionsLerp += Time.deltaTime/0.5;
        NatureOptions.transform.localPosition = Vector3.Lerp(NatureOptions.transform.localPosition, NatureOptionsActivePosition, NatureOptionsLerp);
    }

    if (EnemyOptionsLerp <1 && CharacterNumber == 2){
        EnemyOptionsLerp += Time.deltaTime/0.5;
        EnemyOptions.transform.localPosition = Vector3.Lerp(EnemyOptions.transform.localPosition, EnemyOptionsActivePosition, EnemyOptionsLerp);
    }
}

function MoveEnemyOptionsToActivePosition(NatureChoice: int, EnemyChoice: int){
    EnemyOptionsActivePosition = Vector3(2.84, 0,0);
    EnemyOptionsLerp = 0;
    yield WaitForSeconds(0.5f);
    EnemyOptionsButtonSprite[EnemyChoice].color = Color.green;
    
    if (NatureChoice == 1){
        NatureAnimations[1].SetActive(true);
        yield WaitForSeconds(0.5f);
        if (EnemyChoice == 0) EnemyAnimations[0].SetActive(true);
        if (EnemyChoice == 1) EnemyAnimations[1].SetActive(true);
    }

    if (NatureChoice == 0){

        if (EnemyChoice ==0){
            NatureAnimations[0].SetActive(true);
            yield WaitForSeconds(0.5f);
            EnemyAnimations[0].SetActive(true);
        }
        if (EnemyChoice == 1){
            EnemyAnimations[1].SetActive(true);
            yield WaitForSeconds(0.5f);
            NatureAnimations[0].SetActive(true);
        }
    }
}

function MoveToActivePosition(){

    if (CharacterNumber == 1) {
        ActivePosition = Vector3(2.8, 0.1, -3.6);
        NatureOptionsActivePosition = Vector3(-2.84, 0, 0);    
    }

    if (CharacterNumber == 2) ActivePosition = Vector3(-4.04, 0.1, -3.6);
    MoveLerp = 0;
    yield WaitForSeconds(0.5f);
    NatureOptionsLerp = 0;
}

function MoveToInactivePosition(){
    
    if (CharacterNumber == 1) {
        ActivePosition =  InitialPosition;
        NatureOptionsActivePosition = NatureOptionsInitialPosition;
        NatureOptionsLerp = 0;
    }

    if (CharacterNumber == 2){ 
        ActivePosition =  InitialPosition;
        EnemyOptionsActivePosition = EnemyOptionsInitialPosition;
        EnemyOptionsLerp = 0;
    }
    yield WaitForSeconds(0.5f);
    MoveLerp = 0;
}

var NatureOptionsButtonSprite = new UnityEngine.UI.Image [4];
var EnemyOptionsButtonSprite = new UnityEngine.UI.Image [4];
var NatureAnimations = new GameObject [2]; // [0] is attack, [1] is defend
var EnemyAnimations = new GameObject [2];  // [0] is attack, [1] is defend.