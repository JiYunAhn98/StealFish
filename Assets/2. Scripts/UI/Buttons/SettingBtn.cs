using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SettingBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager._instance.OpenPopup(DefineHelper.ePrefabPopup.SettingWnd);
        SoundManager._instance.BackupOriginVolume();
    }
}
