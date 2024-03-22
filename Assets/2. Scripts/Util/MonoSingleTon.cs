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
        {   // DCL����(Double Checked Lock ����)
            //Debug.Log("We Make Singleton for " + typeof(T).Name);
            if (_uniqueInstance == null)    //�̹� �ѹ� �����ߴٸ� �״�� return�ϸ� ��
            {
                lock (typeof(T))             //Multi Threadȯ�濡�� ��ȣ
                {
                    if (_uniqueInstance == null && _uniqueObject == null)   // ��� ���� Thread���� �ѹ��� Ȯ��
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
