using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class UserInformManager : CSSingleTon<UserInformManager>
{
    // 게임 진행 관련
    public SlotItemInfo[] _equipItems { get; set; }

    public void GetEquipItem(int i, SlotItemInfo item)
    {
        if(_equipItems == null) _equipItems = new SlotItemInfo[2];
        _equipItems[i] = new SlotItemInfo(item._icon, item._count, item._coolTime);
    }
    public SlotItemInfo NowEquipItem(int i)
    {
        if (_equipItems == null) return new SlotItemInfo(null, 0, 10);
        return _equipItems[i];
    }
}
