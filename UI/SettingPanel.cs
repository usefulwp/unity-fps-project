using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Weapon;
public class SettingPanel : BasePanel
{
    public static SettingPanel instance;
    public List<AudioSource> BgmAudioSources;
    public List<AudioSource> SoundAudioSources;
    public List<AssaultRifle> AssaultRifles;
    public Sprite NormalMusicSprite;
    public Sprite PressedMusicSprite;
    public Sprite NormalSoundSprite;
    public Sprite PressedSoundSprite;
    private bool isClickMusicIcon=true;
    private bool isClickSoundIcon=true;
    private Image musicImage;
    private Image soundImage;
    private Scrollbar musicScrollbar;
    private Text musicScrollbarText;
    private Scrollbar soundScrollbar;
    private Text soundScrollbarText;
    protected override void Start()
    {
        base.Start();
        instance = this;
        transform.Find("MusicBtn").GetComponent<Button>().onClick.AddListener(OnClickMusicBtn);
        transform.Find("SoundBtn").GetComponent<Button>().onClick.AddListener(OnClickSoundBtn);
        musicImage = transform.Find("MusicBtn").GetComponent<Image>();
        soundImage = transform.Find("SoundBtn").GetComponent<Image>();

        InitSoundAudioSources();

        musicScrollbar = transform.Find("MusicScrollbar").GetComponent<Scrollbar>();
        musicScrollbarText = transform.Find("MusicScrollbar/Text").GetComponent<Text>();
        soundScrollbar= transform.Find("SoundScrollbar").GetComponent<Scrollbar>();
        soundScrollbarText = transform.Find("SoundScrollbar/Text").GetComponent<Text>();
        musicScrollbarText.text = musicScrollbar.value*100 + "%";
        soundScrollbarText.text = soundScrollbar.value*100+"%";
    }
    private void Update()
    {
        SyncScrollbarAndText();
    }
    void SyncScrollbarAndText()//同步滑动条数值与文本对应   同时更改音量大小
    {
        musicScrollbarText.text = (musicScrollbar.value * 100).ToString("f0") + "%";
        foreach (AudioSource bgmAudioSource in BgmAudioSources)
        {
            bgmAudioSource.volume=musicScrollbar.value;
        }
        
        soundScrollbarText.text = (soundScrollbar.value * 100).ToString("f0") + "%";
        foreach (AudioSource soundAudioSource in SoundAudioSources)
        {
            soundAudioSource.volume = soundScrollbar.value;
        }

    }
    void InitSoundAudioSources()
    {
        int i;
        for (i=0;i<AssaultRifles.Count; i++)
        {
            SoundAudioSources.AddRange(AssaultRifles[i].GetComponents<AudioSource>());
        }   
    }
    void OnClickMusicBtn()
    {
        if (isClickMusicIcon)
        {
            musicImage.sprite = PressedMusicSprite;
            isClickMusicIcon = false;
            foreach (AudioSource audioSource in BgmAudioSources)
            {
                audioSource.mute = true;
            }
        }
        else
        {
            musicImage.sprite = NormalMusicSprite;
            isClickMusicIcon = true;
            foreach (AudioSource audioSource in BgmAudioSources)
            {
                audioSource.mute = false;
            }
        }
    }
    void OnClickSoundBtn()
    {
        if (isClickSoundIcon)
        {
            soundImage.sprite = PressedSoundSprite;
            isClickSoundIcon = false;
            foreach (AudioSource audioSource in SoundAudioSources)
            {
                audioSource.mute = true;
            }
        }
        else
        {
            soundImage.sprite = NormalSoundSprite;
            isClickSoundIcon = true;
            foreach (AudioSource audioSource in SoundAudioSources)
            {
                audioSource.mute = false;
            }
        }
    }
    public void MuteMusic()
    { 
            foreach (AudioSource audioSource in BgmAudioSources)
            {
                audioSource.mute = true;
            }
    }
    public void ResumeMusic()
    {
        foreach (AudioSource audioSource in BgmAudioSources)
        {
            audioSource.mute = false;
        }
    }
    public override void Translate()
    {
        base.Translate();
    }
}
