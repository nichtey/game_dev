#pragma strict

function Start () {
	
}

function Update () {
	
}

var BigExplosion: ParticleSystem;
var Explosions = new ParticleSystem[3];
var ExplosionPositions = new Transform[3];
var UnlockCardExplosion: ParticleSystem;
var Weather = new ParticleSystem[3];
var WeatherInPlay: int = 0;

function DestroyExtraCards(){
    BigExplosion.Play();
}

function DestroyCards(){
    ExplosionPositions[0].localPosition.x = -0.11;
    Explosions[0].Play();
}

function Destroy2Cards(){
    ExplosionPositions[0].localPosition.x = 1.13;
    ExplosionPositions[1].localPosition.x = -1.13;
    Explosions[0].Play();
    Explosions[1].Play();
}

function Destroy3Cards(){
    ExplosionPositions[0].localPosition.x = 2.46;
    ExplosionPositions[1].localPosition.x = -0.08;
    ExplosionPositions[2].localPosition.x = -2.62;
    Explosions[0].Play();
    Explosions[1].Play();
    Explosions[2].Play();
}

function UnlockedCard(){
    UnlockCardExplosion.Play();
}

function SetWeather(WeatherNumber: int){

    Weather[WeatherNumber - 1].Play();
    WeatherInPlay = WeatherNumber;
}