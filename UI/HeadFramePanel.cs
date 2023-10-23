using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeadFramePanel : MonoBehaviour
{
    private Text playerName;
    private PlayerStatus playerStatus;
    private Text hpText;
    private Image hpImage;
    // Start is called before the first frame update
    void Start()
    {
        playerName = transform.Find("Name/Content").GetComponent<Text>();
        playerName.text = PlayerPrefs.GetString("Name");
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        hpText = transform.Find("HpBg/HpImage/Text").GetComponent<Text>();
        hpImage= transform.Find("HpBg/HpImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = ((playerStatus.CurrentHp / playerStatus.TotalHp)*100).ToString("f0")+"%";
        hpImage.fillAmount = playerStatus.CurrentHp / playerStatus.TotalHp;
    }
}
