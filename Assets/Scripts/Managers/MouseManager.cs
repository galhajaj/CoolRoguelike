using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    NONE,
    DEFAULT,
    WAITING,
    CAN_MELEE_HIT,
    CAN_RANGED_HIT,
    CANNOT_CAST,
    CAN_CAST,
    CAN_CAST_FROM_POCKET,
    CAN_CAST_ON_PORTRAIT,
    CAN_CAST_ON_CREATURE,
    CAN_CAST_ON_DUNGEON_TILE,
    CANNOT_THROW,
    CAN_THROW,
    CAN_DRAG,
    DRAGGING,
    CAN_WALK,
    CAN_WALK_TO_KILL,
    CAN_WALK_NORTH,
    CAN_WALK_SOUTH,
    CAN_WALK_WEST,
    CAN_WALK_EAST,
    CAN_WALK_VILLAGE,
    CAN_PICKUP,
    CAN_USE_STAIRS,
    CAN_DRINK,
    CAN_SELECT_OR_UNSELECT_SOCKET
}

public class MouseManager : Singleton<MouseManager>
{
    [SerializeField]
    private Texture2D _defaultCursor = null;
    [SerializeField]
    private Texture2D _waitingCursor = null;
    [SerializeField]
    private Texture2D _canRangedHitCursor = null;
    [SerializeField]
    private Texture2D _canMeleeHitCursor = null;
    [SerializeField]
    private Texture2D _cannotCastCursor = null;
    [SerializeField]
    private Texture2D _canCastCursor = null;
    [SerializeField]
    private Texture2D _canWalkCursor = null;
    [SerializeField]
    private Texture2D _canPickupCursor = null;
    [SerializeField]
    private Texture2D _canUseStairsCursor = null;
    [SerializeField]
    private Texture2D _canWalkNorthCursor = null;
    [SerializeField]
    private Texture2D _canWalkSouthCursor = null;
    [SerializeField]
    private Texture2D _canWalkWestCursor = null;
    [SerializeField]
    private Texture2D _canWalkEastCursor = null;
    [SerializeField]
    private Texture2D _canWalkVillageCursor = null;

    // objects under mouse
    [Header("Objects Under Mouse")]
    [Space]
    public DungeonTile DungeonTileUnderMouse;
    public Creature CreatureUnderMouse;
    public Pocket PocketUnderMouse;
    public Item ItemInPocketUnderMouse;
    public Portrait PortraitUnderMouse;

