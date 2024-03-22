using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.EventSystems;
public class GameStartBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager._instance.OpenPopup(ePrefabPopup.ReadyWnd);
    }
}
