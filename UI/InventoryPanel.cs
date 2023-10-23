using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryPanel : BasePanel
{
    public static InventoryPanel instance;
    private Sprite[] Sprites;
    private GameObject itemPrefab;
    public List<Transform> itemOfParentTransformList;
    private int latticeCount;
    private List<string> itemIdList=new List<string>();
    protected override void Start()
    {
        base.Start();
        instance = this;
        Sprites = Resources.LoadAll<Sprite>("Orange theme spritesheet 2");
        itemPrefab = Resources.Load<GameObject>("Item");
        latticeCount =20;
        InitItemOfParentList();
    }
    void InitItemOfParentList()
    {
        for (int i = 1; i <= latticeCount; i++)
        {
            itemOfParentTransformList.Add(transform.Find("ItemManager/Slot"+i));
        }
    }
    public override void Translate()
    {
        base.Translate();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Translate();
        }
    }
    public void IntoInventory(string idStr,int itemCount=1)
    {
        if (itemIdList.Count == 0)
        {
            GameObject go = Instantiate(itemPrefab);
            SetParentAndPosition(go);
            go.GetComponent<Item>().SetId(idStr);
            int iconIndex = int.Parse(ShopPanel.instance.jTokenDic[idStr]["iconIndex"].ToString());
            go.GetComponent<Image>().sprite = Sprites[iconIndex];
            go.transform.Find("NumberText").GetComponent<Text>().text = itemCount.ToString();
            itemIdList.Add(idStr);
            return;
        }
        bool isExist=false;
        for (int j=0;j<itemIdList.Count;j++)
        {
            if (idStr == itemIdList[j])
            {
                isExist = true;
                if (idStr == itemIdList[j])
                {

                    for (int i = 0; i < itemOfParentTransformList.Count; i++)
                    {
                        if (itemOfParentTransformList[i].GetComponentInChildren<Item>() != null)
                        {
                            if (itemOfParentTransformList[i].GetComponentInChildren<Item>().idStr == idStr)//找到空位
                            {
                                GameObject go = itemOfParentTransformList[i].GetComponentInChildren<Item>().gameObject;
                                int preNumber = int.Parse(go.transform.Find("NumberText").GetComponent<Text>().text);
                                go.transform.Find("NumberText").GetComponent<Text>().text = (preNumber + itemCount).ToString();
                                return;
                            }
                        }
                    }

                }
            }
        }
        if (isExist == false)
        {
            GameObject go = Instantiate(itemPrefab);
            SetParentAndPosition(go);
            go.GetComponent<Item>().SetId(idStr);
            int iconIndex = int.Parse(ShopPanel.instance.jTokenDic[idStr]["iconIndex"].ToString());
            go.GetComponent<Image>().sprite = Sprites[iconIndex];
            go.transform.Find("NumberText").GetComponent<Text>().text = itemCount.ToString();
            itemIdList.Add(idStr);
            return;
        }

    }
    void SetParentAndPosition(GameObject go)
    {
        for (int i = 0; i < itemOfParentTransformList.Count; i++)
        {
            if (itemOfParentTransformList[i].GetComponentInChildren<Item>()==null)//找到空位
            {
                go.transform.SetParent(itemOfParentTransformList[i]);
                break;
            }
        }    
        go.transform.localPosition = new Vector3(0, 0.9f, 0);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
}
