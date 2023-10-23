using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Item : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public string idStr;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetId(string idStr)
    {
        this.idStr = idStr;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfoPanel.instance.ShowInfo(idStr);
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfoPanel.instance.Hide();
    }
}
