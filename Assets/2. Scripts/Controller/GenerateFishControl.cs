using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DefineHelper;
public class GenerateFishControl : MonoBehaviour
{
    // 지정범위 안에 생선을 지정 개수만큼 생성
    // 생성되어있던 생선들을 제거
    List<FishObj> _fishes;
    Queue<int> _numbers;

    void Awake()
    {
        _numbers = new Queue<int>();
        _fishes = new List<FishObj>();

    }
    public int GenerateFish(GameObject fishPrefab, int fishCount)
    {
        if (_numbers.Count <= 0) return -1;

        //float x = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2;
        //float y = gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        //float fishX = prefab.GetComponent<RectTransform>().sizeDelta.x / 2;
        //float fishY = prefab.GetComponent<RectTransform>().sizeDelta.y / 2;
        int sum = 0;
        for (int i = 0; i < fishCount; i++)
        {
            Vector2 dir = new Vector2(Random.Range(-100,100), Random.Range(-100, 100));
            GameObject go = Instantiate(fishPrefab, gameObject.transform);
            go.GetComponent<FishObj>().InitFish(dir, _numbers.Peek());
            _numbers.Dequeue();
            _fishes.Add(go.GetComponent<FishObj>());
            sum += _fishes[i]._myNum;
        }
        EffectManager._instance.ShowEffect(ePrefabEffect.SpawnFish, transform.position);
        return sum;
    }
    public void FishNumberSetting(int fishCount)
    {
        if (_numbers.Count > 0) _numbers.Clear();

        int[] array = new int[fishCount * 10];

        for (int i = 0; i < array.Length; i++)
        {
            int fishNum;
            while (true)
            {
                int j;
                fishNum = Random.Range(1, 256);
                for (j = 0; j < i; j++) if (fishNum == array[j]) break;
                if (j == i)
                {
                    array[j] = fishNum;
                    break;
                }
            }
            _numbers.Enqueue(fishNum);
        }
    }
    public void DestroyAll(int answer)
    {
        if (_fishes.Count <= 0) return;
        for (int i = 0; i < _fishes.Count; i++)
        {
            if (_fishes[i]._myNum == answer)
                EffectManager._instance.ShowEffect(ePrefabEffect.SuccessStealFish, _fishes[i].gameObject.transform.position);
            else
                EffectManager._instance.ShowEffect(ePrefabEffect.FailStealFish, _fishes[i].gameObject.transform.position);
            Destroy(_fishes[i].gameObject);
        }
        _fishes.Clear();
    }
    public int isMatch(int answer)
    {
        int score = 0;
        for (int i = 0; i < _fishes.Count; i++)
        {
            if (_fishes[i]._myNum == answer)
            {
                score = answer;
            }
        }
        return score;
    }
    public int FindNum(int roundFish)
    {
        int hintFish = Random.Range(0, roundFish);
        _fishes[hintFish].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        EffectManager._instance.ShowEffect(ePrefabEffect.ForHint, _fishes[hintFish].transform.position);
        SoundManager._instance.PlayEffect(eEffectSound.ForHintItemUse);
        return _fishes[hintFish]._myNum;
    }

}
