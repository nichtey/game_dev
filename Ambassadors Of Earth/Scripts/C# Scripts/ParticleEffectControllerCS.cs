using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectControllerCS : MonoBehaviour {

    public ParticleSystem BigExplosion;
    public ParticleSystem[] Explosions = new ParticleSystem[3];
    public Transform[] ExplosionPositions = new Transform[3];
    public ParticleSystem UnlockCardExplosion;
    public ParticleSystem[] Weather = new ParticleSystem[3];
    public int WeatherInPlay = 0;

    void Start() { 
    }

    public void DestroyExtraCards() {
        BigExplosion.Play();
    }

    public void DestroyCards() {
        Vector3 Position = ExplosionPositions[0].localPosition;
        Position.x = -0.11f;
        ExplosionPositions[0].localPosition = Position;
        Explosions[0].Play();
    }

    public void Destroy2Cards() {
        Vector3 Position1 = ExplosionPositions[0].localPosition;
        Vector3 Position2 = ExplosionPositions[1].localPosition;
        Position1.x = 1.13f;
        Position2.x = -1.13f;
        ExplosionPositions[0].localPosition = Position1;
        ExplosionPositions[1].localPosition = Position2;
        Explosions[0].Play();
        Explosions[1].Play();
    }

    public void Destroy3Cards() {
        Vector3 Position1 = ExplosionPositions[0].localPosition;
        Vector3 Position2 = ExplosionPositions[1].localPosition;
        Vector3 Position3 = ExplosionPositions[2].localPosition;
        Position1.x = 2.46f;
        Position2.x = -0.08f;
        Position3.x = -2.62f;
        ExplosionPositions[0].localPosition = Position1;
        ExplosionPositions[1].localPosition = Position2;
        ExplosionPositions[2].localPosition = Position3;
        Explosions[0].Play();
        Explosions[1].Play();
        Explosions[2].Play();
    }

    public void UnlockedCard() {
        UnlockCardExplosion.Play();
    }

    public void SetWeather(int WeatherNumber) {
        Weather[WeatherNumber - 1].Play();
        WeatherInPlay = WeatherNumber;
    }
}
