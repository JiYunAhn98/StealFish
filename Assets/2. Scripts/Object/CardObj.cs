using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardObj : MonoBehaviour, IPointerClickHandler
{
    // 참조 변수
    [SerializeField] TextMeshProUGUI _txtBitNums;
    [SerializeField] RectTransform _card;
    [SerializeField] TextMeshProUGUI _digitNum;

    // 정보 변수
    
    bool _isBack;           // 뒤로 돌았나?
    bool _isZero = true;    // 지금 0인가?
    bool _isTurn;         // 클릭을 했나?
    float _turnTime = 0.8f;
    float _time = 0;
    int _number;
    bool _isHint;

    public int _numDigit
    {
        get { return 1 << (_number - 1); }      //1, 2, 3, 4,...으로 들어온다.
        set { _number = value; }
    }
    ////강사님꺼
    //Transform tfBG;
    //bool isBack;
    //bool isTurn;
    //float timeCheck = 0;
    //float rotAngle = 180;

    void Awake()
    {
        //강사님코드
        //tfBG = transform.GetChild(0);
    }
    void Update()
    {
        if (_isTurn)
            ChangeNumber();
        //강사님코드
        //if (isTurn)
        //    TeacherChangeBit();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _isTurn = true;
        _time = 0;
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.CardTurn);

        ////강사님코드
        //if (isTurn) return;
        //isBack = !isBack;
        //isTurn = true;
        //timeCheck = 0;
    }
    //public void TeacherChangeBit()
    //{
    //    tfBG.Rotate(Vector3.up * Time.deltaTime * rotAngle);
    //    timeCheck += Time.deltaTime;
    //    if (timeCheck >= ((360 / rotAngle) / 4))
    //    {
    //        _txtBitNums.text = isBack ? "1" : "0";
    //    }
    //    if (timeCheck >= ((360 / rotAngle) / 2))
    //    {
    //        isTurn = false;
    //        tfBG.rotation = isBack ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 360, 0);
    //    }
    //}
    public void ChangeNumber()
    {
        _time += Time.deltaTime;
        if (_time < _turnTime / 2)
        {
            _card.Rotate(new Vector3(0, 90 * Time.deltaTime * _turnTime * 2, 0));
            if(!_isBack)_isBack = true;
        }
        else if (_time < _turnTime)
        {
            if (_isBack)
            {
                _isZero = !_isZero;
                _txtBitNums.text = _isZero ? "0" : "1";
                _isBack = false;
            }
            _card.Rotate(new Vector3(0, -90 * Time.deltaTime * _turnTime * 2, 0));
        }
        else
        {
            _isTurn = false;
            _card.rotation = Quaternion.identity;
        }
    }
    public void HintNumber()
    {
        _isHint = true;
        _digitNum.color = Color.red;
    }
    public void CardSpawnSetting(int num)
    {
        _numDigit = num;
        _digitNum.text = _numDigit.ToString();
    }
    public IEnumerator ResetCard()
    {
        if (_isHint)
        {
            _isHint = false;
            _digitNum.color = Color.yellow;
        }
        if (!_isZero)
        {
            _isTurn = true;
            _time = 0;
            yield return new WaitForSeconds(0.15f);
        }
    }
    public int Sum()
    {
        return _isZero ? 0 : _numDigit; 
    }

}
