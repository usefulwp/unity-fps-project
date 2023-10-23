using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmsItem : BaseItem
{
    public enum FirearmsType
    { 
       AssaultRifle,
       Handgun,
       Submachinegun,//冲锋枪
       SniperRifle//狙击枪
    }
    public FirearmsType CurrentFirearmsType;
    public string ArmsName;
}
