using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPurposeButton : GameButton
{
    public enum MultiPurposeButtonState
    {
        NONE,
        EXIT,
        MELEE_ATTACK,
        RANGED_ATTACK,
        SKIP_TURN,
        END_TURN
    }

    private MultiPurposeButtonState _state = MultiPurposeButtonState.NONE;

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

    // ====================================================================================================== //
    void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += onWindowLoaded;
        Hide();
	}
    // ====================================================================================================== //
    void Update ()
    {
		
	}
    // ====================================================================================================== //
    // triggered on WindowManager -> Event_WindowLoaded
    private void onWindowLoaded()
    {
        this.Show();

        if (WindowManager.Instance.CurrentWindowName == Consts.WindowNames.DUNGEON) 
        {
            _state = MultiPurposeButtonState.MELEE_ATTACK;
            _iconRenderer.sprite = _meleeHitSprite;
        }
        else if (WindowManager.Instance.CurrentWindowName == Consts.WindowNames.VILLAGE ||
            WindowManager.Instance.CurrentWindowName == Consts.WindowNames.MAIN_MENU)
        {
            this.Hide();
        }
        else
        {
            _state = MultiPurposeButtonState.EXIT;
            _iconRenderer.sprite = _exitSprite;
        }
    }
    // ====================================================================================================== //
    protected override void Clicked()
    {
        Debug.Log("Clicked by " + name);

        if (_state == MultiPurposeButtonState.EXIT)
        {
            if (Party.Instance.Loaction == Consts.WindowNames.VILLAGE)
                WindowManager.Instance.LoadWindow(Consts.WindowNames.VILLAGE);
            else
                WindowManager.Instance.LoadWindow(Consts.WindowNames.DUNGEON);
        }
    }
    // ====================================================================================================== //
}
