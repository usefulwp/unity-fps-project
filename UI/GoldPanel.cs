using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoldPanel : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private Text goldNumText;
    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        goldNumText = transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        goldNumText.text = playerStatus.GoldNum.ToString();
    }
}
