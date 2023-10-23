using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfoPanel : MonoBehaviour
{
    public static ItemInfoPanel instance;
    private Text IDtext;
    private Text ItemTypeText;
    private Text NameText;
    private Text PriceText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        this.gameObject.SetActive(false);
        IDtext = transform.Find("ID/Text").GetComponent<Text>();
        ItemTypeText = transform.Find("ItemType/Text").GetComponent<Text>();
        NameText = transform.Find("Name/Text").GetComponent<Text>();
        PriceText = transform.Find("Price/Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowInfo(string idStr)
    {
        transform.position = Input.mousePosition + new Vector3(100,-80,0);
        gameObject.SetActive(true);


        IDtext.text = ShopPanel.instance.jTokenDic[idStr]["ID"].ToString();
        ItemTypeText.text = ShopPanel.instance.jTokenDic[idStr]["ItemType"].ToString();
        NameText.text = ShopPanel.instance.jTokenDic[idStr]["Name"].ToString();
        PriceText.text = ShopPanel.instance.jTokenDic[idStr]["Price"].ToString();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
