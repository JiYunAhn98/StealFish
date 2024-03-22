using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoSingleTon<SoundManager>
{
    // resource 참조
    AudioClip[] _effectAudioSources;
    AudioClip[] _bgmAudioSources;

    // 참조 변수
    AudioSource _bgmSoundPlayer;
    AudioSource _effectSoundPlayer;

    // 소리
    public float _totalSoundSize { get; set; }
    public float _effectSoundSize { get; set; }
    public float _bgmSoundSize { get; set; }

    float _originTotalSoundsize;
    float _originBGMSoundsize;
    float _originEffectSoundsize;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlayEffect(eEffectSound.MouseClick);
    }
    // 초기설정
    public void Initialize(float totalSize = 1, float effectSize = 1, float bgmSize = 1)
    {
        if (_effectSoundPlayer != null || _bgmSoundPlayer != null) return;
        _effectSoundPlayer = new GameObject("EffectSoundPlayer").AddComponent<AudioSource>();
        _effectSoundPlayer.transform.parent = gameObject.transform;
        _bgmSoundPlayer = new GameObject("BGMSoundPlayer").AddComponent<AudioSource>();
        _bgmSoundPlayer.transform.parent = gameObject.transform;

        _effectAudioSources = new AudioClip[(int)eEffectSound.Count];
        for (int i = 0; i < (int)eEffectSound.Count; i++)
        {
            _effectAudioSources[i] = Resources.Load("Sound/Effect/" + ((eEffectSound)i).ToString()) as AudioClip;
        }
        _bgmAudioSources = new AudioClip[(int)eSceneName.Count];
        for (int i = 0; i < (int)eSceneName.Count; i++)
        {
            _bgmAudioSources[i] = Resources.Load("Sound/BGM/" + ((eSceneName)i).ToString()) as AudioClip;
        }
        _totalSoundSize = totalSize;
        _originTotalSoundsize = totalSize;
        _effectSoundSize = effectSize;
        _originBGMSoundsize = bgmSize;
        _bgmSoundSize = bgmSize;
        _originEffectSoundsize = effectSize;
    }
    // setting wnd가 켜졌을 때 초기 음량으로 돌리기 위한 origin Volume manage
    public void BackupOriginVolume()
    {
        _originTotalSoundsize = _totalSoundSize;
        _originBGMSoundsize = _bgmSoundSize;
        _originEffectSoundsize = _effectSoundSize;
    }
    public void ReturnOriginVolume()
    {
        _totalSoundSize = _originTotalSoundsize;
        _bgmSoundSize = _originBGMSoundsize;
        _effectSoundSize = _originEffectSoundsize;

        SetTotalSoundSize(_totalSoundSize);
        SetEffectSoundSize(_bgmSoundSize);
        SetBGMSoundSize(_effectSoundSize);
    }
    // 소리 크기를 Setting
    public void SetTotalSoundSize(float size)
    {
        _totalSoundSize = size;
        _bgmSoundPlayer.volume = _bgmSoundSize * _totalSoundSize;
        _effectSoundPlayer.volume = _effectSoundSize * _totalSoundSize;
    }
    public void SetEffectSoundSize(float size)
    {
        _effectSoundSize = size;
        _effectSoundPlayer.volume = _effectSoundSize * _totalSoundSize;
    }
    public void SetBGMSoundSize(float size)
    {
        _bgmSoundSize = size;
        _bgmSoundPlayer.volume = _bgmSoundSize * _totalSoundSize;
    }
    // 소리를 Play
    public void PlayBGM(eSceneName type, bool _isLoop = true)
    {
        _bgmSoundPlayer.clip = _bgmAudioSources[(int)type];
        _bgmSoundPlayer.loop = _isLoop;

        _bgmSoundPlayer.Play();
    }
    public void PlayEffect(eEffectSound type)
    {
        _effectSoundPlayer.PlayOneShot(_effectAudioSources[(int)type]);
    }
    // 소리를 Off
    public void OffBGM()
    {
        if(_bgmSoundPlayer.isPlaying) _bgmSoundPlayer.Pause();
    }
}
