using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip[] bgm;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = bgm[0];
        source.volume = 0.3f;
        source.loop = true;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
