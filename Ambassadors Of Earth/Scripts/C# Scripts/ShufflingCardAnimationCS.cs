using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShufflingCardAnimationCS : MonoBehaviour {

    public Animation ShufflingCardAnimation;
    private float WaitTime = 0.5f;
    
	void Start () {
		
	}

    public IEnumerator PlayShuffle()
    {
        ShufflingCardAnimation.Play("ShufflingCard");
        yield return new WaitForSeconds(WaitTime);
        ShufflingCardAnimation.Play("ShufflingCard2");
        yield return new WaitForSeconds(WaitTime);
        ShufflingCardAnimation.Play("ShufflingCard");
        yield return new WaitForSeconds(WaitTime);
        ShufflingCardAnimation.Play("ShufflingCard2");
        yield return new WaitForSeconds(WaitTime);
        ShufflingCardAnimation.Play("ShufflingCard");
        yield return new WaitForSeconds(WaitTime);
        ShufflingCardAnimation.Play("ShufflingCard2");
    }
}
