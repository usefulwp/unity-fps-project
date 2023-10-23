using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Weapon;
public class Crosshair : MonoBehaviour
{
    public RectTransform RectTrans;
    public CharacterController CharacterController;
    public float OriginalSize;
    public float TargetSize;
    
    public WeaponManager WeaponManager;
    private float currentSize;
    private void Update()
    {
       
        if (CharacterController.velocity.magnitude > 0 || WeaponManager.isFiring)
        {

            currentSize = Mathf.Lerp(currentSize, TargetSize, Time.deltaTime * 5f);
        }
        else
        { 
            currentSize= Mathf.Lerp(currentSize, OriginalSize, Time.deltaTime * 5f);
        }
        RectTrans.sizeDelta = new Vector2(currentSize,currentSize);
    }
}
