using DefineHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWnd : Popup
{
    public override void InitSetting()
    {
    }
    public void ClickOKBtn()
    {
        PopupManager._instance.ClosePopup(ePrefabPopup.CheckWnd);
        SceneControlManager._instance.LoadScene();
    }
    public void ClickNOBtn()
    {
        PopupManager._instance.ClosePopup(ePrefabPopup.CheckWnd);
    }
}
