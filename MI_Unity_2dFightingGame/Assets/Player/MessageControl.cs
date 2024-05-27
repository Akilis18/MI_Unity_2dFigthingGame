using TMPro;
using UnityEngine;

public class MessageControl : MonoBehaviour
{
    private TMP_Text message;
    private GameObject player;
    private float ShowTm = 0f;

    // Start is called before the first frame update
    void Start()
    {
        message = GetComponent<TMP_Text>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowTm > 0f)
        {
            gameObject.transform.localPosition = new Vector2(0f, 1.4f - ShowTm * 0.2f);
            message.color = new Color(1f, 153f/255f, 0f, ShowTm / 3f);
            ShowTm -= Time.deltaTime;
        }
        if (ShowTm < 0f)
        {
            ShowTm = 0f;
            hideMessage();
        }
    }

    public void showMessage()
    {
        message.enabled = true;
        ShowTm = 3f;
    }

    public void hideMessage()
    {
        message.enabled = false;
    }
}
