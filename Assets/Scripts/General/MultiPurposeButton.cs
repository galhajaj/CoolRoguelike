using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPurposeButton : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _iconRenderer = null;

    [SerializeField]
    private Sprite _exitSprite = null;
    [SerializeField]
    private Sprite _meleeHitSprite = null;
    [SerializeField]
    private Sprite _rangedHitSprite = null;
    [SerializeField]
    private Sprite _skipTurnSprite = null;
    [SerializeField]
    private Sprite _endTurnSprite = null;

    void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += onWindowLoaded;
	}
	
	void Update ()
    {
		
	}

    private void onWindowLoaded()
    {
        if (WindowManager.Instance.CurrentWindowName == Consts.WINDOW_DUNGEON)
        {
            _iconRenderer.sprite = _meleeHitSprite;
        }
        else
        {
            _iconRenderer.sprite = _exitSprite;
        }
    }
}
