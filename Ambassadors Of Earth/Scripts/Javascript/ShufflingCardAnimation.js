#pragma strict

var ShufflingCardAnimation: Animation;

function Start () {

}

function PlayShuffle(){
    ShufflingCardAnimation.Play("ShufflingCard");
    yield WaitForSeconds(0.5);
    ShufflingCardAnimation.Play("ShufflingCard2");
    yield WaitForSeconds(0.5);
    ShufflingCardAnimation.Play("ShufflingCard");
    yield WaitForSeconds(0.5);
    ShufflingCardAnimation.Play("ShufflingCard2");
    yield WaitForSeconds(0.5);
    ShufflingCardAnimation.Play("ShufflingCard");
    yield WaitForSeconds(0.5);
    ShufflingCardAnimation.Play("ShufflingCard2");
}

function Update () {
	
}
