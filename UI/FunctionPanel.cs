using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FunctionPanel : MonoBehaviour
{
    public static FunctionPanel instance;
    public Button MusicSettingBtn;
    public Button SignBtn;
    public Button SettingBtn;
    public Button ShopBtn;
    public Button LotteryBtn;
    public AudioSource audioSource;
    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        MusicSettingBtn.onClick.AddListener(OnClickMusicSettingBtn);
        SignBtn.onClick.AddListener(OnClickSignBtn);
        SettingBtn.onClick.AddListener(OnClickSettingBtn);
        ShopBtn.onClick.AddListener(OnClickShopBtn);
        LotteryBtn.onClick.AddListener(OnClickLottertBtn);

        tweener = transform.DOLocalMoveY(1200f, 0.5f);
        tweener.SetAutoKill(false);
        tweener.Pause();
    }
    void OnClickLottertBtn()
    {
        audioSource.Play();
        LotteryPanel.instance.Translate();
    }
    void OnClickMusicSettingBtn()//设置音乐
    {
        audioSource.Play();
        MusicSettingPanel.instance.Translate();
    }
    void OnClickSignBtn()
    {
        audioSource.Play();
        SignInPanel.instance.Translate();
    }
    void OnClickSettingBtn()
    {
        audioSource.Play();
        SettingPanel.instance.Translate();
    }
    void OnClickShopBtn()
    {
        audioSource.Play();
        ShopPanel.instance.Translate();
    }

    public void HidePanel()
    {
        tweener.PlayForward();
    }
    public void ShowPanel()
    {
        tweener.PlayBackwards();
    }
}
