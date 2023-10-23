using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lattice : MonoBehaviour
{
    public Text IDText;
    public Text NameText;
    public Text ItemTypeText;
    public Text PriceText;
    public Button buyBtn;
    public Image image;
    // Start is called before the first frame update
    void Awake()
    {
        IDText = transform.Find("ID/IDText").GetComponent<Text>();
        NameText = transform.Find("Name/NameText").GetComponent<Text>();
        ItemTypeText = transform.Find("ItemType/ItemTypeText").GetComponent<Text>();
        PriceText = transform.Find("Price/PriceText").GetComponent<Text>();
        image = transform.Find("Image").GetComponent<Image>();
        buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        buyBtn.onClick.AddListener(OnClickBuyBtn);
    }
    void OnClickBuyBtn()
    {
        transform.GetComponentInParent<ShopPanel>().transform.Find("Scroll View/BuyBtnContainer").gameObject.SetActive(true);
        transform.GetComponentInParent<ShopPanel>().IDTextLog = IDText.text;
    }

}