    // mouse state, when changed - set the cursor
    private MouseState _state = MouseState.NONE;
    public MouseState State
    {
        get { return _state; }
        private set
        {
            if (_state == value)
                return;
            _state = value;
            setCursor();
        }
    }
    // ====================================================================================================== //
    void Start ()
    {
		
	}
    // ====================================================================================================== //
    // TODO: cursor manager to work with events rather than update...
    void Update()
    {
        updateObjectsUnderMouse();
        State = getMouseState();
        leftClickCheck();
        rightClickCheck();
    }
    // ====================================================================================================== //
    private void updateObjectsUnderMouse()
    {
        // dungeon tile & monster in it
        DungeonTileUnderMouse = Utils.GetObjectUnderCursor<DungeonTile>("DungeonTile");
        CreatureUnderMouse = (DungeonTileUnderMouse != null) ? DungeonTileUnderMouse.GetContainedCreature() : null;
        

        // pocket & item in it
        PocketUnderMouse = Utils.GetObjectUnderCursor<Pocket>("Pocket");
        ItemInPocketUnderMouse = (PocketUnderMouse != null) ? PocketUnderMouse.transform.GetComponentInChildren<Item>() : null;

        // party member portrait
        PortraitUnderMouse = Utils.GetObjectUnderCursor<Portrait>("Portrait");
    }
    // ====================================================================================================== //
    private MouseState getMouseState()
    {
        // in dungeon window
        if (WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.DUNGEON))
        {
            // not party turn or projectiles flying = waiting state
            if (!DungeonTurnManager.Instance.IsPartyTurn || ProjectileManager.Instance.IsFlownProjectileExist)
            {
                return MouseState.WAITING;
            }


            // with selected pocket item
            if (Party.Instance.SelectedMember.SelectedPocketItem != null)
            {
                // the selected item is scroll
                if (Party.Instance.SelectedMember.SelectedPocketItem.Type == ItemType.SCROLL)
                {
                    Scroll selectedScroll = (Party.Instance.SelectedMember.SelectedPocketItem as Scroll);
                    // the scroll target type is dungeon tile
                    if (selectedScroll.TargetType == Scroll.ScrollTargetType.DUNGEON_TILE)
                    {
                        // mouse on creature in dungeon tile
                        if (DungeonTileUnderMouse != null)
                            return MouseState.CAN_CAST_ON_DUNGEON_TILE;
                    }
                    // the scroll target type is creature
                    else if (selectedScroll.TargetType == Scroll.ScrollTargetType.CREATURE)
                    {
                        // mouse on creature in dungeon tile
                        if (CreatureUnderMouse != null)
                            return MouseState.CAN_CAST_ON_CREATURE;
                    }
                    // the scroll target type is party member (miniature)
                    else if (selectedScroll.TargetType == Scroll.ScrollTargetType.PARTY_MEMBER)
                    {
                        // mouse on portrait
                        if (PortraitUnderMouse != null)
                            return MouseState.CAN_CAST_ON_PORTRAIT;
                    }
                }

                return MouseState.CANNOT_CAST;
            }
            // no selected pocket item
            else
            {
                // on dungeon tile
                if (DungeonTileUnderMouse != null)
                {
                    // has monster on the tile
                    if (CreatureUnderMouse != null)
                    {
                        // the monster is close to party
                        if (Party.Instance.Position.DistanceTo(CreatureUnderMouse.Position) <= 1)
                        {
                            return MouseState.CAN_MELEE_HIT;
                        }
                        // the monster is far from party
                        else
                        {
                            // selected member can shoot
                            if (Party.Instance.SelectedMember.IsCanShoot)
                            {
                                return MouseState.CAN_RANGED_HIT;
                            }
                            // selected member cannot shoot - but it can go towards the monster to kill it!
                            else
                            {
                                return MouseState.CAN_WALK_TO_KILL;
                            }
                        }
                    }
                    // no monster on the tile
                    else
                    {
                        // tile is not blocked - walkable...
                        if (!DungeonTileUnderMouse.IsBlockPath)
                        {
                            // in peace mode
                            if (DungeonTurnManager.Instance.IsPartyInPeaceMode)
                            {
                                // tile contain loot
                                if (DungeonTileUnderMouse.IsContainLoot)
                                    return MouseState.CAN_PICKUP;
                                // tile contain stairs
                                if (DungeonTileUnderMouse.IsPortal)
                                    return MouseState.CAN_USE_STAIRS;
                                // tile in origin area & on border
                                if (Dungeon.Instance.IsInOriginArea && DungeonTileUnderMouse.Position.IsBorder)
                                    return MouseState.CAN_WALK_VILLAGE;
                                // tile on north border
                                if (DungeonTileUnderMouse.Position.IsNorthBorder)
                                    return MouseState.CAN_WALK_NORTH;
                                // tile on south border
                                if (DungeonTileUnderMouse.Position.IsSouthBorder)
                                    return MouseState.CAN_WALK_SOUTH;
                                // tile on west border
                                if (DungeonTileUnderMouse.Position.IsWestBorder)
                                    return MouseState.CAN_WALK_WEST;
                                // tile on east border
                                if (DungeonTileUnderMouse.Position.IsEastBorder)
                                    return MouseState.CAN_WALK_EAST;
                            }

                            return MouseState.CAN_WALK;
                        }
                    }
                }
                // on pocket
                else if (PocketUnderMouse != null)
                {
                    // has item in pocket
                    if (ItemInPocketUnderMouse != null)
                    {
                        // the item is potion
                        if (ItemInPocketUnderMouse.Type == ItemType.POTION)
                        {
                            return MouseState.CAN_DRINK;
                        }
                        // the item is scroll
                        else if (ItemInPocketUnderMouse.Type == ItemType.SCROLL)
                        {
                            // the scroll not needed to be targeted - just cast!
                            if ((ItemInPocketUnderMouse as Scroll).TargetType == Scroll.ScrollTargetType.NONE)
                            {
                                return MouseState.CAN_CAST_FROM_POCKET;
                            }
                            // the scroll is need to be targeted
                            else
                            {
                                return MouseState.CAN_SELECT_OR_UNSELECT_SOCKET;
                            }
                        }
                        // otherwise, it's an item that can be selected or unselected
                        else
                        {
                            return MouseState.CAN_SELECT_OR_UNSELECT_SOCKET;
                        }
                    }
                }
            }
        }

