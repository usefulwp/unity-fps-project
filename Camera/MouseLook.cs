using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一人称
/// </summary>
public class MouseLook : MonoBehaviour
{  
    
    private Transform cameraTrans;
    private Vector3 cameraRotation;

  
    public Transform fpsTransform;
    public float mouseSensitivity;//鼠标灵敏度
    public Vector2 verticalMaxMinAngle;//垂直方向最大最小旋转角度

    public AnimationCurve RecoilCurve;
    public Vector2 RecoilRange;
    public float RecoilFadeOutOfTime=0.3f;
    private float currentRecoilTime;
    private Vector2 currentRecoil;
    private CameraSpring cameraSpring;
    // Start is called before the first frame update
    void Start()
    {
        cameraTrans = transform;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        cameraRotation.y += x* mouseSensitivity;
        cameraRotation.x -= y*mouseSensitivity;
        cameraRotation.z = 0;

        cameraRotation.y += currentRecoil.y;//后座力效果
        cameraRotation.x -= currentRecoil.x;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x, verticalMaxMinAngle.x, verticalMaxMinAngle.y);
        cameraTrans.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y ,0);
        fpsTransform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);

        CalculateRecoilOffset();
    }
    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float fraction = currentRecoilTime / RecoilFadeOutOfTime;
        float fractionValue = RecoilCurve.Evaluate(fraction);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, fractionValue);
    }
    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}
