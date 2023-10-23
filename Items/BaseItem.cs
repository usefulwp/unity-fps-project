using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public enum ItemType
    { 
      Firearms,
      Attachment,
      Others
    }
    public ItemType CurrentItemType;
    public int ItemID;
    public string ItemName;
}
