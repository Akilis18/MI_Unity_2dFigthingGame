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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeVolumeDec();

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ChangeVolumeInc();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void ChangeVolumeInc()
    {
        Global.VOLUME += 0.2f;
        if (Global.VOLUME > 1f)
            Global.VOLUME = 1f;

        sliderVolume.value = Global.VOLUME;
        audioSource.volume = Global.VOLUME;
        audioSource.Play();
    }

    private void ChangeVolumeDec()
    {
        Global.VOLUME -= 0.2f;
        if (Global.VOLUME < 0f)
            Global.VOLUME = 0f;

        sliderVolume.value = Global.VOLUME;
        audioSource.volume = Global.VOLUME;
        audioSource.Play();
    }
}
