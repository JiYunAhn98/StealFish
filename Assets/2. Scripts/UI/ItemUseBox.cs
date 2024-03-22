using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseBox : MonoBehaviour
{
    List<SlotObj> _itemSlots;

    void Awake()
    {
        _itemSlots = new List<SlotObj>();

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("UIItemUseSlotObj"))
            _itemSlots.Add(go.GetComponent<SlotObj>());
    }

    public void Init()
    {
    }
}
