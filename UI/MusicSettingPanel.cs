using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MusicSettingPanel : BasePanel
{
    public static MusicSettingPanel instance;
    public MusicData MusicData;
    public Sprite NormalPauseSprite;
    public Sprite PressedPauseSprite;

    private Text musicName;
    private AudioSource audioSource;
    private bool isPause=true;//是否暂停
    private Image pauseImage;
    private int musicIndex;//音乐下标
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        instance = this;
        MusicData = Resources.Load<MusicData>("Data/Music Data");
        musicName = transform.Find("MusicName").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
      
        transform.Find("LeftBtn").GetComponent<Button>().onClick.AddListener(OnClickLeftBtn);
        transform.Find("RightBtn").GetComponent<Button>().onClick.AddListener(OnClickRightBtn);
        transform.Find("PauseBtn").GetComponent<Button>().onClick.AddListener(OnClickPauseBtn);
        musicName.text = "穿越火线主题曲";
        musicIndex = 1;//因为穿越火线的音乐的下标是1
        pauseImage = transform.Find("PauseBtn").GetComponent<Image>();
    }
    public override void Translate()
    {
        base.Translate();
    }
    void OnClickPauseBtn()
    {
        if (isPause)
        {
            pauseImage.sprite = PressedPauseSprite;
            audioSource.Pause();
            isPause = false;
        }
        else
        {
            pauseImage.sprite = NormalPauseSprite;
            audioSource.Play();
            isPause = true;
        }
    }
    void OnClickLeftBtn()
    {
        pauseImage.sprite = NormalPauseSprite;
        if (musicIndex == 0)
        {
            musicIndex = 4;
            audioSource.clip = MusicData.Musics[musicIndex].AudioClip;
            musicName.text = MusicData.Musics[musicIndex].Name;
            audioSource.Play();
        }
        else
        {
            musicIndex--;
            audioSource.clip = MusicData.Musics[musicIndex].AudioClip;
            musicName.text = MusicData.Musics[musicIndex].Name;
            audioSource.Play();
        }
    }
    void OnClickRightBtn()
    {
        pauseImage.sprite = NormalPauseSprite;
        if (musicIndex == 4)
        {
            musicIndex = 0;
            audioSource.clip = MusicData.Musics[musicIndex].AudioClip;
            musicName.text = MusicData.Musics[musicIndex].Name;
            audioSource.Play();
        }
        else
        {
            musicIndex++;
            audioSource.clip = MusicData.Musics[musicIndex].AudioClip;
            musicName.text = MusicData.Musics[musicIndex].Name;
            audioSource.Play();
        }
       
    }
  
}