        return MouseState.DEFAULT;
    }
    // ====================================================================================================== //
    private void setCursor()
    {
        Texture2D cursorTexture = _defaultCursor;

        switch (_state)
        {
            case MouseState.WAITING:
                cursorTexture = _waitingCursor;
                break;
            case MouseState.CAN_MELEE_HIT:
                cursorTexture = _canMeleeHitCursor;
                break;
            case MouseState.CAN_RANGED_HIT:
                cursorTexture = _canRangedHitCursor;
                break;
            case MouseState.CANNOT_CAST:
                cursorTexture = _cannotCastCursor;
                break;
            case MouseState.CAN_CAST:
                cursorTexture = _canCastCursor;
                break;
            case MouseState.CAN_CAST_FROM_POCKET:
                cursorTexture = _canCastCursor;
                break;
            case MouseState.CAN_CAST_ON_PORTRAIT:
                cursorTexture = _canCastCursor;
                break;
            case MouseState.CAN_CAST_ON_CREATURE:
                cursorTexture = _canCastCursor;
                break;
            case MouseState.CAN_CAST_ON_DUNGEON_TILE:
                cursorTexture = _canCastCursor;
                break;
            case MouseState.CANNOT_THROW:
                break;
            case MouseState.CAN_THROW:
                break;
            case MouseState.CAN_DRAG:
                break;
            case MouseState.DRAGGING:
                break;
            case MouseState.CAN_WALK:
                cursorTexture = _canWalkCursor;
                break;
            case MouseState.CAN_WALK_TO_KILL:
                cursorTexture = _canMeleeHitCursor;
                break;
            case MouseState.CAN_WALK_NORTH:
                cursorTexture = _canWalkNorthCursor;
                break;
            case MouseState.CAN_WALK_SOUTH:
                cursorTexture = _canWalkSouthCursor;
                break;
            case MouseState.CAN_WALK_WEST:
                cursorTexture = _canWalkWestCursor;
                break;
            case MouseState.CAN_WALK_EAST:
                cursorTexture = _canWalkEastCursor;
                break;
            case MouseState.CAN_WALK_VILLAGE:
                cursorTexture = _canWalkVillageCursor;
                break;
            case MouseState.CAN_PICKUP:
                cursorTexture = _canPickupCursor;
                break;
            case MouseState.CAN_USE_STAIRS:
                cursorTexture = _canUseStairsCursor;
                break;
            case MouseState.CAN_DRINK:
                break;
            case MouseState.CAN_SELECT_OR_UNSELECT_SOCKET:
                break;
        }

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    // ====================================================================================================== //
    private void leftClickCheck()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        // select unselect pocket with item in it
        if (State == MouseState.CAN_SELECT_OR_UNSELECT_SOCKET)
        {
            bool isItemAlreadySelected = (Party.Instance.SelectedMember.SelectedPocketItem == ItemInPocketUnderMouse);
            Party.Instance.SelectedMember.SelectedPocketItem = isItemAlreadySelected ? null : ItemInPocketUnderMouse;
            return; // should return to prevent losing the selection of the selected item
        }
        // melee attack
        if (State == MouseState.CAN_MELEE_HIT)
            Party.Instance.SelectedMember.MeleeAttack(CreatureUnderMouse);
        // ranged attack
        if (State == MouseState.CAN_RANGED_HIT)
            Party.Instance.SelectedMember.RangedAttack(CreatureUnderMouse);
        // walk
        if (State == MouseState.CAN_WALK || State == MouseState.CAN_WALK_NORTH || State == MouseState.CAN_WALK_SOUTH || 
            State == MouseState.CAN_WALK_WEST || State == MouseState.CAN_WALK_EAST || State == MouseState.CAN_PICKUP || 
            State == MouseState.CAN_USE_STAIRS || State == MouseState.CAN_WALK_VILLAGE || State == MouseState.CAN_WALK_TO_KILL)
            DungeonTurnManager.Instance.PartyTargetPosition = DungeonTileUnderMouse.Position;
        // potion drink
        if (State == MouseState.CAN_DRINK)
            Party.Instance.SelectedMember.ConsumeItemFromPocket(ItemInPocketUnderMouse);
        // activate scroll from pocket (when no need for target)
        if (State == MouseState.CAN_CAST_FROM_POCKET)
            (ItemInPocketUnderMouse as Scroll).Activate();

        Scroll selectedScroll = (Party.Instance.SelectedMember.SelectedPocketItem as Scroll);
        // activate scroll on dungeon tile
        if (State == MouseState.CAN_CAST_ON_DUNGEON_TILE)
        {
            selectedScroll.TargetDungeonTile = DungeonTileUnderMouse;
            selectedScroll.Activate();
        }
        // activate scroll on creature
        if (State == MouseState.CAN_CAST_ON_CREATURE)
        {
            selectedScroll.TargetDungeonTile = DungeonTileUnderMouse;
            selectedScroll.TargetCreature = CreatureUnderMouse;
            selectedScroll.Activate();
        }
        // activate scroll on party member
        if (State == MouseState.CAN_CAST_ON_PORTRAIT)
        {
            selectedScroll.TargetCreature = PortraitUnderMouse.Creature;
            selectedScroll.Activate();
        }

        // no matter what, after click - lose the selection
        Party.Instance.SelectedMember.SelectedPocketItem = null;
    }
    // ====================================================================================================== //
    private void rightClickCheck()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

        // cancel selected pocket item
        Party.Instance.SelectedMember.SelectedPocketItem = null;
    }
    // ====================================================================================================== //
}
