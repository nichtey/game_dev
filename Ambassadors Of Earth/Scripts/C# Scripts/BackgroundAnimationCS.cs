using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimationCS : MonoBehaviour {

    public int PositionNumber;
    public Sprite[] FlyingBird = new Sprite[15];
    public SpriteRenderer FlyingBirdImage;
    public Sprite[] HumanKing = new Sprite[15];
    public SpriteRenderer HumanKingImage;

	void Start () {
        InvokeRepeating("Fly", 2, 0.08f);
    }
	
	void Update () {
		
	}

    void Fly () {
        PositionNumber++;
        FlyingBirdImage.sprite = FlyingBird[PositionNumber % 15];
        HumanKingImage.sprite = HumanKing[PositionNumber % 15];
    }
}
