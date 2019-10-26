using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCardControlCS : MonoBehaviour {

    public float ResourceType;       // 0.01=life, 0.1=Resource, 1=faith, 10=humans, 100=Destruction, -100= life starred, -200=Resource starred, -300=faith starred, -400=Human starred, -500= Destruction starred
    public int CardNumber;           // Card Number is the order number of the card so actual card number is -1
    public int CardPlayPosition;     // 1st, 2nd or 3rd card played?
    public Sprite[] AttributeImages;

	void Start () {
		
	}
	
	void Update () {
		
	}
}
