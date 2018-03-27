using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : GameButton
{
    void Start()
    {
        WindowManager.Instance.Event_BeforeWindowLoaded += onBeforeWindowLoaded;
    }

    private void onBeforeWindowLoaded()
    {
        // before every window loaded - set active if only not in village
        this.gameObject.SetActive(!Party.Instance.IsInVillage);
    }
}
