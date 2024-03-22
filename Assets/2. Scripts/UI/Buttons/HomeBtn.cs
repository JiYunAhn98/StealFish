using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DefineHelper;

public class HomeBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneControlManager._instance.SettingNextScene(eSceneName.Lobby);
        PopupManager._instance.OpenPopup(ePrefabPopup.CheckWnd);
    }
}
