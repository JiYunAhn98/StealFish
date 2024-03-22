using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DefineHelper;
using TMPro;
public class DroppableSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject _other;

    [SerializeField] Image _iconImage;
    [SerializeField] Color _highlightColor = Color.white;
    [SerializeField] Color _BlockedColor = Color.red;
    Color _normalColor = Color.white;

    [SerializeField] TextMeshProUGUI _ItemExplainText;
    [SerializeField] TextMeshProUGUI _ItemCountText;

    public void OnDrop(PointerEventData eventData)
    {
        Image _droppedImage = eventData.pointerDrag.transform.GetChild(0).GetComponent<Image>();

        if (_other.transform.GetChild(0).GetComponent<Image>().enabled &&
            _droppedImage.sprite.name == _other.transform.GetChild(0).GetComponent<Image>().sprite.name)
        {
            if (!_iconImage.enabled) _other.transform.GetChild(0).GetComponent<Image>().enabled = false;
            else _other.transform.GetChild(0).GetComponent<Image>().sprite = _iconImage.sprite;
            _other.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _ItemExplainText.text;
        }

        if (!_iconImage.enabled) _iconImage.enabled = true;

        _iconImage.sprite = _droppedImage.sprite;
        _iconImage.color = _normalColor;
        _ItemExplainText.text = TableManager._instance.TakeString(TableManager.eTableJsonNames.Items, (int)System.Enum.Parse(typeof(eItemType), _droppedImage.sprite.name), TableManager.eIndex.Text.ToString());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) //드래그 중인 상황
        {
            if (_iconImage.sprite != null) _iconImage.color = _highlightColor;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) //드래그 중인 상황
        {
            if (_iconImage.sprite != null) _iconImage.color = _normalColor;
        }
    }
    public void ClickItemPlusCountBtn()
    {
        int tmp = int.Parse(_ItemCountText.text) + 1;
        if (tmp < 10) _ItemCountText.text = tmp.ToString();
    }
    public void ClickItemMinusCountBtn()
    {
        int tmp = int.Parse(_ItemCountText.text) - 1;
        if(tmp > 0) _ItemCountText.text = tmp.ToString();
    }
    public SlotItemInfo GetItemSlotInform()
    {
        return _iconImage == null ? new SlotItemInfo(null, 0, 10) : new SlotItemInfo(_iconImage.sprite, int.Parse(_ItemCountText.text), 10);
    }
    public void ResetItemSlotInform()
    {
        _iconImage.sprite = null;
        _iconImage.enabled = false;
        _ItemExplainText.text = string.Empty;
    }
    public void SetItemSlotInform(SlotItemInfo item)
    {
        _iconImage.sprite = item._icon;
        _iconImage.color = _normalColor;
        _iconImage.enabled = true;
        _ItemExplainText.text = TableManager._instance.TakeString(TableManager.eTableJsonNames.Items, (int)System.Enum.Parse(typeof(eItemType), item._icon.name), TableManager.eIndex.Text.ToString());
    }

    /*  강사님 DroppableSlot 원본
     * 
    [SerializeField] Image _iconImage;
    [SerializeField] Color _highlightColor = Color.white; //tip: 지정해놓으면 alpha값이 0이 아니다

    Color _normalColor;
    void Awake()
    {
        _normalColor = _iconImage.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 이미지를 slot 안에다가 넣어줄 것이다. 하지만 실제로 사용할 경우에는 그 이미지의 데이터까지 같이 넘겨줘야 한다.
        Image _droppedImage = eventData.pointerDrag.transform.GetComponent<Image>();
        _iconImage.sprite = _droppedImage.sprite;
        _iconImage.color = _normalColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) //드래그 중인 상황
        {
            _iconImage.color = _highlightColor;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) //드래그 중인 상황
        {
            _iconImage.color = _normalColor;
        }
    }
     */
}
