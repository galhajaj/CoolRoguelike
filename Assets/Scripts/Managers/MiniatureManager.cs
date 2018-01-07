using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureManager : Singleton<MiniatureManager>
{
    public void AddItem(Item item)
    {
        //item.transform.position = this.transform.position;
        item.transform.parent = this.transform;
        item.State = ItemState.EQUIPPED;
    }
}
