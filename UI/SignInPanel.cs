using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
public class SignInPanel : BasePanel
{
    public static SignInPanel instance;
    public GameObject VideoPlayer;
    private Button signInBtn;
    private Button advertisingBtn;
    private List<GameObject> wrongIconGameObjects = new List<GameObject>(); 
    private int index;
    private Sprite[] sprites;
    private List<Text> rewardTexts=new List<Text>();
    private PlayerStatus playerStatus;
    private DateTime today;//今天日期
    private DateTime lastDay;//上次领取时间
    private TimeSpan Interval;//时间间隔
    private Text intervalText;
    private bool isShowTime;
    private const string SignDataPrefs="lastDay";
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        instance = this;
        signInBtn = transform.Find("SignInBtn").GetComponent<Button>();
        advertisingBtn = transform.Find("AdvertisingBtn").GetComponent<Button>();
        signInBtn.onClick.AddListener(OnClickSignInBtn);
        advertisingBtn.onClick.AddListener(OnClickAdvertingBtn);

        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        sprites = Resources.LoadAll<Sprite>("GUI");
        for (int i = 1; i <= 7; i++)
        {
            wrongIconGameObjects.Add(transform.Find("Text" + i + "/wrongIcon").gameObject);
            rewardTexts.Add(transform.Find("Text" + i + "/Image/Text").GetComponent<Text>());
        }

        switch (DateTime.Now.DayOfWeek)
        {
            case DayOfWeek.Monday:
                index = 0;
                break;
            case DayOfWeek.Tuesday:
                index = 1;
                break;
            case DayOfWeek.Wednesday:
                index = 2;
                break;
            case DayOfWeek.Thursday:
                index = 3;
                break;
            case DayOfWeek.Friday:
                index = 4;
                break;
            case DayOfWeek.Saturday:
                index = 5;
                break;
            case DayOfWeek.Sunday:
                index = 6;
                break;
        }
        wrongIconGameObjects[index].GetComponent<Image>().enabled = false;


        intervalText = transform.Find("SignInRemainTime").GetComponent<Text>();

        today = DateTime.Now;
        //lastDay = DateTime.Parse(PlayerPrefs.GetString(SignDataPrefs, DateTime.MinValue.ToString()));
        lastDay = DateTime.Parse(DateTime.MinValue.ToString());
        if (IsOneDay())//是否可以签到逻辑
        {
            signInBtn.interactable = true;
        }
        else
        {
            isShowTime = true;
            signInBtn.interactable = false;
        }
    }
    private void Update()
    {
        if (isShowTime)
        {
            Interval = DateTime.Now.AddDays(1).Date - DateTime.Now;
            string Intervalstr = string.Format("{0:D2}:{1:D2}:{2:D2}", Interval.Hours, Interval.Minutes, Interval.Seconds);
            intervalText.text = Intervalstr;
            if (Interval.Hours == 0 && Interval.Minutes == 0 && Interval.Seconds == 0)
            {
                    signInBtn.interactable = true;
                    isShowTime = false;
            }
        }
    }
    void OnClickSignInBtn()//签到要处理的逻辑
    {
        isShowTime = true;
        wrongIconGameObjects[index].GetComponent<Image>().enabled = true;
        wrongIconGameObjects[index].GetComponent<Image>().sprite = sprites[59];
        playerStatus.GoldNum=playerStatus.GoldNum+ int.Parse(rewardTexts[index].text);//奖励
        signInBtn.interactable = false;

        lastDay = DateTime.Now;
        PlayerPrefs.SetString(SignDataPrefs, lastDay.ToString());

    }
    bool IsOneDay()
    {
        if (today.Year == lastDay.Year && today.Month == lastDay.Month && today.Day == lastDay.Day)
        {
            return false;
        }
        if (DateTime.Compare(lastDay, today) < 0)
        {
            return true;
        }
        return false;
    }
    void OnClickAdvertingBtn()
    {
        VideoPlayer.GetComponent<RawImage>().enabled = true;
        SettingPanel.instance.MuteMusic();
        VideoPlayer.GetComponent<VideoPlayer>().Play();
        VideoPlayer.GetComponent<VideoPlayer>().loopPointReached += Completed;
    }
    private void Completed(VideoPlayer videoPlayer)//视频播放完毕处理的事情
    {
        VideoPlayer.GetComponent<RawImage>().enabled = false;
        Debug.Log("视频播放完毕,当日奖励已获得");
        SettingPanel.instance.ResumeMusic();
        advertisingBtn.interactable = false;
        playerStatus.GoldNum = playerStatus.GoldNum + int.Parse(rewardTexts[index].text);
    }
    public override void Translate()
    {
        base.Translate();
    }

}
