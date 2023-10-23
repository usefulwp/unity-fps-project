using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentItem : BaseItem
{
    public enum AttachmentType
    { 
      Scope,
      other
    }
    public AttachmentType CurrentAttachmentType;
}
