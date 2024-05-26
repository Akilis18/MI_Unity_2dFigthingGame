using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneControl : MonoBehaviour
{
    public Button btnSetting;
    public Button btnStart;
    public Button btnClose;

    void Start()
    {
        btnSetting.onClick.AddListener(ChangeSettingScene);
        btnStart.onClick.AddListener(ChangeScene);
        btnClose.onClick.AddListener(CloseGame);
    }

    private void ChangeSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
