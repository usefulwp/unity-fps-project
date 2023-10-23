using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BasePanel : MonoBehaviour
{
    private MouseLook mouseLook;
    private Tweener tweener;
    private bool isShowing=false;//是否已经展示面板
    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetTweener();
        mouseLook = GameObject.Find("Assault_Rifle_01_FPSController/Assault_Rifle_01_Arms").GetComponent<MouseLook>();
    }

    void SetTweener()
    {
        tweener = transform.DOLocalMove(new Vector3(16, 3, 0), 1f);
        tweener.Pause();
        tweener.SetAutoKill(false);
    }

    public void Show()
    {
        tweener.PlayForward();
    }
    public void Hide()
    {
        tweener.PlayBackwards();
    }
    public virtual void Translate()//点击面板过来，再次点击面板回去
    {
        if (!isShowing)
        {
            mouseLook.enabled = false;
            Show();
            isShowing = true;
        }
        else
        {
            mouseLook.enabled = true;
            Hide();
            isShowing = false;
        }
    }
}
