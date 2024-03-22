using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FishObj : MonoBehaviour
{
    RectTransform _myRect;
    [SerializeField]TextMeshProUGUI _txtNumber;

    public int _myNum
    {
        get; set;
    }
    public void InitFish(Vector2 pos, int number)
    {
        _myRect = GetComponent<RectTransform>();
        _myRect.anchoredPosition = pos;
        _myNum = number;
        _txtNumber.text = number.ToString();
    }
}
