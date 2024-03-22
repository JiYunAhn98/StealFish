using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineHelper;
using UnityEngine.SceneManagement;
public class ResultWnd : Popup
{
    [SerializeField] Image _rank;
    [SerializeField] TextMeshProUGUI _userScore;
    [SerializeField] RectTransform[] _stealFishes;

    Sprite[] _rankSprites;
    bool _isTouch;
    public override void InitSetting()
    {
        _rankSprites = new Sprite[(int)eRankSprite.Count];
        for (int i = 0; i < _rankSprites.Length; i++)
        {
            _rankSprites[i] = Resources.Load(((eRankSprite)i).ToString()) as Sprite;
        }
        for (int i = 0; i < _stealFishes.Length; i++)
        {
            _stealFishes[i].GetChild(2).GetComponent<Image>().enabled = false;
            _stealFishes[i].gameObject.SetActive(false);
        }
        _rank.enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _isTouch = true;
        }

    }
    public IEnumerator ShowResultSlow(List<int> scores, int spawnTotalScore, int stealTotalScore)
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < scores.Count; i++)
        {
            _stealFishes[i].gameObject.SetActive(true);
            _stealFishes[i].GetComponentInChildren<TextMeshProUGUI>().text = scores[i].ToString();
            SoundManager._instance.PlayEffect(eEffectSound.Result);
            if (scores[i] <= 0)
            {
                EffectManager._instance.ShowEffect(ePrefabEffect.ResultFail, _stealFishes[i].position);
                _stealFishes[i].GetChild(2).GetComponent<Image>().enabled = true;
            }
            else
            {
                EffectManager._instance.ShowEffect(ePrefabEffect.ResultSuccess, _stealFishes[i].position);
                _stealFishes[i].GetChild(2).GetComponent<Image>().enabled = false;
            }
            if (!_isTouch) yield return new WaitForSeconds(0.2f);
        }
        _userScore.text = stealTotalScore.ToString();
        _rank.enabled = true;

        int rankPoint = stealTotalScore * 100 / spawnTotalScore;
        if (rankPoint < 10)
            _rank.sprite = _rankSprites[(int)eRankSprite.F];
        else if (rankPoint < 20)
            _rank.sprite = _rankSprites[(int)eRankSprite.C];
        else if (rankPoint < 30)
            _rank.sprite = _rankSprites[(int)eRankSprite.B];
        else
            _rank.sprite = _rankSprites[(int)eRankSprite.A];
    }

    public void ReplayButtonOnClick()
    {
        PopupManager._instance.OpenPopup(ePrefabPopup.ReadyWnd);
    }
    public void CloseButtonClick()
    {
        StopAllCoroutines();
        SceneControlManager._instance.SettingNextScene(eSceneName.Lobby);
        SceneControlManager._instance.LoadScene();
    }
}
