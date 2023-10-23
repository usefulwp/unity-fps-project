using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CanvasLogic : MonoBehaviour
{
    public Vector3 OriginalCameraPosition;
    [Tooltip("the speed of x axis")]
    public float X_Speed;

  
    private Camera mainCamera;
    private CanvasGroup whiteScreenOfCanvasGroup;
    private Text nameText;
    private InputField passwordInputField;
    private Button loginBtn;
    private bool isPassing;//密码是否正确


    // Start is called before the first frame update
    void Start()
    {
        whiteScreenOfCanvasGroup = transform.Find("WhiteScreen").GetComponent<CanvasGroup>();
        nameText = transform.Find("Login/UserName/Name").GetComponent<InputField>().textComponent;
        passwordInputField = transform.Find("Login/UserPassWord/PassWord").GetComponent<InputField>();
        loginBtn = transform.Find("Login/LoginBtn").GetComponent<Button>();

        loginBtn.onClick.AddListener(OnLoginbtn);

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

       
    }
    void OnLoginbtn()
    {
        if (nameText.text.Equals("wp@qq.com") && passwordInputField.text.Equals("123"))
        {
            isPassing = true;
        }
        if (isPassing)
        {
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            Debug.Log("用户名或者密码错误!!!!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (whiteScreenOfCanvasGroup.alpha <= 0.05f)
        {
            whiteScreenOfCanvasGroup.alpha = 0;
        }
        whiteScreenOfCanvasGroup.alpha = Mathf.Lerp(whiteScreenOfCanvasGroup.alpha, 0, 0.6f*Time.deltaTime);

        
        if (whiteScreenOfCanvasGroup.alpha <0.8)
        {
            MoveCamera();
        }
      
    }
    
    
    void MoveCamera()   //把相机从后往前推  制作开场动画
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, OriginalCameraPosition,X_Speed*Time.deltaTime);
    }
   
}
