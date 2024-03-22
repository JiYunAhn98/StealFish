using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineHelper;
public class IngameManager : MonoBehaviour
{
    static IngameManager _uniqueInstance;

    // Prefabs
    GameObject _prefabCard;
    GameObject _prefabRoundMK;
    GameObject _prefabFish;

    // UI
    ProcessMsgBox _processMsgBox;
    TextMeshProUGUI _txtAcquisionScore;
    Slider _barTimer;
    GameMsgWnd _wndGMsg;
    GenerateFishControl _ctrlGenFish;
    Image _fireAnimation;
    List<SlotObj> _itemSlots;
    

    // 참조
    RectTransform _cardBoard;
    RectTransform _cardRefPos;
    RectTransform _mkRound;
    RectTransform _mkRefPos;
    Dictionary<int, CardObj> _cards;
    Dictionary<int, RoundMarkerObj> _roundMarkers;
    List<int> _acquisitionScoreList;
    Animator _catMotionControl;

    // 정보
    int _roundFish = 3;
    int _maxCardCount = 8;
    int _maxRoundCount = 10;
    const float _distBeCard = 100;
    const float _distBeRoundMarker = 75;
    const float _limitSelectTime = 10;
    eProgstate _nowProgress;
    float _checkTime;
    int _roundCount = 0;
    int _totalGeneratePoint;

    // 아이템 사용
    bool _isDoublePoint;
    bool _isSlowTime;

