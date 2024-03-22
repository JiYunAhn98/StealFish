using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;
public class GameMsgWnd : MonoBehaviour
{
    ScrollRect _myWnd;
    GameObject _prefabTxtLine;

    public void InitSet()
    {
        _myWnd = GetComponent<ScrollRect>();
        _prefabTxtLine = Resources.Load(ePrefabObj.TextLine.ToString()) as GameObject;
    }

    public void SendGameMessage(string msg, Color color)
    {
        GameObject go = Instantiate(_prefabTxtLine, _myWnd.content);
        Text text = go.GetComponent<Text>();
        text.text = msg;
        text.color = color;
    }
    public void SendGameMessage(string msg)
    {
        GameObject go = Instantiate(_prefabTxtLine, _myWnd.content);
        Text text = go.GetComponent<Text>();
        text.text = msg;
        text.color = Color.white;
    }

}
