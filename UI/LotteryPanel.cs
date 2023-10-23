using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LotteryPanel : BasePanel
{
    public static LotteryPanel instance;
    private Button startBtn;
    public Transform HaloTransform;
    public Transform ImageParentTransform;
    private Transform[] rewardSlotTransformArray;

    private bool drawEnd;
    private bool drawWinning;

    private float rewardTime = 0.8f;
    private float rewardTiming = 0;

    private int haloIndex =0;
    private int rewardIndex = 0;
    private bool isOnClickPlaying;//点了抽奖按钮正在抽奖

    private string[] idStrArray;//物品信息ID存储
    protected override void Start()
    {
        base.Start();
        instance = this;
        startBtn = transform.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(OnClickStartBtn);

        rewardSlotTransformArray = new Transform[ImageParentTransform.childCount];
        for (int i = 0; i < ImageParentTransform.childCount; i++)
        {
            rewardSlotTransformArray[i] = ImageParentTransform.GetChild(i);
        }
        rewardTime =0.6f;
        rewardTiming =0;
        drawEnd = false;
        drawWinning = false;
        isOnClickPlaying = false;

        InitRewardInfo();
    }
    void InitRewardInfo()//把奖品id放入数组中
    {
        idStrArray = new string[5];
        idStrArray[0]="1001";
        idStrArray[1]="1002";
        idStrArray[2]="2001";
        idStrArray[3]="2002";
        idStrArray[4]="3001";
    }
    private void Update()
    {
        if (drawEnd) return;
        rewardTiming += Time.deltaTime;
        if (rewardTiming >= rewardTime)
        {
            rewardTiming = 0;
            haloIndex++;
            if (haloIndex >= rewardSlotTransformArray.Length)
            {
                haloIndex = 0;
            }
            SetHaloPos(haloIndex);
        }
    }
    void SetHaloPos(int haloIndex)
    {
        HaloTransform.position = rewardSlotTransformArray[haloIndex].position;
        if (drawWinning && haloIndex == rewardIndex)
        {
            isOnClickPlaying = false;
            drawEnd = true;
            InventoryPanel.instance.IntoInventory(idStrArray[haloIndex],1);//将奖品放入背包
            Debug.Log("恭喜您中奖，中奖物品索引是：" + haloIndex + "号,该物品已放入背包");
            StartCoroutine(ResumeLotteryInitAnimation());
        }
    }
    IEnumerator ResumeLotteryInitAnimation()//抽奖一次之后恢复抽奖的初始动画
    {
        yield return new WaitForSeconds(3);
        drawEnd = false;
        drawWinning = false;
    }
    void OnClickStartBtn()//开始抽奖的逻辑
    {
        if (!isOnClickPlaying)
        {
            rewardIndex =Random.Range(0,rewardSlotTransformArray.Length);//随机中奖索引
            Debug.Log("开始抽奖,本次抽奖的中奖的索引号是：" + rewardIndex);
            isOnClickPlaying = true;
            drawEnd = false;
            drawWinning = false;
            StartCoroutine(StartLottery());
        }
    }
    /// <summary>
    /// 开始抽奖动画
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLottery()
    {
        rewardTime = 0.8f;
        for (int i = 0; i < 7; i++)  //加速
        {
            yield return new WaitForSeconds(0.1f);
            rewardTime -=0.1f;
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++)//减速
        {
            yield return new WaitForSeconds(0.1f);
            rewardTime += 0.1f;
        }
        yield return new WaitForSeconds(1f);
        drawWinning = true;
    }
    public override void Translate()
    {
        base.Translate();
    }
}
