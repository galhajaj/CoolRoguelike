using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButton : GameButton
{
    /*public enum MultiPurposeButtonState
    {
        NONE,
        MELEE_ATTACK,
        RANGED_ATTACK,
        SKIP_TURN,
        END_TURN
    }*/

    /*private MultiPurposeButtonState _state = MultiPurposeButtonState.NONE;

    [SerializeField]
    private SpriteRenderer _iconRenderer = null;

    [SerializeField]
    private Sprite _meleeHitSprite = null;
    [SerializeField]
    private Sprite _rangedHitSprite = null;
    [SerializeField]
    private Sprite _skipTurnSprite = null;
    [SerializeField]
    private Sprite _endTurnSprite = null;*/

    // ====================================================================================================== //
    void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += onWindowLoaded;
        gameObject.SetActive(false);
	}
    // ====================================================================================================== //
    void Update ()
    {
		
	}
    // ====================================================================================================== //
    // triggered on WindowManager -> Event_WindowLoaded
    private void onWindowLoaded()
    {
        gameObject.SetActive(true);

        if (WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.DUNGEON)) 
        {
            /*_state = MultiPurposeButtonState.MELEE_ATTACK;
            _iconRenderer.sprite = _meleeHitSprite;*/
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    // ====================================================================================================== //
    protected override void afterClicked()
    {
        // TODO: implement auto turn button
    }
    // ====================================================================================================== //
}
