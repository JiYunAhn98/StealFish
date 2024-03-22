using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.UI;

public class SettingWnd : Popup
{
    [SerializeField] Slider _totalSoundSlider;
    [SerializeField] Slider _effectSoundSlider;
    [SerializeField] Slider _bgmSoundSlider;

    public override void InitSetting()
    {
        SliderSetting();
        SoundManager._instance.BackupOriginVolume();
    }
    public void ClickOK()
    {
        PopupManager._instance.ClosePopup(ePrefabPopup.SettingWnd);
    }
    public void ClickCancle()
    {
        SoundManager._instance.ReturnOriginVolume();
        PopupManager._instance.ClosePopup(ePrefabPopup.SettingWnd);
    }
    public void SliderSetting()
    {
        _totalSoundSlider.value = SoundManager._instance._totalSoundSize;
        _effectSoundSlider.value = SoundManager._instance._effectSoundSize;
        _bgmSoundSlider.value = SoundManager._instance._bgmSoundSize;
    }
    // slider에 따른 소리 변화
    public void TotalSoundSetting()
    {
        SoundManager._instance.SetTotalSoundSize(_totalSoundSlider.value);
    }
    public void BGMSoundSetting()
    {
        SoundManager._instance.SetBGMSoundSize(_bgmSoundSlider.value);
    }
    public void EffectSoundSetting()
    {
        SoundManager._instance.SetEffectSoundSize(_effectSoundSlider.value);
    }
}
