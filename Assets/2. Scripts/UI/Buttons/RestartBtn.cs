using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DefineHelper;

public class RestartBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneControlManager._instance.SettingNextScene(eSceneName.Ingame);
        PopupManager._instance.OpenPopup(ePrefabPopup.CheckWnd);
    }
}