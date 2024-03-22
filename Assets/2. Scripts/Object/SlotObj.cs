using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;
using TMPro;
public class SlotObj : MonoBehaviour
{
    [SerializeField] Image _frameBG;
    [SerializeField] Image _iconItem;
    [SerializeField] Image _coolTime;
    [SerializeField] TextMeshProUGUI _txtCount;

    // 정보변수
    SlotItemInfo _slotItemInfo;
    bool _isUsable;

    void Update()
    {
        if (_isUsable)
        {
            if (_coolTime.fillAmount <= 0)
            {
                _coolTime.enabled = false;
            }
            else
            {
                _coolTime.fillAmount -= Time.deltaTime / _slotItemInfo._coolTime;
            }
        }
    }

    public void InitDataSet(SlotItemInfo info)
    {
        _slotItemInfo = info;
        if (info._icon != null)
        {
            _isUsable = true;
            _iconItem.sprite = _slotItemInfo._icon;
            _frameBG.color = Color.white;
            _coolTime.enabled = false;
            _txtCount.text = _slotItemInfo._count.ToString();
        }
        else
        {
            _isUsable = false;
            _iconItem.enabled = false;  //icon을 끈다
            _txtCount.enabled = false;
            _frameBG.color = Color.gray;
        }
        _coolTime.fillAmount = 0;
    }

    public void ItemUse()
    {
        if (_isUsable)
        {
            if (_coolTime.fillAmount <= 0)
            {
                _coolTime.fillAmount = 1;
                _slotItemInfo._count--;
                _txtCount.text = _slotItemInfo._count.ToString();
                _coolTime.enabled = true;
                IngameManager._instance.OnClickItem(_slotItemInfo._icon.name);
                if (_slotItemInfo._count <= 0)
                {
                    // 아이템을 다 사용했으니 해당 처리
                    _isUsable = false;
                }
            }
        }
    }
}
