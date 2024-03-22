using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundMarkerObj : MonoBehaviour
{
    TextMeshProUGUI _txtRoundNumber;

    public void InitSet(int Num)
    {
        _txtRoundNumber = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _txtRoundNumber.text = Num.ToString();
    }
}
