using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AsyncLoadScene : MonoBehaviour
{
    public bool IsClickStartBtn = false;
    public AudioSource LoadAudioSource;
    public float FillSpeed=2f;//进度条填充速度
 
    private Text fillText;//异步加载场景
    private Image fillImage;
    private AsyncOperation asyncOperation;
    private GameObject LoadBgGameObject;
    private Button playGameBtn;
    private InputField nameInputField;
    // Start is called before the first frame update
    void Start()
    {
        fillText = transform.Find("LoadBg/FillText").GetComponent<Text>();
        fillText.text = "";
        fillImage = transform.Find("LoadBg/FillImage").GetComponent<Image>();
        LoadBgGameObject = transform.Find("LoadBg").gameObject;
        playGameBtn = transform.Find("PlayGameBtn").GetComponent<Button>();
        playGameBtn.onClick.AddListener(OnClickPlayGameBtn);
        nameInputField = transform.Find("NameInputField").GetComponent<InputField>();

    }
    void OnClickPlayGameBtn()
    {
        if (nameInputField.text!="")
        {
            IsClickStartBtn = true;
            PlayerPrefs.SetString("Name", nameInputField.text);
            LoadBgGameObject.SetActive(true);
            StartCoroutine(StartAsync());
            LoadAudioSource.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (asyncOperation != null)//异步加载逻辑
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, 1, FillSpeed * Time.deltaTime);
            fillText.text = (fillImage.fillAmount * 100).ToString("f1") + "%";
            if (fillImage.fillAmount > 0.99f)
            {
                fillImage.fillAmount = 1;
                fillText.text = "100%";
            }
            if (fillImage.fillAmount == 1)
            {
                asyncOperation.allowSceneActivation = true;
            }
        }
    }
    IEnumerator StartAsync()
    {
        asyncOperation = SceneManager.LoadSceneAsync(2);
        asyncOperation.allowSceneActivation = false;
        yield return asyncOperation;
    }
}
