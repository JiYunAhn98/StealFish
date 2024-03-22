using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DefineHelper;

public class HowToPlayBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager._instance.OpenPopup(ePrefabPopup.HowToPlayWnd);
    }
}