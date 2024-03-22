using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class PopupManager : MonoSingleTon<PopupManager>
{
    // prefab
    GameObject[] _prefabPopups;

    // instance
    Dictionary<ePrefabPopup, GameObject> _popups;

    // trigger
    public eSceneName _checkWndMoveScene { get; set; }

    public void Initialize()
    {
        _prefabPopups = new GameObject[(int)ePrefabPopup.Count];
        for (int i = 0; i < _prefabPopups.Length; i++)
            _prefabPopups[i] = Resources.Load<GameObject>("UI/" + ((ePrefabPopup)i).ToString());
        _popups = new Dictionary<ePrefabPopup, GameObject>();
    }
    public void LoadOtherScene()
    {
        foreach(ePrefabPopup key in _popups.Keys)
        {
            ClosePopup(key);
        }
    }
    // instance popup manage
    public void OpenPopup(ePrefabPopup type)
    {
        if (_popups.ContainsKey(type))
        {
            if(!_popups[type].activeSelf)
                _popups[type].SetActive(true);
        }
        else
        {
            GameObject go = Instantiate(_prefabPopups[(int)type], transform);
            _popups.Add(type, go);
        }
        _popups[type].GetComponent<Canvas>().worldCamera = Camera.main;
        _popups[type].GetComponent<Canvas>().sortingLayerName = "Popup";
        _popups[type].GetComponentInChildren<Popup>().InitSetting(); 
    }
    public void ClosePopup(ePrefabPopup type)
    {
        _popups[type].SetActive(false);
    }
}