    public static IngameManager _instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
    }
    void Start()
    {
        ProgInit();
    }
    void Update()
    {
        switch (_nowProgress)
        {
            case eProgstate.Init:
                _checkTime += Time.deltaTime;
                if (_checkTime >= 1)
                {
                    IEnumerator it = ProgReady().GetEnumerator();
                    StartCoroutine(it);
                }
                break;
            case eProgstate.Start:
                _checkTime += Time.deltaTime;
                if (_checkTime >= 1)
                {
                    StartCoroutine(ProgGeneratingNum());
                }
                break;
            case eProgstate.Play:
                _checkTime -= _isSlowTime ? Time.deltaTime / 2 : Time.deltaTime;
                if (_checkTime <= 0)
                {
                    _wndGMsg.SendGameMessage("시간초과 되었습니다.");
                    _processMsgBox.OpenBox("Time Over");
                    _acquisitionScoreList.Add(0);
                    ChangeCatMotion(eRoundResultType.TimeOut);
                    SoundManager._instance.PlayEffect(eEffectSound.Fail);
                    RoundFinish();
                }
                _barTimer.value = _checkTime / _limitSelectTime;
                break;
            case eProgstate.Result:
                _checkTime += Time.deltaTime;
                if (_checkTime >= 1  && Input.GetMouseButton(0))
                {
                    StopCoroutine("ShowResultSlow");
                    ResultWnd _resultWnd = GameObject.FindGameObjectWithTag("UIResultWnd").GetComponent<ResultWnd>();
                    _resultWnd.ShowResultSlow(_acquisitionScoreList, _totalGeneratePoint, int.Parse(_txtAcquisionScore.text));
                }
                break;
            case eProgstate.End:
                _checkTime += Time.deltaTime;
                if (_checkTime >= 4) ProgResult();
                break;
        }
    }

    // 내부 처리 함수
    IEnumerator SettingCards()
    {
        for (int i = 0; i < _maxCardCount; i++)
        {
            GameObject go = Instantiate(_prefabCard, _cardRefPos);
            go.GetComponent<RectTransform>().anchoredPosition =  new Vector2(_distBeCard * i, 0);
            go.GetComponent<CardObj>().CardSpawnSetting(_maxCardCount - i);
            _cards.Add(_maxCardCount - i, go.GetComponent<CardObj>());
            SoundManager._instance.PlayEffect(eEffectSound.CardGenerate);
            yield return new WaitForSeconds(0.2f);
        }
        _wndGMsg.SendGameMessage("카드세팅이 끝났습니다.");
    }
    void SettingRoundMarkers()
    {
        Vector2 position = _mkRefPos.anchoredPosition;

        for (int i = 0; i < _maxRoundCount; i++)
        {
            GameObject go = Instantiate(_prefabRoundMK, _mkRound);
            go.GetComponent<RectTransform>().anchoredPosition = position + new Vector2(0, _distBeRoundMarker * i);
            go.GetComponent<RoundMarkerObj>().InitSet(i+1);
            go.GetComponent<RectTransform>().SetAsFirstSibling();
            _roundMarkers.Add(i + 1, go.GetComponent<RoundMarkerObj>());
        }
    }
    void GenerationFish()
    {
        _totalGeneratePoint+= _ctrlGenFish.GenerateFish(_prefabFish, _roundFish);    // 생선을 원하는 갯수만큼 생성해낸다
    }
    void ChangeCatMotion(eRoundResultType type)
    {
        _catMotionControl.SetInteger("eRoundResultType", (int)type);
    }
    void RoundFinish()
    {
        _isSlowTime = false;
        _isDoublePoint = false;
        EffectManager._instance.EffectAllDown();

        if (_roundCount < _maxRoundCount)
        {
            StartCoroutine(ProgGeneratingNum());
        }
        else
            ProgEnd();
    }

    //============== 게임 프로세스 함수 ===============================
    public void ProgInit()
    {
        _nowProgress = eProgstate.Init;
        // 필요 프리팹 저장
        _prefabCard = Resources.Load(ePrefabObj.CardObj.ToString()) as GameObject;                    // 2진수 카드
        _prefabRoundMK = Resources.Load(ePrefabObj.RoundMarker.ToString()) as GameObject;             // 라운드 마크
        _prefabFish = Resources.Load(ePrefabObj.FishObj.ToString()) as GameObject;                    // 생선

        // 게임 진행 상태를 보여주는 메시지창
        _processMsgBox = GameObject.FindGameObjectWithTag("UIProcessMsgBox").GetComponent<ProcessMsgBox>();
        // 게임 중 취득한 전체 점수를 보여주는 컴포넌트
        _txtAcquisionScore = GameObject.FindGameObjectWithTag("UIScoreText").GetComponent<TextMeshProUGUI>();
        // 숫자 비트를 선택하는데 주어진 시간을 보여주는 타이머
        _barTimer = GameObject.FindGameObjectWithTag("UISelectTimer").GetComponentInChildren<Slider>();
        // 카드 생성 기본 위치
        _cardRefPos = GameObject.FindGameObjectWithTag("CardGenPos").GetComponent<RectTransform>();
        _cardBoard = _cardRefPos.parent.GetComponent<RectTransform>();
        // 라운드 마커 생성 기본 위치
        _mkRefPos = GameObject.FindGameObjectWithTag("RoundMKPos").GetComponent<RectTransform>();
        _mkRound = _mkRefPos.parent.GetComponent<RectTransform>();
        // 게임 메세지 윈도우
        _wndGMsg = GameObject.FindGameObjectWithTag("UIGameMsgWindow").GetComponent<GameMsgWnd>();
        _wndGMsg.InitSet();

        // 생선 생성기
        _ctrlGenFish = GameObject.FindGameObjectWithTag("UIGenerationFish").GetComponent<GenerateFishControl>();
        // 불 애니메이션
        _fireAnimation = GameObject.FindGameObjectWithTag("UIRoundBarFireAnimation").GetComponent<Image>();
        // 라운드 당 게임 점수 저장
        _acquisitionScoreList = new List<int>();
        // 고양이 모션
        _catMotionControl = GameObject.FindGameObjectWithTag("UICatMotion").GetComponent<Animator>();
        // 현재 아이템 상황
        _itemSlots = new List<SlotObj>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("UIItemUseSlotObj"))
        {
            _itemSlots.Add(go.GetComponent<SlotObj>());
        }
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            _itemSlots[i].InitDataSet(UserInformManager._instance._equipItems[i]);
        }

        // 참조한 컴포턴트 초기값 설정
        _cards = new Dictionary<int, CardObj>();
        _roundMarkers = new Dictionary<int, RoundMarkerObj>();
        _fireAnimation.enabled = false;
        _roundCount = 0;
        _txtAcquisionScore.text = "0";
        _barTimer.value = 1;
        SettingRoundMarkers();

        _wndGMsg.SendGameMessage("Ready~!!", Color.red);
    }

    public IEnumerable ProgReady()
    {
        _nowProgress = eProgstate.Ready;

        _processMsgBox.OpenBox("Ready~!!");

        yield return SettingCards();

        yield return new WaitForSeconds(2.0f);
        ProgStart();
    }
    public void ProgStart()
    {
        _nowProgress = eProgstate.Start;
        _processMsgBox.OpenBox("GameStart!!!");
        _ctrlGenFish.FishNumberSetting(_roundFish);

        _fireAnimation.enabled = true;
        _checkTime = 0;
    }
    public IEnumerator ProgGeneratingNum()
    {
        _nowProgress = eProgstate.GeneratingNum;
        if(_roundCount > 0)
            _ctrlGenFish.DestroyAll(_acquisitionScoreList[_roundCount-1]);
        _checkTime = 0;
        _roundCount++;
        _barTimer.value = 1;

        _wndGMsg.SendGameMessage(_roundCount + " 라운드", Color.red);


        foreach (CardObj obj in _cards.Values)
        {
            yield return obj.ResetCard();
        }

        yield return new WaitForSeconds(1.0f);

        _processMsgBox.CloseBox();
        ChangeCatMotion(eRoundResultType.Idle);

        GenerationFish();
        ProgPlay();
    }
    public void ProgPlay()
    {
        _nowProgress = eProgstate.Play;
        EffectManager._instance.ShowEffect(ePrefabEffect.RoundCheck, _roundMarkers[_roundCount].transform.position);
        Destroy(_roundMarkers[_roundCount].gameObject);

        _checkTime = _limitSelectTime;
        _wndGMsg.SendGameMessage("숫자를 맞추세요");
    }
    public void ProgEnd()
    {
        _nowProgress = eProgstate.End;
        SoundManager._instance.OffBGM();

        _fireAnimation.enabled = false;
        _processMsgBox.OpenBox("Game Over");
        _wndGMsg.SendGameMessage("Game Over");
    }
    public void ProgResult()
    {
        _processMsgBox.CloseBox();
        _nowProgress = eProgstate.Result;
        ChangeCatMotion(eRoundResultType.Idle);
        _catMotionControl.SetInteger("eRoundResultType", (int)eRoundResultType.Idle);
        _checkTime = 0;

        PopupManager._instance.OpenPopup(ePrefabPopup.ResultWnd);
        ResultWnd _resultWnd = GameObject.FindGameObjectWithTag("UIResultWnd").GetComponent<ResultWnd>();
        StartCoroutine(_resultWnd.ShowResultSlow(_acquisitionScoreList, _totalGeneratePoint, int.Parse(_txtAcquisionScore.text)));
    }
    //===================================================================


    public void OnClickSelectBtn()
    {
        if (_nowProgress == eProgstate.Play)
        {
            //체크, 점수 계산
            int answer = 0;
            foreach (CardObj obj in _cards.Values)
            {
                answer += obj.Sum();
            }
            answer = _ctrlGenFish.isMatch(answer);
            if (_isDoublePoint) answer <<= 1;
            _acquisitionScoreList.Add(answer);

            // 결과 행동
            _txtAcquisionScore.text = (int.Parse(_txtAcquisionScore.text) + answer).ToString();
            if (answer == 0)
            {
                _wndGMsg.SendGameMessage("틀렸습니다...");
                _processMsgBox.OpenBox("Fail");
                ChangeCatMotion(eRoundResultType.Fail);
                SoundManager._instance.PlayEffect(eEffectSound.Fail);
            }
            else
            {
                _wndGMsg.SendGameMessage("정답입니다!!!");
                _processMsgBox.OpenBox("Success");
                ChangeCatMotion(eRoundResultType.Success);
                SoundManager._instance.PlayEffect(eEffectSound.Correct);
            }
            RoundFinish();
        }
    }
    public void OnClickItem(string type)
    {
        if (type.Equals(eItemType.InitTime.ToString()))
        {
            SoundManager._instance.PlayEffect(eEffectSound.InitTimeItemUse);
            EffectManager._instance.ShowEffect(ePrefabEffect.InitTime, _ctrlGenFish.transform.position);
            _checkTime = _limitSelectTime;
        }
        else if (type.Equals(eItemType.ForHint.ToString()))
        {
            int hintNum = _ctrlGenFish.FindNum(_roundFish);
            int i = 1;

            while(hintNum > 0)
            {
                if ((hintNum & 1) == 1) _cards[i].HintNumber();
                hintNum >>= 1;
                i++;
            }
        }
        else if (type.Equals(eItemType.DoublePoint.ToString()))
        {
            SoundManager._instance.PlayEffect(eEffectSound.DoublePointItemUse);
            EffectManager._instance.ShowEffect(ePrefabEffect.DoublePoint, _ctrlGenFish.transform.position);
            _isDoublePoint = true;
        }
        else if (type.Equals(eItemType.SlowTime.ToString()))
        {
            SoundManager._instance.PlayEffect(eEffectSound.SlowTimeItemUse);
            EffectManager._instance.ShowEffect(ePrefabEffect.SlowTime, transform.position);
            _isSlowTime = true;
        }
        else 
        {
        }
        _wndGMsg.SendGameMessage(type + "를 사용하셨습니다");
    }
}
