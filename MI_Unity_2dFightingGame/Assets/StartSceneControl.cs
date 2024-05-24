using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneControl : MonoBehaviour
{
    public Button btnStart;
    public Button btnClose;

    void Start()
    {
        btnStart.onClick.AddListener(ChangeScene);
        btnClose.onClick.AddListener(CloseGame);
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
