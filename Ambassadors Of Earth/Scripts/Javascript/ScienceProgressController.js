#pragma strict
var IncreaseTechAnimation: AnimationClip;
var ClipTime: float;
var anim: Animator;
var AnimationStoppingPoint:float;
var TimeToAnimateFor: float;

function Start () {
    ClipTime = IncreaseTechAnimation.length;
    anim = this.gameObject.GetComponent.<Animator>();

    anim.SetBool("StartPlaying",true);
    IncreaseHumanTech (5);                  // Replace with sNumber of human Cards during the "Time Passes Event"
}

function Update () {
	
}

function IncreaseHumanTech (NumberOfHumanCards: int){

    TimeToAnimateFor = ((NumberOfHumanCards + NumberOfHumanCards%2)/2 * 0.2)*ClipTime;  // For NumberOfHumanCards/2 rounded up, Gauge moves up 20%

    if (TimeToAnimateFor + AnimationStoppingPoint >= ClipTime){         // If Gauge Passes 100% reset Gauge and continue to increase by the excess amount.
        anim.speed = 1;
        anim.Play("ScienceGaugeFillingUp 0",-1, AnimationStoppingPoint);
        yield WaitForSeconds(ClipTime - AnimationStoppingPoint);
        anim.speed = 0;
        AnimationStoppingPoint += TimeToAnimateFor;
    }

    if (TimeToAnimateFor + AnimationStoppingPoint < ClipTime){
        anim.speed = 1;
        anim.Play("ScienceGaugeFillingUp 0",-1, AnimationStoppingPoint);
        yield WaitForSeconds(TimeToAnimateFor);
        anim.speed = 0;
        AnimationStoppingPoint += TimeToAnimateFor;
    }

    if (AnimationStoppingPoint >= ClipTime){
        AnimationStoppingPoint -= ClipTime;
        anim.speed = 1;
        anim.Play("ScienceGaugeFillingUp 0",-1, 0.0f);
        yield WaitForSeconds(AnimationStoppingPoint);
        anim.speed = 0;
    }
}