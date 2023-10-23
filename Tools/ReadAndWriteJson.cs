using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
[System.Serializable]
public class Message
{
    public string Name;
    public string CreateTime;
}
public class ReadAndWriteJson : MonoBehaviour
{
   
    public InputField NameInputField;
    public AsyncLoadScene AsyncLoadScene;
    public Message savemessage;//要保存的对象
    private Message readmessage;//要读取的对象
    private string jsonPath;
    // Start is called before the first frame update
    void Start()
    {
        jsonPath = Application.streamingAssetsPath + "/createInfo.json";
 
    }

    void SaveJson()
    {
        if (!File.Exists(jsonPath))
        {
            File.Create(jsonPath);
        }
        string  jsonstr=JsonUtility.ToJson(savemessage, true);
        File.WriteAllText(jsonPath, jsonstr);
        Debug.Log("写入json成功");
    }
    void ReadJson()
    {
        string jsonStr = File.ReadAllText(jsonPath);
        readmessage = JsonUtility.FromJson<Message>(jsonStr);
        Debug.Log(readmessage.Name+"\t");
        //Debug.Log("读出成功");
    }
    // Update is called once per frame
    void Update()
    {
        savemessage.Name = NameInputField.text;
        savemessage.CreateTime = System.DateTime.Now.ToString();
        Archieve();
    }
    void Archieve()//把输入的名字进行保存
    {
        if (AsyncLoadScene.IsClickStartBtn)
        { 
            SaveJson();
            Destroy(GetComponent<ReadAndWriteJson>());
        }
    }
}
