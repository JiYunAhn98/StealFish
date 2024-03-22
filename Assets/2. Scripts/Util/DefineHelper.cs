using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
    // SceneName
    public enum eSceneName
    {
        Logo,
        Lobby,
        Ingame,

        Count
    }

    // Prefabs
    public enum ePrefabObj
    {
        CardObj,
        FishObj,
        RoundMarker,
        TextLine,

        Count
    }
    public enum ePrefabEffect
    {
        SpawnFish,
        ForHint,
        SlowTime,
        DoublePoint,
        InitTime,
        ResultSuccess,
        ResultFail,
        A,
        B,
        C,
        F,
        Click,
        RoundCheck,
        FailStealFish,
        SuccessStealFish,

        Count
    }
    public enum ePrefabPopup
    {
        SettingWnd,
        ResultWnd,
        ReadyWnd,
        CheckWnd,
        HowToPlayWnd,

        Count
    }

    // Informations
    public enum eProgstate
    {
        Init,
        Ready,
        Start,
        GeneratingNum,
        Play,
        End,
        Result,

        Count
    }
    public enum eItemType
    {
        InitTime,
        SlowTime,
        ForHint,
        DoublePoint,


        Count,
    }
    public enum eRankSprite
    {
        A       =0,
        B,
        C,
        F,

        Count
    }
    public enum eRoundResultType
    {
        Idle,
        Success,
        Fail,
        TimeOut,

        Count
    }

    // 환경설정
    public enum eEffectSound
    {
        CardGenerate,
        //FishGenerate,
        //Destroy,
        Correct,
        Fail,
        CardTurn,
        InitTimeItemUse,
        SlowTimeItemUse,
        ForHintItemUse,
        DoublePointItemUse,
        MouseClick,
        Result,

        Count
    }

    // Sturct Information
    public struct SlotItemInfo
    {
        public Sprite _icon;
        public int _count;
        public float _coolTime;

        public SlotItemInfo(Sprite icon, int cnt, float time)
        {
            _icon = icon;
            _count = cnt;
            _coolTime = time;
        }
    }
}

