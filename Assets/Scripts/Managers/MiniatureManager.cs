using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureManager : Singleton<MiniatureManager>
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    private Creature _currentCreature;

    void Update()
    {

    }

    void LateUpdate()
    {
        // default miniature show (in inventory it's the current selected member, otherwise - it's null)
        if (WindowManager.Instance.IsCurrentWindow(Consts.INVENTORY))
            ShowCreature(Party.Instance.SelectedMember);
        else
            ShowCreature(null);

        cursorOverDungeonTile();
        cursorOverPortrait();
    }

    public void ShowCreature(Creature creature)
    {
        // show creature image (or set to null if the creature is null)
        Sprite imageToShow = (creature != null) ? creature.Image : null;
        this.ShowImage(imageToShow);

        // hide previous creature items
        if (_currentCreature != null)
            foreach (Item item in _currentCreature.EquippedItems.Values)
                item.Hide();

        // show creature equipped items
        if (creature != null)
            foreach (Item item in creature.EquippedItems.Values)
            {
                //item.transform.position = this.transform.position;
                item.transform.parent = this.transform;
                item.Show();
            }

        // update current creature
        _currentCreature = creature;
    }

    public void ShowImage(Sprite image)
    {
        _spriteRenderer.sprite = image;
    }

    private void cursorOverDungeonTile()
    {
        DungeonTile dungeonTile = Utils.GetObjectUnderCursor<DungeonTile>("DungeonTile");
        if (dungeonTile == null)
            return;

        Creature creature = dungeonTile.GetContainedCreature();
        if (creature != null)
            ShowCreature(creature);
        else
            ShowImage(dungeonTile.GetImage());
    }

    private void cursorOverPortrait()
    {
        Portrait portrait = Utils.GetObjectUnderCursor<Portrait>("Portrait");
        if (portrait == null)
            return;

        ShowCreature(portrait.Creature);
    }
}
