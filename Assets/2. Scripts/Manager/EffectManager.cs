using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class EffectManager : MonoSingleTon<EffectManager>
{
    Dictionary<ePrefabEffect, GameObject> _prefabsEffect;
    List<GameObject> _clickEffect;
    Dictionary<ePrefabEffect, List<GameObject>> _gameEffect;

    public void InitializePrefab()
    {
        _prefabsEffect = new Dictionary<ePrefabEffect, GameObject>();
        for (int i = 0; i < (int)ePrefabEffect.Count; i++)
        {
            _prefabsEffect.Add((ePrefabEffect)i, Resources.Load("Effect/" + ((ePrefabEffect)i).ToString()) as GameObject);
        }
        _clickEffect = new List<GameObject>();
        _gameEffect = new Dictionary<ePrefabEffect, List<GameObject>>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShowEffect(ePrefabEffect.Click, Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward));
    }
    public void ShowEffect(ePrefabEffect effect, Vector3 pos)
    {
        int i;
        GameObject go;
        if (effect == ePrefabEffect.Click)
        {
            for (i = 0; i < _clickEffect.Count; i++)
            {
                if (!_clickEffect[i].activeSelf) break;
            }

            if (i == _clickEffect.Count)
                _clickEffect.Add(Instantiate(_prefabsEffect[effect], pos, Quaternion.identity, transform));
            else
            {
                _clickEffect[i].SetActive(true);
                _clickEffect[i].transform.position = pos;
            }
        }
        else
        {
            if (_gameEffect.ContainsKey(effect))
            {
                for (i = 0; i < _gameEffect[effect].Count; i++)
                {
                    if (!_gameEffect[effect][i].activeSelf) break;
                }
                if (i == _gameEffect[effect].Count)
                    _gameEffect[effect].Add(Instantiate(_prefabsEffect[effect], pos, Quaternion.identity, transform));
                else
                {
                    _gameEffect[effect][i].SetActive(true);
                    _gameEffect[effect][i].transform.position = pos;
                }
            }
            else
            {
                _gameEffect.Add(effect, new List<GameObject>());
                _gameEffect[effect].Add(Instantiate(_prefabsEffect[effect], pos, Quaternion.identity, transform));
            }
        }

    }
    public void EffectAllDown()
    {

        foreach(ePrefabEffect effect in _gameEffect.Keys)
        {
            for (int i = 0; i < _gameEffect[effect].Count; i++)
            {
                if (_gameEffect[effect][i] == null)
                    Debug.Log(_gameEffect[effect][i].name);
                _gameEffect[effect][i].SetActive(false);
            }
        }
    }
}
