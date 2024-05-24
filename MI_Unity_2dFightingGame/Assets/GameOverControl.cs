using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverControl : MonoBehaviour
{
    public Button btnRetry;
    public Button btnBackToTitle;

    // Start is called before the first frame update
    void Start()
    {
        btnRetry.onClick.AddListener(ChangeScene1);
        btnBackToTitle.onClick.AddListener(ChangeScene2);
    }

    private void ChangeScene1()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ChangeScene2()
    {
        SceneManager.LoadScene("StartScene");
    }
}
