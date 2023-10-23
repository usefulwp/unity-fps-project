using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FoldBtn : MonoBehaviour
{
    private bool isClickFoldBtn=true;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickFoldBtn);
    }
    void OnClickFoldBtn()
    {
        if (isClickFoldBtn)
        {
            FunctionPanel.instance.HidePanel();
            isClickFoldBtn = false;
        }
        else
        {
            FunctionPanel.instance.ShowPanel();
            isClickFoldBtn = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
