using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.SceneManagement;
public class SceneControlManager : MonoSingleTon<SceneControlManager>
{
    [SerializeField] eSceneName _startScene;
    eSceneName _nowScene;
    eSceneName _nextScene;

    void Awake()
    {
        _instance.StartLoad();
    }
    public void StartLoad()
    {
        PopupManager._instance.Initialize();
        SoundManager._instance.Initialize();
        TableManager._instance.CreateFiles();
        EffectManager._instance.InitializePrefab();

        SoundManager._instance.PlayBGM(_startScene);
    }
    public void LoadScene()
    {
        _nowScene = _nextScene;
        PopupManager._instance.LoadOtherScene();
        SceneManager.LoadSceneAsync(_nowScene.ToString());
        SoundManager._instance.PlayBGM(_nowScene);
    }
    public void SettingNextScene(eSceneName scene)
    {
        _nextScene = scene;
    }
}
