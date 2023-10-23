using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
public class ShopPanel : BasePanel
{
    public static ShopPanel instance;
    private TextAsset textAsset;
    public Transform Content { get; private set; }
    private Text buyNumberText;
    private Button confirmBtn;
    private PlayerStatus playerStatus;
    public string IDTextLog;
    private GameObject buyBtnContainer;
    public Dictionary<string, JToken> jTokenDic = new Dictionary<string, JToken>();
    private GameObject latticePrefab;
    protected override void Start()
    {
        base.Start();
        instance = this;
        latticePrefab = Resources.Load<GameObject>("lattice");
        Content = transform.Find("Scroll View/Viewport/Content");
        textAsset = Resources.Load<TextAsset>("Json/shop");
        Init();
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        buyBtnContainer = transform.Find("Scroll View/BuyBtnContainer").gameObject;
        buyNumberText = transform.Find("Scroll View/BuyBtnContainer/InputField/Text").GetComponent<Text>();
        confirmBtn = transform.Find("Scroll View/BuyBtnContainer/ConfirmBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
    }
    void OnClickConfirmBtn()
    {       
        int needGoldNum = int.Parse(buyNumberText.text) * int.Parse(jTokenDic[IDTextLog]["Price"].ToString());
        if (playerStatus.GoldNum >= needGoldNum)
        {
            playerStatus.GoldNum = playerStatus.GoldNum - needGoldNum;
            Debug.Log("购买成功！！！");
            InventoryPanel.instance.IntoInventory(IDTextLog, int.Parse(buyNumberText.text));//将购买的物品放入背包
            Debug.Log("购买的物品已经放入背包!!!");
            buyBtnContainer.SetActive(false);
            buyNumberText.transform.parent.GetComponent<InputField>().text = "";
        } 
        else
        {
            Debug.LogWarning("金币不足！！！");
            buyBtnContainer.SetActive(false);
            buyNumberText.transform.parent.GetComponent<InputField>().text = "";
        }  
    }
    public override void Translate()
    {
        base.Translate();     
    }
    private void Init()
    {
        GameObject go = null;
        Lattice lattice = null;
        JArray jArray = null;   
        jArray = JArray.Parse(textAsset.text);
        foreach (JToken jToken in jArray)
        {
            jTokenDic.Add(jToken["ID"].ToString(), jToken);
        }
        int i = 0;
        for (;i< 5;i++)
        {
           go= Instantiate(latticePrefab);
           go.transform.SetParent(Content);
           go.transform.localScale = new Vector3(1, 1, 1);
           lattice = go.GetComponent<Lattice>();

           lattice.IDText.text = jArray[i]["ID"].ToString();
           lattice.NameText.text = jArray[i]["Name"].ToString();
           lattice.ItemTypeText.text = jArray[i]["ItemType"].ToString();
           lattice.PriceText.text = jArray[i]["Price"].ToString();
           lattice.image.sprite= Resources.LoadAll<Sprite>("Orange theme spritesheet 2")[int.Parse(jArray[i]["iconIndex"].ToString())];
        }
    }
}
