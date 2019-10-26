using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionCardMovementCS : MonoBehaviour {

    public int OptionCardNumber;
    public GameObject OptionCard;
    public float Speed = 1.0f;
    public Vector3 MovePoint;
    public float LerpProgress = 0;

	void Start () {
        MovePoint = OptionCard.transform.localPosition;
    }
	
	void Update () {
        if (LerpProgress <= 1)
        {
            LerpProgress += Time.deltaTime / 1.5f;
            OptionCard.transform.localPosition = Vector3.Lerp(OptionCard.transform.localPosition, MovePoint, LerpProgress);
        }
    }

    public void MoveOnClick()
    {
        LerpProgress = 0;
        if (OptionCardNumber == 1) { MovePoint = new Vector3(3.09f, 0.1f, -3.6f); }
        if (OptionCardNumber == 2) { MovePoint = new Vector3(-0.61f, 0.1f, -3.6f); }
        if (OptionCardNumber == 3) { MovePoint = new Vector3(-4.31f, 0.1f, -3.6f); }
    }

    public void MoveBackOnClick()
    {
        LerpProgress = 0;
        if (OptionCardNumber == 1) { MovePoint = new Vector3(16, 5.41f, -2.64f); }
        if (OptionCardNumber == 2) { MovePoint = new Vector3(21, 7.05f, -2.64f); }
        if (OptionCardNumber == 3) { MovePoint = new Vector3(26.31f, 9.35f, -2.64f); }
    }
}
