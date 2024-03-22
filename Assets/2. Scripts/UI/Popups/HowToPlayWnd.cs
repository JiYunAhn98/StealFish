using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayWnd : Popup
{
    public override void InitSetting()
    {
        
    }
    public void ExitBtn()
    {
        PopupManager._instance.ClosePopup(DefineHelper.ePrefabPopup.HowToPlayWnd);
    }
}
