using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Clips")]
    public AudioClip menuBGM;
    public AudioClip gameBGM;

    private AudioSource audioSource;

    private void Awake()
    {
        // 保证单例并在场景切换时不销毁
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayMenuBGM(); // 主菜单自动播放
    }

    public void PlayMenuBGM()
    {
        PlayBGM(menuBGM);
    }

    public void PlayGameBGM()
    {
        PlayBGM(gameBGM);
    }

    private void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
