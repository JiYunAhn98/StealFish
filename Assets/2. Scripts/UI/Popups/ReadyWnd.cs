using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineHelper;
public class ReadyWnd : Popup
{
    // 하위 UI객체
    [SerializeField] GameObject[] _equipSlots;
    [SerializeField] Transform _rootUnequipItemSlot;

    // Resources
    Sprite[] _ItemIcons;
    GameObject _unequipItemSlot;

    void Awake()
    {
        _unequipItemSlot = Resources.Load("Item/UnequipItemSlot") as GameObject;

        // 아이템 unequip slot 가져오기
        _ItemIcons = new Sprite[(int)eItemType.Count];
        for (int i = 0; i < (int)eItemType.Count; i++)
        {
            _ItemIcons[i] = Resources.Load<Sprite>("Item/" + ((eItemType)i).ToString());
            GameObject go = Instantiate(_unequipItemSlot, _rootUnequipItemSlot);
            go.transform.GetChild(0).GetComponent<Image>().sprite = _ItemIcons[i];

            float distance = go.GetComponent<RectTransform>().sizeDelta.x + 10;
            go.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * distance, 0);
        }
        _equipSlots[0].GetComponent<DroppableSlot>().ResetItemSlotInform();
        _equipSlots[1].GetComponent<DroppableSlot>().ResetItemSlotInform();
    }
    public override void InitSetting()
    {
        for (int i = 0; i < _equipSlots.Length; i++)
        {
            SlotItemInfo slotItemInfo = UserInformManager._instance.NowEquipItem(i);
            if (slotItemInfo._icon == null)
            {
                return;
            }
            else
            {
                _equipSlots[i].GetComponent<DroppableSlot>().SetItemSlotInform(slotItemInfo);
            }
        }
    }

    // 클릭 버튼 관련
    public void ClickStartButton()
    {
        SceneControlManager._instance.SettingNextScene(eSceneName.Ingame);
        for (int i = 0; i < _equipSlots.Length; i++)
        {
            UserInformManager._instance.GetEquipItem(i, _equipSlots[i].GetComponent<DroppableSlot>().GetItemSlotInform());
        }
        if (UserInformManager._instance._equipItems[0]._icon == null || UserInformManager._instance._equipItems[1]._icon == null)
        {
            PopupManager._instance.OpenPopup(ePrefabPopup.CheckWnd);
        }
        else
        {
            //PopupManager._instance.ClosePopup(ePrefabPopup.ReadyWnd);
            SceneControlManager._instance.LoadScene();
        }
    }
    public void ClickCloseButton()
    {
        PopupManager._instance.ClosePopup(ePrefabPopup.ReadyWnd);
    }

    public void ClickResetButton()
    {
        _equipSlots[0].GetComponent<DroppableSlot>().ResetItemSlotInform();
        _equipSlots[1].GetComponent<DroppableSlot>().ResetItemSlotInform();
    }
}
