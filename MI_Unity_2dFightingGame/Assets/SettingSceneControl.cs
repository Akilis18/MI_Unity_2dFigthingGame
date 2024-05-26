using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingSceneControl : MonoBehaviour
{
    public Button btnBack;
    public Slider sliderVolume;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        btnBack.onClick.AddListener(ChangeScene);
        sliderVolume.value = Global.VOLUME;
        sliderVolume.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void ChangeVolume(float value)
    {
        Global.VOLUME = value;
        audioSource.volume = Global.VOLUME;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
