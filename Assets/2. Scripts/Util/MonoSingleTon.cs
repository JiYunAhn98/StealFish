using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : MonoSingleTon<T>
{
    static volatile T _uniqueInstance = null;
    static volatile GameObject _uniqueObject = null;

    protected MonoSingleTon()
    {

    }

    public static T _instance
    {
        get
        {   // DCL구조(Double Checked Lock 구조)
            //Debug.Log("We Make Singleton for " + typeof(T).Name);
            if (_uniqueInstance == null)    //이미 한번 생성했다면 그대로 return하면 됨
            {
                lock (typeof(T))             //Multi Thread환경에서 보호
                {
                    if (_uniqueInstance == null && _uniqueObject == null)   // 대기 중인 Thread에게 한번더 확인
                    {
                        T[] tmp = FindObjectsOfType<T>();
                        if (tmp.Length > 1)
                        {
                            //Debug.Log("-----Singleton Error-----");
                            return _uniqueInstance;
                        }
                        else if (tmp.Length == 1)
                        {
                            //Debug.Log("-----Already Scene Have This-----");
                            _uniqueInstance = tmp[0];
                            _uniqueObject = _uniqueInstance.gameObject;
                        }
                        if (_uniqueInstance == null)
                        {
                            //Debug.Log("-----Make New Object-----");
                            _uniqueObject = new GameObject(typeof(T).Name, typeof(T));
                            _uniqueInstance = _uniqueObject.GetComponent<T>();
                        }
                        _uniqueInstance.Init();
                    }
                    else
                    {
                        //Debug.Log("-----Before Thread Make This-----");
                    }
                }
            }
            else
            {
                //Debug.Log("-----Already Single Object-----");
            }
            return _uniqueInstance;
        }
    }
    protected virtual void Init()
    {
        DontDestroyOnLoad(gameObject);
    }
}
